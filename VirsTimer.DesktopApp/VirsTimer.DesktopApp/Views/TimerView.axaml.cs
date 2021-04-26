using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public class TimerView : Window
    {
        public TimerViewModel Model { get; }

        public TimerView()
        {
            InitializeComponent();
            Model = new TimerViewModel();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public void WindowKeyDown(object? sender, KeyEventArgs keyEventArgs)
        {
            keyEventArgs.Handled = true;
            if (keyEventArgs.Key == Key.Space && !Model.Timer.IsRunning && !Model.Timer.CountdownStarted)
                Model.Timer.StartCountdown();
            else if (Model.Timer.IsRunning)
                Model.Timer.InvertWork();
        }

        public void WindowKeyUp(object? sender, KeyEventArgs keyEventArgs)
        {
            if (keyEventArgs.Key == Key.Space && !Model.Timer.IsRunning)
                Model.Timer.Start();
        }
    }
}
