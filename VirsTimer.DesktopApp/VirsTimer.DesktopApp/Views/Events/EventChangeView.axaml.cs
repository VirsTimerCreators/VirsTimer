using System;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VirsTimer.DesktopApp.Extensions;
using VirsTimer.DesktopApp.ViewModels.Events;
 
namespace VirsTimer.DesktopApp.Views.Events
{
    public class EventChangeView : ReactiveWindow<EventChangeViewModel>
    {
        private readonly ListBox _eventsListBox;
 
        public EventChangeView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            _eventsListBox = this.FindControl<ListBox>("EventsListBox");
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
            _eventsListBox.UnselectAll();
        }
 
        private void ClickWithUnselect(object? sender, RoutedEventArgs e)
        {
            _eventsListBox.UnselectAll();
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
