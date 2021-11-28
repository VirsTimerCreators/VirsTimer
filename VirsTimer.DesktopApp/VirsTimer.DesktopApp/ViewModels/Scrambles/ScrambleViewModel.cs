using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;
using VirsTimer.Core.Services.Scrambles;
using VirsTimer.Scrambles;

namespace VirsTimer.DesktopApp.ViewModels.Scrambles
{
    public class ScrambleViewModel : ViewModelBase
    {
        private readonly IScrambleGenerator _scrambleGenerator;
        private readonly ICustomScrambleGeneratorsCollector _customScrambleGeneratorsCollector;

        private ICustomScrambleGenerator? _customScrambleGenerator;
        private bool _isCustom = false;
        private Event _currentEvent = null!;
        private Queue<Scramble> _scrambles = null!;

        [Reactive]
        public Scramble CurrentScramble { get; set; } = new Scramble();

        public ScrambleViewModel(
            IScrambleGenerator? scrambleGenerator = null,
            ICustomScrambleGeneratorsCollector? customScrambleGeneratorsCollector = null)
        {
            _scrambleGenerator = scrambleGenerator ?? Ioc.GetService<IScrambleGenerator>();
            _customScrambleGeneratorsCollector = customScrambleGeneratorsCollector ?? Ioc.GetService<ICustomScrambleGeneratorsCollector>();
        }

        public override Task ConstructAsync()
        {
            return ChangeEventAsync(_currentEvent);
        }

        public async Task ChangeEventAsync(Event newEvent)
        {
            IsBusy = true;
            _currentEvent = newEvent;

            if (Core.Constants.Events.Predefined.Contains(newEvent.Name))
            {
                _isCustom = false;
                var scrabmles = await _scrambleGenerator.GenerateScrambles(_currentEvent, 10).ConfigureAwait(false);
                _scrambles = new Queue<Scramble>(scrabmles);
                await GetNextScrambleAsync();
                IsBusy = false;
                return;
            }

            _isCustom = true;
            _customScrambleGenerator = _customScrambleGeneratorsCollector.GetCustomScrambleGenerators().FirstOrDefault(x => x.EventName == newEvent.Name);
            await GetNextScrambleAsync();
            IsBusy = false;
        }

        public async Task GetNextScrambleAsync()
        {
            if (_isCustom)
            {
                try
                {
                    CurrentScramble = _customScrambleGenerator?.GenerateScramble() ?? new Scramble();
                }
                catch
                {
                    CurrentScramble = new Scramble();
                }

                return;
            }

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
