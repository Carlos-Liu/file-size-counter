using System;
using System.Collections.ObjectModel;
using FileSizeCounter.Constants;
using FileSizeCounter.MicroMvvm;
using Res;

namespace FileSizeCounter.Model
{
  internal abstract class Element : ObservableObject, IElement
  {

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
          double power2 = Math.Pow(Consts.SizeMeasurement, 2);
          double power3 = Math.Pow(Consts.SizeMeasurement, 3);

          if (Size < Consts.SizeMeasurement)
              return Size + Resources.Suffix_SizeUnit_Bytes;

          if (Size < power2)
              return (Size / Consts.SizeMeasurement).ToString("F1") + Resources.Suffix_SizeUnit_Kilobytes;

          if (Size < power3)
              return (Size / power2).ToString("F1") + Resources.Suffix_SizeUnit_Megabytes;

          return (Size / power3).ToString("F1") + Resources.Suffix_SizeUnit_Gigabytes;
      }
    }

    public string Name { get; protected set; }

    public abstract string ShortName { get; }

    public abstract string ImagePath { get; }

    public IElement Parent { get; set; }

    public bool IsVisible
    {
        get { return _IsVisible; }
        set
        {
            if (_IsVisible != value)
            {
                _IsVisible = value;
                RaisePropertyChanged();
            }
        }
    }

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
    private bool _IsVisible = true;

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