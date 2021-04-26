using Avalonia.Threading;
using System;
using System.Diagnostics;

namespace VirsTimer.Core.Timers
{
    public class StopwatchTimer
    {
        private readonly DispatcherTimer _timer;
        private readonly Stopwatch _stopwatch;

        public TimeSpan CurrentTime => _stopwatch.Elapsed;
        public bool IsRunning => _stopwatch.IsRunning;

        public void InvertWork()
        {
            if (!IsRunning)
                Start();
            else
                Stop();
        }

        public StopwatchTimer()
        {
            _timer = new DispatcherTimer()
            {
                Interval = new TimeSpan(0, 0, 0, 0, 1)
            };
            _stopwatch = new Stopwatch();
        }

        public void Start()
        {
            _timer.Start();
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
            _timer.Stop();
        }

        public void AddEvent(EventHandler eventHandler)
        {
            _timer.Tick += eventHandler;
        }
    }
}
