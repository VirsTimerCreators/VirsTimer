using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using Avalonia.VisualTree;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using VirsTimer.DesktopApp.Constants;
using VirsTimer.DesktopApp.ViewModels.Rooms;

namespace VirsTimer.DesktopApp.Views.Rooms
{
    public partial class RoomSolveFlagsView : ReactiveUserControl<RoomSolveFlagsViewModel>
    {
        private Window? _ancestor;
        public RadioButton OkRadioButton { get; }
        public RadioButton Plus2RadioButton { get; }
        public RadioButton DnfRadioButton { get; }
        public TextBlock SolveTimeTextBlock { get; }
        public TextBlock ChooseTextBlock { get; }

        public RoomSolveFlagsView()
        {
            InitializeComponent();

            OkRadioButton = this.FindControl<RadioButton>("OkButton");
            Plus2RadioButton = this.FindControl<RadioButton>("Plus2Button");
            DnfRadioButton = this.FindControl<RadioButton>("DnfButton");
            SolveTimeTextBlock = this.FindControl<TextBlock>("SolveTimeTextBlock");
            ChooseTextBlock = this.FindControl<TextBlock>("ChooseTextBlock");

            this.WhenActivated(dispasableRegistration =>
            {
                _ancestor = this.FindAncestorOfType<Window>();
                _ancestor.Opened += (_, _) => OnOpen();

                if (_ancestor is not null && _ancestor.IsActive)
                    OnOpen();

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.RadioButtonFocusedCommand,
                    view => view.OkRadioButton,
                    Observable.Return(0),
                    toEvent: "GotFocus")
                .DisposeWith(dispasableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.RadioButtonFocusedCommand,
                    view => view.Plus2RadioButton,
                    Observable.Return(1),
                    toEvent: "GotFocus")
                .DisposeWith(dispasableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.RadioButtonFocusedCommand,
                    view => view.DnfRadioButton,
                    Observable.Return(2),
                    toEvent: "GotFocus")
                .DisposeWith(dispasableRegistration);

                this.Events().KeyDown
                .Select(x => x.Key)
                .Where(x => x == Key.Right)
                .Subscribe(_ =>
                {
                    if (OkRadioButton.IsFocused)
                    {
                        Plus2RadioButton.Focus();
                        return;
                    }

                    if (Plus2RadioButton.IsFocused)
                    {
                        DnfRadioButton.Focus();
                        return;
                    }

                    if (DnfRadioButton.IsFocused)
                    {
                        OkRadioButton.Focus();
                        return;
                    }

                    OkRadioButton.Focus();
                })
                .DisposeWith(dispasableRegistration);

                this.Events().KeyDown
                .Select(x => x.Key)
                .Where(x => x == Key.Left)
                .Subscribe(_ =>
                {
                    if (OkRadioButton.IsFocused)
                    {
                        DnfRadioButton.Focus();
                        return;
                    }

                    if (Plus2RadioButton.IsFocused)
                    {
                        OkRadioButton.Focus();
                        return;
                    }

                    if (DnfRadioButton.IsFocused)
                    {
                        Plus2RadioButton.Focus();
                        return;
                    }

                    OkRadioButton.Focus();
                })
                .DisposeWith(dispasableRegistration);

                Focus();
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        private void OnOpen()
        {
            var windowHeight = _ancestor!.Height;
            SolveTimeTextBlock.FontSize = windowHeight switch
            {
                >= ScreenHeight.Big => 32,
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => 28,
                (< ScreenHeight.Medium) => 24,
                _ => 24
            };

            ChooseTextBlock.FontSize = windowHeight switch
            {
                >= ScreenHeight.Big => 32,
                (< ScreenHeight.Big) and (>= ScreenHeight.Medium) => 28,
                (< ScreenHeight.Medium) => 24,
                _ => 24
            };
        }
    }
}