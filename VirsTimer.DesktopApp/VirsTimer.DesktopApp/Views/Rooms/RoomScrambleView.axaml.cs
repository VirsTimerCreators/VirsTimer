using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using ReactiveUI;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels.Rooms;

namespace VirsTimer.DesktopApp.Views.Rooms
{
    public partial class RoomScrambleView : ReactiveUserControl<RoomScrambleViewModel>
    {
        private Window? _ancestor;
        public TextBlock ScrambleTextBlock { get; }

        public RoomScrambleView()
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
                >= ScreenHeight.Big => 46,
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => 34,
                (< ScreenHeight.Medium) => 24,
                _ => 24
            };

            ScrambleTextBlock.Padding = windowHeight switch
            {
                >= ScreenHeight.Big => new Thickness(30),
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => new Thickness(25),
                (< ScreenHeight.Medium) => new Thickness(15),
                _ => new Thickness(15)
            };
        }
    }
}