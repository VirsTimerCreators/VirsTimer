using ReactiveUI;
using System;
using System.Threading.Tasks;
using VirsTimer.Core.Timers;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class TimerViewModel : ViewModelBase
    {
        private TimeSpan _currentTime = TimeSpan.Zero;
        private TimeSpan _savedTime = TimeSpan.Zero;

        public DelayStopwatchTimer Timer { get; }
        public TimeSpan SavedTime
        {
            get => _savedTime;
            set => this.RaiseAndSetIfChanged(ref _savedTime, value);
        }

        public TimeSpan CurrentTime
        {
            get => _currentTime;
            set => this.RaiseAndSetIfChanged(ref _currentTime, value);
        }

        public TimerViewModel()
        {
            Timer = new DelayStopwatchTimer();
            Timer.AddRefreshEvent(UpdateCurrentTime);
            Timer.Stopped += UpdateSavedTime;
        }

        private async void UpdateCurrentTime(object? sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                this.RaisePropertyChanged(nameof(Timer));
                CurrentTime = Timer.CurrentTime;
            });
        }

        private void UpdateSavedTime()
        {
            SavedTime = Timer.CurrentTime;
        }
    }
}
