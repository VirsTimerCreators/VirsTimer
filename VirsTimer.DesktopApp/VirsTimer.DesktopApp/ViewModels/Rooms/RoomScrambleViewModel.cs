using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using VirsTimer.Core.Multiplayer;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomScrambleViewModel : ViewModelBase
    {
        private readonly Queue<RoomScramble> _scrambles;

        [Reactive]
        public RoomScramble? Current { get; set; }

        public RoomScrambleViewModel(IEnumerable<RoomScramble> scrambles)
        {
            _scrambles = new Queue<RoomScramble>(scrambles);
        }

        public void GetNextScramble()
        {
            Current = _scrambles.Dequeue();
        }
    }
}