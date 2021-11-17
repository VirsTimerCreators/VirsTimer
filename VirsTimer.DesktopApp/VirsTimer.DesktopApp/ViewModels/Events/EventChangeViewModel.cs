using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Events;
using VirsTimer.DesktopApp.Commands;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventChangeViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;

        public bool Accepted { get; private set; } = false;

        public ObservableCollection<EventViewModel> Events { get; private set; } = null!;

        [Reactive]
        public EventViewModel? SelectedEvent { get; set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; }
        public ReactiveCommand<EventViewModel, Event> AcceptRenameEventCommand { get; private set; } = null!;
        public AsyncRelayCommand<EventViewModel> DeleteEventCommand { get; }

        public EventChangeViewModel(IEventsRepository? eventsRepository = null)
        {
            _eventsRepository = eventsRepository ?? Ioc.GetService<IEventsRepository>();

            var canAccpet = this.WhenAnyValue<EventChangeViewModel, bool, EventViewModel?>(x => x.SelectedEvent, x => x != null);
            AcceptCommand = ReactiveCommand.Create<Window>(AcceptEvent, canAccpet);

            AddEventCommand = ReactiveCommand.CreateFromTask(AddEventAsync);

            DeleteEventCommand = new AsyncRelayCommand<EventViewModel>(DeleteEventAsync);
        }

        public override async Task ConstructAsync()
        {
            var repositoryResponse = await _eventsRepository.GetEventsAsync().ConfigureAwait(false);
            var eventViewModels = repositoryResponse.Value.Select(e => new EventViewModel(e));
            Events = new(eventViewModels);

            var canAcceptRenaming = Events
                .ToObservableChangeSet()
                .AutoRefresh(x => x.Name)
                .ToCollection()
                .Select(vms =>
                {
                    return vms.All(vm => !string.IsNullOrWhiteSpace(vm.Name))
                    && vms.AllDistinctBy(vm => vm.Name)
                    && Server.Events.All
                    .All(serverEvent => vms.Count(vm => Server.Events.GetServerEventName(vm.Name) == serverEvent) <= 1);
                });

            AcceptRenameEventCommand = ReactiveCommand.CreateFromTask<EventViewModel, Event>(AcceptRename, canAcceptRenaming);
        }

        private void AcceptEvent(Window window)
        {
            Accepted = SelectedEvent != null;
            window.Close();
        }

        private async Task AddEventAsync()
        {
            var maxEventNumber = Events
                .Select(eventVm => Constants.Events.NewEventNameRegex.Match(eventVm.Event.Name))
                .Where(match => match.Success)
                .Select(match => int.Parse(match.Groups[1].Value))
                .DefaultIfEmpty(0)
                .Max();
            var nextAvailableNumber = maxEventNumber + 1;
            var @event = new Event($"{Constants.Events.NewEventNameBase}{nextAvailableNumber}");
            await _eventsRepository.AddEventAsync(@event).ConfigureAwait(true);

            var eventVm = new EventViewModel(@event);
            Events.Add(eventVm);
        }

        private async Task<Event> AcceptRename(EventViewModel eventViewModel)
        {
            if (eventViewModel.Name == eventViewModel.Event.Name)
            {
                eventViewModel.EditingEvent = false;
                return eventViewModel.Event;
            }

            eventViewModel.Event.Name = eventViewModel.Name;
            await _eventsRepository.UpdateEventAsync(eventViewModel.Event).ConfigureAwait(false);
            eventViewModel.EditingEvent = false;
            return eventViewModel.Event;
        }

        private async Task DeleteEventAsync(EventViewModel eventViewModel)
        {
            await _eventsRepository.DeleteEventAsync(eventViewModel.Event).ConfigureAwait(false);
            Events.Remove(eventViewModel);
        }
    }
}