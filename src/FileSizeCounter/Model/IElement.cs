using System.Collections.ObjectModel;

namespace FileSizeCounter.Model
{
  public interface IElement
  {
    string Name { get; }

    /// <summary>
    ///   The size of the element (in byte)
    /// </summary>
    long Size { get; set; }

    /// <summary>
    ///   The string will be displayed on the UI for the element size
    /// </summary>
    string DisplaySize { get; }

    /// <summary>
    ///   Short name of the element, e.g. the short name for
    ///   element "c:\file.txt" will be "file.txt"
    /// </summary>
    string ShortName { get; }

    /// <summary>
    ///   The friendly display string on the UI, e.g. including short name and display
    ///   size
    /// </summary>
    string DisplayString { get; }

    /// <summary>
    ///   The element image path (for display purpose)
    /// </summary>
    string ImagePath { get; }

    /// <summary>
    ///   Parent of the element
    /// </summary>
    IElement Parent { get; set; }

    /// <summary>
    ///   The children elements list
    /// </summary>
    ObservableCollection<IElement> Children { get; }
    /// <summary>
    /// Remove the specified element from the children list
    /// </summary>
    /// <param name="elementToBeRemoved"></param>
    void Remove(IElement elementToBeRemoved);
  }
}