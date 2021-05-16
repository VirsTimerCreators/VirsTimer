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
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Solves;
using VirsTimer.DesktopApp.Views.Solves;

namespace VirsTimer.DesktopApp.Views
{
    public class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; }
        public ICommand ChangeEventCommand { get; }
        public ICommand ChangeSessionCommand { get; }
        public ReactiveCommand<SolveViewModel, Unit> EditSolveCommand { get; }

        private event Func<Task> Constructed;

        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            this.AttachDevTools();
#endif
            this.Constructed += LoadSolvesAsync;

            ViewModel = new MainWindowViewModel(new Event("3x3x3"));
            ChangeEventCommand = ReactiveCommand.CreateFromTask(ChangeEventAsync);
            ChangeSessionCommand = ReactiveCommand.CreateFromTask(ChangeSessionAsync);
            EditSolveCommand = ReactiveCommand.CreateFromTask<SolveViewModel>(EditSolveAsync);
            DataContext = this;

            Constructed();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task ChangeEventAsync()
        {
            var eventChangeViewModel = new EventChangeViewModel();
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
            var sessionChangeViewModel = new SessionChangeViewModel(ViewModel.EventViewModel.CurrentEvent);
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

        private async Task EditSolveAsync(SolveViewModel solveViewModel)
        {
            var dialog = new SolveView
            {
                DataContext = solveViewModel
            };
            await dialog.ShowDialog(this);
            if (solveViewModel.Accepted)
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

                var solve = new Solve(ViewModel.TimerViewModel.SavedTime, ViewModel.ScrambleViewModel.CurrentScramble.Value);
                ViewModel.SolvesListViewModel.Solves.Insert(0, new SolveViewModel(solve));
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
