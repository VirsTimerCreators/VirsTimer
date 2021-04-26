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

        public DelayFireTimer DelayFireTimer => Model.DelayFireTimer;
        public TimerViewModel Model { get; }

        public TimerView()
        {
            InitializeComponent();
            DataContext = this;
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
            if (!Model.Model.IsRunning && !DelayFireTimer.IsRunning && keyEventArgs.Key == Key.Space)
            {
                //timerTextBlock.Foreground = Brushes.MediumVioletRed;
                DelayFireTimer.Start();
            }
            else if (Model.Model.IsRunning)
                Model.Model.InvertWork();
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (!Model.Model.IsRunning && keyEventArgs.Key == Key.Space)
            {
                DelayFireTimer.Stop();
                //timerTextBlock.Foreground = Brushes.DarkGray;
                if (DelayFireTimer.CanFire)
                    Model.Model.InvertWork();
                DelayFireTimer.ResetCanFire();
            }
        }

        public void TimerFireEvent(object? sender, EventArgs eventArgs)
        {
            //timerTextBlock.Foreground = Brushes.GreenYellow;
        }
    }
}
