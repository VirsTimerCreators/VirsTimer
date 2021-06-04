using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VirsTimer.DesktopApp.Views.Scrambles
{
    public class ScrambleView : UserControl
    {
        public ScrambleView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
