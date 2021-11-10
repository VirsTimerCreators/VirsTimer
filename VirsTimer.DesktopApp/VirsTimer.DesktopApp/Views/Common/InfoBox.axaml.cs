using System;
using System.Reactive.Disposables;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.Views.Common
{
    public partial class InfoBox : ReactiveWindow<InfoBoxViewModel>
    {
        public TextBlock MessageTextBlock { get; }
        public Button OkButton { get; }

        public InfoBox()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            MessageTextBlock = this.FindControl<TextBlock>("MessageTextBox");
            OkButton = this.FindControl<Button>("OkButton");

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.Message,
                    view => view.MessageTextBlock.Text)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.OkCommand,
                    view => view.OkButton)
                .DisposeWith(disposableRegistration);

                ViewModel!.OkCommand.Subscribe(_ => Close()).DisposeWith(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}