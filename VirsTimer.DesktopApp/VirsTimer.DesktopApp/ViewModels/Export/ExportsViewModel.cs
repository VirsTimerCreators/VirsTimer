using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Export;
using VirsTimer.Core.Extensions;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.ViewModels.Common;
using VirsTimer.DesktopApp.ViewModels.Solves;

namespace VirsTimer.DesktopApp.ViewModels.Export
{
    public class ExportsViewModel : ViewModelBase
    {
        private readonly Session _session;
        private readonly ObservableCollection<SolveViewModel> _solveViewModels;
        private readonly List<Solve> _solves;
        private readonly ISolvesJsonExporter _solvesJsonExporter;
        private readonly ISolvesCsvExporter _solvesCsvExporter;
        private readonly ISolvesRepository _solvesRepository;

        public SnackbarViewModel SnackbarViewModel { get; }

        public ReactiveCommand<Unit, Unit> ExportJsonCommand { get; }
        public ReactiveCommand<Unit, Unit> ImportJsonCommand { get; }
        public ReactiveCommand<Unit, Unit> ExportCsvCommand { get; }
        public ReactiveCommand<Unit, Unit> ImportCsvCommand { get; }
        public ReactiveCommand<Unit, Unit> OkCommand { get; }

        public bool Imported { get; private set; }

        public Interaction<Unit, string[]> ShowJsonFileDialog { get; }
        public Interaction<Unit, string[]> ShowCsvFileDialog { get; }

        public ExportsViewModel(
            Session session,
            ObservableCollection<SolveViewModel> solves,
            ISolvesJsonExporter? solvesJsonExporter = null,
            ISolvesCsvExporter? solvesCsvExporter = null,
            ISolvesRepository? solvesRepository = null)
        {
            _session = session;
            _solveViewModels = solves;
            _solves = solves.Select(x => x.Model).ToList();
            _solvesJsonExporter = solvesJsonExporter ?? Ioc.GetService<ISolvesJsonExporter>();
            _solvesCsvExporter = solvesCsvExporter ?? Ioc.GetService<ISolvesCsvExporter>();
            _solvesRepository = solvesRepository ?? Ioc.GetService<ISolvesRepository>();

            SnackbarViewModel = new SnackbarViewModel(500, 96, 4);
            ExportJsonCommand = ReactiveCommand.CreateFromTask(ExportJsonAsync);
            ImportJsonCommand = ReactiveCommand.CreateFromTask(ImportJsonAsync);
            ExportCsvCommand = ReactiveCommand.CreateFromTask(ExportCsvAsync);
            ImportCsvCommand = ReactiveCommand.CreateFromTask(ImportCsvAsync);

            ExportJsonCommand.ThrownExceptions.Subscribe(async (e) => await Catch(e, "Podczas ekspotu wystąpił błąd."));
            ImportJsonCommand.ThrownExceptions.Subscribe(async (e) => await Catch(e, "Podczas impoertu wystąpił błąd."));
            ExportCsvCommand.ThrownExceptions.Subscribe(async (e) => await Catch(e, "Podczas ekspotu wystąpił błąd."));
            ImportCsvCommand.ThrownExceptions.Subscribe(async (e) => await Catch(e, "Podczas impoertu wystąpił błąd."));

            OkCommand = ReactiveCommand.Create(() => { SnackbarViewModel.Disposed = true; });

            ShowJsonFileDialog = new Interaction<Unit, string[]>();
            ShowCsvFileDialog = new Interaction<Unit, string[]>();
        }

        private async Task Catch(Exception e, string message)
        {
            IsBusy = false;
            await SnackbarViewModel.Enqueue(message);
        }

        public async Task ExportJsonAsync()
        {
            const int Length = 60;

            IsBusy = true;

            var path = await _solvesJsonExporter.ExportAsync(_solves);
            var cutted = new List<string>();
            while (path.Any())
            {
                if (path.Length >= Length)
                {
                    cutted.Add(path[0..Length]);
                    path = path[Length..];
                }
                else
                {
                    cutted.Add(path);
                    path = string.Empty;
                }
            }

            var pathCutted = string.Join(Environment.NewLine, cutted);

            IsBusy = false;

            await SnackbarViewModel.Enqueue($"Wyeksportowano pomyślnie do {Environment.NewLine}{pathCutted}");
        }

        public async Task ImportJsonAsync()
        {
            var choosen = await ShowJsonFileDialog.Handle(Unit.Default);
            if (choosen is null || choosen.Length == 0)
                return;

            IsBusy = true;
            var firstFile = choosen[0];
            var solves = await _solvesJsonExporter.ImportAsync(firstFile);
            solves.ForEach(solve => solve.Session = _session);

            var response = await _solvesRepository.AddSolvesAsync(solves);

            IsBusy = false;

            if (response.IsSuccesfull)
            {
                Imported = true;
                await SnackbarViewModel.Enqueue($"Import zakończył się pomyślnie. Lista ułożeń odświeży się po zamknięciu okna.");
                return;
            }

            await SnackbarViewModel.Enqueue($"Podczas importu wystąpił problem");
        }

        public async Task ExportCsvAsync()
        {
            const int Length = 60;

            IsBusy = true;

            var path = await _solvesCsvExporter.ExportAsync(_solves);
            var cutted = new List<string>();
            while (path.Any())
            {
                if (path.Length >= Length)
                {
                    cutted.Add(path[0..Length]);
                    path = path[Length..];
                }
                else
                {
                    cutted.Add(path);
                    path = string.Empty;
                }
            }

            var pathCutted = string.Join(Environment.NewLine, cutted);

            IsBusy = false;

            await SnackbarViewModel.Enqueue($"Wyeksportowano pomyślnie do {Environment.NewLine}{pathCutted}");
        }

        public async Task ImportCsvAsync()
        {
            var choosen = await ShowCsvFileDialog.Handle(Unit.Default);
            if (choosen is null || choosen.Length == 0)
                return;

            IsBusy = true;

            var firstFile = choosen[0];
            var solves = await _solvesCsvExporter.ImportAsync(firstFile);
            solves.ForEach(solve => solve.Session = _session);

            var response = await _solvesRepository.AddSolvesAsync(solves);

            IsBusy = false;

            if (response.IsSuccesfull)
            {
                Imported = true;
                await SnackbarViewModel.Enqueue($"Import zakończył się pomyślnie. Lista ułożeń odświeży się po zamknięciu okna.");
                return;
            }

            await SnackbarViewModel.Enqueue($"Podczas importu wystąpił problem");
        }
    }
}