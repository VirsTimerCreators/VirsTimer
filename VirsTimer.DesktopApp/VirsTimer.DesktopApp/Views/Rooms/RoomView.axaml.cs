using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels;
using VirsTimer.DesktopApp.ViewModels.Rooms;

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

        public ContentControl TimerPanel { get; }

        public ContentControl ScrambleContenteControl { get; }

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

            TimerPanel = this.FindControl<ContentControl>("TimerPanel");
            ScrambleContenteControl = this.FindControl<ContentControl>("ScrambleContenteControl");

            Opened += (_, _) => OnOpen();

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

        public void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (ViewModel!.Status != "Rozpoczêto" || ViewModel.ScrambleViewModel.Current is null)
                return;

            if (keyEventArgs.Key == Key.Space
                && ViewModel.TimerContent is TimerViewModel
                && !ViewModel.TimerViewModel.Timer.IsRunning
                && !ViewModel.TimerViewModel.Timer.CountdownStarted)
                ViewModel.TimerViewModel.Timer.StartCountdown();
            else if (ViewModel.TimerViewModel.Timer.IsRunning)
            {
                ViewModel.TimerViewModel.Timer.Stop();
            }
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space && !ViewModel.TimerViewModel.Timer.IsRunning)
                ViewModel.TimerViewModel.Timer.Start();
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
    }
}