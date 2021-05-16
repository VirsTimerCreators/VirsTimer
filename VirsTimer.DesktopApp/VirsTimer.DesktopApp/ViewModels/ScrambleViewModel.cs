using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Collections.Generic;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class ScrambleViewModel : ViewModelBase
    {
        private readonly IScrambleGenerator _scrambleGenerator;
        private Event _currentEvent = null!;
        private Scramble _currentScramble = null!;
        private Queue<Scramble> _scrambles = null!;

        public Scramble CurrentScramble
        {
            get => _currentScramble;
            set => this.RaiseAndSetIfChanged(ref _currentScramble, value);
        }

        public ScrambleViewModel(Event @event)
        {
            _scrambleGenerator = Ioc.Services.GetRequiredService<IScrambleGenerator>();
            ChangeEvent(@event);
        }

        public void ChangeEvent(Event newEvent)
        {
            _currentEvent = newEvent;
            _scrambles = new Queue<Scramble>(_scrambleGenerator.GenerateScrambles(_currentEvent, 10).GetAwaiter().GetResult());
            CurrentScramble = _scrambles.Dequeue();
        }

        public void NextScramble()
        {
            CurrentScramble = _scrambles.Dequeue();
            if (_scrambles.Count < 5)
            {
                var generatedScrambles = _scrambleGenerator.GenerateScrambles(_currentEvent, 5).GetAwaiter().GetResult();
                foreach (var scramble in generatedScrambles)
                    _scrambles.Enqueue(scramble);
            }
        }
    }
}
