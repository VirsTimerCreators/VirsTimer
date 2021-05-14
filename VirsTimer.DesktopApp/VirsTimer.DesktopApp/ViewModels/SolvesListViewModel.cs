using ReactiveUI;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using System.Windows.Input;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolvesListViewModel : ViewModelBase
    {
        private readonly IPastSolvesGetter _pastSolvesGetter;
        private readonly ISolvesSaver _solvesSaver;
        public ObservableCollection<Solve> Solves { get; }

        public ReactiveCommand<Solve, Unit> DeleteItemCommand { get; }

        public SolvesListViewModel(IPastSolvesGetter pastSolvesGetter, ISolvesSaver solvesSaver)
        {
            Solves = new ObservableCollection<Solve>();
            _pastSolvesGetter = pastSolvesGetter;
            _solvesSaver = solvesSaver;
            DeleteItemCommand = ReactiveCommand.Create<Solve>(DeleteItem);
        }

        public async Task Load(Event @event, Session session)
        {
            var solves = await _pastSolvesGetter.GetSolvesAsync(@event, session).ConfigureAwait(false);
            Solves.Clear();
            foreach (var solve in solves.OrderByDescending(x => x.Date))
                Solves.Add(solve);
        }

        public async Task Save(Event @event, Session session)
        {
            await _solvesSaver.SaveSolvesAsync(Solves, @event, session);
        }

        private void DeleteItem(Solve solve)
        {
            Solves.Remove(solve);
        }
    }
}
