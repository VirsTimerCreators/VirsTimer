using System;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public EventViewModel EventViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }

        public MainWindowViewModel(IPastSolvesGetter pastSolvesGetter, ISolvesSaver solvesSaver)
        {
            EventViewModel = new EventViewModel();
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel(pastSolvesGetter, solvesSaver);
        }
    }
}
