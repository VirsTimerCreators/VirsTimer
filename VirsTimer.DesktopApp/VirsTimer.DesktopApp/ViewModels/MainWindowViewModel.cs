using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.ViewModels.Events;
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
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public MainWindowViewModel()
        {
            _solvesRepository = Ioc.Services.GetRequiredService<ISolvesRepository>();

            EventViewModel = new EventSummaryViewModel();
            SessionSummaryViewModel = new SessionSummaryViewModel();
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel();
            ScrambleViewModel = new ScrambleViewModel();
            StatisticsViewModel = new StatisticsViewModel();

            AddSolveManualyCommand = ReactiveCommand.CreateFromTask<Window>(AddSolveManually);
            ExitCommand = ReactiveCommand.Create(() =>
            {
                (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
            });

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
        }

        public override async Task ConstructAsync()
        {
            await EventViewModel.ConstructAsync().ConfigureAwait(false);
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