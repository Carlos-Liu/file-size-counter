//  Original author - Josh Smith - http://msdn.microsoft.com/en-us/magazine/dd419663.aspx#id0090030

using System;
using System.Diagnostics;
using System.Windows.Input;

namespace FileSizeCounter.MicroMvvm
{
  /// <summary>
  /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
  /// </summary>
  public class RelayCommand<T> : ICommand
  {

    #region Declarations

    readonly Predicate<T> _CanExecute;
    readonly Action<T> _Execute;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    public RelayCommand(Action<T> execute)
      : this(execute, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action<T> execute, Predicate<T> canExecute)
    {

      if (execute == null)
        throw new ArgumentNullException("execute");
      _Execute = execute;
      _CanExecute = canExecute;
    }

    #endregion

    #region ICommand Members

    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add
      {

        if (_CanExecute != null)
          CommandManager.RequerySuggested += value;
      }
      remove
      {

        if (_CanExecute != null)
          CommandManager.RequerySuggested -= value;
      }
    }

    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    [DebuggerStepThrough]
    public Boolean CanExecute(Object parameter)
    {
      return _CanExecute == null || _CanExecute((T)parameter);
    }

    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    public void Execute(Object parameter)
    {
      _Execute((T)parameter);
    }

    #endregion
  }

  /// <summary>
  /// A command whose sole purpose is to relay its functionality to other objects by invoking delegates. The default return value for the CanExecute method is 'true'.
  /// </summary>
  public class RelayCommand : ICommand
  {

    #region Declarations

    readonly Func<Boolean> _CanExecute;
    readonly Action _Execute;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class and the command can always be executed.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    public RelayCommand(Action execute)
      : this(execute, null)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="RelayCommand&lt;T&gt;"/> class.
    /// </summary>
    /// <param name="execute">The execution logic.</param>
    /// <param name="canExecute">The execution status logic.</param>
    public RelayCommand(Action execute, Func<Boolean> canExecute)
    {

      if (execute == null)
        throw new ArgumentNullException("execute");
      _Execute = execute;
      _CanExecute = canExecute;
    }

    #endregion

    #region ICommand Members

    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    public event EventHandler CanExecuteChanged
    {
      add
      {

        if (_CanExecute != null)
          CommandManager.RequerySuggested += value;
      }
      remove
      {

        if (_CanExecute != null)
          CommandManager.RequerySuggested -= value;
      }
    }

    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    [DebuggerStepThrough]
    public Boolean CanExecute(Object parameter)
    {
      return _CanExecute == null || _CanExecute();
    }

    /// <summary>
    /// Implementation of ICommand
    /// </summary>
    public void Execute(Object parameter)
    {
      _Execute();
    }

    #endregion
  }
}
