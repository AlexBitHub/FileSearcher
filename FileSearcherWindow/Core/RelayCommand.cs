using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FileSearcherWindow.Core
{
    public class RelayCommand : ICommand
    {
        private Action<object> executeMethod;
        private Func<object, bool> canExecuteMethod;
        
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested += value; }
        }

        public RelayCommand(Action<object> executeMethod, Func<object, bool> canExecuteMethod = null)
        {
            this.executeMethod = executeMethod;
            this.canExecuteMethod = canExecuteMethod;
        }

        public bool CanExecute(object parameter)
        {
            return this.canExecuteMethod == null || this.canExecuteMethod(parameter);
        }

        public void Execute(object parameter = null)
        {
            this.executeMethod(parameter);
        }
    }
}
