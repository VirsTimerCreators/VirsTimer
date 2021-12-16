using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using ReactiveUI;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels.Statistics;

namespace VirsTimer.DesktopApp.Views.Statistics
{
    public partial class StatisticsView : ReactiveUserControl<StatisticsViewModel>
    {
        private Window? _ancestor;

        public StatisticsView()
        {
            InitializeComponent();
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
            for (var i = 1; i <= 17; i++)
            {
                var textBloxk = this.FindControl<TextBlock>($"TextBlock{i}");
                textBloxk.FontSize = windowHeight switch
                {
                    >= ScreenHeight.Big => 24,
                    (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => 18,
                    (< ScreenHeight.Medium) => 16,
                    _ => 16
                };
            }
        }
    }
}
