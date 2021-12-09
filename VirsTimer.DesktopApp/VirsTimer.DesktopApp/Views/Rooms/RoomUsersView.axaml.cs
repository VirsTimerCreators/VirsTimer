using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using VirsTimer.DesktopApp.ViewModels.Rooms;

namespace VirsTimer.DesktopApp.Views.Rooms
{
    public partial class RoomUsersView : ReactiveUserControl<RoomUsersViewModel>
    {
        public RoomUsersView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
