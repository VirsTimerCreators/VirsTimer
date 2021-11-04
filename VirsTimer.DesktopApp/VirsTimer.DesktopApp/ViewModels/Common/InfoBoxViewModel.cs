using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace VirsTimer.DesktopApp.ViewModels.Common
{
    public class InfoBoxViewModel : ViewModelBase
    {
        [Reactive]
        public string Message { get; set; }

        public ReactiveCommand<Unit, Unit> OkCommand { get; }

        public InfoBoxViewModel()
        {
            Message = string.Empty;
            OkCommand = ReactiveCommand.Create(() => { });
        }
    }
}
