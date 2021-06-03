using Microsoft.Extensions.DependencyInjection;
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
        private Event _event;
        private Session _session;

        [Reactive]
        public ObservableCollection<SolveViewModel> Solves { get; set; }

        public ReactiveCommand<SolveViewModel, Unit> DeleteItemCommand { get; }

        public SolvesListViewModel(Event @event, Session session)
        {
            _solvesRepository = Ioc.Services.GetRequiredService<ISolvesRepository>();

            _event = @event;
            _session = session;

            Solves = new ObservableCollection<SolveViewModel>();
            Solves.CollectionChanged += UpdateIndexesAsync;

            DeleteItemCommand = ReactiveCommand.CreateFromTask<SolveViewModel>(DeleteSolveAsync);
            OnConstructedAsync(this, EventArgs.Empty);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            await ChangeEventAndSession(_event, _session).ConfigureAwait(false);
        }

        public Task ChangeEventAndSession(Event @event, Session session)
        {
            _event = @event;
            _session = session;
            return LoadAsync();
        }

        private async Task LoadAsync()
        {
            var solves = await _solvesRepository.GetSolvesAsync(_event, _session).ConfigureAwait(false);
            var ordered = solves.OrderByDescending(solve => solve.Date).Select(solve => new SolveViewModel(solve, _solvesRepository));
            Solves = new ObservableCollection<SolveViewModel>(ordered);
            Solves.CollectionChanged += UpdateIndexesAsync;
            UpdateIndexesAsync(this, EventArgs.Empty);
        }

        private async void UpdateIndexesAsync(object? sender, EventArgs e)
        {
            var tasks = Solves.Select(solve => Task.Run(() => solve.Index = $"{Solves.Count - Solves.IndexOf(solve)}."));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private async Task DeleteSolveAsync(SolveViewModel solveViewModel)
        {
            await _solvesRepository.DeleteSolveAsync(solveViewModel.Model).ConfigureAwait(false);
            Solves.Remove(solveViewModel);
        }
    }
}
