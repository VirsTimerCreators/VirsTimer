using Avalonia.Threading;
using System;
using System.Diagnostics;

namespace VirsTimer.Core.Timers
{
    public class DelayStopwatchTimer
    {
        private readonly DispatcherTimer _delayTimer;
        private readonly DispatcherTimer _timerRefresher;
        private readonly Stopwatch _stopwatch;

        public bool CanFire { get; private set; } = false;

        public bool CountdownStarted => _delayTimer.IsEnabled;

        public TimeSpan CurrentTime => _stopwatch.Elapsed;

        public bool IsRunning => _stopwatch.IsRunning;

        public event Action Stopped;

        public void InvertWork()
        {
            if (!IsRunning)
                Start();
            else
                Stop();
        }

        public DelayStopwatchTimer(TimeSpan fireDelay)
        {
            _timerRefresher = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1)
            };
            _timerRefresher.Start();

            _delayTimer = new DispatcherTimer
            {
                Interval = fireDelay
            };
            _delayTimer.Tick += FireEvent;

            _stopwatch = new Stopwatch();
        }

        public DelayStopwatchTimer()
            : this(TimeSpan.FromMilliseconds(500)) { }

        public void StartCountdown()
        {
            _stopwatch.Reset();
            _delayTimer.Start();
        }

        public void Start()
        {
            _delayTimer.Stop();
            if (!CanFire)
                return;

            CanFire = false;
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
            Stopped();
        }

        public void AddRefreshEvent(EventHandler eventHandler)
        {
            _timerRefresher.Tick += eventHandler;
        }

        private void FireEvent(object? sender, EventArgs eventArgs)
        {
            CanFire = true;
        }
    }
}
