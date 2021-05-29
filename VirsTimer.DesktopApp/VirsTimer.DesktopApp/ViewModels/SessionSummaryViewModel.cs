using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.Linq;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SessionSummaryViewModel : ViewModelBase
    {
        private Session _currentSession = new();
        public Session CurrentSession
        {
            get => _currentSession;
            set => this.RaiseAndSetIfChanged(ref _currentSession, value);
        }

        public SessionSummaryViewModel(Event @event)
        {
            CurrentSession = Ioc.Services.GetRequiredService<ISessionsManager>().GetSessionsAsync(@event).GetAwaiter().GetResult().FirstOrDefault() ?? new();
        }
    }
}
