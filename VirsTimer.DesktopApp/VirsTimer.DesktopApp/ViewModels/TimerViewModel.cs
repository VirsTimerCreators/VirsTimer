using ReactiveUI;
using System;
using System.Threading.Tasks;
using VirsTimer.Core.Timers;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class TimerViewModel : ViewModelBase
    {
        private TimeSpan currentTime = TimeSpan.Zero;
        private TimeSpan savedTime = TimeSpan.Zero;

        public DelayStopwatchTimer Timer { get; }
        public TimeSpan SavedTime
        {
            get => savedTime;
            set => this.RaiseAndSetIfChanged(ref savedTime, value);
        }

        public TimeSpan CurrentTime
        {
            get => currentTime;
            set => this.RaiseAndSetIfChanged(ref currentTime, value);
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
                this.RaisePropertyChanged("Timer");
                CurrentTime = Timer.CurrentTime;
            });
        }

        private void UpdateSavedTime()
        {
            SavedTime = Timer.CurrentTime;
        }
    }
}
