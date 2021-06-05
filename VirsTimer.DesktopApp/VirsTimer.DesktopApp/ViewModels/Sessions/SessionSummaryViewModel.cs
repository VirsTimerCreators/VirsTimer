using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.Generic;
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
        private IReadOnlyList<Session>? _sessions;
        private Event _event = null!;

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
            _event = @event;
            var repositoryResponse = await _sessionRepository.GetSessionsAsync(_event).ConfigureAwait(false);
            _sessions = repositoryResponse.Value;
            if (_sessions.IsNullOrEmpty())
            {
                var session = new Session(@event, $"{Constants.Sessions.NewSessionNameBase}1");
                await _sessionRepository.AddSessionAsync(session).ConfigureAwait(false);
                return;
            }

            CurrentSession = _sessions[0];
        }

        private async Task ChangeSessionAsync(Window window)
        {
            var sessionChangeViewModel = new SessionChangeViewModel(_event, _sessionRepository);
            var dialog = new SessionChangeView
            {
                DataContext = sessionChangeViewModel
            };

            await dialog.ShowDialog(window);
            if (sessionChangeViewModel.Accepted)
                CurrentSession = sessionChangeViewModel.SelectedSession!.Session;
        }
    }
}
