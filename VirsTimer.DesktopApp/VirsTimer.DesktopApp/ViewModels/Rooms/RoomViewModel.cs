using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Multiplayer;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomViewModel : ViewModelBase
    {
        private readonly Room _model;
        private readonly IValueConverter<string, Bitmap> _svgToBitmapConverter;
        private readonly bool _isAdmin;
        private readonly IRoomsService _roomsService;
        private readonly List<RoomSolve> _failedSolves = new();

        public bool Valid { get; } = true;
        public string AccessCode => _model.AccessCode;

        public bool FinishedWithError { get; set; }

        [Reactive]
        public string BorderColor { get; set; }

        [Reactive]
        public string Status { get; set; }

        [Reactive]
        public ViewModelBase TimerContent { get; set; }

        public RoomScrambleViewModel ScrambleViewModel { get; }

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
            bool isAdmin,
            Room room,
            IRoomsService roomsService,
            IValueConverter<string, Bitmap>? svgToBitmapConverter = null)
        {
            _model = room;
            _svgToBitmapConverter = svgToBitmapConverter ?? new SvgToBitmapConverter(300);
            _isAdmin = isAdmin;
            _roomsService = roomsService;
            Status = _isAdmin ? "Zapraszanie" : "Oczekiwanie na rozpoczęcie administratora.";
            BorderColor = "#4185fa";
            ScrambleViewModel = new RoomScrambleViewModel(room.Scrambles);
            ScrambleViewModel.Finished.Subscribe(x =>
            {
                if (x is false)
                    return;

                Status = "Zakończono";
                BorderColor = "#0959db";
            });
            TimerViewModel = new TimerViewModel();
            TimerViewModel.Timer.Stopped += SolveFinished;
            TimerContent = TimerViewModel;
            RoomUsersViewModel = new RoomUsersViewModel();
            SnackbarViewModel = new SnackbarViewModel(400, 64);

            CopyToClipboardCommand = ReactiveCommand.CreateFromTask(CopyToClipboard);

            var canStart = this.WhenAnyValue(x => x.Status)
                .Select(x => x == "Zapraszanie" && _isAdmin);

            StartCommand = ReactiveCommand.CreateFromTask(StartCompetition, canStart);
            ExitCommand = ReactiveCommand.CreateFromTask(LeaveRoom);

            CopyToClipboardCommand.ThrownExceptions.Subscribe((e) => SnackbarViewModel.EnqueueSchedule("Podczas kopiowania do schowka wystąpił problem"));
            StartCommand.ThrownExceptions.Subscribe(ExceptionThrown);

            var observer = new NotificationObserver(this);
            _roomsService.Notifications.Subscribe(observer);

            this.WhenActivated(disposableRegistration =>
            {
                RxApp.TaskpoolScheduler.SchedulePeriodic(TimeSpan.FromSeconds(3), async () =>
                {
                    for (var i = 0; i < _failedSolves.Count; i++)
                    {
                        var failed = _failedSolves[i];
                        var failedResponse = await _roomsService.SendSolveAsync(_model.Id, failed);
                        if (failedResponse.IsSuccesfull)
                            _failedSolves.Remove(failed);
                    }
                }).DisposeWith(disposableRegistration);
            });
        }

        public void SolveFinished()
        {
            var roomSolveFlagViewModel = new RoomSolveFlagsViewModel(TimerViewModel.SavedTime);
            TimerContent = roomSolveFlagViewModel;
            roomSolveFlagViewModel.AcceptFlagCommand.Subscribe(async x => await AddSolveToMeAsync(x));
            roomSolveFlagViewModel.AcceptFlagCommand.ThrownExceptions.Subscribe(ExceptionThrown);
            roomSolveFlagViewModel.RadioButtonFocusedCommand.ThrownExceptions.Subscribe(ExceptionThrown);
        }

        private async Task AddSolveToMeAsync(SolveFlag solveFlag)
        {
            IsBusy = true;

            var time = TimerViewModel.SavedTime;
            var solve = new RoomSolve
            {
                Date = DateTime.Now,
                Time = time.Ticks,
                Flag = solveFlag,
                ScrambleId = ScrambleViewModel.Current!.Id
            };

            var response = await _roomsService.SendSolveAsync(_model.Id, solve);
            if (response.IsSuccesfull is false)
            {
                SnackbarViewModel.EnqueueSchedule("Podczas wysyłania ułożenia wystąpił błąd. Próba zostanie ponowiona.");
                _failedSolves.Add(solve);
            }

            TimerContent = TimerViewModel;
            ScrambleViewModel.GetNextScramble();
        }

        public override async Task<bool> ConstructAsync()
        {
            var exitSvgTask = await File.ReadAllTextAsync("Assets/copyToClipboard.svg");
            CopyImage = _svgToBitmapConverter.Convert(exitSvgTask);
            return true;
        }

        private async Task CopyToClipboard()
        {
            await App.Current.Clipboard.SetTextAsync(AccessCode);
            await SnackbarViewModel.Enqueue("Skopiowano klucz dostępu do schowka");
        }

        private async Task StartCompetition()
        {
            IsBusy = true;

            var response = await _roomsService.StartRoomAsync(_model.Id);
            if (response.IsSuccesfull is false)
                SnackbarViewModel.EnqueueSchedule("Podczas łączenia z serwerem wystąpił błąd. Nie można ropocząć kokurencji.");

            IsBusy = false;
        }

        private Task LeaveRoom()
        {
            SnackbarViewModel.Disposed = true;
            return _roomsService.LeaveRoomAsync();
        }

        public void ExceptionThrown(Exception e)
        {
            SnackbarViewModel.Enqueue("Wystąpił problem podczas łączenia z serwem.");
        }

        private class NotificationObserver : IObserver<RoomNotification>
        {
            private readonly RoomViewModel _viewModel;

            public NotificationObserver(RoomViewModel viewModel)
            {
                _viewModel = viewModel;
            }

            public void OnCompleted()
            {
                _viewModel.FinishedWithError = true;
            }

            public void OnError(Exception error)
            {
                _viewModel.FinishedWithError = true;
            }

            public void OnNext(RoomNotification notification)
            {
                var usersViewModels = notification.RoomUsers
                .Select(roomUser => new RoomUserViewModel(roomUser));

                RxApp.MainThreadScheduler.Schedule(async () =>
                {
                    _viewModel.IsBusy = true;

                    if ((_viewModel.Status == "Zapraszanie" || _viewModel.Status == "Oczekiwanie na rozpoczęcie administratora.") && notification.Status == RoomStatus.InProgress)
                    {
                        _viewModel.Status = "Rozpoczęto";
                        _viewModel.BorderColor = "#9e5e4d";
                        _viewModel.ScrambleViewModel.GetNextScramble();
                    }

                    _viewModel.RoomUsersViewModel.Users = new(usersViewModels);
                    await _viewModel.RoomUsersViewModel.Refresh();

                    _viewModel.IsBusy = false;
                });
            }
        }
    }
}