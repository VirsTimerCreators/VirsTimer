using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Sessions;
using VirsTimer.DesktopApp.Views.Sessions;

namespace VirsTimer.DesktopApp.ViewModels.Sessions
{
    public class SessionSummaryViewModel : ViewModelBase
    {
        private readonly ISessionRepository _sessionRepository;
        private Event _event = null!;

        [Reactive]
        public ObservableCollection<SessionViewModel> Sessions { get; set; } = null!;

        [Reactive]
        public Session CurrentSession { get; set; } = null!;

        public ReactiveCommand<Window, Unit> ChangeSessionCommand { get; }

        public SessionSummaryViewModel(ISessionRepository sessionRepository)
        {
            _sessionRepository = sessionRepository;
            ChangeSessionCommand = ReactiveCommand.CreateFromTask<Window>(ChangeSessionAsync);
        }

        public async Task ChangeSessionAsync(Event @event)
        {
            IsBusy = true;
            _event = @event;
            var repositoryResponse = await _sessionRepository.GetSessionsAsync(_event).ConfigureAwait(false);
            var sessions = repositoryResponse.Value.Select(session => new SessionViewModel(session)).OrderBy(x => x.Name).ToList();
            if (sessions.IsNullOrEmpty())
            {
                var session = new Session(@event, $"{Constants.Sessions.NewSessionNameBase}1");
                await _sessionRepository.AddSessionAsync(session).ConfigureAwait(false);
                sessions.Add(new SessionViewModel(session));
            }

            Sessions = new(sessions);
            CurrentSession = Sessions[0].Session;
            IsBusy = false;
        }

        private async Task ChangeSessionAsync(Window window)
        {
            var sessionChangeViewModel = new SessionChangeViewModel(_event, _sessionRepository, Sessions);
            var dialog = new SessionChangeView
            {
                DataContext = sessionChangeViewModel
            };

            sessionChangeViewModel.DeleteSessionCommand.Subscribe(Observer.Create<Session>(session => ChangeSessionIfDeleted(session)));
            await dialog.ShowDialog(window);
            if (sessionChangeViewModel.Accepted)
                CurrentSession = sessionChangeViewModel.SelectedSession!.Session;
        }

        void ChangeSessionIfDeleted(Session session)
        {
            if (session == CurrentSession)
                CurrentSession = Sessions[0].Session;
        }
    }
}
