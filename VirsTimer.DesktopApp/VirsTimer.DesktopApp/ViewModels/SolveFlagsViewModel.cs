using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolveFlagsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<bool> flagsArray;

        public ObservableCollection<bool> FlagsArray => flagsArray;

        public Solve Solve { get; }

        public SolveFlagsViewModel(Solve solve)
        {
            var flags = Enum.GetValues<SolveFlag>().Select(flag => flag == solve.Flag);
            flagsArray = new ObservableCollection<bool>(flags);
            flagsArray.CollectionChanged += FlagChanged;
            this.Solve = solve;
        }

        public void FlagChanged(object? sender, EventArgs e)
        {
            Solve.Flag = (SolveFlag)FlagsArray.IndexOf(true);
        }
    }
}
