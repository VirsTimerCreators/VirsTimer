using System;
using System.Collections.ObjectModel;
using System.Linq;
using VirsTimer.Core.Constants;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolveFlagsViewModel : ViewModelBase
    {
        public ObservableCollection<bool> FlagsArray { get; }
        public SolveFlag ChoosenFlag { get; set; }

        public SolveFlagsViewModel(SolveFlag initilaFlag)
        {
            var flags = Enum.GetValues<SolveFlag>().Select(flag => flag == initilaFlag);
            FlagsArray = new ObservableCollection<bool>(flags);
            FlagsArray.CollectionChanged += FlagChanged;
            ChoosenFlag = initilaFlag;
        }

        public void FlagChanged(object? sender, EventArgs e)
        {
            ChoosenFlag = (SolveFlag)FlagsArray.IndexOf(true);
        }
    }
}