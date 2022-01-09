using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Multiplayer;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomCreationViewModel : ViewModelBase
    {
        private readonly IRoomsService _roomsService;

        public ReactiveCommand<Unit, RoomViewModel?> CreateRoomCommand { get; }
        public IReadOnlyList<string> AllEvents { get; set; }

        [Reactive]
        public string? SelectedEvent { get; set; }

        public ReactiveCommand<Unit, RoomViewModel?> JoinRoomCommand { get; }

        [Reactive]
        public string AccessCode { get; set; }

        [Reactive]
        public string ScramblesAmount { get; set; }

        public SnackbarViewModel SnackbarViewModel { get; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }

        public RoomCreationViewModel(
            IRoomsService? roomsService = null)
        {
            _roomsService = roomsService ?? Ioc.GetService<IRoomsService>();
            ScramblesAmount = string.Empty;
            AllEvents = Core.Constants.Events.Predefined;
            CreateRoomCommand = ReactiveCommand.CreateFromTask(CreateRoomAsync);
            JoinRoomCommand = ReactiveCommand.CreateFromTask(JoinRoomAsync);
            CancelCommand = ReactiveCommand.Create(() =>
            {
                SnackbarViewModel.Disposed = true;
            });
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

            if (string.IsNullOrEmpty(ScramblesAmount))
            {
                await SnackbarViewModel.Enqueue("Uzupełnij liczbę scrambli.");
                return null;
            }
            var scramblesAmountParsed = int.TryParse(ScramblesAmount, out var scramblesAmount)
                && 3 <= scramblesAmount
                && scramblesAmount <= 20;

            if (scramblesAmountParsed is false)
            {
                await SnackbarViewModel.Enqueue("Liczba scrambli musi być w przedziale [3, 20].");
                return null;
            }

            IsBusy = true;

            var response = await _roomsService.CreateRoomAsync(SelectedEvent!, scramblesAmount);

            IsBusy = false;

            if (response.IsSuccesfull is false)
            {
                await SnackbarViewModel.Enqueue("Podczas tworzenia pokoju wystąpił problem.");
                return null;
            }

            SnackbarViewModel.Disposed = true;
            return new RoomViewModel(
                isAdmin: true,
                response.Value!,
                _roomsService);
        }

        public async Task<RoomViewModel?> JoinRoomAsync()
        {
            if (string.IsNullOrWhiteSpace(AccessCode))
            {
                await SnackbarViewModel.Enqueue("Uzupełnij kod dostępu.");
                return null;
            }

            IsBusy = true;

            var response = await _roomsService.JoinRoomAsync(AccessCode!);

            IsBusy = false;

            if (response.IsSuccesfull is false)
            {
                await SnackbarViewModel.Enqueue("Nie można dołaczyć do pokoju ponieważ już się rozpoczął lub nie istnieje.");
                return null;
            }

            SnackbarViewModel.Disposed = true;
            return new RoomViewModel(
                isAdmin: false,
                response.Value!,
                _roomsService);
        }
    }
}