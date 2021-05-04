using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EventViewModel EventViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }

        public MainWindowViewModel(string @event, IPastSolvesGetter pastSolvesGetter, ISolvesSaver solvesSaver, IScrambleGenerator scrambleGenerator)
        {
            EventViewModel = new EventViewModel();
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel(pastSolvesGetter, solvesSaver);
            ScrambleViewModel = new ScrambleViewModel(@event, scrambleGenerator);
        }
    }
}
