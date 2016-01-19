using System.Collections.ObjectModel;
using System.IO;

namespace FileSizeCounter.Model
{
  internal class FolderElement : Element
  {
    private readonly ObservableCollection<IElement> _Children = new ObservableCollection<IElement>();

    public FolderElement(string name)
    {
      Name = name;
    }

    public override string ShortName
    {
      get
      {
        var directoryInfo = new DirectoryInfo(Name);
        return directoryInfo.Name;
      }
    }

    public override ObservableCollection<IElement> Children
    {
      get { return _Children; }
    }


    public override string ImagePath
    {
      get { return @"Images\folder16.png"; }
    }

    public void Add(IElement element)
    {
      Size += element.Size;

      element.Parent = this;
      Children.Add(element);
    }

    public void Remove(IElement element)
    {
      Size -= element.Size;

      Children.Remove(element);
    }
  }
}