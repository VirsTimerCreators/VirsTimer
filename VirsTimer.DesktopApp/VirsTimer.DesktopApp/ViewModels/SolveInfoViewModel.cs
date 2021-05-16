using Avalonia.Controls;
using ReactiveUI;
using System.Windows.Input;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolveInfoViewModel : ViewModelBase
    {
        public bool Accepted { get; private set; }
        public Solve Solve { get; }
        public SolveFlagsViewModel SolveFlagsViewModel { get; }
        public ICommand AcceptCommand { get; }

        public SolveInfoViewModel(Solve solve)
        {
            Solve = solve;
            SolveFlagsViewModel = new SolveFlagsViewModel(solve.Flag);
            AcceptCommand = ReactiveCommand.Create<Window>(SaveFlag);
        }

        private void SaveFlag(Window window)
        {
            Solve.Flag = SolveFlagsViewModel.ChoosenFlag;
            Accepted = true;
            window.Close();
        }
    }
}
