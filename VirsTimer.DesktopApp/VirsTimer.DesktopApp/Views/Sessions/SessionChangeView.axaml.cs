using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.Extensions;
using VirsTimer.DesktopApp.ViewModels.Sessions;
using VirsTimer.DesktopApp.Views.Common;

namespace VirsTimer.DesktopApp.Views.Sessions
{
    public class SessionChangeView : BaseWindow<SessionChangeViewModel>
    {
        public ListBox SessionsListBox { get; }
        public Button CancelButton { get; }
        public Button AddButton { get; }
        public Button AcceptButton { get; }

        public SessionChangeView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            SessionsListBox = this.FindControl<ListBox>("SessionsListBox");
            CancelButton = this.FindControl<Button>("CancelButton");
            AddButton = this.FindControl<Button>("AddButton");
            AcceptButton = this.FindControl<Button>("AcceptButton");

            this.WhenActivated(disposableRegistration =>
            {
                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.CancelCommand,
                    view => view.CancelButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.AddSessionCommand,
                    view => view.AddButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.AcceptCommand,
                    view => view.AcceptButton)
                .DisposeWith(disposableRegistration);

                Activate(disposableRegistration);

                ViewModel.CancelCommand.Subscribe(_ => Close()).DisposeWith(disposableRegistration);
                ViewModel.AcceptCommand.Subscribe(Close).DisposeWith(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void ListBoxSelectionChanged(object? sender, SelectionChangedEventArgs e)
        {
            ResetEditing();
        }

        private void ListItemTextBoxGotFocus(object? sender, GotFocusEventArgs e)
        {
            SessionsListBox.UnselectAll();
        }

        private void ClickWithUnselect(object? sender, RoutedEventArgs e)
        {
            SessionsListBox.UnselectAll();
            ResetEditing();

            var button = sender as Button;
            button?.FindRelatedControl<Button, Grid, TextBox>((tb) =>
            {
                tb.Focus();
                tb.CaretIndex = tb.Text.Length;
            });
        }

        private void ResetEditing()
        {
            if (DataContext is not SessionChangeViewModel changeViewModel)
                throw new ArgumentException("DataContext must be SessionChangeViewModel", nameof(DataContext));

            var session = changeViewModel.Sessions.FirstOrDefault(x => x.EditingSession == true);
            if (session != null)
                session.EditingSession = false;
        }
    }
}