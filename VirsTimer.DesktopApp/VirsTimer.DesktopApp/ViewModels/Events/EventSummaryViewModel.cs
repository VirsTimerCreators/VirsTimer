using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Cache;
using VirsTimer.Core.Services.Events;
using VirsTimer.DesktopApp.ValueConverters;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventSummaryViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;
        private readonly IApplicationCache _applicationCache;
        private readonly IApplicationCacheSaver _applicationCacheSaver;
        private readonly IValueConverter<string, Bitmap> _svgToBitmapConverter;

        [Reactive]
        public IImage? Image { get; private set; }

        [Reactive]
        public Event CurrentEvent { get; set; } = null!;

        public ReactiveCommand<Unit, Unit> ChangeEventCommand { get; }

        public Interaction<EventChangeViewModel, EventViewModel?> ShowEventChangeDialog { get; }

        public EventSummaryViewModel(
            IEventsRepository? eventsRepository = null,
            IApplicationCache? applicationCache = null,
            IApplicationCacheSaver? applicationCacheSaver = null)
        {
            _svgToBitmapConverter = new SvgToBitmapConverter();
            _eventsRepository = eventsRepository ?? Ioc.GetService<IEventsRepository>();
            _applicationCache = applicationCache ?? Ioc.GetService<IApplicationCache>();
            _applicationCacheSaver = applicationCacheSaver ?? Ioc.GetService<IApplicationCacheSaver>();
            ChangeEventCommand = ReactiveCommand.CreateFromTask(ChangeEventAsync);
            ShowEventChangeDialog = new Interaction<EventChangeViewModel, EventViewModel?>();
        }

        public override async Task<bool> ConstructAsync()
        {
            IsBusy = true;
            var repositoryResponse = await _eventsRepository.GetEventsAsync().ConfigureAwait(false);
            if (repositoryResponse.IsSuccesfull is false)
                await ShutdownDialogHandleAsync("Nie można załadować eventów.");

            CurrentEvent = repositoryResponse.Value!.FirstOrDefault(e => e.Id == _applicationCache.LastChoosenEvent) ?? repositoryResponse.Value![0];

            var svg = await File.ReadAllTextAsync("Assets/event.svg");
            Image = _svgToBitmapConverter.Convert(svg);
            IsBusy = false;

            return true;
        }

        private async Task ChangeEventAsync()
        {
            var eventChangeViewModel = new EventChangeViewModel(_eventsRepository);
            var contructed = await eventChangeViewModel.ConstructAsync();
            if (contructed is false)
            {
                await CloseWindowDialogHandleAsync("Nie można załadować eventów.");
                return;
            }

            eventChangeViewModel.AcceptRenameEventCommand.Subscribe(e => UpdateEventIfNameChanged(e));
            eventChangeViewModel.Events.CollectionChanged += (_, e) => OnEventDelete(e, eventChangeViewModel.Events[0].Event);

            var result = await ShowEventChangeDialog.Handle(eventChangeViewModel);
            if (result is not null && result.Event.Id != CurrentEvent.Id)
            {
                CurrentEvent = result.Event;
                _applicationCache.LastChoosenEvent = CurrentEvent.Id!;
                await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(true);
            }
        }

        private void UpdateEventIfNameChanged(Event @event)
        {
            if (CurrentEvent.Id == @event.Id && CurrentEvent.Name != @event.Name)
                CurrentEvent = new Event(CurrentEvent.Id!, @event.Name);
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