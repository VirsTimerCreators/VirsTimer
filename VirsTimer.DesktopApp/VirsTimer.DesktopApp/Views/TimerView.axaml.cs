using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using ReactiveUI;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public class TimerView : ReactiveUserControl<TimerViewModel>
    {
        private Window? _ancestor;
        public TextBlock TimerTextBlock { get; }

        public TimerView()
        {
            InitializeComponent();

            TimerTextBlock = this.FindControl<TextBlock>("TimerTextBlock");

            this.WhenActivated(disposableRegistration =>
            {
                _ancestor = this.FindAncestorOfType<Window>();
                _ancestor.Opened += (_, _) => OnOpen();

                if (_ancestor is not null && _ancestor.IsActive)
                    OnOpen();
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOpen()
        {
            var windowHeight = _ancestor!.Height;
            TimerTextBlock.FontSize = windowHeight switch
            {
                >= ScreenHeight.Big => 144,
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => 122,
                (< ScreenHeight.Medium) => 92,
                _ => 92
            };
        }
    }
}
