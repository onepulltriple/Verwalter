using System.Windows.Input;

namespace AnwendungWPF.Utilities
{
    public class AsyncRelayCommand : ICommand
    {
        private readonly Func<object?, Task> _execute;

        private readonly Func<object?, bool>? _canExecute;

        private bool _isExecuting;

        public event EventHandler? CanExecuteChanged;

        #region Constructors
        public AsyncRelayCommand(Func<Task> execute)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = _ => execute();
        }


        public AsyncRelayCommand(Func<object?, Task> execute)
            : this(execute, null)
        {
        }


        public AsyncRelayCommand(
            Func<object?, Task> execute,
            Func<object?, bool>? canExecute = null)
        {
            ArgumentNullException.ThrowIfNull(execute);

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors


        public bool CanExecute(object? parameter)
        {
            return !_isExecuting &&
                   (_canExecute?.Invoke(parameter) ?? true);
        }


        public void Execute(object? parameter)
        {
            _ = ExecuteAsync(parameter);
        }


        public async Task ExecuteAsync(object? parameter)
        {
            if (!CanExecute(parameter))
                return;

            try
            {
                _isExecuting = true;
                RaiseCanExecuteChanged();

                await _execute(parameter);
            }
            finally
            {
                _isExecuting = false;
                RaiseCanExecuteChanged();
            }
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
