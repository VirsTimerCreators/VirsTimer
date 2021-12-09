using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Models.Authorization;
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
        private readonly IUserClient _userClient;

        public bool Valid { get; } = true;
        public string AccessCode { get; }

        [Reactive]
        public string BorderColor { get; set; }

        [Reactive]
        public string Status { get; set; }

        [Reactive]
        public ViewModelBase TimerContent { get; set; }

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

        private RoomUserViewModel Me => RoomUsersViewModel.Users.First(x => x.UserName == _userClient.Id);

        public RoomViewModel(
            string accessCode,
            bool isAdmin,
            IUserClient userClient,
            IEnumerable<Scramble> scrambles,
            IRoomsService? roomsService = null,
            IValueConverter<string, Bitmap>? svgToBitmapConverter = null)
        {
            _svgToBitmapConverter = svgToBitmapConverter ?? new SvgToBitmapConverter(300);
            _isAdmin = isAdmin;
            _userClient = userClient;
            AccessCode = accessCode;
            Status = "Zapraszanie";
            BorderColor = "#4185fa";
            ScrambleViewModel = new ScrambleViewModel();
            TimerViewModel = new TimerViewModel();
            TimerViewModel.Timer.Stopped += SolveFinished;
            TimerContent = TimerViewModel;
            SnackbarViewModel = new SnackbarViewModel();
            //roomsService.Notifications.Select(notification =>
            //{
            //    return new object();
            //});

            CopyToClipboardCommand = ReactiveCommand.CreateFromTask(CopyToClipboard);
            StartCommand = ReactiveCommand.CreateFromTask(StartCompetition, Observable.Return(_isAdmin));
            ExitCommand = ReactiveCommand.Create(() => { });

            var users = new List<RoomUserViewModel>
            {
                new RoomUserViewModel("Adam",5),
                new RoomUserViewModel("Bartek",5),
                new RoomUserViewModel("Kuba",5),
                new RoomUserViewModel("padge",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
                new RoomUserViewModel("Michał",5),
            };

            users[0].Solves.Add(new RoomUserSolveViewModel(new  Solve(null!, TimeSpan.FromSeconds(6).Add(TimeSpan.FromMilliseconds(42)), "R U X A T R' C")));
            users[0].Solves.Add(new RoomUserSolveViewModel(new  Solve(null!, TimeSpan.FromSeconds(7).Add(TimeSpan.FromMilliseconds(567)), "R U X A T R' C")));
                                                                
            users[12].Solves.Add(new RoomUserSolveViewModel(new Solve(null!, TimeSpan.FromSeconds(6).Add(TimeSpan.FromMilliseconds(42)), "R U X A T R' C")));
            users[12].Solves.Add(new RoomUserSolveViewModel(new Solve(null!, TimeSpan.FromSeconds(7).Add(TimeSpan.FromMilliseconds(567)), "R U X A T R' C")));
                                                                
            users[20].Solves.Add(new RoomUserSolveViewModel(new Solve(null!, TimeSpan.FromSeconds(6).Add(TimeSpan.FromMilliseconds(42)), "R U X A T R' C")));
            users[20].Solves.Add(new RoomUserSolveViewModel(new Solve(null!, TimeSpan.FromSeconds(7).Add(TimeSpan.FromMilliseconds(567)), "R U X A T R' C")));
            RoomUsersViewModel = new RoomUsersViewModel(users);
        }

        public void SolveFinished()
        {
            var roomSolveFlagViewModel = new RoomSolveFlagsViewModel(TimerViewModel.SavedTime);
            TimerContent = roomSolveFlagViewModel;
            roomSolveFlagViewModel.AcceptFlagCommand.Subscribe(async x => await AddSolveToMeAsync(x));
        }

        private Task AddSolveToMeAsync(SolveFlag solveFlag)
        {
            var time = TimerViewModel.SavedTime;
            var solve = new Solve(null!, time, "xxx")
            {
                Flag = solveFlag
            };
            Me.Solves.Insert(0, new RoomUserSolveViewModel(solve));

            TimerContent = TimerViewModel;
            return Task.CompletedTask;
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
