using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Linq;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.Extensions;
using VirsTimer.DesktopApp.ViewModels.Events;

namespace VirsTimer.DesktopApp.Views.Events
{
    public class EventChangeView : ReactiveWindow<EventChangeViewModel>
    {
        public ListBox EventsListBox { get; }
        public Button CancelButton { get; }
        public Button AddButton { get; }
        public Button AcceptButton { get; }

        public EventChangeView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            EventsListBox = this.FindControl<ListBox>("EventsListBox");
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
                    viewModel => viewModel.AddEventCommand,
                    view => view.AddButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.AcceptCommand,
                    view => view.AcceptButton)
                .DisposeWith(disposableRegistration);

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
            EventsListBox.UnselectAll();
        }

        private void ClickWithUnselect(object? sender, RoutedEventArgs e)
        {
            EventsListBox.UnselectAll();
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
            if (DataContext is not EventChangeViewModel changeViewModel)
                throw new ArgumentException("DataContext must be SessionChangeViewModel", nameof(DataContext));

            var session = changeViewModel.Events.FirstOrDefault(x => x.EditingEvent == true);
            if (session != null)
                session.EditingEvent = false;
        }
    }
}