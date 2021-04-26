using Avalonia.Threading;
using System;

namespace VirsTimer.Core.Timers
{
    public class DelayFireTimer
    {
        private readonly DispatcherTimer _timer;
        public bool CanFire { get; private set; }
        public bool IsRunning => _timer.IsEnabled;

        public DelayFireTimer(TimeSpan delayFireTimeSpan)
        {
            _timer = new DispatcherTimer
            {
                Interval = delayFireTimeSpan
            };

            _timer.Tick += FireEvent;
        }

        public DelayFireTimer()
            : this(TimeSpan.FromMilliseconds(500)) { }

        public void AddEvent(EventHandler eventHandler)
        {
            _timer.Tick += eventHandler;
        }

        public void Start() => _timer.IsEnabled = true;

        public void Stop() => _timer.IsEnabled = false;

        public void ResetCanFire() => CanFire = false;

        private void FireEvent(object? sender, EventArgs eventArgs)
        {
            CanFire = true;
        }
    }
}
