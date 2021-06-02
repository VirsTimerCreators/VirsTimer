using Avalonia;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VirsTimer.DesktopApp.ViewModels.Solves;

namespace VirsTimer.DesktopApp.Views.Solves
{
    public partial class SolveView : ReactiveWindow<SolveViewModel>
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
