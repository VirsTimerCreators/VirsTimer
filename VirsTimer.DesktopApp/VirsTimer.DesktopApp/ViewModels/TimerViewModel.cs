using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Threading.Tasks;
using VirsTimer.Core.Timers;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class TimerViewModel : ViewModelBase
    {
        public DelayStopwatchTimer Timer { get; }

        [Reactive]
        public TimeSpan SavedTime { get; set; } = TimeSpan.Zero;

        [Reactive]
        public TimeSpan CurrentTime { get; set; } = TimeSpan.Zero;

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
