using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VirsTimer.DesktopApp.Extensions;
using VirsTimer.DesktopApp.ViewModels.Sessions;

namespace VirsTimer.DesktopApp.Views.Sessions
{
    public class SessionChangeView : ReactiveWindow<SessionChangeViewModel>
    {
        private readonly ListBox _sessionsListBox;

        public SessionChangeView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _sessionsListBox = this.FindControl<ListBox>("SessionsListBox");
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
            _sessionsListBox.UnselectAll();
        }

        private void ClickWithUnselect(object? sender, RoutedEventArgs e)
        {
            _sessionsListBox.UnselectAll();
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
