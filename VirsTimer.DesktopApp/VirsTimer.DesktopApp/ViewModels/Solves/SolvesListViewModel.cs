using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
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
        private ObservableCollection<SolveViewModel> _solves;

        public ObservableCollection<SolveViewModel> Solves
        {
            get => _solves;
            set => this.RaiseAndSetIfChanged(ref _solves, value);
        }
        public ReactiveCommand<SolveViewModel, Unit> DeleteItemCommand { get; }

        public SolvesListViewModel()
        {
            _pastSolvesGetter = Ioc.Services.GetRequiredService<IPastSolvesGetter>();
            _solvesSaver = Ioc.Services.GetRequiredService<ISolvesSaver>();

            _solves = new ObservableCollection<SolveViewModel>();
            _solves.CollectionChanged += UpdateIndexes;
            DeleteItemCommand = ReactiveCommand.Create<SolveViewModel>(DeleteItem);
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
            foreach(var solve in Solves)
                solve.Index = $"{Solves.Count - Solves.IndexOf(solve)}.";
        }

        public async Task SaveAsync(Event @event, Session session)
        {
            await _solvesSaver.SaveSolvesAsync(Solves.Select(x => x.Model), @event, session);
        }

        private void DeleteItem(SolveViewModel solve)
        {
            Solves.Remove(solve);
        }
    }
}
