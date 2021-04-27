using ReactiveUI;
using System;
using VirsTimer.Core.Timers;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class TimerViewModel : ViewModelBase
    {
        private string currentTime = "00.00";
        public string CurrentTime
        {
            get => currentTime;
            set => this.RaiseAndSetIfChanged(ref currentTime, value);
        }

        public DelayStopwatchTimer Timer { get; }

        public TimerViewModel()
        {
            Timer = new DelayStopwatchTimer();
            Timer.AddEvent(UpdateCurrentTime);
        }

        private void UpdateCurrentTime(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged("Timer");
            var currentTime = Timer.CurrentTime;
            if (currentTime.Hours > 0)
                CurrentTime = currentTime.ToString("hh\\:mm\\:ss\\.ff");
            else if (currentTime.Minutes > 0)
                CurrentTime = currentTime.ToString("mm\\:ss\\.ff");
            else
                CurrentTime = currentTime.ToString("ss\\.ff");
        }
    }
}
