using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public partial class RegisterView : Window
    {
        public RegisterViewModel ViewModel { get; }

        public RegisterView()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            ViewModel = new RegisterViewModel(null);
            DataContext = ViewModel;

            var passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            passwordTextBox.PasswordChar = '\u2022';

            var repeatPasswordTextBox = this.FindControl<TextBox>("RepeatPasswordTextBox");
            repeatPasswordTextBox.PasswordChar = '\u2022';
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
