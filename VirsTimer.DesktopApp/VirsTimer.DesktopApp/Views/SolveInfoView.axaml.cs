using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VirsTimer.DesktopApp.Views
{
    public partial class SolveInfoView : Window
    {
        public SolveInfoView()
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
