using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Reactive;
using VirsTimer.Core.Constants;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomSolveFlagsViewModel : ViewModelBase
    {
        public string SolveTimeText { get; }

        public ObservableCollection<RoomFlagViewModel> FlagsArray { get; }

        public ReactiveCommand<int, Unit> RadioButtonFocusedCommand { get; }

        public ReactiveCommand<string, SolveFlag> AcceptFlagCommand { get; }

        public RoomSolveFlagsViewModel(TimeSpan solveTime)
        {
            SolveTimeText = "Czas ułożenia: " + solveTime.ToDynamicString();
            var flags = new RoomFlagViewModel[3]
            {
                new(),
                new(),
                new()
            };

            FlagsArray = new ObservableCollection<RoomFlagViewModel>(flags);
            RadioButtonFocusedCommand = ReactiveCommand.Create<int>(RadioButtonFocused);

            AcceptFlagCommand = ReactiveCommand.Create<string, SolveFlag>(ChooseFlag);
        }

        private SolveFlag ChooseFlag(string flag)
        {
            return (SolveFlag)int.Parse(flag);
        }

        public void RadioButtonFocused(int index)
        {
            Action action = index switch
            {
                0 => () =>
                {
                    FlagsArray[0].Choosen = true;
                    FlagsArray[1].Choosen = false;
                    FlagsArray[2].Choosen = false;
                }
                ,
                1 => () =>
                {
                    FlagsArray[0].Choosen = false;
                    FlagsArray[1].Choosen = true;
                    FlagsArray[2].Choosen = false;
                }
                ,
                2 => () =>
                {
                    FlagsArray[0].Choosen = false;
                    FlagsArray[1].Choosen = false;
                    FlagsArray[2].Choosen = true;
                }
                ,
                _ => () => { }
            };

            action();
        }
    }
}