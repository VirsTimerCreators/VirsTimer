using Avalonia.Controls;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System.Reactive;
using VirsTimer.DesktopApp.Views;
 
namespace VirsTimer.DesktopApp.ViewModels
{
    public class LoginViewModel
    {
        [Reactive]
        public string LoginName { get; set; }
 
        [Reactive]
        public string LoginPassowd { get; set; }
 
        public ReactiveCommand<Window, Unit> AcceptLoginCommand { get; }
 
        public LoginViewModel()
        {
            AcceptLoginCommand = ReactiveCommand.Create<Window>(AcceptLogin);
        }
 
        public void AcceptLogin(Window parent)
        {
            // logika
            // logowanie się do server
 
            // otwórz główne okno jeżeli się udało
            var mainWinow = new MainWindow();
            mainWinow.Show();
            parent.Close();
        }
    }
}