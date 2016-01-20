using System.Windows;
using FileSizeCounter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest
{
  [TestClass]
  public class SizeCounterViewModelTest
  {
    private SizeCounterViewModel _SizeCounterViewModel;

    [TestInitialize]
    public void TestInit()
    {
      Window parentWindow = new Window();
      _SizeCounterViewModel = new SizeCounterViewModel(parentWindow);
    }
    
    [TestMethod]
    public void CanStart_TargetDirectoryIsWhiteSpaceOnly_CannotStart()
    {
      _SizeCounterViewModel.TargetDirectory = "  ";
      var canStart = _SizeCounterViewModel.CanStart();
      Assert.IsFalse(canStart);
    }

    [TestMethod]
    public void CanStart_TargetDirectoryDoesNotExist_CannotStart()
    {
      _SizeCounterViewModel.TargetDirectory = @"A:\FolderShouldNotExist-CarlosLiu-2016-1-20";
      var canStart = _SizeCounterViewModel.CanStart();
      Assert.IsFalse(canStart);
    }

    [TestMethod]
    public void CanStart_TargetDirectoryExists_CannotStart()
    {
      _SizeCounterViewModel.TargetDirectory = @"C:\Windows\";
      var canStart = _SizeCounterViewModel.CanStart();
      Assert.IsTrue(canStart);
    }

    [TestMethod]
    public void CanDelete_SelectedElementIsNull_CannotDelete()
    {
      _SizeCounterViewModel.SelectedElement = null;
      var canDelete = _SizeCounterViewModel.CanDelete();
      Assert.IsFalse(canDelete);
    }

    [TestMethod]
    public void CanDelete_SelectedElementIsTheRoot_CannotDelete()
    {
      InitTheElementsList();

      // element 0 is the root
      _SizeCounterViewModel.SelectedElement = _SizeCounterViewModel.ElementList[0];
      var canDelete = _SizeCounterViewModel.CanDelete();
      Assert.IsFalse(canDelete);
    }

    [TestMethod]
    public void CanDelete_SelectedElementIsNotTheRoot_CanDelete()
    {
      InitTheElementsList();

      // select root file 1
      _SizeCounterViewModel.SelectedElement = _SizeCounterViewModel.ElementList[0].Children[0];
      var canDelete = _SizeCounterViewModel.CanDelete();
      Assert.IsTrue(canDelete);
    }

    // Initialize the directories as below
    // - root
    //    - root-file1.text
    //    - root-file2.text
    //    - sub
    //        - sub file1.txt
    //        - sub file2.txt
    private void InitTheElementsList()
    {
      var rootFolder = new FolderElement("root");
      var rootFile1 = new FileElement("root-file1.text", 100);
      var rootFile2 = new FileElement("root-file1.text", 100);
      var subFolder = new FolderElement("sub");
      var subFile1 = new FileElement("sub file1.txt", 300);
      var subFile2 = new FileElement("sub file2.txt", 400);
      
      rootFolder.Add(rootFile1);
      rootFolder.Add(rootFile2);
      rootFolder.Add(subFolder);

      subFolder.Add(subFile1);
      subFolder.Add(subFile2);

      _SizeCounterViewModel.ElementList.Add(rootFolder);
    }
  }
}
