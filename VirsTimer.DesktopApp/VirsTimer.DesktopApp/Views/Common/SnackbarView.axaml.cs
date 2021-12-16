using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.Views.Common
{
    public partial class SnackbarView : ReactiveUserControl<SnackbarViewModel>
    {
        private static readonly TimeSpan Speed = TimeSpan.FromMilliseconds(8);

        private ContentControl _parent = null!;
        private readonly SemaphoreSlim _semaphoreSlim = new(1, 1);

        public Border Border { get; }
        public TextBlock MessageTextBlock { get; }
        public Panel MainPanel { get; }

        public SnackbarView()
        {
            _semaphoreSlim = new(1, 1);

            InitializeComponent();
            Border = this.FindControl<Border>("Border");
            MessageTextBlock = this.FindControl<TextBlock>("MessageTextBlock");
            MainPanel = this.FindControl<Panel>("MainPanel");

            this.WhenActivated(disposableRegistration =>
            {
                _parent = this.FindAncestorOfType<ContentControl>();
                _parent.ZIndex = -1;

                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.Message,
                    view => view.MessageTextBlock.Text)
                .DisposeWith(disposableRegistration);

                ViewModel!.QueueMessage.Subscribe(async _ => await Enqueue()).DisposeWith(disposableRegistration);

                _semaphoreSlim.DisposeWith(disposableRegistration);

                Canvas.SetBottom(Border, ViewModel.Height);

                Parent!.ZIndex = -100;
                MainPanel.Width = ViewModel.Width;
                Border.Width = ViewModel.Width;
                MainPanel.Height = ViewModel.Height;
                Border.Height = ViewModel.Height;
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public async Task Enqueue()
        {
            if (ViewModel!.Disposed)
                return;

            await _semaphoreSlim.WaitAsync();

            await MoveCanvasAsync();

            if (ViewModel!.Disposed)
                return;

            _semaphoreSlim.Release();
        }

        public async Task MoveCanvasAsync()
        {
            _parent.ZIndex = 5;
            for (var i = ViewModel.Height; i >= 0; i -= 4)
            {
                await Task.Delay(Speed);
                Canvas.SetTop(Border, i);
            }

            await Task.Delay(TimeSpan.FromSeconds(ViewModel.Length));

            for (var i = 0; i <= ViewModel.Height; i += 4)
            {
                await Task.Delay(Speed);
                Canvas.SetTop(Border, i);
            }

            _parent.ZIndex = -5;
        }
    }
}