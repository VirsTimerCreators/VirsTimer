using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using VirsTimer.Core.Models;
using VirsTimer.Core.Services;

namespace VirsTimer.DesktopApp.ViewModels.Events
{
    public class EventChangeViewModel : ViewModelBase
    {
        public bool Accepted { get; private set; } = false;
        public ObservableCollection<Event> Events { get; private set; } = null!;

        [Reactive]
        public Event? SelectedEvent { get; set; }

        public ReactiveCommand<Window, Unit> AcceptCommand { get; }

        public EventChangeViewModel()
        {
            Events = new ObservableCollection<Event>(Ioc.GetService<IEventsGetter>().GetEventsAsync().GetAwaiter().GetResult());
            AcceptCommand = ReactiveCommand.Create<Window>((window) =>
            {
                Accepted = SelectedEvent != null;
                window.Close();
            });

            OnConstructedAsync(this, EventArgs.Empty);
        }

        protected override async void OnConstructedAsync(object? sender, EventArgs e)
        {
            var events = await Ioc.GetService<IEventsGetter>().GetEventsAsync().ConfigureAwait(false);
            Events = new ObservableCollection<Event>(events);
        }
    }
}
