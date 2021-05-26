using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.ViewModels.Solves;

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

        public Task LoadSolvesAsync()
        {
            return SolvesListViewModel.LoadAsync(EventViewModel.CurrentEvent, SessionViewModel.CurrentSession);
        }

        public async Task SaveSolveAsync(Solve solve)
        {
            SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve));
            await SolvesListViewModel.SaveAsync(EventViewModel.CurrentEvent, SessionViewModel.CurrentSession);
            ScrambleViewModel.NextScramble();
        }
    }
}
