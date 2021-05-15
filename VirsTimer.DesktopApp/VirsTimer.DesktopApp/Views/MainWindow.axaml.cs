using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public class MainWindow : Window
    {
        private readonly IEventsGetter eventsGetter;
        private readonly ISessionsManager sessionsManager;

        public MainWindowViewModel ViewModel { get; }

        public ICommand ChangeEventCommand { get; }
        public ICommand ChangeSessionCommand { get; }
        public ReactiveCommand<Solve, Unit> EditSolveCommand { get; }

        private event Func<Task> Constructed;

        public MainWindow() { }

        public MainWindow(MainWindowViewModel mainWindowViewModel, IEventsGetter eventsGetter, ISessionsManager sessionsManager)
        {
            InitializeComponent();
            ViewModel = mainWindowViewModel;
            DataContext = this;
#if DEBUG
            this.AttachDevTools();
#endif
            this.Constructed += LoadSolvesAsync;
            this.eventsGetter = eventsGetter;
            this.sessionsManager = sessionsManager;


            ChangeEventCommand = ReactiveCommand.CreateFromTask(ChangeEventAsync);
            ChangeSessionCommand = ReactiveCommand.CreateFromTask(ChangeSessionAsync);
            EditSolveCommand = ReactiveCommand.CreateFromTask<Solve>(EditSolveAsync);
            Constructed();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task ChangeEventAsync()
        {
            var eventChangeViewModel = new EventChangeViewModel(eventsGetter);
            var dialog = new EventChangeView
            {
                DataContext = eventChangeViewModel
            };

            await dialog.ShowDialog(this);
            if (eventChangeViewModel.Accepted)
            {
                ViewModel.EventViewModel.CurrentEvent = eventChangeViewModel.SelectedEvent!;
                await ViewModel.SolvesListViewModel.Load(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
            }
        }

        private async Task ChangeSessionAsync()
        {
            var sessionChangeViewModel = new SessionChangeViewModel(ViewModel.EventViewModel.CurrentEvent, sessionsManager);
            var dialog = new SessionChangeView
            {
                DataContext = sessionChangeViewModel
            };

            await dialog.ShowDialog(this);
            if (sessionChangeViewModel.Accepted)
            {
                ViewModel.SessionViewModel.CurrentSession = sessionChangeViewModel.SelectedSession!;
                await ViewModel.SolvesListViewModel.Load(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
            }
        }

        private async Task EditSolveAsync(Solve solve)
        {
            var solveInfoViewModel = new SolveInfoViewModel(solve);
            var dialog = new SolveInfoView
            {
                DataContext = solveInfoViewModel
            };
            await dialog.ShowDialog(this);
            if (solveInfoViewModel.Accepted)
                await ViewModel.SolvesListViewModel.Save(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
        }

        public async void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning && !ViewModel.TimerViewModel.Timer.CountdownStarted)
                ViewModel.TimerViewModel.Timer.StartCountdown();
            else if (ViewModel.TimerViewModel.Timer.IsRunning)
            {
                ViewModel.TimerViewModel.Timer.Stop();
                ViewModel.SolvesListViewModel.Solves.Insert(0, new Solve(ViewModel.TimerViewModel.SavedTime, ViewModel.ScrambleViewModel.CurrentScramble.Value));
                await ViewModel.SolvesListViewModel.Save(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
                ViewModel.ScrambleViewModel.NextScramble();
            }
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning)
                ViewModel.TimerViewModel.Timer.Start();
        }

        private async Task LoadSolvesAsync()
        {
            await ViewModel.SolvesListViewModel.Load(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
        }
    }
}
