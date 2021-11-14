using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels.Scrambles
{
    public class ScrambleViewModel : ViewModelBase
    {
        private readonly IScrambleGenerator _scrambleGenerator;
        private Event _currentEvent = null!;
        private Queue<Scramble> _scrambles = null!;

        [Reactive]
        public Scramble CurrentScramble { get; set; } = new Scramble();

        public ScrambleViewModel(IScrambleGenerator? scrambleGenerator = null)
        {
            _scrambleGenerator = scrambleGenerator ?? = Ioc.GetService<IScrambleGenerator>();
        }

        public override Task ConstructAsync()
        {
            return ChangeEventAsync(_currentEvent);
        }

        public async Task ChangeEventAsync(Event newEvent)
        {
            IsBusy = true;
            _currentEvent = newEvent;
            var scrabmles = await _scrambleGenerator.GenerateScrambles(_currentEvent, 10).ConfigureAwait(false);
            _scrambles = new Queue<Scramble>(scrabmles);
            CurrentScramble = _scrambles.Dequeue();
            IsBusy = false;
        }

        public async Task GetNextScrambleAsync()
        {
            CurrentScramble = _scrambles.Dequeue();
            if (_scrambles.Count < 3)
            {
                IsBusy = true;
                var generatedScrambles = await _scrambleGenerator.GenerateScrambles(_currentEvent, 5).ConfigureAwait(false);
                foreach (var scramble in generatedScrambles)
                    _scrambles.Enqueue(scramble);
                IsBusy = false;
            }
        }
    }
}
