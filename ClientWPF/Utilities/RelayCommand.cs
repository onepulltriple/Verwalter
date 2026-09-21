using System.Windows.Input;

namespace AnwendungWPF.Infrastructure
{
    public class RelayCommand : ICommand  
    {
        private readonly Action<object?> _execute;

        private readonly Func<object?, bool>? _canExecute;

        #pragma warning disable CS0067
        public event EventHandler? CanExecuteChanged;
        #pragma warning restore CS0067

        #region Constructors
        public RelayCommand(Action execute)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = _ => execute();
        }


        public RelayCommand(Action<object?> execute) : this(execute, null) { }


        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);
            _execute = execute;
            _canExecute = canExecute;
        }
        #endregion // Constructors

        public bool CanExecute(object? parameter)
        {
            return _canExecute?.Invoke(parameter) ?? true;
        }


        public void Execute(object? parameter)
        {
            _execute(parameter);
        }


        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
