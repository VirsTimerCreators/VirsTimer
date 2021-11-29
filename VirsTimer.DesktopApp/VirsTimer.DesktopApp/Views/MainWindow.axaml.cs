using Avalonia;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Export;
using VirsTimer.DesktopApp.Views.Export;

namespace VirsTimer.DesktopApp.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            this.WhenActivated(async disposableRegistration =>
            {
                ViewModel.ShowExportDialog.RegisterHandler(DoShowExportDialogAsync).DisposeWith(disposableRegistration);

                await ViewModel!.ConstructAsync().ConfigureAwait(false);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task DoShowExportDialogAsync(InteractionContext<ExportsViewModel, Unit> interaction)
        {
            var dialog = new ExportsView
            {
                DataContext = interaction.Input
            };

            var output = await dialog.ShowDialog<Unit>(this);
            interaction.SetOutput(output);
        }

        public async void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning && !ViewModel.TimerViewModel.Timer.CountdownStarted)
                ViewModel.TimerViewModel.Timer.StartCountdown();
            else if (ViewModel.TimerViewModel.Timer.IsRunning)
            {
                ViewModel.TimerViewModel.Timer.Stop();

                var solve = new Solve(
                    ViewModel.SessionSummaryViewModel.CurrentSession,
                    ViewModel.TimerViewModel.SavedTime,
                    ViewModel.ScrambleViewModel.CurrentScramble.Value);

                await ViewModel.SaveSolveAsync(solve).ConfigureAwait(false);
            }
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning)
                ViewModel.TimerViewModel.Timer.Start();
        }
    }
}