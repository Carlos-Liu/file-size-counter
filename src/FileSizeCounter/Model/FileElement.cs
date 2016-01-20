using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FileSizeCounter.Model
{
  internal class FileElement : Element
  {
    public FileElement(string name, long size)
    {
      Name = name;
      Size = size;
    }

    public override string ShortName
    {
      get { return Path.GetFileName(Name); }
    }

    public override ObservableCollection<IElement> Children
    {
      get { return new ObservableCollection<IElement>(); }
    }

    public override string ImagePath
    {
      get { return @"Images\file16.png"; }
    }

    public override void Remove(IElement elementToBeRemoved)
    {
      throw new NotSupportedException("Invalid operation on file element.");
    }
  }
}