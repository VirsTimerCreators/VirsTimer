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

        public SessionChangeViewModel ParentViewModel { get; }

        [Reactive]
        public string Name { get; set; }

        public bool EditingSession
        {
            get => _editingSession;
            set
            {
                this.RaiseAndSetIfChanged(ref _editingSession, value);
                if (value == true)
                    Name = Session.Name;
            }
        }

        public ReactiveCommand<Unit, Unit> EditSessionCommand { get; }

        public SessionViewModel(SessionChangeViewModel parentViewModel, Session session)
        {
            Session = session;
            ParentViewModel = parentViewModel;
            Name = Session.Name;
            EditSessionCommand = ReactiveCommand.Create(() => { EditingSession = true; });
        }
    }
}
