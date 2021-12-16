using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using VirsTimer.Core.Multiplayer;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomScrambleViewModel : ViewModelBase
    {
        private readonly Queue<RoomScramble> _scrambles;
        private readonly bool _started;
        private readonly Subject<bool> _finished;

        public IObservable<bool> Finished => _finished;

        public IReadOnlyCollection<RoomScramble> Scrambles => _scrambles;

        [Reactive]
        public RoomScramble? Current { get; set; }

        public RoomScrambleViewModel(IEnumerable<RoomScramble> scrambles)
        {
            _finished = new Subject<bool>();
            _scrambles = new Queue<RoomScramble>(scrambles);
            if (_scrambles.Count > 0)
                _started = true;
        }

        public void GetNextScramble()
        {
            if (_scrambles.Count == 0 && _started)
            {
                _finished.OnNext(true);
                return;
            }

            Current = _scrambles.Dequeue();
        }
    }
}