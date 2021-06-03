using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Events;
using VirsTimer.DesktopApp.Views.Events;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;

        [Reactive]
        public Event CurrentEvent { get; set; } = null!;

        public ReactiveCommand<Window, Unit> ChangeEventCommand { get; }

        public EventViewModel(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
            ChangeEventCommand = ReactiveCommand.CreateFromTask<Window>(ChangeEventAsync);

            OnConstructedAsync(this, EventArgs.Empty);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            var events = await _eventsRepository.GetEventsAsync().ConfigureAwait(false);
            CurrentEvent = events[0];
        }

        private async Task ChangeEventAsync(Window window)
        {
            var eventChangeViewModel = new EventChangeViewModel(_eventsRepository);
            var dialog = new EventChangeView
            {
                DataContext = eventChangeViewModel
            };

            await dialog.ShowDialog(window);
            if (eventChangeViewModel.SelectedEvent != null)
                CurrentEvent = eventChangeViewModel.SelectedEvent;
        }
    }
}
