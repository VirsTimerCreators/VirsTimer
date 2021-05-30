using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reactive;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Sessions;

namespace VirsTimer.DesktopApp.ViewModels.Sessions
{
    public class SessionChangeViewModel : ViewModelBase
    {
        private const string SessionNameBase = "Sesja";
        private static readonly Regex DefaultSessionNameRegex = new($"{SessionNameBase}([0-9]+)", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly Event _event;
        private readonly ISessionsManager _sessionsManager;

        public bool Accepted { get; private set; } = false;

        [Reactive]
        public ObservableCollection<SessionViewModel> Sessions { get; set; }

        [Reactive]
        public bool CanRename { get; set; } = true;

        [Reactive]
        public SessionViewModel? SelectedSession { get; set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }
        public ReactiveCommand<Unit, bool> AddSessionCommand { get; }
        public ReactiveCommand<SessionViewModel, bool> AcceptRenameSessionCommand { get; }

        public SessionChangeViewModel(Event @event)
        {
            _event = @event;
            _sessionsManager = Ioc.GetService<ISessionsManager>();
            Sessions = new ObservableCollection<SessionViewModel>();

            var acceptEnabled = this.WhenAnyValue<SessionChangeViewModel, bool, SessionViewModel?>(x => x.SelectedSession, x => x != null);
            AcceptCommand = ReactiveCommand.Create<Window>(AcceptSession, acceptEnabled);
            AddSessionCommand = ReactiveCommand.CreateFromTask(AddSessionAsync);

            var acceptRenameSessionEnabled = this.WhenAnyValue(x => x.CanRename, x => x == true);
            AcceptRenameSessionCommand = ReactiveCommand.CreateFromTask<SessionViewModel, bool>(AcceptRename, acceptRenameSessionEnabled);
            LoadSessionsAsync();
        }

        private async void LoadSessionsAsync()
        {
            var sessions = await _sessionsManager.GetAllSessionsAsync(_event).ConfigureAwait(false);
            var sessionsVM = sessions.Select(session => new SessionViewModel(this, session));
            Sessions = new ObservableCollection<SessionViewModel>(sessionsVM);
            foreach (var session in Sessions)
                session.PropertyChanged += UpdateCanRename;
        }

        private void AcceptSession(Window window)
        {
            Accepted = SelectedSession != null;
            window.Close();
        }

        private async Task<bool> AddSessionAsync()
        {
            var maxSessionNumber = Sessions
                .Select(sessionVM => DefaultSessionNameRegex.Match(sessionVM.Session.Name))
                .Where(match => match.Success)
                .Select(match => int.Parse(match.Groups[1].Value))
                .DefaultIfEmpty(0)
                .Max();
            var nextAvailableNumber = maxSessionNumber + 1;
            var session = await _sessionsManager.AddSessionAsync(_event, $"{SessionNameBase}{nextAvailableNumber}");
            if (session == null)
                return false;

            var sessionVM = new SessionViewModel(this, session);
            sessionVM.PropertyChanged += UpdateCanRename;
            Sessions.Add(sessionVM);
            return true;
        }

        public async Task<bool> AcceptRename(SessionViewModel sessionViewModel)
        {
            sessionViewModel.EditingSession = false;
            if (sessionViewModel.Name == sessionViewModel.Session.Name)
                return true;

            var renamed = await _sessionsManager.RenameSessionAsync(_event, sessionViewModel.Session, sessionViewModel.Name).ConfigureAwait(false);
            Sessions[Sessions.IndexOf(sessionViewModel)] = new SessionViewModel(this, renamed);
            return renamed != null;
        }

        private void UpdateCanRename(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SessionViewModel.Name))
                return;

            var names = Sessions.Select(x => x.Name).ToList();
            CanRename = names.All(name => !string.IsNullOrWhiteSpace(name)) && names.Count == names.Distinct().Count();
        }
    }
}
