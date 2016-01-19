using FileSizeCounter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest
{
  [TestClass]
  public class ElementTest
  {
    [TestMethod]
    public void Size_IncreseElementSize_ParentSizeAlsoIncreased()
    {
      var folderElement = new FolderElement("root");
      var fileElement1 = new FileElement("file1.txt", 10);
      var fileElement2 = new FileElement("file2.txt", 50);
      folderElement.Add(fileElement1);
      folderElement.Add(fileElement2);
      // change the size from 10 to 100
      fileElement1.Size = 100;
      Assert.AreEqual(150, folderElement.Size);
    }

    [TestMethod]
    public void Size_DecreseElementSize_ParentSizeAlsoDecreased()
    {
      var folderElement = new FolderElement("root");
      var fileElement1 = new FileElement("file1.txt", 100);
      var fileElement2 = new FileElement("file2.txt", 50);
      folderElement.Add(fileElement1);
      folderElement.Add(fileElement2);
      // change the size from 100 to 10
      fileElement1.Size = 10;
      Assert.AreEqual(60, folderElement.Size);
    }

    [TestMethod]
    public void DisplayString_FileNameIsFullPath_DisplayFormatIsCorrect()
    {
      var fileElement = new FileElement(@"c:\folder1\file1.txt", 1024);
      var actualDisplayString = fileElement.DisplayString;
      Assert.AreEqual("file1.txt [1.0 K]", actualDisplayString);
    }

    [TestMethod]
    public void IsExpanded_ChangeThePropertyValue_ValueIsSetCorrectly()
    {
      var folerElement = new FolderElement("folder");
      folerElement.IsExpanded = true;
      Assert.IsTrue(folerElement.IsExpanded);

      folerElement.IsExpanded = false;
      Assert.IsFalse(folerElement.IsExpanded);
    }

    [TestMethod]
    public void IsSelected_ChangeThePropertyValue_ValueIsSetCorrectly()
    {
      var folerElement = new FolderElement("folder");
      folerElement.IsSelected = true;
      Assert.IsTrue(folerElement.IsSelected);

      folerElement.IsSelected = false;
      Assert.IsFalse(folerElement.IsSelected);
    }
  }
}