using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using FileSizeCounter.Common;
using FileSizeCounter.MicroMvvm;
using Res;

namespace FileSizeCounter.Model
{
  public class SizeCounterViewModel : ObservableObject, IDataErrorInfo
  {
    private const double DefaultFilterSize = double.MaxValue;
    public SizeCounterViewModel(Window ownerWindow)
    {
      Debug.Assert(ownerWindow != null);

      TargetDirectory = @"C:\Users\zliu02\Downloads";
      OwnerWindow = ownerWindow;
      FilterSize = DefaultFilterSize;
    }

    private string _SizeFilterValue;
    private Window OwnerWindow { get; set; }

    private double _FilterSize;

    private double FilterSize
    {
      get { return _FilterSize; }
      set
      {
        if (_FilterSize != value)
        {
          _FilterSize = value;

          if (ElementList.Count == 0) return;
          Stack<FolderElement> stack = new Stack<FolderElement>();
          stack.Push(ElementList[0] as FolderElement);

          while (stack.Count > 0)
          {
            var currentFolder = stack.Pop();
            foreach (var element in currentFolder.Children)
            {
              // clear previous settings
              element.ShouldBeHighlighted = false;
              // in bytes
              if (element.Size > (value * 1024 * 1024))
              {
                element.ShouldBeHighlighted = true;

                if (element is FolderElement)
                {
                  stack.Push(element as FolderElement);
                }
              }
            }
          }

            //var collection = ElementList;

            //while (collection.Count > 0)
            //{
            //  foreach (var element in collection)
            //  {
            //    // clear previous settings
            //    element.ShouldBeHighlighted = false;
            //    // in bytes
            //    if (element.Size > (value * 1024 * 1024))
            //    {
            //      element.ShouldBeHighlighted = true;
            //    }

            //    collection = element.Children;
            //  }
            //}
          //}
        }
      }
    }

    public IElement SelectedElement { get; set; }
    
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

    public string SizeFilterValue
    {
      get { return _SizeFilterValue; }
      set
      { 
        if(!_SizeFilterValue.CompareOrdinal(value))
        {
          _SizeFilterValue = value;

          if (string.IsNullOrWhiteSpace(value))
            FilterSize = DefaultFilterSize;
          else
          {
            double parsedValue;
            bool succeeded = double.TryParse(value, out parsedValue);
            if (succeeded)
              FilterSize = parsedValue;
          }
        }
      }
    }

    #region Delete Command

    private RelayCommand _DeleteCmd;

    /// <summary>
    /// Command for deleting the selected item
    /// </summary>
    public RelayCommand DeleteCmd
    {
      get
      {
        if (_DeleteCmd == null)
          _DeleteCmd = new RelayCommand(OnDeleteSelectedItem, CanDelete);

        return _DeleteCmd;
      }
    }

    internal bool CanDelete()
    {
      return SelectedElement != null &&
             SelectedElement.Parent != null;
    }

    internal void OnDeleteSelectedItem()
    {
      Debug.Assert(SelectedElement != null);

      var parentElement = SelectedElement.Parent as FolderElement;
      Debug.Assert(parentElement != null);

      var confirmToDelte = MessageBox.Show(Resources.Message_DeleteFileConfirmMsg, Resources.Message_ApplicationTitle,
        MessageBoxButton.YesNo, MessageBoxImage.Question);

      if (confirmToDelte != MessageBoxResult.Yes)
        return;

      try
      {

        if (SelectedElement is FileElement)
        {
          File.Delete(SelectedElement.Name);
        }
        else
        {
          Directory.Delete(SelectedElement.Name, true);
        }

        // do this after the file/folder was removed from disk
        parentElement.Remove(SelectedElement);
      }
      catch (Exception ex)
      {
        MessageBox.Show(string.Format(Resources.Message_Error_FailToDeletePrompt, SelectedElement.Name, ex.Message),
          Resources.Message_ApplicationTitle, MessageBoxButton.OK, MessageBoxImage.Warning);
      }
    }

    #endregion

    #region Open in explorer Command

    private RelayCommand _OpenInExplorerCmd;

    public RelayCommand OpenInExplorerCmd
    {
      get
      {
        if (_OpenInExplorerCmd == null)
          _OpenInExplorerCmd = new RelayCommand(OnOpenInExplorer);

        return _OpenInExplorerCmd;
      }
    }

    private void OnOpenInExplorer()
    {
      Debug.Assert(SelectedElement != null);

      Helper.OpenFolderAndSelectFile(SelectedElement.Name);
    }

    #endregion

    #region Start Inspect Command

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
    internal bool CanStart()
    {
      return !string.IsNullOrWhiteSpace(TargetDirectory) &&
             Directory.Exists(TargetDirectory) &&
             string.IsNullOrEmpty(Error);
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
      Stack<FolderElement> stack = new Stack<FolderElement>();
      var rootElement = new FolderElement(TargetDirectory);
      stack.Push(rootElement);
      
      while (stack.Count > 0)
      {
        var currentFolderElement = stack.Pop();
        var directoryName = currentFolderElement.Name;

        var fileEntries = Directory.GetFiles(directoryName);
        foreach (var fileName in fileEntries)
        {
          var fileInfo = new FileInfo(fileName);
          var fileElement = new FileElement(fileName, fileInfo.Length);
          currentFolderElement.Add(fileElement);
        }

        var subDirectoryEntries = Directory.GetDirectories(directoryName);
        foreach (var subDirectory in subDirectoryEntries)
        {
          var folderElement = new FolderElement(subDirectory);
          currentFolderElement.Add(folderElement);
          
          stack.Push(folderElement);
        }
      }

      return rootElement;
    }

    #endregion


    #endregion

    #region IDataErrorInfo Members

    public string Error
    {
      get
      {
        var error = this["TargetDirectory"];
        if (!string.IsNullOrEmpty(error))
          return error;

        error = this["SizeFilterValue"];
        if (!string.IsNullOrEmpty(error))
          return error;

        return string.Empty;
      }
    }

    public string this[string columnName]
    {
      get
      {
        switch (columnName)
        {
          case "TargetDirectory":
            return Validator.ValidateInspectDirectory(TargetDirectory);

          case "SizeFilterValue":
            return Validator.ValidateSizeFilterValue(SizeFilterValue);

          default:
            return string.Empty;
        }
      }
    }

    #endregion
  }
}