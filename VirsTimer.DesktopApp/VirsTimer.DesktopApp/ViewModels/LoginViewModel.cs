using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Services.Login;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IValueConverter<RepositoryResponseStatus, string> _loginStatusConverter;

        public SnackbarViewModel SnackbarViewModel { get; }

        [Reactive]
        public string LoginName { get; set; } = string.Empty;

        [Reactive]
        public string LoginPassowd { get; set; } = string.Empty;

        public ReactiveCommand<Unit, Unit> RegisterCommand { get; }

        public ReactiveCommand<Unit, bool> AcceptLoginCommand { get; }

        public ReactiveCommand<Unit, Unit> ContinueLocalCommand { get; }

        public Interaction<RegisterViewModel, Unit> ShowRegisterDialog { get; }

        public Interaction<MainWindowViewModel, Unit> ShowMainWindowDialog { get; }

        public LoginViewModel(
            ILoginRepository? loginRepository = null,
            IValueConverter<RepositoryResponseStatus, string>? repositoryResponseValueConverter = null)
        {
            _loginRepository = loginRepository ?? Ioc.GetService<ILoginRepository>();
            _loginStatusConverter = repositoryResponseValueConverter ?? new LoginStatusConverter();

            SnackbarViewModel = new SnackbarViewModel { Message = String.Empty };

            var acceptLoginEnabled = this.WhenAnyValue(
                x => x.LoginName,
                x => x.LoginPassowd,
                (name, password) => !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(password));

            RegisterCommand = ReactiveCommand.CreateFromTask(Register);
            AcceptLoginCommand = ReactiveCommand.CreateFromTask(AcceptLoginAsync, acceptLoginEnabled);
            ContinueLocalCommand = ReactiveCommand.CreateFromTask(ContinueLocalAsync);

            ShowRegisterDialog = new Interaction<RegisterViewModel, Unit>();
            ShowMainWindowDialog = new Interaction<MainWindowViewModel, Unit>();
        }

        private async Task Register()
        {
            var registerViewModel = new RegisterViewModel();
            await ShowRegisterDialog.Handle(registerViewModel);
        }

        private async Task<bool> AcceptLoginAsync()
        {
            IsBusy = true;

            var request = new LoginRequest(LoginName, LoginPassowd);

            var response = await _loginRepository.LoginAsync(request).ConfigureAwait(true);
            if (response.IsSuccesfull)
            {
                SnackbarViewModel.Disposed = true;

                await Ioc.AddApplicationCacheAsync(serverSide: true);
                Ioc.ConfigureServerServices(response.Value);

                var mainWindowViewModel = new MainWindowViewModel(online: true);
                await ShowMainWindowDialog.Handle(mainWindowViewModel);

                return true;
            }

            var message = _loginStatusConverter.Convert(response.Status);
            await SnackbarViewModel.QueueMessage.Execute(message);
            IsBusy = false;

            return false;
        }

        private async Task ContinueLocalAsync()
        {
            await Ioc.AddApplicationCacheAsync();
            Ioc.ConfigureLocalServices();

            var mainWindowViewModel = new MainWindowViewModel(online: false);
            await ShowMainWindowDialog.Handle(mainWindowViewModel);
        }
    }
}