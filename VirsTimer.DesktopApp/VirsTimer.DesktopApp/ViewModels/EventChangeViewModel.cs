using Avalonia.Controls;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class EventChangeViewModel : ViewModelBase
    {
        private Event? selectedEvent;
        public Event? SelectedEvent
        {
            get => selectedEvent;
            set => this.RaiseAndSetIfChanged(ref selectedEvent, value);
        }

        public bool Accepted { get; private set; } = false;

        public ObservableCollection<Event> Events { get; }

        public ICommand AcceptCommand { get; }
        public EventChangeViewModel(IEventsGetter eventsGetter)
        {
            Events = new ObservableCollection<Event>(eventsGetter.GetEventsAsync().GetAwaiter().GetResult());
            AcceptCommand = ReactiveCommand.Create<Window>((window) =>
            {
                Accepted = SelectedEvent != null;
                window.Close();
            });
        }
    }
}
