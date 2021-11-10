using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Specialized;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Events;
using VirsTimer.DesktopApp.Views.Events;
 
namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventSummaryViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;
 
        [Reactive]
        public Event CurrentEvent { get; set; } = null!;
 
        public ReactiveCommand<Window, Unit> ChangeEventCommand { get; }
 
        public EventSummaryViewModel(IEventsRepository eventsRepository)
        {
            _eventsRepository = eventsRepository;
            ChangeEventCommand = ReactiveCommand.CreateFromTask<Window>(ChangeEventAsync);
        }
 
        public override async Task ConstructAsync()
        {
            IsBusy = true;
            var repositoryResponse = await _eventsRepository.GetEventsAsync().ConfigureAwait(false);
            CurrentEvent = repositoryResponse.Value[0];
            IsBusy = false;
        }
 
        private async Task ChangeEventAsync(Window window)
        {
            var eventChangeViewModel = new EventChangeViewModel(_eventsRepository);
            await eventChangeViewModel.ConstructAsync().ConfigureAwait(true);
            var dialog = new EventChangeView
            {
                DataContext = eventChangeViewModel
            };
 
            eventChangeViewModel.Events.CollectionChanged += (_, e) => OnEventDelete(e, eventChangeViewModel.Events[0].Event);
            await dialog.ShowDialog(window);
            if (eventChangeViewModel.Accepted)
                CurrentEvent = eventChangeViewModel.SelectedEvent!.Event;
        }
 
        private void OnEventDelete(NotifyCollectionChangedEventArgs e, Event newEvent)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove)
                return;
 
            foreach (EventViewModel @event in e.OldItems)
            {
                if (CurrentEvent.Id == @event.Event.Id)
                {
                    CurrentEvent = newEvent;
                    break;
                }
            }
        }
    }
}
