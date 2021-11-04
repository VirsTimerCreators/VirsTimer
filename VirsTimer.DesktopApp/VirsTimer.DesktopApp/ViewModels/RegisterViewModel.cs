using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Models.Requests;
using VirsTimer.Core.Models.Responses;
using VirsTimer.Core.Services.Register;
using VirsTimer.Core.Utils;
using VirsTimer.DesktopApp.ViewModels.Common;
using VirsTimer.DesktopApp.Views.Common;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IRegisterRepository _registerRepository;
        private static readonly Regex LoginRegex = new Regex(@"[\p{L}.-]{3,}");

        [Reactive]
        public RepositoryResponseStatus? RegisterStatus { get; set; }

        [Reactive]
        public string? RegisterMessage { get; set; }

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

        public ReactiveCommand<Window, Unit> AcceptRegisterCommand { get; }

        public RegisterViewModel(IRegisterRepository registerRepository)
        {
            Login = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
            RepeatedPassword = string.Empty;

            _registerRepository = registerRepository;

            this.WhenAnyValue(x => x.Login)
                .Skip(1)
                .Select(x => LoginRegex.IsMatch(x))
                .ToPropertyEx(this, x => x.LoginOk, scheduler: RxApp.MainThreadScheduler);

            this.WhenAnyValue(x => x.Email)
                .Skip(1)
                .Select(x => x.IsValidEmail())
                .ToPropertyEx(this, x => x.EmailOk, scheduler: RxApp.MainThreadScheduler);

            this.WhenAnyValue(x => x.Password, x => x.Length > 4)
                .Skip(1)
                .ToPropertyEx(this, x => x.PasswordOk);

            this.WhenAnyValue(x => x.Password, x => x.RepeatedPassword, (x, y) => x == y)
                .ToPropertyEx(this, x => x.PasswordsAreSame);

            var acceptRegisterEnabled = this.WhenAnyValue(
                x => x.LoginOk,
                x => x.EmailOk,
                x => x.PasswordOk,
                x => x.PasswordsAreSame,
                (x, y, z, w) => x && y && z && w);

            AcceptRegisterCommand = ReactiveCommand.CreateFromTask<Window>(AcceptRegisterAsync, acceptRegisterEnabled);
        }

        private async Task AcceptRegisterAsync(Window parent)
        {
            IsBusy = true;

            var registerRequest = new RegisterRequest
            {
                Username = Login,
                Email = Email,
                Password = Password
            };
            var response = await _registerRepository.RegisterAsync(registerRequest).ConfigureAwait(true);
            RegisterStatus = response.Status;
            RegisterMessage = response.Message;
            if (response.Succesfull)
            {
                var infoBox = new InfoBox
                {
                    ViewModel = new InfoBoxViewModel { Message = $"Rejestracja konta {Login} ukończyła się powodzeniem." }
                };
                await infoBox.ShowDialog(parent).ConfigureAwait(true);
                parent.Close();
            }

            RegisterStatus = response.Status;
            ShowUnsuccesfullControlAsync();
            IsBusy = false;
        }
    }
}