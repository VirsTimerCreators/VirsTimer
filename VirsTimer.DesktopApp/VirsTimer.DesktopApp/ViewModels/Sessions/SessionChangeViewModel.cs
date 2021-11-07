using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Sessions;

namespace VirsTimer.DesktopApp.ViewModels.Sessions
{
    public class SessionChangeViewModel : ViewModelBase
    {
        private readonly Event _event;
        private readonly ISessionRepository _sessionRepository;

        public bool Accepted { get; private set; } = false;

        [Reactive]
        public ObservableCollection<SessionViewModel> Sessions { get; set; } = null!;

        [Reactive]
        public bool CanRename { get; set; } = true;

        [Reactive]
        public SessionViewModel? SelectedSession { get; set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }
        public ReactiveCommand<Unit, Unit> AddSessionCommand { get; }
        public ReactiveCommand<SessionViewModel, Unit> AcceptRenameSessionCommand { get; }
        public ReactiveCommand<SessionViewModel, Unit> DeleteSessionCommand { get; }

        public SessionChangeViewModel(Event @event, ISessionRepository? sessionRepository = null)
        {
            _event = @event;
            _sessionRepository = sessionRepository ?? Ioc.GetService<ISessionRepository>();

            var acceptEnabled = this.WhenAnyValue<SessionChangeViewModel, bool, SessionViewModel?>(x => x.SelectedSession, x => x != null);
            AcceptCommand = ReactiveCommand.Create<Window>(AcceptSession, acceptEnabled);
            AddSessionCommand = ReactiveCommand.CreateFromTask(AddSessionAsync);

            var acceptRenameSessionEnabled = this.WhenAnyValue(x => x.CanRename, x => x == true);
            AcceptRenameSessionCommand = ReactiveCommand.CreateFromTask<SessionViewModel>(AcceptRename, acceptRenameSessionEnabled);

            var canDelete = this.WhenAnyValue(x => x.Sessions.Count, x => x > 1);
            DeleteSessionCommand = ReactiveCommand.CreateFromTask<SessionViewModel>(DeleteSessionAsync, canDelete);

        }

        public override async Task ConstructAsync()
        {
            var repositoryResponse = await _sessionRepository.GetSessionsAsync(_event).ConfigureAwait(false);
            var sessions = repositoryResponse.Value.Select(session => new SessionViewModel(session)).OrderBy(x => x.Name).ToList();
            Sessions = new(sessions);

            foreach (var session in Sessions)
                session.PropertyChanged += UpdateCanRename;
        }

        private void AcceptSession(Window window)
        {
            foreach (var session in Sessions)
                session.PropertyChanged -= UpdateCanRename;
            Accepted = SelectedSession != null;
            window.Close();
        }

        private async Task AddSessionAsync()
        {
            var maxSessionNumber = Sessions
                .Select(sessionVM => Constants.Sessions.NewSessionNameRegex.Match(sessionVM.Session.Name))
                .Where(match => match.Success)
                .Select(match => int.Parse(match.Groups[1].Value))
                .DefaultIfEmpty(0)
                .Max();
            var nextAvailableNumber = maxSessionNumber + 1;
            var session = new Session(_event, $"{Constants.Sessions.NewSessionNameBase}{nextAvailableNumber}");
            await _sessionRepository.AddSessionAsync(session).ConfigureAwait(true);

            var sessionVM = new SessionViewModel(session);
            sessionVM.PropertyChanged += UpdateCanRename;
            Sessions.Add(sessionVM);
        }

        private async Task AcceptRename(SessionViewModel sessionViewModel)
        {
            if (sessionViewModel.Name == sessionViewModel.Session.Name)
            {
                sessionViewModel.EditingSession = false;
                return;
            }

            sessionViewModel.Session.Name = sessionViewModel.Name;
            await _sessionRepository.UpdateSessionAsync(sessionViewModel.Session).ConfigureAwait(false);
            sessionViewModel.EditingSession = false;
        }

        private void UpdateCanRename(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SessionViewModel.Name))
                return;

            var names = Sessions.Select(x => x.Name).ToList();
            CanRename = names.All(name => !string.IsNullOrWhiteSpace(name)) && names.Count == names.Distinct().Count();
        }

        private async Task DeleteSessionAsync(SessionViewModel sessionViewModel)
        {
            await _sessionRepository.DeleteSessionAsync(sessionViewModel.Session).ConfigureAwait(false);
            Sessions.Remove(sessionViewModel);
        }
    }
}