using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using System;
using VirsTimer.Core.Timers;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public class TimerView : Window
    {
        private readonly TextBlock timerTextBlock;

        public DelayStopwatchTimer DelayFireTimer => Model.Model;
        public TimerViewModel Model { get; }

        public TimerView()
        {
            InitializeComponent();
            timerTextBlock = this.FindControl<TextBlock>("TimerTextBlock");
            DataContext = Model = new TimerViewModel();
            DelayFireTimer.AddEvent(TimerFireEvent);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (!Model.Model.IsRunning && !DelayFireTimer.CountdownStarted && keyEventArgs.Key == Key.Space)
            {
                //timerTextBlock.Foreground = Brushes.MediumVioletRed;
                DelayFireTimer.StartCountdown();
            }
            else if (Model.Model.IsRunning)
                Model.Model.InvertWork();
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (!Model.Model.IsRunning && keyEventArgs.Key == Key.Space)
            {
                DelayFireTimer.Start();
            }
        }

        public void TimerFireEvent(object? sender, EventArgs eventArgs)
        {
            //timerTextBlock.Foreground = Brushes.GreenYellow;
        }
    }
}
