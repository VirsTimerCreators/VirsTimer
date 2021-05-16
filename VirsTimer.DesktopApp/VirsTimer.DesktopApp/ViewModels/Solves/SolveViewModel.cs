using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Windows.Input;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolveViewModel : ViewModelBase
    {
        private string _summary = string.Empty;

        public bool Accepted { get; private set; }
        public Solve Model { get; }
        public TimeSpan Time { get; }
        public SolveFlag Flag { get; set; }
        public DateTime Date { get; }
        public string Scramble { get; }
        public string Summary
        {
            get => _summary;
            set => this.RaiseAndSetIfChanged(ref _summary, value);
        }
        public SolveFlagsViewModel SolveFlagsViewModel { get; }
        public ICommand AcceptCommand { get; }

        public SolveViewModel(Solve solve)
        {
            Model = solve;
            Time = solve.TimeAsSpan;
            Flag = solve.Flag;
            Date = solve.Date;
            Scramble = solve.Scramble;

            SolveFlagsViewModel = new SolveFlagsViewModel(solve.Flag);
            AcceptCommand = ReactiveCommand.Create<Window>(SaveFlag);

            UpdateSummary();
        }

        private void SaveFlag(Window window)
        {
            Accepted = Flag != SolveFlagsViewModel.ChoosenFlag;
            Model.Flag = Flag = SolveFlagsViewModel.ChoosenFlag;
            UpdateSummary();
            window.Close();
        }

        private void UpdateSummary() =>
            Summary = Flag switch
            {
                SolveFlag.OK => Time.ToDynamicString(),
                SolveFlag.DNF => $"{SolveFlag.DNF} ({Time.ToDynamicString()})",
                SolveFlag.Plus2 => $"{Time.Add(TimeSpan.FromSeconds(2)).ToDynamicString()} (+2)",
                _ => throw new ArgumentException(nameof(Flag))
            };
    }
}
