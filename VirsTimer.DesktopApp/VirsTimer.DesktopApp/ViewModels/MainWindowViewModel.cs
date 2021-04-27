namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public TimerViewModel TimerViewModel { get; }

        public MainWindowViewModel()
        {
            TimerViewModel = new TimerViewModel();
        }
    }
}
