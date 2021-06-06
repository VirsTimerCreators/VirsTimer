using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.Extensions;
using VirsTimer.DesktopApp.Views.Solves;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolveViewModel : ViewModelBase
    {
        private readonly ISolvesRepository _solvesRepository;
        public static event EventHandler FlagChanged;

        public bool Accepted { get; private set; }
        public Solve Model { get; }
        public TimeSpan Time { get; }
        public SolveFlag Flag { get; set; }
        public DateTime Date { get; }
        public string Scramble { get; }

        [Reactive]
        public string Summary { get; set; } = string.Empty;

        [Reactive]
        public string Index { get; set; } = "-1";

        public ReactiveCommand<Window, Unit> EditSolveCommand { get; }
        public SolveFlagsViewModel SolveFlagsViewModel { get; }
        public ReactiveCommand<Window, Unit> AcceptCommand { get; }

        public SolveViewModel(Solve solve, ISolvesRepository solvesSaver)
        {
            _solvesRepository = solvesSaver;

            Model = solve;
            Time = solve.TimeAsSpan;
            Flag = solve.Flag;
            Date = solve.Date;
            Scramble = solve.Scramble;

            SolveFlagsViewModel = new SolveFlagsViewModel(solve.Flag);

            EditSolveCommand = ReactiveCommand.CreateFromTask<Window>(EditSolve);
            AcceptCommand = ReactiveCommand.CreateFromTask<Window>(SaveFlag);

            UpdateSummary();
        }

        private async Task EditSolve(Window window)
        {
            var dialog = new SolveView
            {
                DataContext = this
            };
            await dialog.ShowDialog(window);
        }

        private async Task SaveFlag(Window window)
        {
            //TODO watchout for exception - rollback flag change
            Accepted = Flag != SolveFlagsViewModel.ChoosenFlag;
            Model.Flag = Flag = SolveFlagsViewModel.ChoosenFlag;
            await _solvesRepository.UpdateSolveAsync(Model).ConfigureAwait(true);
            FlagChanged(this.Model, EventArgs.Empty);
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
