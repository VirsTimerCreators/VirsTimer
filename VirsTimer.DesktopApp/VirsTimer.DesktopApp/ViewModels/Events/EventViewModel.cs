using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Models;
using CoreEvents = VirsTimer.Core.Constants.Events;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventViewModel : ViewModelBase
    {
        private bool _editingEvent = false;

        public Event Event { get; }

        [Reactive]
        public string Name { get; set; }

        [ObservableAsProperty]
        public bool IsPredefined { get; set; }

        public bool EditingEvent
        {
            get => _editingEvent;
            set
            {
                this.RaiseAndSetIfChanged(ref _editingEvent, value);
                Name = Event.Name;
            }
        }

        [ObservableAsProperty]
        public bool CanEdit { get; set; }

        public ReactiveCommand<Unit, Unit> RenameCommand { get; }

        public EventViewModel(Event @event)
        {
            Event = @event;
            Name = Event.Name;

            this.WhenAnyValue(x => x.Name)
                .Select(n => CoreEvents.Predefined.Any(e => e == n))
                .ToPropertyEx(this, x => x.IsPredefined);

            this.WhenAnyValue(
                x => x.EditingEvent,
                x => x.IsPredefined,
                (x, y) => !x && !y)
                .ToPropertyEx(this, x => x.CanEdit);

            RenameCommand = ReactiveCommand.Create(() => { EditingEvent = true; });
        }
    }
}