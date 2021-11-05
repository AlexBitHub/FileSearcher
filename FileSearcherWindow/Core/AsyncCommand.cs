using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.ServiceModel.Dispatcher;

namespace FileSearcherWindow.Core
{
    public interface IAsyncCommand<T> : ICommand
    {
        Task ExecuteAsync(T par);
        bool CanExecute(T par);
    }
    public class AsyncCommand<T> : IAsyncCommand<T>
    {
        public event EventHandler CanExecuteChanged;

        private bool _isExecuting;
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool> _canExecute;
        private readonly IErrorHandler _errorHandler;

        public AsyncCommand(Func<T, Task> execute,
                            Func<T, bool> canExecute = null, 
                            IErrorHandler errorHandler = null)
        {
            _execute = execute;
            _canExecute = canExecute;
            _errorHandler = errorHandler;
        }
        public bool CanExecute(T par)
        {
            return !_isExecuting && (_canExecute?.Invoke(par) ?? true); // in brackets return true if _canExecute null else return result of invoke 
        }

        bool ICommand.CanExecute(object par)
        {
            return CanExecute((T)par);
        }

        void ICommand.Execute(object par)
        {
            ExecuteAsync((T)par).FireAndForgetSafeAsync(_errorHandler);
        }

        public async Task ExecuteAsync(T par)
        {
            if (CanExecute(par))
            {
                try
                {
                    _isExecuting = true;
                    await _execute(par);
                    Console.WriteLine("End operation in try");
                }
                finally
                {
                    _isExecuting = false;
                    Console.WriteLine("End operation in finaly");
                }
            }
            Console.WriteLine("End operation in ExecuteAsync");
            RaiseCanExecuteChanged();
        }

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public static class TaskExtension
    {
        public static async void FireAndForgetSafeAsync(this Task task, IErrorHandler handler = null)
        {
            try
            {
                await task;
                Console.WriteLine("End operation in try FAFSA");
            }
            catch(Exception ex)
            {
                handler?.HandleError(ex);
            }
            Console.WriteLine("End operation in FAFSA");
        }
    }
}
