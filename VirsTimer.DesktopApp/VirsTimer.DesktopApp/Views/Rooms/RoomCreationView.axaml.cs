using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.ViewModels.Rooms;

namespace VirsTimer.DesktopApp.Views.Rooms
{
    public partial class RoomCreationView : ReactiveWindow<RoomCreationViewModel>
    {
        public Button CreateRoomButton { get; }
        public Button JoinRoomButton { get; }
        public Button CancelButton { get; }
        public ListBox EventsListBox { get; }
        public TextBox AccessCodeTextBox { get; }
        public ContentControl SnackBarContentControl { get; }

        public RoomCreationView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            CreateRoomButton = this.Find<Button>("CreateRoomButton");
            JoinRoomButton = this.Find<Button>("JoinRoomButton");
            CancelButton = this.Find<Button>("CancelButton");
            EventsListBox = this.Find<ListBox>("EventsListBox");
            AccessCodeTextBox = this.Find<TextBox>("AccessCodeTextBox");
            SnackBarContentControl = this.Find<ContentControl>("SnackBarContentControl");

            this.WhenActivated(disposableRegistration =>
            {
                this.Bind(
                    ViewModel,
                    viewModel => viewModel.SelectedEvent,
                    view => view.EventsListBox.SelectedItem)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.AccessCode,
                    view => view.AccessCodeTextBox.Text)
                .DisposeWith(disposableRegistration);

                this.Bind(
                    ViewModel,
                    viewModel => viewModel.SnackbarViewModel,
                    view => view.SnackBarContentControl.Content)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.CreateRoomCommand,
                    view => view.CreateRoomButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.JoinRoomCommand,
                    view => view.JoinRoomButton)
                .DisposeWith(disposableRegistration);

                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.CancelCommand,
                    view => view.CancelButton)
                .DisposeWith(disposableRegistration);

                ViewModel!.CreateRoomCommand.Subscribe(x =>
                {
                    if (x is null)
                        return;

                    Close(x);
                })
                .DisposeWith(disposableRegistration);

                ViewModel!.JoinRoomCommand.Subscribe(x =>
                {
                    if (x is null)
                        return;

                    Close(x);
                })
                .DisposeWith(disposableRegistration);

                ViewModel!.CancelCommand.Subscribe(_ => Close())
                .DisposeWith(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}