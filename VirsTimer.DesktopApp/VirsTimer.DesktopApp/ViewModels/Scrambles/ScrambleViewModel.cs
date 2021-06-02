using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels.Scrambles
{
    public class ScrambleViewModel : ViewModelBase
    {
        private readonly IScrambleGenerator _scrambleGenerator;
        private Event _currentEvent;
        private Queue<Scramble> _scrambles = null!;

        [Reactive]
        public Scramble CurrentScramble { get; set; } = new Scramble();

        public ScrambleViewModel(Event @event)
        {
            _scrambleGenerator = Ioc.Services.GetRequiredService<IScrambleGenerator>();
            _currentEvent = @event;
            OnConstructedAsync(this, EventArgs.Empty);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            await ChangeEventAsync(_currentEvent).ConfigureAwait(false);
        }

        public async Task ChangeEventAsync(Event newEvent)
        {
            _currentEvent = newEvent;
            var scrabmles = await _scrambleGenerator.GenerateScrambles(_currentEvent, 10).ConfigureAwait(false);
            _scrambles = new Queue<Scramble>(scrabmles);
            CurrentScramble = _scrambles.Dequeue();
        }

        public async Task GetNextScrambleAsync()
        {
            CurrentScramble = _scrambles.Dequeue();
            if (_scrambles.Count < 3)
            {
                var generatedScrambles = await _scrambleGenerator.GenerateScrambles(_currentEvent, 5).ConfigureAwait(false);
                foreach (var scramble in generatedScrambles)
                    _scrambles.Enqueue(scramble);
            }
        }
    }
}
