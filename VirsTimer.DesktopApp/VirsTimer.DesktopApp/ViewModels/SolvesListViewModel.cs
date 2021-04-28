using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolvesListViewModel : ViewModelBase
    {
        private readonly IPastSolvesGetter _pastSolvesGetter;
        private readonly ISolvesSaver _solvesSaver;
        public ObservableCollection<Solve> Solves { get; }

        public SolvesListViewModel(IPastSolvesGetter pastSolvesGetter, ISolvesSaver solvesSaver)
        {
            Solves = new ObservableCollection<Solve>(pastSolvesGetter.GetSolvesAsync("3x3", "1").GetAwaiter().GetResult());
            _pastSolvesGetter = pastSolvesGetter;
            _solvesSaver = solvesSaver;
        }

        public void Save()
        {
           _solvesSaver.SaveSolvesAsync(Solves, "3x3", "1").GetAwaiter().GetResult();
        }
    }
}
