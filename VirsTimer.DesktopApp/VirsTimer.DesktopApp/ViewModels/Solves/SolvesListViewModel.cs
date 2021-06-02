using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolvesListViewModel : ViewModelBase
    {
        private readonly IPastSolvesGetter _pastSolvesGetter;
        private readonly ISolvesSaver _solvesSaver;

        [Reactive]
        public ObservableCollection<SolveViewModel> Solves { get; set; }

        public ReactiveCommand<SolveViewModel, Unit> DeleteItemCommand { get; }

        public SolvesListViewModel()
        {
            _pastSolvesGetter = Ioc.Services.GetRequiredService<IPastSolvesGetter>();
            _solvesSaver = Ioc.Services.GetRequiredService<ISolvesSaver>();

            Solves = new ObservableCollection<SolveViewModel>();
            Solves.CollectionChanged += UpdateIndexes;

            DeleteItemCommand = ReactiveCommand.Create<SolveViewModel>((solve) => Solves.Remove(solve));
        }

        public async Task LoadAsync(Event @event, Session session)
        {
            var solves = await _pastSolvesGetter.GetSolvesAsync(@event, session).ConfigureAwait(false);
            var ordered = solves.OrderByDescending(solve => solve.Date).Select(solve => new SolveViewModel(solve));
            Solves = new ObservableCollection<SolveViewModel>(ordered);
            Solves.CollectionChanged += UpdateIndexes;
            UpdateIndexes(null, EventArgs.Empty);
        }

        private void UpdateIndexes(object? sender, EventArgs e)
        {
            foreach (var solve in Solves)
                solve.Index = $"{Solves.Count - Solves.IndexOf(solve)}.";
        }

        public Task SaveAsync(Event @event, Session session)
        {
            return _solvesSaver.SaveSolvesAsync(Solves.Select(x => x.Model), @event, session);
        }
    }
}
