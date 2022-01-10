using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.Views.Common
{
    public class BaseWindow<T> : ReactiveWindow<T> where T : ViewModelBase
    {
        public void Activate(CompositeDisposable disposableRegistration)
        {
            ViewModel.ShowCloseWindowDialog.RegisterHandler(DoShowCloseWindowBoxDialogAsync).DisposeWith(disposableRegistration);
            ViewModel.ShowShutdownDialog.RegisterHandler(DoShowShutdownBoxDialogAsync).DisposeWith(disposableRegistration);
        }

        private async Task DoShowCloseWindowBoxDialogAsync(InteractionContext<CloseWindowBoxViewModel, Unit> interaction)
        {
            var dialog = new CloseWindowBox
            {
                DataContext = interaction.Input
            };

            await dialog.ShowDialog(this);
            interaction.SetOutput(Unit.Default);
            Close();
        }

        private async Task DoShowShutdownBoxDialogAsync(InteractionContext<ShutdownBoxViewModel, Unit> interaction)
        {
            var dialog = new ShutdownBox
            {
                DataContext = interaction.Input
            };

            await dialog.ShowDialog(this);
            interaction.SetOutput(Unit.Default);
        }
    }
}