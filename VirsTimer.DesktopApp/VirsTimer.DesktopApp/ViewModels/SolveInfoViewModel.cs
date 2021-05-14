using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolveInfoViewModel : ViewModelBase
    {
        public Solve Solve { get; }
        public SolveFlagsViewModel SolveFlagsViewModel { get; }

        public SolveInfoViewModel(Solve solve)
        {
            Solve = solve;
            SolveFlagsViewModel = new SolveFlagsViewModel(solve);
        }
    }
}
