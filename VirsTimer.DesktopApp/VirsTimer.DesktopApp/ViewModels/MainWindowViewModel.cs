using System;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }

        public MainWindowViewModel()
        {
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel(Array.Empty<Solve>());
        }
    }
}
