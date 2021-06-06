using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Utils;
using VirsTimer.DesktopApp.ViewModels.Solves;

namespace VirsTimer.DesktopApp.ViewModels.Statistics
{
    public class StatisticsViewModel : ViewModelBase
    {
        [Reactive]
        public TimeSpan? CurrentTime { get; private set; }

        [Reactive]
        public TimeSpan? CurrentMo3 { get; private set; }

        [Reactive]
        public TimeSpan? CurrentAo5 { get; private set; }

        [Reactive]
        public TimeSpan? CurrentAo12 { get; private set; }

        [Reactive]
        public TimeSpan? CurrentAo100 { get; private set; }

        [Reactive]
        public TimeSpan? BestTime { get; private set; }

        [Reactive]
        public TimeSpan? BestMo3 { get; private set; }

        [Reactive]
        public TimeSpan? BestAo5 { get; private set; }

        [Reactive]
        public TimeSpan? BestAo12 { get; private set; }

        [Reactive]
        public TimeSpan? BestAo100 { get; private set; }

        [Reactive]
        public ObservableCollection<Solve> Solves { get; private set; } = new ObservableCollection<Solve>();

        public StatisticsViewModel()
        {
            SolveViewModel.FlagChanged += OnFlagChanged;
        }

        public async Task Construct(IEnumerable<Solve> solves)
        {
            Solves = new ObservableCollection<Solve>(solves);
            await Refresh().ConfigureAwait(false);
        }

        public Task AddSolve(Solve solve)
        {
            Solves.Add(solve);
            return CalculateStatistics();
        }

        public async Task DeleteSolve(Solve solve)
        {
            Solves.Remove(solve);
            await Refresh().ConfigureAwait(false);
        }

        private async void OnFlagChanged(object? sender, EventArgs e)
        {
            await Refresh().ConfigureAwait(false);
        }

        private async Task Refresh()
        {
            await RefreshBests().ConfigureAwait(false);
            await CalculateStatistics().ConfigureAwait(false);
        }

        private Task CalculateStatistics()
        {
            var bestTask = Task.Run(() => CaclulateBest());
            var mo3Task = Task.Run(() => CalculateMo3());
            var ao5Task = Task.Run(() => CalculateAo5());
            var ao12Task = Task.Run(() => CalculateAo12());
            var ao100Task = Task.Run(() => CalculateAo100());
            return Task.WhenAll(bestTask, mo3Task, ao5Task, ao12Task, ao100Task);
        }

        private Task RefreshBests()
        {
            var bestTask = Task.Run(() => CaclulateBest());
            var mo3Task = Task.Run(() => BestMo3 = Solves.StepCollection(3).Min(five => five.ToList().Mo3()));
            var ao5Task = Task.Run(() => BestAo5 = Solves.StepCollection(5).Min(five => five.ToList().Ao5()));
            var ao12Task = Task.Run(() => BestAo12 = Solves.StepCollection(12).Min(five => five.ToList().Ao12()));
            var ao100Task = Task.Run(() => BestAo100 = Solves.StepCollection(100).Min(five => five.ToList().Ao100()));
            return Task.WhenAll(mo3Task, ao5Task, ao12Task, ao100Task);
        }

        private void CaclulateBest()
        {
            if (Solves.Count > 0)
            {
                CurrentTime = Solves[^1].TimeWithFlag;
                BestTime = Solves.BestTime();
                return;
            }

            CurrentTime = null;
            BestTime = null;
        }

        private void CalculateMo3()
        {
            CurrentMo3 = Solves.Mo3();
            if (BestMo3 == null || CurrentMo3 < BestMo3)
                BestMo3 = CurrentMo3;
        }

        private void CalculateAo5()
        {
            CurrentAo5 = Solves.Ao5();
            if (BestAo5 == null || CurrentAo5 < BestAo5)
                BestAo5 = CurrentAo5;
        }

        private void CalculateAo12()
        {
            CurrentAo12 = Solves.Ao12();
            if (BestAo12 == null || CurrentAo12 < BestAo12)
                BestAo12 = CurrentAo12;
        }

        private void CalculateAo100()
        {
            CurrentAo100 = Solves.Ao100();
            if (BestAo100 == null || CurrentAo100 < BestAo100)
                BestAo100 = CurrentAo100;
        }
    }
}
