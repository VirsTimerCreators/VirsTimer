using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Common;
using VirsTimer.DesktopApp.ViewModels.Events;
using VirsTimer.DesktopApp.ViewModels.Export;
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
        private readonly List<Solve> _failedSolves = new();

        private readonly ISolvesRepository _solvesRepository;
        private readonly IValueConverter<string, Bitmap> _svgToBitmapConverter;

        [Reactive]
        public bool IsBusyManual { get; set; }

        [ObservableAsProperty]
        public new bool IsBusy { get; set; }

        public SnackbarViewModel SnackbarViewModel { get; }

        public EventSummaryViewModel EventViewModel { get; }
        public SessionSummaryViewModel SessionViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }
        public StatisticsViewModel StatisticsViewModel { get; } = null!;

        public ReactiveCommand<Window, Unit> AddSolveManualyCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenMultiplayerCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }
        public ReactiveCommand<Unit, Unit> OpenMenuCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportCommand { get; }

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

        public Interaction<ExportsViewModel, Unit> ShowExportDialog { get; }

        public MainWindowViewModel(
            bool online,
            IValueConverter<string, Bitmap>? svgToBitmapConverter = null)
        {
            _solvesRepository = Ioc.Services.GetRequiredService<ISolvesRepository>();
            _svgToBitmapConverter = svgToBitmapConverter ?? new SvgToBitmapConverter(100);

            SnackbarViewModel = new SnackbarViewModel(400, 96);

            EventViewModel = new EventSummaryViewModel();
            SessionViewModel = new SessionSummaryViewModel(SnackbarViewModel);
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel(SnackbarViewModel);
            ScrambleViewModel = new ScrambleViewModel(SnackbarViewModel);
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
                    var scramblesTask = ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent);
                    var sessionTask = SessionViewModel.LoadSessionAsync(EventViewModel.CurrentEvent);
                    await Task.WhenAll(scramblesTask, sessionTask);
                });

            this.WhenAnyValue(x => x.SessionViewModel.CurrentSession)
                .Skip(1)
                .Subscribe(async _ =>
                {
                    await SolvesListViewModel.ChangeSessionAsync(SessionViewModel.CurrentSession);
                    await StatisticsViewModel.Construct(SolvesListViewModel.Solves);
                });

            this.WhenAnyValue(
                x => x.EventViewModel.IsBusy,
                x => x.SessionViewModel.IsBusy,
                x => x.SolvesListViewModel.IsBusy,
                x => x.ScrambleViewModel.IsBusy,
                x => x.StatisticsViewModel.IsBusy,
                x => x.IsBusyManual,
                (b1, b2, b3, b4, b5, b6) => b1 || b2 || b3 || b4 || b5 || b6)
                .ToPropertyEx(this, x => x.IsBusy);

            ShowRoomCreationDialog = new Interaction<RoomCreationViewModel, RoomViewModel?>();
            ShowRoomDialog = new Interaction<RoomViewModel, Unit>();

            ShowExportDialog = new Interaction<ExportsViewModel, Unit>();
            ExportCommand = ReactiveCommand.CreateFromTask(ExportAsync);
        }

        public override async Task<bool> ConstructAsync()
        {
            IsBusyManual = true;

            await EventViewModel.ConstructAsync();
            await SessionViewModel.ConstructAsync();

            var hamburgerSvgTask = File.ReadAllTextAsync("Assets/hamburger.svg");
            var exportSvgTask = File.ReadAllTextAsync("Assets/export.svg");
            var multiplayerSvgTask = File.ReadAllTextAsync("Assets/multiplayer.svg");
            var exitSvgTask = File.ReadAllTextAsync("Assets/exit.svg");

            var svgs = await Task.WhenAll(
                hamburgerSvgTask,
                exportSvgTask,
                multiplayerSvgTask,
                exitSvgTask);

            MenuImage = _svgToBitmapConverter.Convert(svgs[0]);
            ImportExportImage = _svgToBitmapConverter.Convert(svgs[1]);
            MultiplayerImage = _svgToBitmapConverter.Convert(svgs[2]);
            ExitImage = _svgToBitmapConverter.Convert(svgs[3]);

            IsBusyManual = false;
            return true;
        }

        public async Task OpenRoomCreationDialog()
        {
            var roomCreationViewModel = new RoomCreationViewModel();
            var roomViewModel = await ShowRoomCreationDialog.Handle(roomCreationViewModel);
            if (roomViewModel is not null && roomViewModel.Valid)
                await ShowRoomDialog.Handle(roomViewModel);
        }

        private async Task ExportAsync()
        {
            var exportsViewModel = new ExportsViewModel(
                SessionViewModel.CurrentSession,
                SolvesListViewModel.Solves);
            await ShowExportDialog.Handle(exportsViewModel);
            if (exportsViewModel.Imported)
                await SolvesListViewModel.ChangeSessionAsync(SessionViewModel.CurrentSession).ConfigureAwait(false);
        }

        public async Task SaveSolveAsync(Solve solve)
        {
            IsBusyManual = true;
            if (_failedSolves.IsNullOrEmpty() is false)
                await _solvesRepository.AddSolvesAsync(_failedSolves);

            var repositoryResponse = await _solvesRepository.AddSolveAsync(solve);
            if (repositoryResponse.IsSuccesfull is false)
            {
                _failedSolves.Add(solve);
                Task.Run(async () => await SnackbarViewModel.Enqueue("Podczas zapisywania ułożenia wystąpił błąd. Próba zapisu będzie powtórzona po kolejnym ułożeniu."));
            }

            SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve, _solvesRepository));
            await ScrambleViewModel.GetNextScrambleAsync();
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
                SessionViewModel.CurrentSession,
                solveAddViewModel.SolveTime,
                ScrambleViewModel.CurrentScramble.Value);

            await SaveSolveAsync(solve).ConfigureAwait(false);
        }
    }
}