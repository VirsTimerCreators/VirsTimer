using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using System.Threading.Tasks;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.Views;

namespace VirsTimer.DesktopApp.ViewModels
{
    public class EventViewModel : ViewModelBase
    {
        [Reactive]
        public Event CurrentEvent { get; set; }

        public ReactiveCommand<Window, Unit> ChangeEventCommand { get; }

        public EventViewModel(Event @event)
        {
            CurrentEvent = @event;
            ChangeEventCommand = ReactiveCommand.CreateFromTask<Window>(ChangeEventAsync);
        }

        private async Task ChangeEventAsync(Window window)
        {
            var eventChangeViewModel = new EventChangeViewModel();
            var dialog = new EventChangeView
            {
                DataContext = eventChangeViewModel
            };

            await dialog.ShowDialog(window);
            if (eventChangeViewModel.Accepted)
                CurrentEvent = eventChangeViewModel.SelectedEvent!;
        }
    }
}
