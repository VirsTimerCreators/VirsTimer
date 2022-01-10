using Avalonia.Controls.ApplicationLifetimes;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;

namespace VirsTimer.DesktopApp.ViewModels.Common
{
    public class ShutdownBoxViewModel : ViewModelBase
    {
        [Reactive]
        public string Message { get; set; }

        public ReactiveCommand<Unit, Unit> ShutdownCommand { get; }

        public ShutdownBoxViewModel()
        {
            Message = string.Empty;
            ShutdownCommand = ReactiveCommand.Create(() =>
            {
                (App.Current.ApplicationLifetime as IClassicDesktopStyleApplicationLifetime)?.Shutdown();
            });
        }
    }
}