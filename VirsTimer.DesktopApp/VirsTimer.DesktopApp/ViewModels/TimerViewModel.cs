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

        public DelayStopwatchTimer Model { get; }

        public TimerViewModel()
        {
            Model = new DelayStopwatchTimer();
            Model.AddEvent(UpdateGretting);
        }

        private void UpdateGretting(object? sender, EventArgs e)
        {
            this.RaisePropertyChanged("Model");
            var currentTime = Model.CurrentTime;
            if (currentTime.Hours > 0)
                CurrentTime = currentTime.ToString("hh\\:mm\\:ss\\.ff");
            else if (currentTime.Minutes > 0)
                CurrentTime = currentTime.ToString("mm\\:ss\\.ff");
            else
                CurrentTime = currentTime.ToString("ss\\.ff");
        }
    }
}
