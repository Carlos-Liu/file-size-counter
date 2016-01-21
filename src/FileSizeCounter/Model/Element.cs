using System.Collections.ObjectModel;
using FileSizeCounter.MicroMvvm;

namespace FileSizeCounter.Model
{
  internal abstract class Element : ObservableObject, IElement
  {
    private const string ByteSuffix = " Byte(s)";
    private const string KbSuffix = " K";
    private const string MbSuffix = " M";
    private const string GbSuffix = " G";
    private const float SizeMeasurement = 1024.0f;

    private long _Size;

    public long Size
    {
      get { return _Size; }
      set
      {
        if (_Size != value)
        {
          var delta = value - _Size;

          _Size = value;
          if (Parent != null)
          {
            Parent.Size += delta;
          }

          // notify the view to do refresh
          RaisePropertyChangedByName(@"DisplayString");
        }
      }
    }

    public string DisplaySize
    {
      get
      {
        if (Size < SizeMeasurement)
          return Size + ByteSuffix;

        if (Size < SizeMeasurement*SizeMeasurement)
          return (Size/SizeMeasurement).ToString("F1") + KbSuffix;

        if (Size < SizeMeasurement*SizeMeasurement*SizeMeasurement)
          return (Size/(SizeMeasurement*SizeMeasurement)).ToString("F1") + MbSuffix;

        return (Size/(SizeMeasurement*SizeMeasurement*SizeMeasurement)).ToString("F1") + GbSuffix;
      }
    }

    public string Name { get; protected set; }

    public abstract string ShortName { get; }

    public abstract string ImagePath { get; }

    public IElement Parent { get; set; }

    public abstract ObservableCollection<IElement> Children { get; }

    public abstract void Remove(IElement elementToBeRemoved);

    #region Data bindings

    public string DisplayString
    {
      get { return string.Format("{0} [{1}]", ShortName, DisplaySize); }
    }
    
    private bool _IsExpanded;
    private bool _IsSelected;
    private bool _ShouldBeHighlighted;

    /// <summary>
    ///   Gets/sets whether the TreeViewItem
    ///   associated with this object is expanded.
    /// </summary>
    public bool IsExpanded
    {
      get { return _IsExpanded; }
      set
      {
        if (value != _IsExpanded)
        {
          _IsExpanded = value;
          RaisePropertyChanged();
        }
      }
    }

    /// <summary>
    ///   Gets/sets whether the TreeViewItem
    ///   associated with this object is selected.
    /// </summary>
    public bool IsSelected
    {
      get { return _IsSelected; }
      set
      {
        if (value != _IsSelected)
        {
          _IsSelected = value;
          RaisePropertyChanged();
        }
      }
    }

    public bool ShouldBeHighlighted
    {
      get { return _ShouldBeHighlighted; }
      set
      {
        if (_ShouldBeHighlighted != value)
        {
          _ShouldBeHighlighted = value;
          RaisePropertyChanged();
        }
      }
    }
    #endregion
  }
}