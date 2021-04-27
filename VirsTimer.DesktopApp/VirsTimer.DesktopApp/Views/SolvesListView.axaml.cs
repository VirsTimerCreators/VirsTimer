using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VirsTimer.DesktopApp.Views
{
    public class SolvesListView : UserControl
    {
        public SolvesListView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
