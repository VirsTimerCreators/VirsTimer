using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models.Authorization;
using VirsTimer.Core.Services.Rooms;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomCreationViewModel : ViewModelBase
    {
        private readonly IUserClient _userClient;
        private readonly IRoomsService _roomsService;

        public ReactiveCommand<Unit, RoomViewModel?> CreateRoomCommand { get; }
        public IReadOnlyList<string> AllEvents { get; set; }

        [Reactive]
        public string? SelectedEvent { get; set; }

        public ReactiveCommand<Unit, RoomViewModel?> JoinRoomCommand { get; }

        [Reactive]
        public string AccessCode { get; set; }

        public SnackbarViewModel SnackbarViewModel { get; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public RoomCreationViewModel(
            IUserClient? userClient = null,
            IRoomsService? roomsService = null)
        {
            _userClient = userClient ?? Ioc.GetService<IUserClient>();
            _roomsService = roomsService ?? Ioc.GetService<IRoomsService>();
            AllEvents = Core.Constants.Events.Predefined;
            CreateRoomCommand = ReactiveCommand.CreateFromTask(CreateRoomAsync);
            JoinRoomCommand = ReactiveCommand.CreateFromTask(JoinRoomAsync);
            CancelCommand = ReactiveCommand.Create(() => { });
            AccessCode = "";
            SnackbarViewModel = new SnackbarViewModel();
        }

        public async Task<RoomViewModel?> CreateRoomAsync()
        {
            if (SelectedEvent is null)
            {
                await SnackbarViewModel.Enqueue("Wybierz event.");
                return null;
            }

            var room = await _roomsService.CreateRoomAsync(SelectedEvent!);

            SnackbarViewModel.Disposed = true;
            return new RoomViewModel(
                "A4xg629p1Q",
                true,
                _userClient,
                room.Scrambles,
                _roomsService);
        }

        public async Task<RoomViewModel?> JoinRoomAsync()
        {
            if (string.IsNullOrWhiteSpace(AccessCode))
            {
                await SnackbarViewModel.Enqueue("Uzupełnij kod dostępu.");
                return null;
            }

            var room = await _roomsService.JoinRoomAsync(SelectedEvent!);

            SnackbarViewModel.Disposed = true;
            return new RoomViewModel(
                "A4xg629p1Q",
                false,
                _userClient,
                room.Scrambles,
                _roomsService);
        }
    }
}