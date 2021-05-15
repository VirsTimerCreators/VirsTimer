using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public partial class SolveFlagsView : UserControl
    {
        public SolveFlagsView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
