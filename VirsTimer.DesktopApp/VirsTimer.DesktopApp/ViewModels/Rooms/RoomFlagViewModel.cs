using ReactiveUI.Fody.Helpers;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomFlagViewModel : ViewModelBase
    {
        [Reactive]
        public bool Choosen { get; set; }
    }
}