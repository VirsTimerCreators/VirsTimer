using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace VirsTimer.DesktopApp.Views.Solves
{
    public partial class SolveView : Window
    {
        public SolveView()
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
