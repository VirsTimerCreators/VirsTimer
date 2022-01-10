using Avalonia.Media;
using Avalonia.Media.Imaging;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Cache;
using VirsTimer.Core.Services.Sessions;
using VirsTimer.DesktopApp.ValueConverters;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels.Sessions
{
    public class SessionSummaryViewModel : ViewModelBase
    {
        private readonly ISessionsRepository _sessionRepository;
        private readonly IApplicationCache _applicationCache;
        private readonly IApplicationCacheSaver _applicationCacheSaver;
        private readonly IValueConverter<string, Bitmap> _svgToBitmapConverter;
        private readonly SnackbarViewModel _snackbarViewModel;

        private Event _event = null!;

        [Reactive]
        public Session CurrentSession { get; set; } = null!;

        [Reactive]
        public IImage? ChooseSessionImage { get; private set; }

        public ReactiveCommand<Unit, Unit> ChangeSessionCommand { get; }

        public Interaction<SessionChangeViewModel, SessionViewModel?> ShowSessionChangeDialog { get; }

        public SessionSummaryViewModel(
            SnackbarViewModel snackbarViewModel,
            ISessionsRepository? sessionRepository = null,
            IApplicationCache? applicationCache = null,
            IApplicationCacheSaver? applicationCacheSaver = null)
        {
            _svgToBitmapConverter = new SvgToBitmapConverter(100);
            _snackbarViewModel = snackbarViewModel;
            _sessionRepository = sessionRepository ?? Ioc.GetService<ISessionsRepository>();
            _applicationCache = applicationCache ?? Ioc.GetService<IApplicationCache>();
            _applicationCacheSaver = applicationCacheSaver ?? Ioc.GetService<IApplicationCacheSaver>();
            ChangeSessionCommand = ReactiveCommand.CreateFromTask(ChangeSessionAsync);
            ShowSessionChangeDialog = new Interaction<SessionChangeViewModel, SessionViewModel?>();
        }

        public override async Task<bool> ConstructAsync()
        {
            var sessionSvg = await File.ReadAllTextAsync("Assets/session.svg");

            ChooseSessionImage = _svgToBitmapConverter.Convert(sessionSvg);
            return true;
        }

        public async Task LoadSessionAsync(Event @event)
        {
            IsBusy = true;
            _event = @event;
            var repositoryResponse = await _sessionRepository.GetSessionsAsync(_event);
            if (repositoryResponse.IsSuccesfull is false)
            {
                await ShutdownDialogHandleAsync("Nie można załadować sesji.");
                IsBusy = false;
                return;
            }

            var sessions = repositoryResponse.Value.ToList();
            if (sessions.IsNullOrEmpty())
            {
                var session = new Session(@event, $"{Constants.Sessions.NewSessionNameBase}1");
                var response = await _sessionRepository.AddSessionAsync(session);
                sessions.Add(session);

                if (response.IsSuccesfull is false)
                {
                    await ShutdownDialogHandleAsync("Nie można załadować sesji.");
                    IsBusy = false;
                    return;
                }
            }

            CurrentSession = sessions.Find(x => x.Id == _applicationCache.LastChoosenSession) ?? sessions[0];
            IsBusy = false;
        }

        private async Task ChangeSessionAsync()
        {
            var sessionChangeViewModel = new SessionChangeViewModel(_event, _sessionRepository);
            var contructed = await sessionChangeViewModel.ConstructAsync().ConfigureAwait(true);
            if (contructed is false)
            {
                await CloseWindowDialogHandleAsync("Nie można załadować sesji");
                return;
            }

            sessionChangeViewModel.AcceptRenameSessionCommand.Subscribe(UpdateSessionIfNameChanged);
            sessionChangeViewModel.Sessions.CollectionChanged += (_, o) => OnSessionDelete(o, sessionChangeViewModel);

            var result = await ShowSessionChangeDialog.Handle(sessionChangeViewModel);
            if (result is not null && result.Session.Id != CurrentSession.Id)
            {
                CurrentSession = result.Session;
                _applicationCache.LastChoosenSession = CurrentSession.Id!;
                await _applicationCacheSaver.SaveCacheAsync(_applicationCache).ConfigureAwait(true);
            }
        }

        private void UpdateSessionIfNameChanged(Session session)
        {
            if (CurrentSession.Id == session.Id && CurrentSession.Name != session.Name)
                CurrentSession = new Session(CurrentSession.Event, CurrentSession.Id!, session.Name);
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