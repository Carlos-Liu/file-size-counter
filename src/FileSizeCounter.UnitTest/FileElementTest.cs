using System;
using FileSizeCounter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest
{
  [TestClass]
  public class FileElementTest
  {
    [TestMethod]
    public void ShortName_NormalFullPath_OnlyReturnTheFileName()
    {
      var fileElement = new FileElement(@"C:\folder1\1.txt", 100);
      var expected = "1.txt";
      var actual = fileElement.ShortName;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void DisplaySize_FileIs1023Bytes_Display1023bytes()
    {
      var fileElement = new FileElement("file1.text", 1023);

      var actual = fileElement.DisplaySize;
      Assert.AreEqual("1023 Byte(s)", actual);
    }

    [TestMethod]
    public void DisplaySize_FileIs1025Bytes_DisplayKbFormat()
    {
      var fileElement = new FileElement("file1.text", 1025);

      var actual = fileElement.DisplaySize;
      Assert.AreEqual("1.0 K", actual);
    }

    [TestMethod]
    public void DisplaySize_FileIsMaximumKBytes_DisplayKbFormat()
    {
      var fileElement = new FileElement("file1.text", 1024*1024 - 1);

      var actual = fileElement.DisplaySize;
      Assert.AreEqual("1024.0 K", actual);
    }

    [TestMethod]
    public void DisplaySize_FileIsMinMBytes_DisplayMbFormat()
    {
      var fileElement = new FileElement("file1.text", 1024*1024 + 1);

      var actual = fileElement.DisplaySize;
      Assert.AreEqual("1.0 M", actual);
    }

    [TestMethod]
    public void DisplaySize_FileIsMaximumMBytes_DisplayGbFormat()
    {
      var fileElement = new FileElement("file1.text", 1024*1024*1024 - 1);

      var actual = fileElement.DisplaySize;
      Assert.AreEqual("1.0 G", actual);
    }

    [TestMethod]
    public void Children_ForFileElement_NoChildren()
    {
        var fileElement = new FileElement("file1.text", 100);
        Assert.AreEqual(0, fileElement.Children.Count);
    }

    [TestMethod]
    public void ImagePath_ForFileElement_ImageIsRight()
    {
        var fileElement = new FileElement("file1.text", 100);
        Assert.AreEqual(@"Images\file16.png", fileElement.ImagePath);
    }
      
    [ExpectedException(typeof(NotSupportedException))]
    [TestMethod]
    public void Remove_Call_ExceptionIsThrown()
    {
        var fileElement = new FileElement("file1.text", 100);

        fileElement.Remove(null);
    }
  }
}