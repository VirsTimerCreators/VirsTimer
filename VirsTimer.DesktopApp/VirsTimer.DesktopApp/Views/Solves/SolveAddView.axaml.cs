using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using ReactiveUI;
using System;
using System.Reactive.Disposables;
using VirsTimer.DesktopApp.ViewModels.Solves;

namespace VirsTimer.DesktopApp.Views.Solves
{
    public partial class SolveAddView : ReactiveWindow<SolveAddViewModel>
    {
        public TextBox SolveTextBox { get; }
        public Button CancelButton { get; }

        public SolveAddView()
        {
            InitializeComponent();
            SolveTextBox = this.FindControl<TextBox>("SolveTextBox");
            SolveTextBox.Text = "00:00:00.00";
            SolveTextBox.CaretIndex = 6;
            SolveTextBox.CaretBrush = Avalonia.Media.Brushes.Yellow;
            SolveTextBox.AddHandler(TextInputEvent, PreviewTextInput, RoutingStrategies.Tunnel);
            SolveTextBox.AddHandler(KeyDownEvent, PreviewKeyDown, RoutingStrategies.Tunnel);

            CancelButton = this.Find<Button>("CancelButton");
#if DEBUG
            this.AttachDevTools();
#endif

            this.WhenActivated(disposableRegistration =>
            {
                this.BindCommand(
                    ViewModel,
                    viewModel => viewModel.CancelCommand,
                    view => view.CancelButton)
                .DisposeWith(disposableRegistration);

                ViewModel!.CancelCommand.Subscribe(_ => Close())
                .DisposeWith(disposableRegistration);
            });
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }

        protected override void OnOpened(EventArgs e)
        {
            base.OnOpened(e);
            SolveTextBox.Focus();
        }

        private void PreviewTextInput(object? sender, TextInputEventArgs e)
        {
            if (sender is not TextBox textBox
                || textBox.CaretIndex == 11
                || e.Text?.Length != 1
                || !int.TryParse(e.Text[0].ToString(), out var number))
            {
                e.Handled = true;
                return;
            }
            if (textBox.CaretIndex == 2
                || textBox.CaretIndex == 5
                || textBox.CaretIndex == 8)
                textBox.CaretIndex++;

            if ((textBox.CaretIndex == 3 || textBox.CaretIndex == 6) && number > 5)
            {
                e.Handled = true;
                return;
            }

            textBox.Text = textBox.Text.Insert(textBox.CaretIndex + 1, number.ToString());
            textBox.Text = textBox.Text.Remove(textBox.CaretIndex, 1);
            textBox.CaretIndex++;
        }

        private void PreviewKeyDown(object? sender, KeyEventArgs e)
        {
            var isAcceptedFlag = e.Key == Key.D0 || e.Key == Key.D1 || e.Key == Key.D2 || e.Key == Key.D3 || e.Key == Key.D4 || e.Key == Key.D5 || e.Key == Key.D6 || e.Key == Key.D7 || e.Key == Key.D8 || e.Key == Key.D9
                || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3 || e.Key == Key.NumPad4 || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9
                || e.Key == Key.Left || e.Key == Key.Right;

            if (!isAcceptedFlag)
            {
                e.Handled = true;
                return;
            }
        }
    }
}