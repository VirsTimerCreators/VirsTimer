using ReactiveUI;
using System.Threading.Tasks;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        public virtual Task ConstructAsync()
        {
            return Task.CompletedTask;
        }
    }
}
