using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace VirsTimer.DesktopApp.ViewModels.Common
{
    public class CloseWindowBoxViewModel : ViewModelBase
    {
        [Reactive]
        public string Message { get; set; }

        public ReactiveCommand<Unit, Unit> CloseWindowCommand { get; }

        public CloseWindowBoxViewModel()
        {
            Message = string.Empty;
            CloseWindowCommand = ReactiveCommand.Create(() => { });
        }
    }
}