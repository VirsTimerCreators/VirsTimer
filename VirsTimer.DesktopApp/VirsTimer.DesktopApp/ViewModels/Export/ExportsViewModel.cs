using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using VirsTimer.Core.Export;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services.Solves;
using VirsTimer.DesktopApp.ViewModels.Common;
using VirsTimer.DesktopApp.ViewModels.Solves;

namespace VirsTimer.DesktopApp.ViewModels.Export
{
    public class ExportsViewModel : ViewModelBase
    {
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

        public Interaction<Unit, string[]> ShowFileDialog { get; }

        public ExportsViewModel(
            ObservableCollection<SolveViewModel> solves,
            ISolvesJsonExporter? solvesJsonExporter = null,
            ISolvesCsvExporter? solvesCsvExporter = null,
            ISolvesRepository? solvesRepository = null)
        {
            _solveViewModels = solves;
            _solves = solves.Select(x => x.Model).ToList();
            _solvesJsonExporter = solvesJsonExporter ??Ioc.GetService<ISolvesJsonExporter>();
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

            ShowFileDialog = new Interaction<Unit, string[]>();
        }

        private async Task Catch(Exception e, string message)
        {
            await SnackbarViewModel.Enqueue(message);
        }

        public async Task ExportJsonAsync()
        {
            const int Length = 60;

            var path = await _solvesJsonExporter.ExportAsync(_solves);
            var c = path;
            var cutted = new List<string>();
            while(path.Any())
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

            var pathCutted = string.Join("\r\n", cutted);
            await SnackbarViewModel.Enqueue($"Wyeksportowano pomyślnie do \n{pathCutted}");
        }

        public async Task ImportJsonAsync()
        {
            var choosen = await ShowFileDialog.Handle(Unit.Default);
            var s = 1;
        }

        public async Task ExportCsvAsync()
        {
        }

        public async Task ImportCsvAsync()
        {
        }
    }
}