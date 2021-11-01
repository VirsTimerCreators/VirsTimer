using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Services.Register;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class RegisterViewModel : ViewModelBase
    {
        private readonly IRegisterRepository _registerRepository1;
        //private static readonly Regex EmailRegex = new Regex("");

        [Reactive]
        public string Login { get; set; } = string.Empty;

        [Reactive]
        public string Email { get; set; } = string.Empty;

        [Reactive]
        public string Password { get; set; } = string.Empty;

        [Reactive]
        public string RepeatedPassword { get; set; } = string.Empty;

        [ObservableAsProperty]
        public bool EmailOk { get; set; }

        public RegisterViewModel(IRegisterRepository registerRepository)
        {
            _registerRepository1 = registerRepository;
        }
    }
}
