using ReactiveUI;
using System.Collections.Generic;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ScrambleViewModel : ViewModelBase
    {
        private Scramble currentScramble = new(string.Empty);
        private string currentEvent;
        private Queue<Scramble> scrambles;
        private readonly IScrambleGenerator scrambleGenerator;

        public Scramble CurrentScramble
        {
            get => currentScramble;
            set => this.RaiseAndSetIfChanged(ref currentScramble, value);
        }

        public ScrambleViewModel(string @event, IScrambleGenerator scrambleGenerator)
        {
            this.currentEvent = @event;
            this.scrambleGenerator = scrambleGenerator;
            this.scrambles = new Queue<Scramble>(scrambleGenerator.GenerateScrambles(@event, 10).GetAwaiter().GetResult());
            CurrentScramble = scrambles.Dequeue();
        }

        public void ChangeEvent(string newEvent)
        {
            currentEvent = newEvent;
            scrambles = new Queue<Scramble>(scrambleGenerator.GenerateScrambles(currentEvent, 10).GetAwaiter().GetResult());
            CurrentScramble = scrambles.Dequeue();
        }

        public void NextScramble()
        {
            CurrentScramble = scrambles.Dequeue();
            if (scrambles.Count < 5)
            {
                var generatedScrambles = scrambleGenerator.GenerateScrambles(currentEvent, 5).GetAwaiter().GetResult();
                foreach (var scramble in generatedScrambles)
                    scrambles.Enqueue(scramble);
            }
        }
    }
}
