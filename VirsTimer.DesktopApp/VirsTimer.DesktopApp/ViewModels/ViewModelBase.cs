using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Threading.Tasks;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        [Reactive]
        public bool IsBusy { get; set; } = false;

        public virtual Task ConstructAsync()
        {
            return Task.CompletedTask;
        }
    }
}
