using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Events;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventChangeViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;

        public ObservableCollection<Event> Events { get; private set; } = null!;

        [Reactive]
        public Event? SelectedEvent { get; set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }

        public EventChangeViewModel(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;

            var canAccpet = this.WhenAnyValue<EventChangeViewModel, bool, Event?>(x => x.SelectedEvent, x => x != null);
            AcceptCommand = ReactiveCommand.Create<Window>((window) => { window.Close(); }, canAccpet);

            OnConstructedAsync(this, EventArgs.Empty);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            var events = await _eventsRepository.GetEventsAsync().ConfigureAwait(false);
            Events = new ObservableCollection<Event>(events);
        }
    }
}
