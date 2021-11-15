using DynamicData;
using DynamicData.Binding;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Extensions;
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
        public ObservableCollection<SolveViewModel> Solves { get; private set; } = new ObservableCollection<SolveViewModel>();

        public Task Construct(ObservableCollection<SolveViewModel> solves)
        {
            Solves = solves;
            var refreshOnFlagChangeObservable = Observer.Create<object>(async _ => await Refresh());
            Solves.ToObservableChangeSet()
                .AutoRefresh(x => x.Flag)
                .Subscribe(refreshOnFlagChangeObservable);

            return Task.CompletedTask;
        }

        private async Task Refresh()
        {
            await RefreshBests();
            await CalculateStatistics();
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
            var mo3Task = Task.Run(() => BestMo3 = Solves.StepCollection(3).Min(t => t.Select(x => x.Model).Mo3()));
            var ao5Task = Task.Run(() => BestAo5 = Solves.StepCollection(5).Min(f => f.Select(x => x.Model).Ao5()));

            var ao12Task = Task.Run(() => BestAo12 = Solves.StepCollection(12).Min(t => t.Select(x => x.Model).Ao12()));
            var ao100Task = Task.Run(() => BestAo100 = Solves.StepCollection(100).Min(h => h.Select(x => x.Model).Ao100()));
            return Task.WhenAll(mo3Task, ao5Task, ao12Task, ao100Task);
        }

        private void CaclulateBest()
        {
            if (Solves.Count > 0)
            {
                CurrentTime = Solves[0].Model.TimeWithFlag;
                BestTime = Solves.Select(x => x.Model).BestTime();
                return;
            }

            CurrentTime = null;
            BestTime = null;
        }

        private void CalculateMo3()
        {
            CurrentMo3 = Solves.Select(x => x.Model).Mo3();
            if (BestMo3 == null || CurrentMo3 < BestMo3)
                BestMo3 = CurrentMo3;
        }

        private void CalculateAo5()
        {
            CurrentAo5 = Solves.Select(x => x.Model).Ao5();
            if (BestAo5 == null || CurrentAo5 < BestAo5)
                BestAo5 = CurrentAo5;
        }

        private void CalculateAo12()
        {
            CurrentAo12 = Solves.Select(x => x.Model).Ao12();
            if (BestAo12 == null || CurrentAo12 < BestAo12)
                BestAo12 = CurrentAo12;
        }

        private void CalculateAo100()
        {
            CurrentAo100 = Solves.Select(x => x.Model).Ao100();
            if (BestAo100 == null || CurrentAo100 < BestAo100)
                BestAo100 = CurrentAo100;
        }
    }
}