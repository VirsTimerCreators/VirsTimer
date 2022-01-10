using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.Views.Common
{
    public partial class ShutdownBox : ReactiveWindow<ShutdownBoxViewModel>
    {
        public TextBlock MessageTextBlock { get; }
        public Button ShutdownButton { get; }

        public ShutdownBox()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            MessageTextBlock = this.FindControl<TextBlock>("MessageTextBox");
            ShutdownButton = this.FindControl<Button>("ShutdownButton");

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.Message,
                    view => view.MessageTextBlock.Text)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.ShutdownCommand,
                    view => view.ShutdownButton)
                .DisposeWith(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}