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

        private int state;
        public int State
        {
            get => state;
            set => this.RaiseAndSetIfChanged(ref state, value);
        }

        public DelayFireTimer DelayFireTimer { get; }

        public StopwatchTimer Model { get; }

        public TimerViewModel()
        {
            Model = new StopwatchTimer();
            Model.AddEvent(UpdateGretting);
            DelayFireTimer = new DelayFireTimer();
        }

        private void UpdateGretting(object? sender, EventArgs e)
        {
            if (DelayFireTimer.IsRunning)
                State = 0;
            if (!DelayFireTimer.IsRunning)
                State = 1;

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
