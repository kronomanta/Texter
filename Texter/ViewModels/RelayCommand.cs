using System;

namespace Texter.ViewModels
{
    public class RelayCommand : System.Windows.Input.ICommand
    {
        private Func<bool> _canExecute;
        private Action _execute;

        public RelayCommand(Action execute) : this(execute, null) { }

        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        public void RaiseCanExecuteChanged() => System.Windows.Input.CommandManager.InvalidateRequerySuggested();
    }

    public class RelayCommand<T> : System.Windows.Input.ICommand
    {
        private Predicate<T> _canExecute;
        private Action<T> _execute;

        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            this._canExecute = canExecute;
            this._execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { System.Windows.Input.CommandManager.RequerySuggested += value; }
            remove { System.Windows.Input.CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        public void RaiseCanExecuteChanged() => System.Windows.Input.CommandManager.InvalidateRequerySuggested();
    }
}
