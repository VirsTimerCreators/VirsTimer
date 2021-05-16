using Avalonia.Controls;
using ReactiveUI;
using System.Collections.ObjectModel;
using System.Windows.Input;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SessionChangeViewModel : ViewModelBase
    {
        private readonly ISessionsManager _sessionsManager;
        private Session? _selectedSession;

        public bool Accepted { get; private set; } = false;
        public ObservableCollection<Session> Sessions { get; }
        public Session? SelectedSession
        {
            get => _selectedSession;
            set => this.RaiseAndSetIfChanged(ref _selectedSession, value);
        }
        public ICommand AcceptCommand { get; }

        public SessionChangeViewModel(Event @event)
        {
            _sessionsManager = Ioc.GetService<ISessionsManager>();
            Sessions = new ObservableCollection<Session>(_sessionsManager.GetSessionsAsync(@event).GetAwaiter().GetResult());
            AcceptCommand = ReactiveCommand.Create<Window>(AcceptSession);
        }

        private void AcceptSession(Window window)
        {
            Accepted = SelectedSession != null;
            window.Close();
        }
    }
}
