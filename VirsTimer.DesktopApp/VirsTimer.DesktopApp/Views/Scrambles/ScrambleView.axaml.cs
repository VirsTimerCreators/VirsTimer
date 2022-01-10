using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels.Scrambles;
using VirsTimer.DesktopApp.Views.Common;

namespace VirsTimer.DesktopApp.Views.Scrambles
{
    public class ScrambleView : BaseUserControl<ScrambleViewModel>
    {
        public TextBlock ScrambleTextBlock { get; }

        public ScrambleView()
        {
            InitializeComponent();
            ScrambleTextBlock = this.FindControl<TextBlock>("ScrambleTextBlock");
            this.WhenActivated(disposableRegistration =>
            {
                Activate(disposableRegistration);
                WindowParent.Opened += (_, _) => OnOpen();
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOpen()
        {
            var windowHeight = WindowParent!.Height;
            ScrambleTextBlock.FontSize = windowHeight switch
            {
                >= ScreenHeight.Big => 42,
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => 32,
                (< ScreenHeight.Medium) => 24,
                _ => 24
            };
        }
    }
}