using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Windows.Input;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; }

        public ICommand ChangeEventCommand { get; }
        public ICommand ChangeSessionCommand { get; }

        public MainWindow() { }

        public MainWindow(MainWindowViewModel mainWindowViewModel, IEventsGetter eventsGetter, ISessionsManager sessionsManager)
        {
            InitializeComponent();
            ViewModel = mainWindowViewModel;
            DataContext = this;
#if DEBUG
            this.AttachDevTools();
#endif

            ViewModel.SolvesListViewModel.Load(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession).GetAwaiter().GetResult();
            ChangeEventCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var dialog = new EventChangeView();
                var eventChangeViewModel = new EventChangeViewModel(eventsGetter);
                dialog.DataContext = eventChangeViewModel;

                await dialog.ShowDialog(this);
                if (eventChangeViewModel.Accepted && eventChangeViewModel.SelectedEvent != null)
                {
                    ViewModel.EventViewModel.CurrentEvent = eventChangeViewModel.SelectedEvent;
                    await ViewModel.SolvesListViewModel.Load(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
                }
            });

            ChangeSessionCommand = ReactiveCommand.CreateFromTask(async () =>
            {
                var dialog = new SessionChangeView();
                var sessionChangeViewModel = new SessionChangeViewModel(mainWindowViewModel.EventViewModel.CurrentEvent, sessionsManager);
                dialog.DataContext = sessionChangeViewModel;

                await dialog.ShowDialog(this);
                if (sessionChangeViewModel.Accepted && sessionChangeViewModel.SelectedSession != null)
                {
                    ViewModel.SessionViewModel.CurrentSession = sessionChangeViewModel.SelectedSession;
                    await ViewModel.SolvesListViewModel.Load(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
                }
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning && !ViewModel.TimerViewModel.Timer.CountdownStarted)
                ViewModel.TimerViewModel.Timer.StartCountdown();
            else if (ViewModel.TimerViewModel.Timer.IsRunning)
            {
                ViewModel.TimerViewModel.Timer.Stop();
                ViewModel.SolvesListViewModel.Solves.Insert(0, new Solve(ViewModel.TimerViewModel.SavedTime, ViewModel.ScrambleViewModel.CurrentScramble.Value));
                ViewModel.SolvesListViewModel.Save(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionViewModel.CurrentSession);
                ViewModel.ScrambleViewModel.NextScramble();
            }
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning)
                ViewModel.TimerViewModel.Timer.Start();
        }
    }
}
