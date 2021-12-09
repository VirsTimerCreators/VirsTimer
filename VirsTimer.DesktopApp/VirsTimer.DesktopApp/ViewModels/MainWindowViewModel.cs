using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Events;
using VirsTimer.DesktopApp.ViewModels.Rooms;
using VirsTimer.DesktopApp.ViewModels.Scrambles;
using VirsTimer.DesktopApp.ViewModels.Sessions;
using VirsTimer.DesktopApp.ViewModels.Solves;
using VirsTimer.DesktopApp.ViewModels.Statistics;
using VirsTimer.DesktopApp.Views.Solves;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ISolvesRepository _solvesRepository;
        private readonly IValueConverter<string, Bitmap> _svgToBitmapConverter;

        [Reactive]
        public bool IsBusyManual { get; set; }

        [ObservableAsProperty]
        public new bool IsBusy { get; set; }

        public EventSummaryViewModel EventViewModel { get; }
        public SessionSummaryViewModel SessionSummaryViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }
        public StatisticsViewModel StatisticsViewModel { get; } = null!;

        public ReactiveCommand<Window, Unit> AddSolveManualyCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenMultiplayerCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenMenuCommand { get; }

        [Reactive]
        public IImage? ChooseEventImage { get; private set; }

        [Reactive]

        public IImage? ChooseSessionImage { get; private set; }

        [Reactive]

        public IImage? ImportExportImage { get; private set; }

        [Reactive]

        public IImage? MultiplayerImage { get; private set; }

        [Reactive]

        public IImage? ExitImage { get; private set; }

        [Reactive]

        public IImage? MenuImage { get; private set; }

        public Interaction<RoomCreationViewModel, RoomViewModel?> ShowRoomCreationDialog { get; }
        public Interaction<RoomViewModel, Unit> ShowRoomDialog { get; }

        public MainWindowViewModel(
            bool online,
            IValueConverter<string, Bitmap>? svgToBitmapConverter = null)
        {
            _solvesRepository = Ioc.Services.GetRequiredService<ISolvesRepository>();
            _svgToBitmapConverter = svgToBitmapConverter ?? new SvgToBitmapConverter(100);

            EventViewModel = new EventSummaryViewModel();
            SessionSummaryViewModel = new SessionSummaryViewModel();
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel();
            ScrambleViewModel = new ScrambleViewModel();
            StatisticsViewModel = new StatisticsViewModel();

            AddSolveManualyCommand = ReactiveCommand.CreateFromTask<Window>(AddSolveManually);
            OpenMultiplayerCommand = ReactiveCommand.CreateFromTask(OpenRoomCreationDialog, Observable.Return(online));
            ExitCommand = ReactiveCommand.Create(() =>
            {
                (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
            });
            OpenMenuCommand = ReactiveCommand.Create(() => { }, Observable.Return(false));

            this.WhenAnyValue(x => x.EventViewModel.CurrentEvent)
                .Skip(1)
                .Subscribe(async _ =>
                {
                    await ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);
                    await SessionSummaryViewModel.LoadSessionAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);
                });

            this.WhenAnyValue(x => x.SessionSummaryViewModel.CurrentSession)
                .Skip(1)
                .Subscribe(async _ =>
                {
                    await SolvesListViewModel.ChangeSessionAsync(SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
                    await StatisticsViewModel.Construct(SolvesListViewModel.Solves).ConfigureAwait(false);
                });

            this.WhenAnyValue(
                x => x.EventViewModel.IsBusy,
                x => x.SessionSummaryViewModel.IsBusy,
                x => x.SolvesListViewModel.IsBusy,
                x => x.ScrambleViewModel.IsBusy,
                x => x.StatisticsViewModel.IsBusy,
                x => x.IsBusyManual,
                (b1, b2, b3, b4, b5, b6) => b1 || b2 || b3 || b4 || b5 || b6)
                .ToPropertyEx(this, x => x.IsBusy);

            ShowRoomCreationDialog = new Interaction<RoomCreationViewModel, RoomViewModel?>();
            ShowRoomDialog = new Interaction<RoomViewModel, Unit>();
        }

        public override async Task ConstructAsync()
        {
            IsBusyManual = true;

            await EventViewModel.ConstructAsync().ConfigureAwait(false);

            var hamburgerSvgTask = File.ReadAllTextAsync("Assets/hamburger.svg");
            var eventSvgTask = File.ReadAllTextAsync("Assets/event.svg");
            var sessionSvgTask = File.ReadAllTextAsync("Assets/session.svg");
            var exportSvgTask = File.ReadAllTextAsync("Assets/export.svg");
            var multiplayerSvgTask = File.ReadAllTextAsync("Assets/multiplayer.svg");
            var exitSvgTask = File.ReadAllTextAsync("Assets/exit.svg");

            var svgs = await Task.WhenAll(
                hamburgerSvgTask,
                eventSvgTask,
                sessionSvgTask,
                exportSvgTask,
                multiplayerSvgTask,
                exitSvgTask).ConfigureAwait(false);

            MenuImage = _svgToBitmapConverter.Convert(svgs[0]);
            ChooseEventImage = _svgToBitmapConverter.Convert(svgs[1]);
            ChooseSessionImage = _svgToBitmapConverter.Convert(svgs[2]);
            ImportExportImage = _svgToBitmapConverter.Convert(svgs[3]);
            MultiplayerImage = _svgToBitmapConverter.Convert(svgs[4]);
            ExitImage = _svgToBitmapConverter.Convert(svgs[5]);

            IsBusyManual = false;
        }

        public async Task OpenRoomCreationDialog()
        {
            var roomCreationViewModel = new RoomCreationViewModel();
            var roomViewModel = await ShowRoomCreationDialog.Handle(roomCreationViewModel);
            if (roomViewModel is not null && roomViewModel.Valid)
                await ShowRoomDialog.Handle(roomViewModel);
        }

        public async Task SaveSolveAsync(Solve solve)
        {
            IsBusyManual = true;
            await _solvesRepository.AddSolveAsync(solve).ConfigureAwait(false);
            SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve, _solvesRepository));

            await ScrambleViewModel.GetNextScrambleAsync().ConfigureAwait(false);
            IsBusyManual = false;
        }

        private async Task AddSolveManually(Window window)
        {
            var solveAddViewModel = new SolveAddViewModel();
            var solveAddView = new SolveAddView
            {
                DataContext = solveAddViewModel
            };
            await solveAddView.ShowDialog(window);
            if (!solveAddViewModel.Accepted)
                return;
            var solve = new Solve(
                SessionSummaryViewModel.CurrentSession,
                solveAddViewModel.SolveTime,
                ScrambleViewModel.CurrentScramble.Value);

            await SaveSolveAsync(solve).ConfigureAwait(false);
        }
    }
}