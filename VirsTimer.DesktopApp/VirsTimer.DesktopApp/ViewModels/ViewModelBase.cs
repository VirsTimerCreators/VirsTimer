using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        [Reactive]
        public bool IsBusy { get; set; }

        [Reactive]
        public bool IsResponseUnsccesfull { get; set; }

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
