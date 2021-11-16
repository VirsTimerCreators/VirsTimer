using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Services.Register;
using VirsTimer.Core.Utils;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private static readonly Regex LoginRegex = new(@"[\p{L}0-9.-]{3,}");

        private readonly IRegisterRepository _registerRepository;
        private readonly IValueConverter<RepositoryResponse, string> _repositoryResponseValueConverter;

        public SnackbarViewModel SnackbarViewModel { get; }

        [Reactive]
        public string Login { get; set; }

        [Reactive]
        public string Email { get; set; }

        [Reactive]
        public string Password { get; set; }

        [Reactive]
        public string RepeatedPassword { get; set; }

        [ObservableAsProperty]
        public bool LoginOk { get; set; }

        [ObservableAsProperty]
        public bool EmailOk { get; set; }

        [ObservableAsProperty]
        public bool PasswordOk { get; set; }

        [ObservableAsProperty]
        public bool PasswordsAreSame { get; set; }

        public ReactiveCommand<Unit, bool> AcceptRegisterCommand { get; }

        public Interaction<InfoBoxViewModel, Unit> ShowInfoBoxDialog { get; }

        public RegisterViewModel(
            IRegisterRepository? registerRepository = null,
            IValueConverter<RepositoryResponse, string>? repositoryResponseValueConverter = null)
        {
            Login = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            RepeatedPassword = string.Empty;

            _registerRepository = registerRepository ?? Ioc.GetService<IRegisterRepository>();
            _repositoryResponseValueConverter = repositoryResponseValueConverter ?? new RegisterStatusConverter();
            SnackbarViewModel = new SnackbarViewModel();

            this.WhenAnyValue(x => x.Login)
                .Select(x => LoginRegex.IsMatch(x) && x.Count(c => char.IsLetter(c)) >= 3)
                .Skip(1)
                .Prepend(true)
                .ToPropertyEx(this, x => x.LoginOk, scheduler: RxApp.MainThreadScheduler);

            this.WhenAnyValue(x => x.Email)
                .Select(x => x.IsValidEmail())
                .Skip(1)
                .Prepend(true)
                .ToPropertyEx(this, x => x.EmailOk, scheduler: RxApp.MainThreadScheduler);

            this.WhenAnyValue(x => x.Password, x => x.Length > 4)
                .Skip(1)
                .Prepend(true)
                .ToPropertyEx(this, x => x.PasswordOk);

            this.WhenAnyValue(x => x.Password, x => x.RepeatedPassword, (x, y) => x == y)
                .ToPropertyEx(this, x => x.PasswordsAreSame);

            var acceptRegisterEnabled = this.WhenAnyValue(
                x => x.Login,
                x => x.Email,
                x => x.Password,
                x => x.RepeatedPassword,
                x => x.LoginOk,
                x => x.EmailOk,
                x => x.PasswordOk,
                x => x.PasswordsAreSame,
                (login, email, password, repeatedPassword, ok1, ok2, ok3, ok4) =>
                    string.IsNullOrWhiteSpace(login) is false
                    && string.IsNullOrWhiteSpace(email) is false
                    && string.IsNullOrWhiteSpace(password) is false
                    && string.IsNullOrWhiteSpace(repeatedPassword) is false
                    && ok1 && ok2 && ok3 && ok4);

            AcceptRegisterCommand = ReactiveCommand.CreateFromTask(AcceptRegisterAsync, acceptRegisterEnabled);
            ShowInfoBoxDialog = new Interaction<InfoBoxViewModel, Unit>();
        }

        private async Task<bool> AcceptRegisterAsync()
        {
            IsBusy = true;

            var registerRequest = new RegisterRequest
            {
                Username = Login,
                Email = Email,
                Password = Password
            };
            var response = await _registerRepository.RegisterAsync(registerRequest).ConfigureAwait(true);
            IsBusy = false;

            if (response.IsSuccesfull)
            {
                var infoBoxViewModel = new InfoBoxViewModel
                {
                    Message = $"Rejestracja konta {Login} ukończyła się powodzeniem."
                };
                await ShowInfoBoxDialog.Handle(infoBoxViewModel);

                return true;
            }

            var message = _repositoryResponseValueConverter.Convert(response);
            await SnackbarViewModel.QueueMessage.Execute(message);

            return false;
        }
    }
}