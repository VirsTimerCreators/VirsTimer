using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using ReactiveUI;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels.Scrambles;

namespace VirsTimer.DesktopApp.Views.Scrambles
{
    public class ScrambleView : ReactiveUserControl<ScrambleViewModel>
    {
        private Window? _ancestor;
        public TextBlock ScrambleTextBlock { get; }

        public ScrambleView()
        {
            InitializeComponent();
            ScrambleTextBlock = this.FindControl<TextBlock>("ScrambleTextBlock");
            this.WhenActivated(disposableRegistration =>
            {
                _ancestor = this.FindAncestorOfType<Window>();
                _ancestor.Opened += (_, _) => OnOpen();
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOpen()
        {
            var windowHeight = _ancestor!.Height;
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
