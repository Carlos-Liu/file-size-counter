using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using FileSizeCounter.Common;
using FileSizeCounter.MicroMvvm;

namespace FileSizeCounter.Model
{
  public class SizeCounterViewModel : ObservableObject
  {
    public SizeCounterViewModel(Window ownerWindow)
    {
      Debug.Assert(ownerWindow != null);

      TargetDirectory = @"C:\Users\zliu02\Downloads";
      OwnerWindow = ownerWindow;
    }

    private Window OwnerWindow { get; set; }

    #region Data bindings

    private readonly ObservableCollection<IElement> _ElementList = new ObservableCollection<IElement>();
    private string _TargetDirectory;

    public ObservableCollection<IElement> ElementList
    {
      get { return _ElementList; }
    }

    /// <summary>
    ///   The root directory that will be processed to get the inside file/folder size
    /// </summary>
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

    /// <summary>
    ///   Command for the start action
    /// </summary>
    public RelayCommand StartCmd
    {
      get
      {
        if (_StartCommand == null)
          _StartCommand = new RelayCommand(Start, CanStart);

        return _StartCommand;
      }
    }

    // If can start the process
    private bool CanStart()
    {
      return !string.IsNullOrWhiteSpace(TargetDirectory) &&
             Directory.Exists(TargetDirectory);
    }

    private void Start()
    {
      ElementList.Clear();

      var message = "Counting the file/folder size";
      var busyWindow = new BusyIndicatorWindow();

      var result = busyWindow.ExecuteAndWait(OwnerWindow, message, InspectDirectory);
      if (busyWindow.IsSuccessfullyExecuted == true)
      {
        ElementList.Add(result);
        result.IsExpanded = true;
      }
      //TODO: what about the case failed
    }

    private FolderElement InspectDirectory()
    {
      var rootElement = new FolderElement(TargetDirectory);
      ParseDirectory(TargetDirectory, rootElement);

      return rootElement;
    }

    private void ParseDirectory(string directory, FolderElement currentFolderElement)
    {
      //TODO: change the recursive to do-while
      var fileEntries = Directory.GetFiles(directory);
      foreach (var fileName in fileEntries)
      {
        var fileInfo = new FileInfo(fileName);
        var fileElement = new FileElement(fileName, fileInfo.Length);
        currentFolderElement.Add(fileElement);
      }

      var subDirectoryEntries = Directory.GetDirectories(directory);
      foreach (var subDirectory in subDirectoryEntries)
      {
        var folderElement = new FolderElement(subDirectory);
        currentFolderElement.Add(folderElement);
        ParseDirectory(subDirectory, folderElement);
      }
    }

    #endregion
  }
}