using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EventViewModel EventViewModel { get; }
        public SessionViewModel SessionViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }

        public MainWindowViewModel(Event @event)
        {
            EventViewModel = new EventViewModel();
            SessionViewModel = new SessionViewModel(EventViewModel.CurrentEvent);
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel();
            ScrambleViewModel = new ScrambleViewModel(@event);
        }
    }
}
