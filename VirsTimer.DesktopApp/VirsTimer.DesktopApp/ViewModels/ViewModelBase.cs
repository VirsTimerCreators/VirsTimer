using System.Reactive.Disposables;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ViewModelBase : ReactiveObject, IActivatableViewModel
    {
        [Reactive]
        public bool IsBusy { get; set; }

        [Reactive]
        public bool IsResponseUnsccesfull { get; set; }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        public ViewModelBase()
        {
            this.WhenActivated(disposables =>
            {
                Disposable
                    .Create(() => { })
                    .DisposeWith(disposables);
            });
        }

        public virtual Task ConstructAsync()
        {
            return Task.CompletedTask;
        }

        protected async void ShowUnsuccesfullControlAsync()
        {
            if (IsResponseUnsccesfull)
                return;

            IsResponseUnsccesfull = true;
            await Task.Delay(3000).ConfigureAwait(false);
            IsResponseUnsccesfull = false;
        }
    }
}
