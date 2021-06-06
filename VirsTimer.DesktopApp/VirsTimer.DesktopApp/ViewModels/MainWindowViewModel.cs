using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;
using VirsTimer.Core.Services.Events;
using VirsTimer.Core.Services.Sessions;
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

        public EventViewModel EventViewModel { get; }
        public SessionSummaryViewModel SessionSummaryViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }
        public StatisticsViewModel StatisticsViewModel { get; } = null!;

        public ReactiveCommand<Window, Unit> AddSolveManualyCommand { get; }
        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        public MainWindowViewModel(IEventsRepository eventsRepository, ISessionRepository sessionRepository, ISolvesRepository solvesRepository, IScrambleGenerator scrambleGenerator)
        {
            _solvesRepository = Ioc.Services.GetRequiredService<ISolvesRepository>();

            EventViewModel = new EventViewModel(eventsRepository);
            SessionSummaryViewModel = new SessionSummaryViewModel(sessionRepository);
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel(solvesRepository);
            ScrambleViewModel = new ScrambleViewModel(scrambleGenerator);
            StatisticsViewModel = new StatisticsViewModel();

            AddSolveManualyCommand = ReactiveCommand.CreateFromTask<Window>(AddSolveManually);
            ExitCommand = ReactiveCommand.Create(() =>
            {
                (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
            });

            EventViewModel.PropertyChanged += OnBusyChange;
            SessionSummaryViewModel.PropertyChanged += OnBusyChange;
            SolvesListViewModel.PropertyChanged += OnBusyChange;
            ScrambleViewModel.PropertyChanged += OnBusyChange;
            StatisticsViewModel.PropertyChanged += OnBusyChange;
        }

        private void OnBusyChange(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(IsBusy))
                return;

            IsBusy = EventViewModel.IsBusy || SessionSummaryViewModel.IsBusy || SolvesListViewModel.IsBusy || ScrambleViewModel.IsBusy || StatisticsViewModel.IsBusy;
        }

        public override async Task ConstructAsync()
        {
            await EventViewModel.ConstructAsync().ConfigureAwait(false);
            await SessionSummaryViewModel.ChangeSessionAsync(EventViewModel.CurrentEvent).ConfigureAwait(false);
            await SolvesListViewModel.ChangeSessionAsync(SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
            await ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent).ConfigureAwait(false);
            await StatisticsViewModel.Construct(SolvesListViewModel.Solves.Reverse().Select(x => x.Model)).ConfigureAwait(false);

            EventViewModel.PropertyChanged += OnEventChangeAsync;
            SessionSummaryViewModel.PropertyChanged += OnSessionChangeAsync;
            SolvesListViewModel.Solves.CollectionChanged += OnSolvesListChangeAsync;
        }

        public async Task SaveSolveAsync(Solve solve)
        {
            IsBusy = true;
            await _solvesRepository.AddSolveAsync(solve).ConfigureAwait(false);
            SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve, _solvesRepository));

            await ScrambleViewModel.GetNextScrambleAsync().ConfigureAwait(false);
            IsBusy = false;
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

        private async void OnSolvesListChangeAsync(object? sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Move)
                await StatisticsViewModel.AddSolve((e.NewItems[0] as SolveViewModel)!.Model).ConfigureAwait(false);
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                await StatisticsViewModel.DeleteSolve((e.OldItems[0] as SolveViewModel)!.Model).ConfigureAwait(false);
        }

        private async void OnEventChangeAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(EventViewModel.CurrentEvent))
                return;

            await ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);
            await SessionSummaryViewModel.ChangeSessionAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);
        }

        private async void OnSessionChangeAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SessionSummaryViewModel.CurrentSession))
                return;

            await SolvesListViewModel.ChangeSessionAsync(SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
            await StatisticsViewModel.Construct(SolvesListViewModel.Solves.Reverse().Select(x => x.Model)).ConfigureAwait(false);
            SolvesListViewModel.Solves.CollectionChanged += OnSolvesListChangeAsync;
        }
    }
}
