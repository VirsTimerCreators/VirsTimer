using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ViewModelBase : ReactiveObject, IActivatableViewModel
    {
        [Reactive]
        public bool IsBusy { get; set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public Interaction<CloseWindowBoxViewModel, Unit> ShowCloseWindowDialog { get; }

        public Interaction<ShutdownBoxViewModel, Unit> ShowShutdownDialog { get; }

        public ViewModelBase()
        {
            ShowCloseWindowDialog = new Interaction<CloseWindowBoxViewModel, Unit>();
            ShowShutdownDialog = new Interaction<ShutdownBoxViewModel, Unit>();
            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() => { })
                    .DisposeWith(disposables);
            });
        }

        public virtual Task<bool> ConstructAsync()
        {
            return Task.FromResult(true);
        }

        public async Task CloseWindowDialogHandleAsync(string message)
        {
            var viewModel = new CloseWindowBoxViewModel { Message = message };
            await ShowCloseWindowDialog.Handle(viewModel);
        }

        public async Task ShutdownDialogHandleAsync(string message)
        {
            var viewModel = new ShutdownBoxViewModel { Message = message };
            await ShowShutdownDialog.Handle(viewModel);
        }
    }
}