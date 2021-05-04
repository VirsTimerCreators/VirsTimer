using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public class EventChangeView : ReactiveWindow<EventChangeViewModel>
    {
        public EventChangeView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
