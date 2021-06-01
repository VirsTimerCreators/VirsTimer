using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Solves;
using VirsTimer.DesktopApp.Views.Solves;

namespace VirsTimer.DesktopApp.Views
{
    public class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; }
        public ICommand AddSolveManualyCommand { get; }
        public ReactiveCommand<SolveViewModel, Unit> EditSolveCommand { get; }

        public MainWindow()
        {
            InitializeComponent();

#if DEBUG
            this.AttachDevTools();
#endif

            ViewModel = new MainWindowViewModel(new Event(Server.Events.ThreeByThree));
            EditSolveCommand = ReactiveCommand.CreateFromTask<SolveViewModel>(EditSolveAsync);
            AddSolveManualyCommand = ReactiveCommand.Create(AddSolveManually);
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task EditSolveAsync(SolveViewModel solveViewModel)
        {
            var dialog = new SolveView
            {
                DataContext = solveViewModel
            };
            await dialog.ShowDialog(this);
            if (solveViewModel.Accepted)
                await ViewModel.SolvesListViewModel.SaveAsync(ViewModel.EventViewModel.CurrentEvent, ViewModel.SessionSummaryViewModel.CurrentSession);
        }

        private async Task AddSolveManually()
        {
            var solveAddViewModel = new SolveAddViewModel();
            var solveAddView = new SolveAddView
            {
                DataContext = solveAddViewModel
            };
            await solveAddView.ShowDialog(this);
            if (!solveAddViewModel.Accepted)
                return;
            var solve = new Solve(solveAddViewModel.SolveTime, ViewModel.ScrambleViewModel.CurrentScramble.Value);
            await ViewModel.SaveSolveAsync(solve);
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
                await ViewModel.SaveSolveAsync(solve);
            }
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning)
                ViewModel.TimerViewModel.Timer.Start();
        }
    }
}
