using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

//Event Design: http://msdn.microsoft.com/en-us/library/ms229011.aspx

namespace FileSizeCounter.MicroMvvm
{
  /// <summary>
  /// One <see cref="INotifyPropertyChanged"/> implementation to facilitate MVVM code.
  /// </summary>
  [Serializable]
  public abstract class ObservableObject : INotifyPropertyChanged
  {
    /// <summary>
    /// Occurs when property changed.
    /// </summary>
    [field: NonSerialized]
    public event PropertyChangedEventHandler PropertyChanged;

    /// <summary>
    /// Called when property changed, and derived classes can override this to customize the behavior.
    /// </summary>
    /// <param name="e">event argument.</param>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
    {
      var handler = PropertyChanged;
      if (handler != null)
      {
        handler(this, e);
      }
    }

    /// <summary>
    /// Helper method to raise the PropetyChanged event.
    /// </summary>
    /// <typeparam name="T">the property type.</typeparam>
    /// <param name="propertyExpresssion">property name.</param>
    protected void RaisePropertyChanged<T>(Expression<Func<T>> propertyExpresssion)
    {
      var propertyName = PropertySupport.ExtractPropertyName(propertyExpresssion);
      RaisePropertyChanged(propertyName);
    }

    /// <summary>
    /// Helper method to raise the ProperyChanged event.
    /// </summary>
    /// <param name="propertyName">property name.</param>
    protected void RaisePropertyChanged([CallerMemberName] String propertyName = "")
    {
      VerifyPropertyName(propertyName);
      OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
    }

    /// <summary>
    /// Warns the developer if this Object does not have a public property with
    /// the specified name. This method does not exist in a Release build.
    /// </summary>
    [Conditional("DEBUG")]
    [DebuggerStepThrough]
    public void VerifyPropertyName(String propertyName)
    {
      // verify that the property name matches a real,  
      // public, instance property on this Object.
      if (TypeDescriptor.GetProperties(this)[propertyName] == null)
      {
        Debug.Fail("Invalid property name: " + propertyName);
      }
    }
  }
}
