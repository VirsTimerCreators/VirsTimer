using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Services.Login;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Common;
using VirsTimer.DesktopApp.Views;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IExplicitValueConverter<RepositoryResponseStatus, string> _loginStatusConverter;

        public SnackbarViewModel SnackbarViewModel { get; }

        [Reactive]
        public string LoginName { get; set; } = string.Empty;

        [Reactive]
        public string LoginPassowd { get; set; } = string.Empty;

        public ReactiveCommand<Window, Unit> RegisterCommand { get; }

        public ReactiveCommand<Window, Unit> AcceptLoginCommand { get; }

        public ReactiveCommand<Window, Unit> ContinueLocalCommand { get; }

        public LoginViewModel(
            ILoginRepository? loginRepository = null,
            IExplicitValueConverter<RepositoryResponseStatus, string>? repositoryResponseValueConverter = null)
        {
            _loginRepository = loginRepository ?? Ioc.GetService<ILoginRepository>();
            _loginStatusConverter = repositoryResponseValueConverter ?? new LoginStatusConverter();

            SnackbarViewModel = new SnackbarViewModel { Message = String.Empty };

            var acceptLoginEnabled = this.WhenAnyValue(
                x => x.LoginName,
                x => x.LoginPassowd,
                (name, password) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(password));

            RegisterCommand = ReactiveCommand.CreateFromTask<Window>(Register);
            AcceptLoginCommand = ReactiveCommand.CreateFromTask<Window>(AcceptLoginAsync, acceptLoginEnabled);
            ContinueLocalCommand = ReactiveCommand.CreateFromTask<Window>(ContinueLocal);
        }

        private async Task Register(Window parent)
        {
            var registerWindow = new RegisterView();
            await registerWindow.ShowDialog(parent).ConfigureAwait(true);
        }

        private async Task AcceptLoginAsync(Window parent)
        {
            IsBusy = true;

            var request = new LoginRequest(LoginName, LoginPassowd);

            var response = await _loginRepository.LoginAsync(request).ConfigureAwait(true);
            if (response.IsSuccesfull)
            {
                SnackbarViewModel.Disposed = true;
                await Ioc.AddApplicationCacheAsync(serverSide: true);
                Ioc.ConfigureServerServices(response.Value);
                var mainWinow = new MainWindow();
                mainWinow.Show();
                parent.Close();
            }

            var message = _loginStatusConverter.Convert(response.Status);
            SnackbarViewModel.QueueMessage.Execute(message).Subscribe();
            ShowUnsuccesfullControlAsync();
            IsBusy = false;
        }

        private async Task ContinueLocal(Window parent)
        {
            await Ioc.AddApplicationCacheAsync();
            Ioc.ConfigureLocalServices();
            var mainWinow = new MainWindow();
            mainWinow.Show();
            parent.Close();
        }
    }
}