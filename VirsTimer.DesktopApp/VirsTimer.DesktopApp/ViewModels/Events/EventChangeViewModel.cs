using System.Collections.ObjectModel;
using System.Reactive;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Services.Events;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventChangeViewModel : ViewModelBase
    {
        private readonly IEventsRepository _eventsRepository;

        public bool Accepted { get; private set; }

        public ObservableCollection<EventViewModel> Events { get; }

        [Reactive]
        public EventViewModel? SelectedEvent { get; set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }
        public ReactiveCommand<Unit, Unit> AddEventCommand { get; }
        public ReactiveCommand<EventViewModel, Unit> AcceptRenameEventCommand { get; }
        public ReactiveCommand<EventViewModel, Unit> DeleteEventCommand { get; }

        public EventChangeViewModel(IEventsRepository? eventsRepository = null)
        {
            _eventsRepository = eventsRepository ?? Ioc.GetService<IEventsRepository>();
        }
    }
}
