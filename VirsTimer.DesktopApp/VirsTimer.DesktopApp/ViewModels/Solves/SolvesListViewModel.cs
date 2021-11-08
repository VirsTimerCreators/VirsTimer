using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolvesListViewModel : ViewModelBase
    {
        private readonly ISolvesRepository _solvesRepository;
        private Session _session = null!;

        [Reactive]
        public ObservableCollection<SolveViewModel> Solves { get; set; }

        public ReactiveCommand<SolveViewModel, Unit> DeleteItemCommand { get; }

        public SolvesListViewModel(ISolvesRepository solvesRepository)
        {
            _solvesRepository = solvesRepository;

            Solves = new ObservableCollection<SolveViewModel>();
            Solves.CollectionChanged += UpdateIndexesAsync;

            DeleteItemCommand = ReactiveCommand.CreateFromTask<SolveViewModel>(DeleteSolveAsync);
        }

        public override Task ConstructAsync()
        {
            return ChangeSessionAsync(_session);
        }

        public Task ChangeSessionAsync(Session session)
        {
            _session = session;
            return LoadAsync();
        }

        private async Task LoadAsync()
        {
            IsBusy = true;
            var repositoryResponse = await _solvesRepository.GetSolvesAsync(_session).ConfigureAwait(false);
            var ordered = repositoryResponse.Value.OrderByDescending(solve => solve.Date).Select(solve => new SolveViewModel(solve, _solvesRepository));
            Solves = new ObservableCollection<SolveViewModel>(ordered);
            Solves.CollectionChanged += UpdateIndexesAsync;
            UpdateIndexesAsync(this, EventArgs.Empty);
            IsBusy = false;
        }

        private async void UpdateIndexesAsync(object? sender, EventArgs e)
        {
            var tasks = Solves.Select(solve => Task.Run(() => solve.Index = $"{Solves.Count - Solves.IndexOf(solve)}."));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task DeleteSolveAsync(SolveViewModel solveViewModel)
        {
            IsBusy = true;
            await _solvesRepository.DeleteSolveAsync(solveViewModel.Model).ConfigureAwait(false);
            Solves.Remove(solveViewModel);
            IsBusy = false;
        }
    }
}
