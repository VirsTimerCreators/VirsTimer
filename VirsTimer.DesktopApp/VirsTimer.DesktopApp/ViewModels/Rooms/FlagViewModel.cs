using ReactiveUI.Fody.Helpers;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class FlagViewModel : ViewModelBase
    {
        [Reactive]
        public bool Choosen { get; set; }
    }
}