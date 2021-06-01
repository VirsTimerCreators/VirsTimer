using System;
using System.ComponentModel;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.ViewModels.Sessions;
using VirsTimer.DesktopApp.ViewModels.Solves;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EventViewModel EventViewModel { get; }
        public SessionSummaryViewModel SessionSummaryViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }

        public MainWindowViewModel(Event @event)
        {
            EventViewModel = new EventViewModel(@event);
            SessionSummaryViewModel = new SessionSummaryViewModel(EventViewModel.CurrentEvent);
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel();
            ScrambleViewModel = new ScrambleViewModel(@event);

            EventViewModel.PropertyChanged += OnEventChangeAsync;
            OnConstructedAsync(this, EventArgs.Empty);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            await LoadSolvesAsync().ConfigureAwait(false);
        }

        private async void OnEventChangeAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(EventViewModel.CurrentEvent))
                return;

            await SessionSummaryViewModel.ChangeSessionAsync(EventViewModel.CurrentEvent).ConfigureAwait(false);
            await ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent).ConfigureAwait(false);
            await SolvesListViewModel.LoadAsync(EventViewModel.CurrentEvent, SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
        }

        public Task LoadSolvesAsync()
        {
            return SolvesListViewModel.LoadAsync(EventViewModel.CurrentEvent, SessionSummaryViewModel.CurrentSession);
        }

        public async Task SaveSolveAsync(Solve solve)
        {
            SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve));
            await SolvesListViewModel.SaveAsync(EventViewModel.CurrentEvent, SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
            await ScrambleViewModel.GetNextScrambleAsync().ConfigureAwait(false);
        }
    }
}
