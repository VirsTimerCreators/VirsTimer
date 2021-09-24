using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Diagnostics;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Login;
using VirsTimer.DesktopApp.Views;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginRepository _loginRepository;

        [Reactive]
        public bool IsResponseUnsccesfull { get; set; } = false;

        [Reactive]
        public string LoginName { get; set; } = string.Empty;

        [Reactive]
        public string LoginPassowd { get; set; } = string.Empty;

        [Reactive]
        public RepositoryResponseStatus? LoginStaus { get; set; }

        public ReactiveCommand<Window, Unit> RegisterCommand { get; }

        public ReactiveCommand<Window, Unit> AcceptLoginCommand { get; }

        public ReactiveCommand<Window, Unit> ContinueLocalCommand { get; }

        public LoginViewModel(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;

            var acceptLoginEnabled = this.WhenAnyValue(
                x => x.LoginName,
                x => x.LoginPassowd,
                (name, password) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(password));

            RegisterCommand = ReactiveCommand.Create<Window>(Register);
            AcceptLoginCommand = ReactiveCommand.CreateFromTask<Window>(AcceptLoginAsync, acceptLoginEnabled);
            ContinueLocalCommand = ReactiveCommand.Create<Window>(ContinueLocal);
        }

        private void Register(Window parent)
        {
            //Todo fill
            Trace.WriteLine("Register clicked");
        }

        private async Task AcceptLoginAsync(Window parent)
        {
            IsBusy = true;

            var request = new LoginRequest(LoginName, LoginPassowd);

            var response = await _loginRepository.LoginAsync(request).ConfigureAwait(true);
            if (response.Succesfull)
            {
                Ioc.ConfigureServerServices(response.Value);
                var mainWinow = new MainWindow();
                mainWinow.Show();
                parent.Close();
            }

            LoginStaus = response.Status;
            ShowUnsuccesfullControlAsync();
            IsBusy = false;
        }

        private void ContinueLocal(Window parent)
        {
            Ioc.ConfigureLocalServices();
            var mainWinow = new MainWindow();
            mainWinow.Show();
            parent.Close();
        }

        private async void ShowUnsuccesfullControlAsync()
        {
            if (IsResponseUnsccesfull)
                return;

            IsResponseUnsccesfull = true;
            await Task.Delay(3000).ConfigureAwait(false);
            IsResponseUnsccesfull = false;
        }
    }
}