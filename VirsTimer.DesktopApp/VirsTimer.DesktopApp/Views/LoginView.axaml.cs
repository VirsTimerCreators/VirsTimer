using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels;
using System;

namespace VirsTimer.DesktopApp.Views
{
    public partial class LoginView : ReactiveWindow<LoginViewModel>
    {
        public ProgressBar ProgressBar { get; }
        public TextBox LoginTextBox { get; }
        public TextBox PasswordTextBox { get; }
        public Button RegisterButton { get; }
        public Button ContinueLocalButton { get; }
        public Button LoginButton { get; }

        public LoginView()
        {
            ViewModel = new LoginViewModel();
            DataContext = ViewModel;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            ProgressBar = this.FindControl<ProgressBar>("ProgressBar");
            LoginTextBox = this.FindControl<TextBox>("LoginTextBox");
            PasswordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            RegisterButton = this.FindControl<Button>("RegisterButton");
            ContinueLocalButton = this.FindControl<Button>("ContinueLocalButton");
            LoginButton = this.FindControl<Button>("LoginButton");

            PasswordTextBox.PasswordChar = '\u2022';

            this.WhenActivated(disposableRegistration =>
            {
                this.WhenAnyValue(x => x.ViewModel!.IsBusy)
                    .BindTo(this, view => view.ProgressBar.IsVisible)
                    .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.LoginName,
                    view => view.LoginTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.LoginPassowd,
                    view => view.PasswordTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.RegisterCommand,
                    view => view.RegisterButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.ContinueLocalCommand,
                    view => view.ContinueLocalButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.AcceptLoginCommand,
                    view => view.LoginButton)
                .DisposeWith(disposableRegistration);

                ViewModel.ShowRegisterDialog.RegisterHandler(DoShowRegisterDialogAsync).DisposeWith(disposableRegistration);
                ViewModel.ShowMainWindowDialog.RegisterHandler(DoShowMainWindowDialogAsync).DisposeWith(disposableRegistration);
                ViewModel.ContinueLocalCommand.Subscribe(_ => Close()).DisposeWith(disposableRegistration);
                ViewModel.AcceptLoginCommand.Subscribe(logged =>
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

        private async Task DoShowRegisterDialogAsync(InteractionContext<RegisterViewModel, Unit> interaction)
        {
            var dialog = new RegisterView
            {
                DataContext = interaction.Input
            };

            var output = await dialog.ShowDialog<Unit>(this);
            interaction.SetOutput(output);
        }

        private Task DoShowMainWindowDialogAsync(InteractionContext<MainWindowViewModel, Unit> interaction)
        {
            var dialog = new MainWindow
            {
                DataContext = interaction.Input
            };

            dialog.Show();
            interaction.SetOutput(Unit.Default);

            return Task.CompletedTask;
        }
    }
}