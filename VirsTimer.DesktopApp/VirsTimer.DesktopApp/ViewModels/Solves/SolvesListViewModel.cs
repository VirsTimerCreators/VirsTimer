using Microsoft.Extensions.DependencyInjection;
using ReactiveUI;
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

        public ObservableCollection<SolveViewModel> Solves { get; }
        public ReactiveCommand<SolveViewModel, Unit> DeleteItemCommand { get; }

        public SolvesListViewModel()
        {
            _pastSolvesGetter = Ioc.Services.GetRequiredService<IPastSolvesGetter>();
            _solvesSaver = Ioc.Services.GetRequiredService<ISolvesSaver>();

            Solves = new ObservableCollection<SolveViewModel>();
            DeleteItemCommand = ReactiveCommand.Create<SolveViewModel>(DeleteItem);
        }

        public async Task Load(Event @event, Session session)
        {
            var solves = await _pastSolvesGetter.GetSolvesAsync(@event, session).ConfigureAwait(false);
            Solves.Clear();
            foreach (var solve in solves.OrderByDescending(x => x.Date))
                Solves.Add(new SolveViewModel(solve));
        }

        public async Task Save(Event @event, Session session)
        {
            await _solvesSaver.SaveSolvesAsync(Solves.Select(x => x.Model), @event, session);
        }

        private void DeleteItem(SolveViewModel solve)
        {
            Solves.Remove(solve);
        }
    }
}
