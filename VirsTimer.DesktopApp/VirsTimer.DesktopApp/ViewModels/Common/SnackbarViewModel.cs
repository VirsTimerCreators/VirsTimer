using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace VirsTimer.DesktopApp.ViewModels.Common
{
    public class SnackbarViewModel : ViewModelBase
    {
        [Reactive]
        public string Message { get; set; } = string.Empty;

        public ReactiveCommand<string, Unit> QueueMessage { get; }

        public bool Disposed { get; set; }

        public SnackbarViewModel()
        {
            QueueMessage = ReactiveCommand.Create<string>(x => Message = x);
        }
    }
}