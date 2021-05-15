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
        private Session? selectedSession;
        public Session? SelectedSession
        {
            get => selectedSession;
            set => this.RaiseAndSetIfChanged(ref selectedSession, value);
        }

        public bool Accepted { get; private set; } = false;

        public ObservableCollection<Session> Sessions { get; }

        public ICommand AddCommand { get; }
        public ICommand AcceptCommand { get; }

        public SessionChangeViewModel(Event @event, ISessionsManager sessionsManager)
        {
            Sessions = new ObservableCollection<Session>(sessionsManager.GetSessionsAsync(@event).GetAwaiter().GetResult());
            AddCommand = AcceptCommand = ReactiveCommand.Create(() =>
            {
                var session = new Session();
                sessionsManager.AddSessionAsync(@event, session);
            });

            AcceptCommand = ReactiveCommand.Create<Window>((window) =>
            {
                Accepted = SelectedSession != null;
                window.Close();
            });
        }
    }
}
