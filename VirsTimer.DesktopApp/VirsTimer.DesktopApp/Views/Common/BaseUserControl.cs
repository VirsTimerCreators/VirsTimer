using Avalonia.Controls;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.Views.Common
{
    public class BaseUserControl<T> : ReactiveUserControl<T> where T : ViewModelBase
    {
        protected Window? WindowParent { get; private set; }

        protected void Activate(CompositeDisposable disposableRegistration)
        {
            WindowParent = this.FindAncestorOfType<Window>();
            ViewModel.ShowCloseWindowDialog.RegisterHandler(DoShowCloseWindowBoxDialogAsync).DisposeWith(disposableRegistration);
            ViewModel.ShowShutdownDialog.RegisterHandler(DoShowShutdownBoxDialogAsync).DisposeWith(disposableRegistration);
        }

        private async Task DoShowCloseWindowBoxDialogAsync(InteractionContext<CloseWindowBoxViewModel, Unit> interaction)
        {
            var dialog = new CloseWindowBox
            {
                DataContext = interaction.Input
            };

            await dialog.ShowDialog(WindowParent);
            interaction.SetOutput(Unit.Default);
        }

        private async Task DoShowShutdownBoxDialogAsync(InteractionContext<ShutdownBoxViewModel, Unit> interaction)
        {
            var dialog = new ShutdownBox
            {
                DataContext = interaction.Input
            };

            await dialog.ShowDialog(WindowParent);
            interaction.SetOutput(Unit.Default);
        }
    }
}