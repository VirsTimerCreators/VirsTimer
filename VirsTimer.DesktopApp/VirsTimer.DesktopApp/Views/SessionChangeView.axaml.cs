using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System;
using System.Linq;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public class SessionChangeView : Window
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
