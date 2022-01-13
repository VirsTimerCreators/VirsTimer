using DynamicData;
using DynamicData.Alias;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Events;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventChangeViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;

        public ObservableCollection<EventViewModel> Events { get; private set; } = null!;

        [Reactive]
        public EventViewModel? SelectedEvent { get; set; }

        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public ReactiveCommand<Unit, EventViewModel?> AcceptCommand { get; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; }
        public ReactiveCommand<EventViewModel, Event> AcceptRenameEventCommand { get; private set; } = null!;
        public ReactiveCommand<EventViewModel, Unit> DeleteEventCommand { get; }

        public EventChangeViewModel(IEventsRepository? eventsRepository = null)
        {
            _eventsRepository = eventsRepository ?? Ioc.GetService<IEventsRepository>();

            CancelCommand = ReactiveCommand.Create(() => { });

            var canAccpet = this.WhenAnyValue<EventChangeViewModel, bool, EventViewModel?>(x => x.SelectedEvent, x => x != null);
            AcceptCommand = ReactiveCommand.Create(() => SelectedEvent, canAccpet);

            AddEventCommand = ReactiveCommand.CreateFromTask(AddEventAsync);

            DeleteEventCommand = ReactiveCommand.CreateFromTask<EventViewModel>(DeleteEventAsync);
        }

        public override async Task<bool> ConstructAsync()
        {
            var repositoryResponse = await _eventsRepository.GetEventsAsync().ConfigureAwait(false);
            if (repositoryResponse.IsSuccesfull is false)
                return false;

            var eventViewModels = repositoryResponse.Value.Select(e => new EventViewModel(e));
            Events = new(eventViewModels);

            var canAcceptRenaming = Events
                .ToObservableChangeSet()
                .AutoRefresh(x => x.Name)
                .ToCollection()
                .Select(vms =>
                {
                    return vms.All(vm => vm?.Name?.Length > 0 && vm?.Name?.Length < 41)
                    && vms.AllDistinctBy(vm => vm.Name)
                    && Server.Events.All
                    .All(serverEvent => vms.Count(vm => Server.Events.GetServerEventName(vm.Name) == serverEvent) <= 1);
                });

            AcceptRenameEventCommand = ReactiveCommand.CreateFromTask<EventViewModel, Event>(AcceptRename, canAcceptRenaming);
            return true;
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