using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels.Events;
using VirsTimer.DesktopApp.Views.Common;

namespace VirsTimer.DesktopApp.Views.Events
{
    public class EventSummaryView : BaseUserControl<EventSummaryViewModel>
    {
        public EventSummaryView()
        {
            InitializeComponent();
            this.WhenActivated(disposableRegistration =>
            {
                ViewModel.ShowEventChangeDialog.RegisterHandler(DoShowEventChangeDialogAsync).DisposeWith(disposableRegistration);
                Activate(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task DoShowEventChangeDialogAsync(InteractionContext<EventChangeViewModel, EventViewModel?> interaction)
        {
            var dialog = new EventChangeView
            {
                DataContext = interaction.Input
            };

            var result = await dialog.ShowDialog<EventViewModel?>(WindowParent);
            interaction.SetOutput(result);
        }
    }
}