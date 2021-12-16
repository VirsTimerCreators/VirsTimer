using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
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
            QueueMessage = ReactiveCommand.Create<string>(x => Dispatcher.UIThread.InvokeAsync(() => Message = x));
        }

        public IObservable<Unit> Enqueue(string message)
        {
            return QueueMessage.Execute(message);
        }
    }
}