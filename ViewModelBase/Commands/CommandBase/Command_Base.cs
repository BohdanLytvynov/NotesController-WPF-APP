using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ViewModelBaseLib.Command.CommandBase
{
    class Command_Base : ICommand
    {
        public event EventHandler CanExecuteChanged
        { 
            add => CommandManager
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }
    }
}
