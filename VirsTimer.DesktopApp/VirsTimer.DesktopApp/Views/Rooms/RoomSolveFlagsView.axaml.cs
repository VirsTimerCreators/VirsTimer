using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using VirsTimer.DesktopApp.ViewModels.Rooms;

namespace VirsTimer.DesktopApp.Views.Rooms
{
    public partial class RoomSolveFlagsView : ReactiveUserControl<RoomSolveFlagsViewModel>
    {
        public RadioButton OkRadioButton { get; }
        public RadioButton Plus2RadioButton { get; }
        public RadioButton DnfRadioButton { get; }
        public RoomSolveFlagsView()
        {
            InitializeComponent();

            OkRadioButton = this.FindControl<RadioButton>("OkButton");
            Plus2RadioButton = this.FindControl<RadioButton>("Plus2Button");
            DnfRadioButton = this.FindControl<RadioButton>("DnfButton");

            this.WhenActivated(dispasableRegistration =>
            {
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
                this.Events().KeyUp
                .Select(x => x.Key)
                .Where(x => x == Key.Enter || x == Key.Space)
                .Select(x => true switch
                {
                    var _ when OkRadioButton.IsChecked == true => "0",
                    var _ when OkRadioButton.IsChecked == true => "1",
                    var _ when OkRadioButton.IsChecked == true => "2",
                    _ => ""
                })
                .Where(x => x != "")
                .InvokeCommand(this, x => x.ViewModel.AcceptFlagCommand)
                .DisposeWith(dispasableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}