using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirsTimer.Core.Constants;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolveFlagsViewModel : ViewModelBase
    {
        private readonly ObservableCollection<bool> flagsArray;

        public ObservableCollection<bool> FlagsArray => flagsArray;

        public SolveFlag ChoosenFlag { get; set; }

        public SolveFlagsViewModel(SolveFlag initilaFlag)
        {
            var flags = Enum.GetValues<SolveFlag>().Select(flag => flag == initilaFlag);
            flagsArray = new ObservableCollection<bool>(flags);
            flagsArray.CollectionChanged += FlagChanged;
            ChoosenFlag = initilaFlag;
        }

        public void FlagChanged(object? sender, EventArgs e)
        {
            ChoosenFlag = (SolveFlag)FlagsArray.IndexOf(true);
        }
    }
}
