using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Utils;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomUserViewModel : ViewModelBase
    {
        private readonly int _scramblesAmount;

        public string UserName { get; set; }

        [Reactive]
        public TimeSpan? Best { get; set; }

        [Reactive]
        public TimeSpan? Worst { get; set; }

        [Reactive]
        public TimeSpan? Ao { get; set; }

        [Reactive]
        public TimeSpan? Avg { get; set; }

        [Reactive]
        public ObservableCollection<RoomUserSolveViewModel> Solves { get; set; } = new();

        public RoomUserViewModel(
            string userName,
            int scramblesAmount)
        {
            UserName = userName;
            _scramblesAmount = scramblesAmount;
            Solves.CollectionChanged += UpdateIndexesAndStatisticAsync;
        }

        private async void UpdateIndexesAndStatisticAsync(object? sender, EventArgs e)
        {
            var tasks = Solves.Select(solve => Task.Run(() => solve.Index = $"{Solves.Count - Solves.IndexOf(solve)}."));
            await Task.WhenAll(tasks).ConfigureAwait(false);
            await Refresh().ConfigureAwait(false);
        }

        private Task Refresh()
        {
            var bestTask = Task.Run(() => CaclulateBest());
            var worstTask = Task.Run(() => CaclulateWorst());
            var aoTask = Task.Run(() => CaclulateAo());
            var avgTask = Task.Run(() => CaclulateAvg());
            return Task.WhenAll(bestTask, worstTask, aoTask, avgTask);
        }

        private void CaclulateBest()
        {
            if (Solves.Count > 0)
            {
                Best = Solves.Select(x => x.Model).BestTime();
                return;
            }

            Best = null;
        }

        private void CaclulateWorst()
        {
            if (Solves.Count > 0)
            {
                Worst = Solves.Select(x => x.Model).WorstTime();
                return;
            }

            Worst = null;
        }

        private void CaclulateAo()
        {
            if (Solves.Count > 2)
            {
                Ao = Solves.Select(x => x.Model).Ao(_scramblesAmount);
                return;
            }

            Ao = null;
        }

        private void CaclulateAvg()
        {
            if (Solves.Count > 0)
            {
                Avg = Solves.Select(x => x.Model).Average();
                return;
            }

            Avg = null;
        }
    }
}