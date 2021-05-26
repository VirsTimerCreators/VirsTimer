using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolveAddViewModel : ViewModelBase
    {
        private TimeSpan _solveTime = TimeSpan.Zero;
        public TimeSpan SolveTime
        {
            get => _solveTime;
            set => this.RaiseAndSetIfChanged(ref _solveTime, value);
        }
        public bool Accepted { get; private set; }
        public ICommand AcceptCommand { get; }

        public SolveAddViewModel()
        {
            AcceptCommand = ReactiveCommand.Create<Window>(SaveSolve);
        }

        private void SaveSolve(Window window)
        {
            Accepted = true;
            window.Close();
        }
    }
}
