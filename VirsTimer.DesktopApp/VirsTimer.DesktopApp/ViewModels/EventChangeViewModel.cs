using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class EventChangeViewModel : ViewModelBase
    {
        public ObservableCollection<Event> Events { get; }

        public EventChangeViewModel()
        {
            Events = new ObservableCollection<Event>(new[] { new Event("3x3x3") });
        }
    }
}
