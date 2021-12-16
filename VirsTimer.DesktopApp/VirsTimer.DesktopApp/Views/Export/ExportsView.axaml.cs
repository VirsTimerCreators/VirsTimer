using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels.Export;

namespace VirsTimer.DesktopApp.Views.Export
{
    public partial class ExportsView : ReactiveWindow<ExportsViewModel>
    {
        public Button ExportJsonButton { get; }
        public Button ImportJsonButton { get; }
        public Button ExportCsvButton { get; }
        public Button ImportCsvButton { get; }
        public Button OkButton { get; }

        public ExportsView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            ExportJsonButton = this.FindControl<Button>("ExportJsonButton");
            ImportJsonButton = this.FindControl<Button>("ImportJsonButton");
            ExportCsvButton = this.FindControl<Button>("ExportCsvButton");
            ImportCsvButton = this.FindControl<Button>("ImportCsvButton");
            OkButton = this.FindControl<Button>("OkButton");

            this.WhenActivated(disposableRegistration =>
            {
                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.ExportJsonCommand,
                    view => view.ExportJsonButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.ImportJsonCommand,
                    view => view.ImportJsonButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.ExportCsvCommand,
                    view => view.ExportCsvButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.ImportCsvCommand,
                    view => view.ImportCsvButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.OkCommand,
                    view => view.OkButton)
                .DisposeWith(disposableRegistration);

                ViewModel!.OkCommand.Subscribe(_ =>
                {
                    ViewModel.SnackbarViewModel.Disposed = true;
                    Close();
                })
                .DisposeWith(disposableRegistration);

                ViewModel.ShowJsonFileDialog.RegisterHandler(DoShowJsonFileDialogAsync).DisposeWith(disposableRegistration);
                ViewModel.ShowCsvFileDialog.RegisterHandler(DoShowCsvFileDialogAsync).DisposeWith(disposableRegistration);
            });
        }

        private async Task DoShowJsonFileDialogAsync(InteractionContext<Unit, string[]> interaction)
        {
            var dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Wybierz plik do importu",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter{ Name = "Json files", Extensions = new List<string>{ "json" } }
                }
            };

            var output = await dialog.ShowAsync(this);
            interaction.SetOutput(output);
        }

        private async Task DoShowCsvFileDialogAsync(InteractionContext<Unit, string[]> interaction)
        {
            var dialog = new OpenFileDialog
            {
                AllowMultiple = false,
                Title = "Wybierz plik do importu",
                Filters = new List<FileDialogFilter>
                {
                    new FileDialogFilter{ Name = "Csv files", Extensions = new List<string>{ "csv" } }
                }
            };

            var output = await dialog.ShowAsync(this);
            interaction.SetOutput(output);
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}