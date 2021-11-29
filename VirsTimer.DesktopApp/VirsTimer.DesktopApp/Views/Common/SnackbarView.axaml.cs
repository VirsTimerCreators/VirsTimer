using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
        private const int HeightConst = 64;
        private static readonly TimeSpan Speed = TimeSpan.FromMilliseconds(8);
        private static readonly TimeSpan Break = TimeSpan.FromMilliseconds(2000);

        private readonly SemaphoreSlim _semaphoreSlim;

        public Border Border { get; }
        public TextBlock MessageTextBlock { get; }

        public SnackbarView()
        {
            _semaphoreSlim = new(1, 1);

            InitializeComponent();
            Border = this.FindControl<Border>("Border");
            MessageTextBlock = this.FindControl<TextBlock>("MessageTextBlock");

            Canvas.SetBottom(Border, HeightConst);

            this.WhenActivated(disposableRegistration =>
            {
                this.OneWayBind(
                    ViewModel,
                    viewModel => viewModel.Message,
                    view => view.MessageTextBlock.Text)
                .DisposeWith(disposableRegistration);

                ViewModel!.QueueMessage.Subscribe(async _ => await Enqueue()).DisposeWith(disposableRegistration);

                _semaphoreSlim.DisposeWith(disposableRegistration);

                Parent!.ZIndex = -100;
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
            Parent!.ZIndex = 100;

            for (var i = HeightConst; i >= 0; i -= 4)
            {
                await Task.Delay(Speed);
                Canvas.SetTop(Border, i);
            }

            await Task.Delay(Break);

            for (var i = 0; i <= HeightConst; i += 4)
            {
                await Task.Delay(Speed);
                Canvas.SetTop(Border, i);
            }

            Parent!.ZIndex = -100;
        }
    }
}