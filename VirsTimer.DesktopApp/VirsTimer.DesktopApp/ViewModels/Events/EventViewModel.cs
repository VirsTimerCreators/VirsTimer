using System.Reactive;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventViewModel : ViewModelBase
    {
        private bool _editingEvent = false;

        public Event Event { get; }

        [Reactive]
        public string Name { get; set; }

        public bool EditingEvent
        {
            get => _editingEvent;
            set
            {
                this.RaiseAndSetIfChanged(ref _editingEvent, value);
                Name = Event.Name;
            }
        }

        public ReactiveCommand<Unit, Unit> RenameCommand { get; }

        public EventViewModel(Event @event)
        {
            Event = @event;
            Name = Event.Name;

            RenameCommand = ReactiveCommand.Create(() => { EditingEvent = true; });
        }
    }
}