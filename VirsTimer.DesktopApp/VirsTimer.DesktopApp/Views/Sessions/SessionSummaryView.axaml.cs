using Avalonia.Markup.Xaml;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using VirsTimer.DesktopApp.ViewModels.Sessions;
using VirsTimer.DesktopApp.Views.Common;

namespace VirsTimer.DesktopApp.Views.Sessions
{
    public class SessionSummaryView : BaseUserControl<SessionSummaryViewModel>
    {
        public SessionSummaryView()
        {
            InitializeComponent();

            this.WhenActivated(disposableRegistration =>
            {
                ViewModel.ShowSessionChangeDialog.RegisterHandler(DoShowEventChangeDialogAsync).DisposeWith(disposableRegistration);
                Activate(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private async Task DoShowEventChangeDialogAsync(InteractionContext<SessionChangeViewModel, SessionViewModel?> interaction)
        {
            var dialog = new SessionChangeView
            {
                DataContext = interaction.Input
            };

            var result = await dialog.ShowDialog<SessionViewModel?>(WindowParent);
            interaction.SetOutput(result);
        }
    }
}
