using DynamicData;
using DynamicData.Binding;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using VirsTimer.Core.Constants;
using VirsTimer.DesktopApp.Extensions;

namespace VirsTimer.DesktopApp.ViewModels.Rooms
{
    public class RoomSolveFlagsViewModel : ViewModelBase
    {
        public string SolveTimeText { get; }

        public ObservableCollection<RoomFlagViewModel> FlagsArray { get; }

        public ReactiveCommand<int, Unit> RadioButtonFocusedCommand { get; }

        public ReactiveCommand<string, string> ClickFlagCommand { get; }

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

            var canAccpet = FlagsArray
                .ToObservableChangeSet()
                .AutoRefresh(x => x.Choosen)
                .ToCollection()
                .Select(x => x.Any(flag => flag.Choosen));
            ClickFlagCommand = ReactiveCommand.Create<string, string>(x => x);
            AcceptFlagCommand = ReactiveCommand.Create<string, SolveFlag>(ChooseFlag, canAccpet);
            ClickFlagCommand.InvokeCommand(AcceptFlagCommand);
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