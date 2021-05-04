using ReactiveUI;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        private Event currentEvent = new("3x3x3");
        public Event CurrentEvent
        {
            get => currentEvent;
            set => this.RaiseAndSetIfChanged(ref currentEvent, value);
        }
    }
}
