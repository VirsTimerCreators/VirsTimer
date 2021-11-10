using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace VirsTimer.DesktopApp.Commands
{
    public class AsyncRelayCommand<T> : ICommand
    {
        readonly Func<T, Task> _execute;
        readonly Predicate<T> _canExecute;

        public AsyncRelayCommand(Func<T, Task> execute)
            : this(execute, (_) => true)
        { }

        public AsyncRelayCommand(Func<T, Task> execute, Predicate<T> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            if (parameter is not T t)
                return false;

            return _canExecute(t);
        }

        public async void Execute(object? parameter)
        {
            if (parameter is not T t)
                return;

            await _execute(t);
        }
    }
}