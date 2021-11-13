using Avalonia.Threading;
using System;
using System.Diagnostics;

namespace VirsTimer.Core.Timers
{
    /// <summary>
    /// Timer that starts after given delay.
    /// </summary>
    public class DelayStopwatchTimer
    {
        private readonly DispatcherTimer _delayTimer;
        private readonly DispatcherTimer _timerRefresher;
        private readonly Stopwatch _stopwatch;

        /// <summary>
        /// Can timer start.
        /// </summary>
        public bool CanFire { get; private set; } = false;

        /// <summary>
        /// Delay countdown has started.
        /// </summary>
        public bool CountdownStarted => _delayTimer.IsEnabled;

        /// <summary>
        /// Current timer time.
        /// </summary>
        public TimeSpan CurrentTime => _stopwatch.Elapsed;

        /// <summary>
        /// Is timer running.
        /// </summary>
        public bool IsRunning => _stopwatch.IsRunning;

        /// <summary>
        /// Timer has stopped event;
        /// </summary>
        public event Action? Stopped;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayStopwatchTimer"/> class.
        /// </summary>
        public DelayStopwatchTimer(TimeSpan fireDelay)
        {
            _timerRefresher = new DispatcherTimer
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DelayStopwatchTimer"/> class with 0.5 second delay.
        /// </summary>
        public DelayStopwatchTimer()
            : this(TimeSpan.FromMilliseconds(500)) { }

        /// <summary>
        /// Start delay countdown.
        /// </summary>
        public void StartCountdown()
        {
            _stopwatch.Reset();
            _delayTimer.Start();
        }

        /// <summary>
        /// Start timer.
        /// </summary>
        public void Start()
        {
            _delayTimer.Stop();
            if (!CanFire)
                return;

            CanFire = false;
            _stopwatch.Start();
        }

        /// <summary>
        /// Stop timer.
        /// </summary>
        public void Stop()
        {
            _stopwatch.Stop();
            Stopped?.Invoke();
        }

        /// <summary>
        /// Stop timer if <see cref="IsRunning"/>. Start otherwise.
        /// </summary>
        public void InvertWork()
        {
            if (!IsRunning)
                Start();
            else
                Stop();
        }

        /// <summary>
        /// Add event that fires on timer tick.
        /// </summary>
        /// <param name="eventHandler"></param>
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