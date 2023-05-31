using System.Windows.Input;

namespace Messenger.Classes
{
    class RelayCommand : ICommand
    {
        private Action<object> _execute;
        private Func<object, bool> _canExecute;
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        /// <summary>
        /// Проверяет, можно ли выполнить команду в текущем состоянии объекта, используя заданный делегат canExecute
        /// </summary>
        /// <returns>Если делегат canExecute не задан, то метод CanExecute всегда возвращает true</returns>
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }
        /// <summary>
        /// Выполняет заданный делегат execute
        /// </summary>
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
