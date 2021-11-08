using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Avalonia.Controls;
using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using VirsTimer.Core.Extensions;
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
        public SessionViewModel? SelectedSession { get; set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }
        public ReactiveCommand<Unit, Unit> AddSessionCommand { get; }
        public ReactiveCommand<SessionViewModel, Unit> AcceptRenameSessionCommand { get; private set; } = null!;
        public ReactiveCommand<SessionViewModel, Unit> DeleteSessionCommand { get; }

        public SessionChangeViewModel(Event @event, ISessionRepository? sessionRepository = null)
        {
            _event = @event;
            _sessionRepository = sessionRepository ?? Ioc.GetService<ISessionRepository>();

            var acceptEnabled = this.WhenAnyValue<SessionChangeViewModel, bool, SessionViewModel?>(x => x.SelectedSession, x => x != null);
            AcceptCommand = ReactiveCommand.Create<Window>(AcceptSession, acceptEnabled);
            AddSessionCommand = ReactiveCommand.CreateFromTask(AddSessionAsync);

            var canDelete = this.WhenAnyValue(x => x.Sessions.Count, x => x > 1);
            DeleteSessionCommand = ReactiveCommand.CreateFromTask<SessionViewModel>(DeleteSessionAsync, canDelete);
        }

        public override async Task ConstructAsync()
        {
            var repositoryResponse = await _sessionRepository.GetSessionsAsync(_event).ConfigureAwait(false);
            var sessions = repositoryResponse.Value.Select(session => new SessionViewModel(session)).OrderBy(x => x.Name).ToList();
            Sessions = new(sessions);

            var canAcceptRenaming = Sessions
                .ToObservableChangeSet()
                .AutoRefresh(x => x.Name)
                .ToCollection()
                .Select(vms =>
                {
                    return vms.All(vm => !string.IsNullOrWhiteSpace(vm.Name))
                    && vms.AllDistinctBy(vm => vm.Name);
                });

            AcceptRenameSessionCommand = ReactiveCommand.CreateFromTask<SessionViewModel>(AcceptRename, canAcceptRenaming);
        }

        private void AcceptSession(Window window)
        {
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

        private async Task DeleteSessionAsync(SessionViewModel sessionViewModel)
        {
            await _sessionRepository.DeleteSessionAsync(sessionViewModel.Session).ConfigureAwait(false);
            Sessions.Remove(sessionViewModel);
        }
    }
}