using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using VirsTimer.Core.Constants;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolveFlagsViewModel : ViewModelBase
    {
        public ObservableCollection<bool> FlagsArray { get; }

        [ObservableAsProperty]
        public SolveFlag ChoosenFlag { get; set; }

        public SolveFlagsViewModel(SolveFlag initilaFlag)
        {
            var flags = Enum.GetValues<SolveFlag>().Select(flag => flag == initilaFlag);
            FlagsArray = new ObservableCollection<bool>(flags);

            FlagsArray
                .ToObservableChangeSet()
                .ToCollection()
                .Select(x => (SolveFlag)x.IndexOf(true))
                .ToPropertyEx(this, x => x.ChoosenFlag);
        }
    }
}