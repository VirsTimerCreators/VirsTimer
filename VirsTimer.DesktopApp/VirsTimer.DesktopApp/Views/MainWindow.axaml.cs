using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Export;
using VirsTimer.DesktopApp.Views.Export;
using VirsTimer.DesktopApp.ViewModels.Rooms;
using VirsTimer.DesktopApp.Views.Rooms;

namespace VirsTimer.DesktopApp.Views
{
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        public SplitView SplitView { get; }

        public Panel TimerPanel { get; }

        public ContentControl ScrambleContenteControl { get; }

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            SplitView = this.FindControl<SplitView>("SplitView");
            TimerPanel = this.FindControl<Panel>("TimerPanel");
            ScrambleContenteControl = this.FindControl<ContentControl>("ScrambleContenteControl");

            Opened += (_, _) => OnOpen();

            this.WhenActivated(async disposableRegistration =>
            {
                ViewModel.ShowRoomCreationDialog.RegisterHandler(DoShowRoomCreationDialogAsync).DisposeWith(disposableRegistration);
                ViewModel.ShowRoomDialog.RegisterHandler(DoShowRoomDialogAsync).DisposeWith(disposableRegistration);
                ViewModel.ShowExportDialog.RegisterHandler(DoShowExportDialogAsync).DisposeWith(disposableRegistration);

                await ViewModel!.ConstructAsync();
            });
        }

        public void OnOpen()
        {
            TimerPanel.Margin = Height switch
            {
                >= ScreenHeight.Big => new Thickness(0, 50, 0, 50),
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => new Thickness(0, 35, 0, 35),
                (< ScreenHeight.Medium) => new Thickness(0, 15, 0, 15),
                _ => new Thickness(0, 15, 0, 15)
            };

            ScrambleContenteControl.MaxHeight = Height switch
            {
                >= ScreenHeight.Big => 300,
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => 225,
                (< ScreenHeight.Medium) => 150,
                _ => 150
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task DoShowRoomCreationDialogAsync(InteractionContext<RoomCreationViewModel, RoomViewModel?> interaction)
        {
            var dialog = new RoomCreationView
            {
                DataContext = interaction.Input
            };

            var output = await dialog.ShowDialog<RoomViewModel?>(this);
            interaction.SetOutput(output);
        }

        private Task DoShowRoomDialogAsync(InteractionContext<RoomViewModel, Unit> interaction)
        {
            var dialog = new RoomView
            {
                DataContext = interaction.Input
            };

            dialog.Show(this);
            dialog.Closed += async (_, _) => await interaction.Input.ExitCommand.Execute();
            interaction.SetOutput(Unit.Default);

            return Task.CompletedTask;
        }

        private async Task DoShowExportDialogAsync(InteractionContext<ExportsViewModel, Unit> interaction)
        {
            var dialog = new ExportsView
            {
                DataContext = interaction.Input
            };

            var output = await dialog.ShowDialog<Unit>(this);
            interaction.Input.SnackbarViewModel.Disposed = true;
            interaction.SetOutput(output);
        }

        public async void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (ViewModel.ScrambleViewModel.Loading)
                return;

            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning && !ViewModel.TimerViewModel.Timer.CountdownStarted)
                ViewModel.TimerViewModel.Timer.StartCountdown();
            else if (ViewModel.TimerViewModel.Timer.IsRunning)
            {
                ViewModel.TimerViewModel.Timer.Stop();

                var solve = new Solve(
                    ViewModel.SessionViewModel.CurrentSession,
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

        public void ShowMenu(object? sender, PointerEventArgs e)
        {
            SplitView.IsPaneOpen = true;
        }

        public void HideMenu(object? sender, PointerEventArgs e)
        {
            SplitView.IsPaneOpen = false;
        }
    }
}