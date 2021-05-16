using ReactiveUI;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        private Event _currentEvent = new("3x3x3");
        public Event CurrentEvent
        {
            get => _currentEvent;
            set => this.RaiseAndSetIfChanged(ref _currentEvent, value);
        }
    }
}
