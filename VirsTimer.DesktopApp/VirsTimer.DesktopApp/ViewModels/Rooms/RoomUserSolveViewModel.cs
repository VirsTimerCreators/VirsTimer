using ReactiveUI.Fody.Helpers;
using System;
using VirsTimer.Core.Constants;
using VirsTimer.Core.Models;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomUserSolveViewModel : ViewModelBase
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

        [Reactive]
        public string Index { get; set; }

        public RoomUserSolveViewModel(Solve solve)
        {
            Model = solve;
            Index = "-1";
        }
    }
}