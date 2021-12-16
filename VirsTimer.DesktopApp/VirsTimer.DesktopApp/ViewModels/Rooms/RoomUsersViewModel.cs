using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomUsersViewModel : ViewModelBase
    {
        [Reactive]
        public ObservableCollection<RoomUserViewModel> Users { get; set; }

        public RoomUsersViewModel()
        {
            Users = new();
        }

        public Task Refresh()
        {
            var tasks = Users.Select(x => x.UpdateIndexesAndStatisticAsync());
            return Task.WhenAll(tasks);
        }
    }
}