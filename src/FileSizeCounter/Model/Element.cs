using System.Collections.ObjectModel;
using FileSizeCounter.MicroMvvm;

namespace FileSizeCounter.Model
{
  internal abstract class Element : ObservableObject, IElement
  {
    private const string ByteSuffix = " BYTES";
    private const string KbSuffix = " K";
    private const string MbSuffix = " M";
    private const string GbSuffix = " G";
    private const float SizeMeasurement = 1024.0f;

    private long _Size;
    public string Name { get; protected set; }

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
        }
      }
    }

    public string DisplaySize
    {
      get
      {
        if (Size < SizeMeasurement)
          return Size + ByteSuffix;

        if (Size < SizeMeasurement * SizeMeasurement)
          return (Size / SizeMeasurement).ToString("F1") + KbSuffix;

        if (Size < SizeMeasurement * SizeMeasurement * SizeMeasurement)
          return (Size / (SizeMeasurement * SizeMeasurement)).ToString("F1") + MbSuffix;

        return (Size / (SizeMeasurement * SizeMeasurement * SizeMeasurement)).ToString("F1") + GbSuffix;
      }
    }

    public abstract string ShortName { get; }
    public abstract string ImagePath { get; }

    public IElement Parent { get; set; }

    public abstract ObservableCollection<IElement> Children { get; }

    public string DisplayString
    {
      get
      {
        return string.Format("{0} [{1}]", ShortName, DisplaySize);
      }
    }

    #region Expand / Collapse the item

    private bool _IsExpanded;
    private bool _IsSelected;

    /// <summary>
    /// Gets/sets whether the TreeViewItem 
    /// associated with this object is expanded.
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

        // Expand all the way up to the root.
        //if (_IsExpanded && Parent != null)
        //  Parent.IsExpanded = true;
      }
    }

    /// <summary>
    /// Gets/sets whether the TreeViewItem 
    /// associated with this object is selected.
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

    #endregion

  }
}
