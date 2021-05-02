using ReactiveUI;
using System.Reactive.Linq;
using System.Windows.Input;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        private Event currentEvent = new Event("3x3x3");
        public Event CurrentEvent
        {
            get => currentEvent;
            set => this.RaiseAndSetIfChanged(ref currentEvent, value);
        }

        public Interaction<EventChangeViewModel, Event> ShowDialog { get; }

        public ICommand ChangeEventCommand { get; }

        public EventViewModel()
        {
            ShowDialog = new Interaction<EventChangeViewModel, Event>();
            ChangeEventCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var changeView = new EventChangeViewModel();
                var result = await ShowDialog.Handle(changeView);
            })
        }
    }
}
