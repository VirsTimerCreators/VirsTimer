using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EventViewModel EventViewModel { get; }
        public SessionViewModel SessionViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }

        public MainWindowViewModel(string @event, IPastSolvesGetter pastSolvesGetter, ISolvesSaver solvesSaver, IScrambleGenerator scrambleGenerator, ISessionsManager sessionsManager)
        {
            EventViewModel = new EventViewModel();
            SessionViewModel = new SessionViewModel(EventViewModel.CurrentEvent.Name, sessionsManager);
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel(pastSolvesGetter, solvesSaver);
            ScrambleViewModel = new ScrambleViewModel(@event, scrambleGenerator);
        }
    }
}
