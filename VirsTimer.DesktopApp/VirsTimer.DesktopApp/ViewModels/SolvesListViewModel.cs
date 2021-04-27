using System.Collections.Generic;
using System.Collections.ObjectModel;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolvesListViewModel : ViewModelBase
    {
        public ObservableCollection<Solve> Solves { get; }

        public SolvesListViewModel(IReadOnlyList<Solve> solves)
        {
            Solves = new ObservableCollection<Solve>(solves);
        }
    }
}
