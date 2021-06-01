using Avalonia.Controls;
using ReactiveUI;
using System;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.ViewModels.Sessions;
using VirsTimer.DesktopApp.ViewModels.Solves;
using VirsTimer.DesktopApp.Views.Solves;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EventViewModel EventViewModel { get; }
        public SessionSummaryViewModel SessionSummaryViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }

        public ReactiveCommand<Window, Unit> AddSolveManualyCommand { get; }

        public MainWindowViewModel(Event @event)
        {
            EventViewModel = new EventViewModel(@event);
            SessionSummaryViewModel = new SessionSummaryViewModel(EventViewModel.CurrentEvent);
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel();
            ScrambleViewModel = new ScrambleViewModel(@event);

            AddSolveManualyCommand = ReactiveCommand.CreateFromTask<Window>(AddSolveManually);

            EventViewModel.PropertyChanged += OnEventChangeAsync;
            SessionSummaryViewModel.PropertyChanged += OnSessionChangeAsync;
            OnConstructedAsync(this, EventArgs.Empty);
        }

        public async Task SaveSolveAsync(Solve solve)
        {
            SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve));
            await SolvesListViewModel.SaveAsync(EventViewModel.CurrentEvent, SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
            await ScrambleViewModel.GetNextScrambleAsync().ConfigureAwait(false);
        }

        public Task LoadSolvesAsync()
        {
            return SolvesListViewModel.LoadAsync(EventViewModel.CurrentEvent, SessionSummaryViewModel.CurrentSession);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            await LoadSolvesAsync().ConfigureAwait(false);
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
            var solve = new Solve(solveAddViewModel.SolveTime, ScrambleViewModel.CurrentScramble.Value);
            await SaveSolveAsync(solve);
        }

        private async void OnEventChangeAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(EventViewModel.CurrentEvent))
                return;

            var scrambleChange = ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);
            var sessionChange = SessionSummaryViewModel.ChangeSessionAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);

            await scrambleChange;
            await sessionChange;
        }

        private async void OnSessionChangeAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SessionSummaryViewModel.CurrentSession))
                return;

            await LoadSolvesAsync().ConfigureAwait(false);
        }
    }
}
