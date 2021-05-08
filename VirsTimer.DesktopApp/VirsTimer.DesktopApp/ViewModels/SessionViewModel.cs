using ReactiveUI;
using System.Linq;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SessionViewModel : ViewModelBase
    {
        private Session currentSession = new();
        public Session CurrentSession
        {
            get => currentSession;
            set => this.RaiseAndSetIfChanged(ref currentSession, value);
        }

        public SessionViewModel(Event @event, ISessionsManager sessionsManager)
        {
            CurrentSession = sessionsManager.GetSessionsAsync(@event).GetAwaiter().GetResult().FirstOrDefault() ?? new();
        }
    }
}
