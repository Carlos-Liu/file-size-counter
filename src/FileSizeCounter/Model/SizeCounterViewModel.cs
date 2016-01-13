using System.Collections.ObjectModel;
using System.IO;
using FileSizeCounter.MicroMvvm;

namespace FileSizeCounter.Model
{
  public class SizeCounterViewModel : ObservableObject
  {
    public SizeCounterViewModel()
    {
      TargetDirectory = @"C:\Users\zliu02\Downloads";
    }

    #region Data bindings

    private readonly ObservableCollection<IElement> _ElementList = new ObservableCollection<IElement>();
    private string _TargetDirectory;

    public ObservableCollection<IElement> ElementList
    {
      get { return _ElementList; }
    }

    public string TargetDirectory
    {
      get { return _TargetDirectory; }
      set
      {
        if (!_TargetDirectory.CompareOrdinal(value, true))
        {
          _TargetDirectory = value;
          RaisePropertyChanged();
        }
      }
    }

    private RelayCommand _StartCommand;

    public RelayCommand StartCmd
    {
      get
      {
        if(_StartCommand ==null)
          _StartCommand = new RelayCommand(Start, CanStart);

        return _StartCommand;
      }
    }

    private bool CanStart()
    {
      return !string.IsNullOrWhiteSpace(TargetDirectory) && 
        Directory.Exists(TargetDirectory);
    }

    private void Start()
    {
      ElementList.Clear();

      var rootElement = new FolderElement(TargetDirectory);
      ProcessDirectory(TargetDirectory, rootElement);

      ElementList.Add(rootElement);
    }

    private void ProcessDirectory(string directory, FolderElement currentFolderElement)
    {
      string[] fileEntries = Directory.GetFiles(directory);
      foreach (var fileName in fileEntries)
      {
        var fileInfo = new FileInfo(fileName);
        var fileElement = new FileElement(fileName, fileInfo.Length);
        currentFolderElement.Add(fileElement);
      }

      string[] subDirectoryEntries = Directory.GetDirectories(directory);
      foreach (var subDirectory in subDirectoryEntries)
      {
        var folderElement = new FolderElement(subDirectory);
        currentFolderElement.Add(folderElement);
        ProcessDirectory(subDirectory, folderElement);
      }
    }

    #endregion
  }
}
