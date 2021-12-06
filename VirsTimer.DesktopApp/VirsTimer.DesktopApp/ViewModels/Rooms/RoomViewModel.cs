using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Services.Rooms;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Common;
using VirsTimer.DesktopApp.ViewModels.Scrambles;
using VirsTimer.Scrambles;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly IValueConverter<string, Bitmap> _svgToBitmapConverter;
        private readonly bool _isAdmin;

        public bool Valid { get; } = true;
        public string AccessCode { get; }

        [Reactive]
        public string BorderColor { get; set; }

        [Reactive]
        public string Status { get; set; }

        public ScrambleViewModel ScrambleViewModel { get; }

        public TimerViewModel TimerViewModel { get; }

        public SnackbarViewModel SnackbarViewModel { get; }

        [Reactive]
        public IImage? CopyImage { get; set; }

        [Reactive]
        public RoomUsersViewModel RoomUsersViewModel { get; set; }

        public ReactiveCommand<Unit, Unit> CopyToClipboardCommand { get; set; }

        public ReactiveCommand<Unit, Unit> StartCommand { get; set; }

        public ReactiveCommand<Unit, Unit> ExitCommand { get; set; }

        public RoomViewModel(
            string accessCode,
            bool isAdmin,
            IEnumerable<Scramble> scrambles,
            IRoomsService? roomsService = null,
            IValueConverter<string, Bitmap>? svgToBitmapConverter = null)
        {
            _svgToBitmapConverter = svgToBitmapConverter ?? new SvgToBitmapConverter(300);
            _isAdmin = isAdmin;
            AccessCode = accessCode;
            Status = "Zapraszanie";
            BorderColor = "#4185fa";
            ScrambleViewModel = new ScrambleViewModel();
            TimerViewModel = new TimerViewModel();
            SnackbarViewModel = new SnackbarViewModel();
            //roomsService.Notifications.Select(notification =>
            //{
            //    return new object();
            //});
            RoomUsersViewModel = new RoomUsersViewModel(Array.Empty<RoomUserViewModel>());

            CopyToClipboardCommand = ReactiveCommand.CreateFromTask(CopyToClipboard);
            StartCommand = ReactiveCommand.CreateFromTask(StartCompetition, Observable.Return(_isAdmin));
            ExitCommand = ReactiveCommand.Create(() => { });
        }

        public override async Task ConstructAsync()
        {
            var exitSvgTask = await File.ReadAllTextAsync("Assets/copyToClipboard.svg");
            CopyImage = _svgToBitmapConverter.Convert(exitSvgTask);
        }

        private async Task CopyToClipboard()
        {
            await App.Current.Clipboard.SetTextAsync(AccessCode);
            await SnackbarViewModel.Enqueue("Skopiowano klucz dostępu do schowka");
        }

        private Task StartCompetition()
        {
            Status = "Rozpoczęto";
            BorderColor = "#9e5e4d";
            return Task.CompletedTask;
        }
    }
}
