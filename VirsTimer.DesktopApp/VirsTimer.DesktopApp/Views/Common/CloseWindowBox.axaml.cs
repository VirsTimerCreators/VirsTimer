using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.Views.Common
{
    public partial class CloseWindowBox : ReactiveWindow<CloseWindowBoxViewModel>
    {
        public TextBlock MessageTextBlock { get; }
        public Button CloseWindowButton { get; }

        public CloseWindowBox()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            MessageTextBlock = this.FindControl<TextBlock>("MessageTextBox");
            CloseWindowButton = this.FindControl<Button>("CloseWindowButton");

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.Message,
                    view => view.MessageTextBlock.Text)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.CloseWindowCommand,
                    view => view.CloseWindowButton)
                .DisposeWith(disposableRegistration);

                ViewModel!.CloseWindowCommand.Subscribe(_ => Close()).DisposeWith(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}