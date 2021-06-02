using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolveAddViewModel : ViewModelBase
    {
        [Reactive]
        public TimeSpan SolveTime { get; set; } = TimeSpan.Zero;
        public bool Accepted { get; private set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }

        public SolveAddViewModel()
        {
            var acceptedEnabled = this.WhenAnyValue(x => x.SolveTime, x => x > TimeSpan.Zero);
            AcceptCommand = ReactiveCommand.Create<Window>(SaveSolve, acceptedEnabled);
        }

        private void SaveSolve(Window window)
        {
            Accepted = true;
            window.Close();
        }
    }
}
