using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.ViewModels.Rooms;
using System;
using Avalonia.Input;

namespace VirsTimer.DesktopApp.Views.Rooms
{
    public partial class RoomView : ReactiveWindow<RoomViewModel>
    {
        public TextBlock AccessCodeTextBlock { get; }
        public TextBlock StatusTextBlock { get; }
        public Button CopyButton { get; }
        public Button StartButton { get; }
        public Button ExitButton { get; }
        public Image CopyImage { get; }

        public RoomView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            AccessCodeTextBlock = this.FindControl<TextBlock>("AccessCodeTextBlock");
            StatusTextBlock = this.FindControl<TextBlock>("StatusTextBlock");
            CopyButton = this.FindControl<Button>("CopyButton");
            StartButton = this.FindControl<Button>("StartButton");
            ExitButton = this.FindControl<Button>("ExitButton");
            CopyImage = this.FindControl<Image>("CopyImage");

            this.WhenActivated(async disposableRegistration =>
            {
                this.Bind(
                    ViewModel,
                    viewModel => viewModel.AccessCode,
                    view => view.AccessCodeTextBlock.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.Status,
                    view => view.StatusTextBlock.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.CopyImage,
                    view => view.CopyImage.Source)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.CopyToClipboardCommand,
                    view => view.CopyButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.StartCommand,
                    view => view.StartButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.ExitCommand,
                    view => view.ExitButton)
                .DisposeWith(disposableRegistration);

                ViewModel!.ExitCommand.Subscribe(_ => Close()).DisposeWith(disposableRegistration);

                await ViewModel.ConstructAsync().ConfigureAwait(false);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (ViewModel!.Status != "Rozpoczęto")
                return;

            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning && !ViewModel.TimerViewModel.Timer.CountdownStarted)
                ViewModel.TimerViewModel.Timer.StartCountdown();
            else if (ViewModel.TimerViewModel.Timer.IsRunning)
            {
                ViewModel.TimerViewModel.Timer.Stop();

                // after stop
            }
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning)
                ViewModel.TimerViewModel.Timer.Start();
        }
    }
}