using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VirsTimer.DesktopApp.Views
{
    public class EventView : UserControl
    {
        public EventView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
