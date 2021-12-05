using Avalonia.Controls;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomUsersViewModel : ViewModelBase
    {
        public ObservableCollection<RoomUserViewModel> Users { get; set; }
        public RoomUsersViewModel(IEnumerable<RoomUserViewModel> roomUsers)
        {
            Users = new(roomUsers);
        }
    }

    public class RoomUserViewModel : ViewModelBase
    {
        public string UserName { get; set; }

        [Reactive]
        public TimeSpan? Best { get; set; }

        [Reactive]
        public TimeSpan? Worst { get; set; }

        [Reactive]
        public TimeSpan? Ao10 { get; set; }

        [Reactive]
        public TimeSpan? Avg { get; set; }

        public ObservableCollection<RoomUserSolveViewModel> Solves { get; set; } = new();
    }

    public class RoomUserSolveViewModel
    {
        public Solve Model { get; }

        public string Summary =>
            Model.Flag switch
            {
                SolveFlag.OK => Model.TimeAsSpan.ToDynamicString(),
                SolveFlag.DNF => $"{SolveFlag.DNF} ({Model.TimeAsSpan.ToDynamicString()})",
                SolveFlag.Plus2 => $"{Model.TimeAsSpan.Add(TimeSpan.FromSeconds(2)).ToDynamicString()} (+2)",
                _ => throw new ArgumentException(nameof(Model.TimeAsSpan))
            };

        public string Index { get; }

        public RoomUserSolveViewModel(Solve solve, string index)
        {
            Model = solve;
            Index = index;
        }
    }
}
