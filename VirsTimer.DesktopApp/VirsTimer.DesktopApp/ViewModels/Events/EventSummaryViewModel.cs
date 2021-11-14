using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Specialized;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Cache;
using VirsTimer.Core.Services.Events;
using VirsTimer.DesktopApp.Views.Events;
 
namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventSummaryViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IApplicationCache _applicationCache;
        private readonly IApplicationCacheSaver _applicationCacheSaver;
 
        [Reactive]
        public Event CurrentEvent { get; set; } = null!;
 
        public ReactiveCommand<Window, Unit> ChangeEventCommand { get; }
 
        public EventSummaryViewModel(
            IEventsRepository? eventsRepository = null,
            IApplicationCache? applicationCache = null,
            IApplicationCacheSaver? applicationCacheSaver = null)
        {
            _eventsRepository = eventsRepository ?? Ioc.GetService<IEventsRepository>();
            _applicationCache = applicationCache ?? Ioc.GetService<IApplicationCache>();
            _applicationCacheSaver = applicationCacheSaver ?? Ioc.GetService<IApplicationCacheSaver>();
            ChangeEventCommand = ReactiveCommand.CreateFromTask<Window>(ChangeEventAsync);
        }
 
        public override async Task ConstructAsync()
        {
            IsBusy = true;
            var repositoryResponse = await _eventsRepository.GetEventsAsync().ConfigureAwait(false);
            CurrentEvent = repositoryResponse.Value.FirstOrDefault(e => e.Id == _applicationCache.LastChoosenEvent) ?? repositoryResponse.Value[0];
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
            {
                CurrentEvent = eventChangeViewModel.SelectedEvent!.Event;
                _applicationCache.LastChoosenEvent = CurrentEvent.Id!;
                await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(true);
            }
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
