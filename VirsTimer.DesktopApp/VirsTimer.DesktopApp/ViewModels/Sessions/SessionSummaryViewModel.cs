using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Specialized;
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
        public Session CurrentSession { get; set; } = null!;

        public ReactiveCommand<Window, Unit> ChangeSessionCommand { get; }

        public SessionSummaryViewModel(ISessionRepository? sessionRepository = null)
        {
            _sessionRepository = sessionRepository ?? Ioc.GetService<ISessionRepository>();
            ChangeSessionCommand = ReactiveCommand.CreateFromTask<Window>(ChangeSessionAsync);
        }

        public async Task LoadSessionAsync(Event @event)
        {
            IsBusy = true;
            _event = @event;
            var repositoryResponse = await _sessionRepository.GetSessionsAsync(_event).ConfigureAwait(false);
            var sessions = repositoryResponse.Value.ToList();
            if (sessions.IsNullOrEmpty())
            {
                var session = new Session(@event, $"{Constants.Sessions.NewSessionNameBase}1");
                await _sessionRepository.AddSessionAsync(session).ConfigureAwait(false);
                sessions.Add(session);
            }

            CurrentSession = sessions[0];
            IsBusy = false;
        }

        private async Task ChangeSessionAsync(Window window)
        {
            var sessionChangeViewModel = new SessionChangeViewModel(_event, _sessionRepository);
            await sessionChangeViewModel.ConstructAsync().ConfigureAwait(true);
            var dialog = new SessionChangeView
            {
                DataContext = sessionChangeViewModel
            };

            sessionChangeViewModel.Sessions.CollectionChanged += (_, o) => OnSessionDelete(o, sessionChangeViewModel);
            await dialog.ShowDialog(window);
            if (sessionChangeViewModel.Accepted)
                CurrentSession = sessionChangeViewModel.SelectedSession!.Session;
        }

        private void OnSessionDelete(NotifyCollectionChangedEventArgs e, SessionChangeViewModel sessionChangeViewModel)
        {
            if (e.Action != NotifyCollectionChangedAction.Remove)
                return;

            foreach (SessionViewModel session in e.OldItems)
            {
                if (CurrentSession.Id != session.Session.Id)
                    continue;
                CurrentSession = sessionChangeViewModel.Sessions[0].Session;
                break;
            }
        }
    }
}
