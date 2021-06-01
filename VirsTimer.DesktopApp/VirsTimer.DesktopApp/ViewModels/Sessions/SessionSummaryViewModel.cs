using Microsoft.Extensions.DependencyInjection;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Sessions;

namespace VirsTimer.DesktopApp.ViewModels.Sessions
{
    public class SessionSummaryViewModel : ViewModelBase
    {
        private readonly ISessionsManager _sessionsManager;
        private IReadOnlyList<Session>? _sessions;
        private Event _event;

        [Reactive]
        public Session CurrentSession { get; set; } = null!;

        public SessionSummaryViewModel(Event @event)
        {
            _sessionsManager = Ioc.Services.GetRequiredService<ISessionsManager>();
            _event = @event;
            OnConstructedAsync(this, EventArgs.Empty);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            await ChangeSessionAsync(_event).ConfigureAwait(false);
        }

        public async Task ChangeSessionAsync(Event @event)
        {
            _event = @event;
            _sessions = await _sessionsManager.GetAllSessionsAsync(_event).ConfigureAwait(false);
            CurrentSession = !_sessions.IsNullOrEmpty()
                ? _sessions[0]
                : await _sessionsManager.AddSessionAsync(_event, $"{Constants.Sessions.NewSessionNameBase}1").ConfigureAwait(false);
        }
    }
}
