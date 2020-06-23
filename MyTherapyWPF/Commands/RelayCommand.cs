using System;
using System.Windows.Input;

namespace MyTherapyWPF.Commands
{
    public class RelayCommand : ICommand
    {
        #region Fields 
        readonly Action<object> execute;
        readonly Predicate<object> canExecute;
        #endregion // Fields 
        #region Constructors 
        public RelayCommand(Action<object> execute) : this(execute, null) { }
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
			this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
		}
		#endregion // Constructors 
		#region ICommand Members 
		public bool CanExecute(object parameter)
        {
            return this.canExecute == null || canExecute(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
	        add => CommandManager.RequerySuggested += value;
	        
		    remove=>    CommandManager.RequerySuggested -= value;
	        
        }
        public void Execute(object parameter) { execute(parameter); }
        #endregion // ICommand Members 
    }
}
