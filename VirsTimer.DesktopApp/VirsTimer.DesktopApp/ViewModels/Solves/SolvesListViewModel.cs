using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.ViewModels.Common;

namespace VirsTimer.DesktopApp.ViewModels.Solves
{
    public class SolvesListViewModel : ViewModelBase
    {
        private readonly SnackbarViewModel _snackbarViewModel;
        private readonly ISolvesRepository _solvesRepository;
        private Session _session = null!;

        [Reactive]
        public ObservableCollection<SolveViewModel> Solves { get; set; }

        public ReactiveCommand<SolveViewModel, Unit> DeleteItemCommand { get; }

        public SolvesListViewModel(
            SnackbarViewModel snackbarViewModel,
            ISolvesRepository? solvesRepository = null)
        {
            _snackbarViewModel = snackbarViewModel;
            _solvesRepository = solvesRepository ?? Ioc.GetService<ISolvesRepository>();

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
            if (repositoryResponse.IsSuccesfull is false)
            {
                _snackbarViewModel.Enqueue("Podczas ładownia ułożeń wysątpił błąd.");
                IsBusy = false;
                return;
            }

            var ordered = repositoryResponse.Value!.OrderByDescending(solve => solve.Date).Select(solve => new SolveViewModel(solve, _solvesRepository));
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
            var repositoryResponse = await _solvesRepository.DeleteSolveAsync(solveViewModel.Model).ConfigureAwait(false);
            if (repositoryResponse.IsSuccesfull is false)
            {
                _snackbarViewModel.Enqueue("Podczas usuwania ułożenia wysątpił błąd.");
                IsBusy = false;
                return;
            }
            Solves.Remove(solveViewModel);
            IsBusy = false;
        }
    }
}
