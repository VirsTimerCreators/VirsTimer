using Avalonia.Controls;
using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using System.ComponentModel;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;
using VirsTimer.Core.Services.Events;
using VirsTimer.Core.Services.Sessions;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.ViewModels.Events;
using VirsTimer.DesktopApp.ViewModels.Scrambles;
using VirsTimer.DesktopApp.ViewModels.Sessions;
using VirsTimer.DesktopApp.ViewModels.Solves;
using VirsTimer.DesktopApp.Views.Solves;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly ISolvesRepository _solvesRepository;

        public EventViewModel EventViewModel { get; }
        public SessionSummaryViewModel SessionSummaryViewModel { get; }
        public TimerViewModel TimerViewModel { get; }
        public SolvesListViewModel SolvesListViewModel { get; }
        public ScrambleViewModel ScrambleViewModel { get; }

        public ReactiveCommand<Window, Unit> AddSolveManualyCommand { get; }

        public MainWindowViewModel(IEventsRepository eventsRepository, ISessionRepository sessionRepository, ISolvesRepository solvesRepository, IScrambleGenerator scrambleGenerator)
        {
            _solvesRepository = Ioc.Services.GetRequiredService<ISolvesRepository>();

            EventViewModel = new EventViewModel(eventsRepository);
            SessionSummaryViewModel = new SessionSummaryViewModel(sessionRepository);
            TimerViewModel = new TimerViewModel();
            SolvesListViewModel = new SolvesListViewModel(solvesRepository);
            ScrambleViewModel = new ScrambleViewModel(scrambleGenerator);

            AddSolveManualyCommand = ReactiveCommand.CreateFromTask<Window>(AddSolveManually);
        }

        public override async Task ConstructAsync()
        {
            await EventViewModel.ConstructAsync().ConfigureAwait(false);
            await SessionSummaryViewModel.ChangeSessionAsync(EventViewModel.CurrentEvent).ConfigureAwait(false);
            await SolvesListViewModel.ChangeSessionAsync(SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
            await ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent).ConfigureAwait(false);

            EventViewModel.PropertyChanged += OnEventChangeAsync;
            SessionSummaryViewModel.PropertyChanged += OnSessionChangeAsync;
        }

        public async Task SaveSolveAsync(Solve solve)
        {
            SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve, _solvesRepository));
            await _solvesRepository.AddSolveAsync(solve).ConfigureAwait(false);
            await ScrambleViewModel.GetNextScrambleAsync().ConfigureAwait(false);
        }

        private async Task AddSolveManually(Window window)
        {
            var solveAddViewModel = new SolveAddViewModel();
            var solveAddView = new SolveAddView
            {
                DataContext = solveAddViewModel
            };
            await solveAddView.ShowDialog(window);
            if (!solveAddViewModel.Accepted)
                return;
            var solve = new Solve(
                SessionSummaryViewModel.CurrentSession,
                solveAddViewModel.SolveTime,
                ScrambleViewModel.CurrentScramble.Value);

            await _solvesRepository.AddSolveAsync(solve).ConfigureAwait(false);
        }

        private async void OnEventChangeAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(EventViewModel.CurrentEvent))
                return;

            await ScrambleViewModel.ChangeEventAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);
            await SessionSummaryViewModel.ChangeSessionAsync(EventViewModel.CurrentEvent).ConfigureAwait(true);
        }

        private async void OnSessionChangeAsync(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(SessionSummaryViewModel.CurrentSession))
                return;

            await SolvesListViewModel.ChangeSessionAsync(SessionSummaryViewModel.CurrentSession).ConfigureAwait(false);
        }
    }
}
