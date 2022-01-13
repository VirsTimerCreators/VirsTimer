using Avalonia.Threading;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace VirsTimer.DesktopApp.ViewModels.Common
{
    public class SnackbarViewModel : ViewModelBase
    {
        public int Width { get; }

        public int Height { get; }

        public int Length { get; }

        [Reactive]
        public string Message { get; set; } = string.Empty;

        public ReactiveCommand<string, Unit> QueueMessage { get; }

        public bool Disposed { get; set; }

        public SnackbarViewModel(int width = 300, int height = 64, int length = 3)
        {
            Width = width;
            Height = height;
            Length = length;
            QueueMessage = ReactiveCommand.Create<string>(x => Dispatcher.UIThread.InvokeAsync(() => Message = x));
        }

        public IObservable<Unit> Enqueue(string message)
        {
            return QueueMessage.Execute(message);
        }

        public void EnqueueSchedule(string message)
        {
            RxApp.MainThreadScheduler.Schedule(async () => await QueueMessage.Execute(message));
        }
    }
}