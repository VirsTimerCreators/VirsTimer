using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class SolvesListViewModel : ViewModelBase
    {
        private readonly IPastSolvesGetter _pastSolvesGetter;
        private readonly ISolvesSaver _solvesSaver;
        public ObservableCollection<Solve> Solves { get; }

        public SolvesListViewModel(IPastSolvesGetter pastSolvesGetter, ISolvesSaver solvesSaver)
        {
            Solves = new ObservableCollection<Solve>();
            _pastSolvesGetter = pastSolvesGetter;
            _solvesSaver = solvesSaver;
        }

        public async Task Load(Event @event, Session session)
        {
            var solves = await _pastSolvesGetter.GetSolvesAsync(@event, session).ConfigureAwait(false);
            Solves.Clear();
            foreach (var solve in solves.OrderByDescending(x => x.Date))
                Solves.Add(solve);
        }

        public void Save(Event @event, Session session)
        {
            _solvesSaver.SaveSolvesAsync(Solves, @event, session).GetAwaiter().GetResult();
        }
    }
}
