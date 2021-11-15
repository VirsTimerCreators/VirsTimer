using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using VirsTimer.DesktopApp.ViewModels;

namespace VirsTimer.DesktopApp.Views
{
    public partial class LoginView : Window
    {
        public LoginViewModel ViewModel { get; }

        public LoginView()
        {
            ViewModel = new LoginViewModel();
            DataContext = ViewModel;

            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            var passwordTextBox = this.FindControl<TextBox>("PasswordTextBox");
            passwordTextBox.PasswordChar = '\u2022';
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}