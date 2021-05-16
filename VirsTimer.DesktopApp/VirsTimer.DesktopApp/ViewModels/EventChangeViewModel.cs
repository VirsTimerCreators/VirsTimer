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
        private Event? _selectedEvent;

        public bool Accepted { get; private set; } = false;
        public ObservableCollection<Event> Events { get; }
        public Event? SelectedEvent
        {
            get => _selectedEvent;
            set => this.RaiseAndSetIfChanged(ref _selectedEvent, value);
        }

        public ICommand AcceptCommand { get; }

        public EventChangeViewModel()
        {
            Events = new ObservableCollection<Event>(Ioc.GetService<IEventsGetter>().GetEventsAsync().GetAwaiter().GetResult());
            AcceptCommand = ReactiveCommand.Create<Window>((window) =>
            {
                Accepted = SelectedEvent != null;
                window.Close();
            });
        }
    }
}
