using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using VirsTimer.Core.Services.Rooms;
using VirsTimer.DesktopApp.ViewModels.Scrambles;
using VirsTimer.Scrambles;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomViewModel : ViewModelBase
    {
        public string AccessCode { get; }

        public ScrambleViewModel ScrambleViewModel { get; }

        public TimerViewModel TimerViewModel { get; }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; }

        [Reactive]
        public RoomUsersViewModel RoomUsersViewModel { get; set; }

        public RoomViewModel(
            string accessCode,
            bool isAdmin,
            IEnumerable<Scramble> scrambles,
            IRoomsService roomsService)
        {
            //roomsService.Notifications.Select(notification =>
            //{
            //    return new object();
            //});
            //RoomUsersViewModel = new RoomUsersViewModel();
        }
    }
}
