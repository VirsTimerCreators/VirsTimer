using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomUsersViewModel : ViewModelBase
    {
        [Reactive]
        public ObservableCollection<RoomUserViewModel> Users { get; set; }

        public RoomUsersViewModel(IEnumerable<RoomUserViewModel> roomUsers)
        {
            Users = new(roomUsers);
        }
    }
}