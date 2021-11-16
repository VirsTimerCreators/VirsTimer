using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Common;
using VirsTimer.DesktopApp.Views.Common;

namespace VirsTimer.DesktopApp.Views
{
    public partial class RegisterView : ReactiveWindow<RegisterViewModel>
    {
        public ProgressBar ProgressBar { get; }
        public TextBox LoginTextBox { get; }
        public TextBox EmailTextBox { get; }
        public TextBox PasswordTextBox { get; }
        public TextBox RepeatPasswordTextBox { get; }
        public TextBlock LoginAlertTextBlock { get; }
        public TextBlock EmailAlertTextBlock { get; }
        public TextBlock PasswordAlertTextBlock { get; }
        public TextBlock PasswordNotSameAlertTextBlock { get; }
        public Button AcceptButton { get; }

        public RegisterView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            ProgressBar = this.FindControl<ProgressBar>("ProgressBar");

            LoginTextBox = this.FindControl<TextBox>("LoginTextBox");
            EmailTextBox = this.FindControl<TextBox>("EmailTextBox");
            PasswordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            RepeatPasswordTextBox = this.FindControl<TextBox>("RepeatPasswordTextBox");

            LoginAlertTextBlock = this.FindControl<TextBlock>("LoginAlertTextBlock");
            EmailAlertTextBlock = this.FindControl<TextBlock>("EmailAlertTextBlock");
            PasswordAlertTextBlock = this.FindControl<TextBlock>("PasswordAlertTextBlock");
            PasswordNotSameAlertTextBlock = this.FindControl<TextBlock>("PasswordNotSameAlertTextBlock");

            AcceptButton = this.FindControl<Button>("AcceptButton");

            PasswordTextBox.PasswordChar = '\u2022';
            RepeatPasswordTextBox.PasswordChar = '\u2022';

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.IsBusy,
                    view => view.ProgressBar.IsVisible)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Login,
                    view => view.LoginTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Email,
                    view => view.EmailTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Password,
                    view => view.PasswordTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.RepeatedPassword,
                    view => view.RepeatPasswordTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.LoginOk,
                    view => view.LoginAlertTextBlock.IsVisible,
                    value => value is false)
                .DisposeWith(disposableRegistration);

                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.EmailOk,
                    view => view.EmailAlertTextBlock.IsVisible,
                    value => value is false)
                .DisposeWith(disposableRegistration);

                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.PasswordOk,
                    view => view.PasswordAlertTextBlock.IsVisible,
                    value => value is false)
                .DisposeWith(disposableRegistration);

                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.PasswordsAreSame,
                    view => view.PasswordNotSameAlertTextBlock.IsVisible,
                    value => value is false)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.AcceptRegisterCommand,
                    view => view.AcceptButton)
                .DisposeWith(disposableRegistration);

                ViewModel.ShowInfoBoxDialog.RegisterHandler(DoShowInfoBoxAsync).DisposeWith(disposableRegistration);
                ViewModel!.AcceptRegisterCommand.Subscribe(logged =>
                {
                    if (logged)
                        Close();
                })
                .DisposeWith(disposableRegistration);


            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task DoShowInfoBoxAsync(InteractionContext<InfoBoxViewModel, Unit> interaction)
        {
            var dialog = new InfoBox
            {
                DataContext = interaction.Input
            };

            var output = await dialog.ShowDialog<Unit>(this);
            interaction.SetOutput(output);
        }
    }
}