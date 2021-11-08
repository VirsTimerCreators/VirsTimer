using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using VirsTimer.Core.Models;

namespace VirsTimer.DesktopApp.ViewModels.Sessions
{
    public class SessionViewModel : ViewModelBase
    {
        private bool _editingSession = false;

        public Session Session { get; }

        [Reactive]
        public string Name { get; set; }

        public bool EditingSession
        {
            get => _editingSession;
            set
            {
                this.RaiseAndSetIfChanged(ref _editingSession, value);
                Name = Session.Name;
            }
        }

        public ReactiveCommand<Unit, Unit> EditSessionCommand { get; }

        public SessionViewModel(Session session)
        {
            Session = session;
            Name = Session.Name;
            EditSessionCommand = ReactiveCommand.Create(() => { EditingSession = true; });
        }
    }
}
