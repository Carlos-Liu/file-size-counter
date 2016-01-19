using FileSizeCounter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest
{
  [TestClass]
  public class FolderElementTest
  {
    private FolderElement _FolderElementTested;

    [TestInitialize]
    public void Init()
    {
      _FolderElementTested = new FolderElement("folder element");
    }

    [TestMethod]
    public void FolderElement_CreateNewFolder_TheFolderSizeIsZeroByDefault()
    {
      var actual = _FolderElementTested.Size;
      var expected = 0;

      Assert.AreEqual(expected, actual);
    }

    private static FileElement AddNewFile(FolderElement folderElement, int fileSize)
    {
      return AddNewFile(folderElement, "file1", fileSize);
    }

    private static FileElement AddNewFile(FolderElement folderElement, string fileName, int fileSize)
    {
      var fileElement = new FileElement(fileName, fileSize);
      folderElement.Add(fileElement);

      return fileElement;
    }

    #region Tests for Add

    [TestMethod]
    public void Add_CreateNewFolderThenAddAFile_TheFolderSizeRecalculated()
    {
      const int fileSize = 100;

      // act
      AddNewFile(_FolderElementTested, fileSize);

      // assert
      var actual = _FolderElementTested.Size;
      var expected = fileSize;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Add_CreateNewFolderThenAddTwoFiles_TheFolderSizeRecalculated()
    {
      // arrange
      const int fileSize = 100;
      // assign
      AddNewFile(_FolderElementTested, fileSize);
      AddNewFile(_FolderElementTested, fileSize);

      // assert
      var actual = _FolderElementTested.Size;
      var expected = 200;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Add_CreateNewFolderThenAddANewFolder_TheFolderSizeIsZero()
    {
      // arrange
      var folder1 = new FolderElement("f1");

      // assign
      _FolderElementTested.Add(folder1);

      // assert
      var actual = _FolderElementTested.Size;
      var expected = 0;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Add_CreateNewFolderThenAddAFolderIncludesFile_TheFolderSizeIsIncreased()
    {
      // arrange
      var folder1 = new FolderElement("f1");
      AddNewFile(folder1, 100);

      // assign
      _FolderElementTested.Add(folder1);

      // assert
      var actual = _FolderElementTested.Size;
      var expected = 100;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Add_NonEmptyFolderThenAddAFolderIncludesFile_TheFolderSizeIsIncreased()
    {
      // arrange
      var folder1 = new FolderElement("f1");
      AddNewFile(folder1, 100);
      AddNewFile(_FolderElementTested, 200);
      // assign
      _FolderElementTested.Add(folder1);

      // assert
      var actual = _FolderElementTested.Size;
      var expected = 300;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Add_AddFileToFolder_FolderParentSizeShouldRecalculate()
    {
      // arrange
      var rootFolder = new FolderElement("root");
      rootFolder.Add(_FolderElementTested);

      // assign
      const int fileSize = 100;
      AddNewFile(_FolderElementTested, fileSize);

      // assert
      var actual = _FolderElementTested.Parent.Size;
      var expected = fileSize;
      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void Add_AddFileTo2LevelFolders_TheRootEmptyFolderSizeIsCorrect()
    {
      var rootFolder = new FolderElement("root");
      var leafLevel1Folder = new FolderElement("Leaf_Level_1");
      var leafLevel2Folder = new FolderElement("Leaf_Level_2");
      rootFolder.Add(leafLevel1Folder);
      leafLevel1Folder.Add(leafLevel2Folder);

      AddNewFile(leafLevel2Folder, 1000);

      Assert.AreEqual(1000, rootFolder.Size);
    }

    [TestMethod]
    public void Add_AddFileTo2LevelFolders_TheRootFolderSizeIsCorrect()
    {
      var rootFolder = new FolderElement("root");
      AddNewFile(rootFolder, 100);

      var leafLevel1Folder = new FolderElement("Leaf_Level_1");
      var leafLevel2Folder = new FolderElement("Leaf_Level_2");
      rootFolder.Add(leafLevel1Folder);
      leafLevel1Folder.Add(leafLevel2Folder);

      AddNewFile(leafLevel2Folder, 1000);

      Assert.AreEqual(1100, rootFolder.Size);
    }

    #endregion

    #region Test for Remove

    [TestMethod]
    public void Remove_RemoveExistingFile_SizeIsRecalculated()
    {
      var fileElement = AddNewFile(_FolderElementTested, "file1", 100);
      _FolderElementTested.Remove(fileElement);

      Assert.AreEqual(0, _FolderElementTested.Size);
    }

    [TestMethod]
    public void Remove_RemoveExistingFolder_SizeIsRecalculated()
    {
      // arrange
      var folder1 = new FolderElement("f1");
      AddNewFile(folder1, 100);
      _FolderElementTested.Add(folder1);
      AddNewFile(_FolderElementTested, 200);
      // assign
      _FolderElementTested.Remove(folder1);
      // asset
      Assert.AreEqual(200, _FolderElementTested.Size);
    }

    [TestMethod]
    public void Remove_AddFileTo2LevelFoldersAndThenRemoveTheRootFile_TheRootFolderSizeIsCorrect()
    {
      var rootFolder = new FolderElement("root");
      var rootFile = AddNewFile(rootFolder, "rootFile", 100);

      var leafLevel1Folder = new FolderElement("Leaf_Level_1");
      var leafLevel2Folder = new FolderElement("Leaf_Level_2");
      rootFolder.Add(leafLevel1Folder);
      leafLevel1Folder.Add(leafLevel2Folder);

      AddNewFile(leafLevel2Folder, 1000);

      // act
      rootFolder.Remove(rootFile);

      Assert.AreEqual(1000, rootFolder.Size);
    }

    [TestMethod]
    public void Remove_AddFileTo2LevelFoldersAndThenRemoveTheSubLevelFile_TheRootFolderSizeIsCorrect()
    {
      var rootFolder = new FolderElement("root");
      AddNewFile(rootFolder, "rootFile", 100);

      var leafLevel1Folder = new FolderElement("Leaf_Level_1");
      var leafLevel2Folder = new FolderElement("Leaf_Level_2");
      rootFolder.Add(leafLevel1Folder);
      leafLevel1Folder.Add(leafLevel2Folder);

      var fileInLeaf2 = AddNewFile(leafLevel2Folder, "leaf2", 1000);

      // act
      leafLevel2Folder.Remove(fileInLeaf2);

      Assert.AreEqual(100, rootFolder.Size);
    }

    [TestMethod]
    public void Remove_AddFileTo2LevelFoldersAndThenRemoveTheSubLevelFolder_TheRootFolderSizeIsCorrect()
    {
      var rootFolder = new FolderElement("root");
      AddNewFile(rootFolder, "rootFile", 100);

      var leafLevel1Folder = new FolderElement("Leaf_Level_1");
      var leafLevel2Folder = new FolderElement("Leaf_Level_2");
      rootFolder.Add(leafLevel1Folder);
      leafLevel1Folder.Add(leafLevel2Folder);

      AddNewFile(leafLevel2Folder, "leaf2", 1000);

      // act
      rootFolder.Remove(leafLevel1Folder);

      Assert.AreEqual(100, rootFolder.Size);
    }

    #endregion

    #region Test for Parent

    [TestMethod]
    public void Parent_AddFolderBToFolderA_AShouldBeParentOfB()
    {
      // arrange
      var rootFolder = new FolderElement("root");
      rootFolder.Add(_FolderElementTested);

      Assert.AreSame(rootFolder, _FolderElementTested.Parent);
    }

    [TestMethod]
    public void Parent_AddFilderElementToFolder_FolderShouldBeParentOfTheFile()
    {
      var fileElement = AddNewFile(_FolderElementTested, 10);

      Assert.AreSame(_FolderElementTested, fileElement.Parent);
    }

    #endregion

    #region Test for Children

    [TestMethod]
    public void Children_AddFileToFolder_TheFileIsChildOfTheFolder()
    {
      var fileElement = AddNewFile(_FolderElementTested, 10);

      var actualChild = _FolderElementTested.Children[0];

      Assert.AreSame(fileElement, actualChild);
    }

    [TestMethod]
    public void Children_AddFileTo2LevelFolders_TheChildrenAreAllRight()
    {
      var rootFolder = new FolderElement("root");
      var rootFile = AddNewFile(rootFolder, "rootFile", 100);

      var leafLevel1Folder = new FolderElement("Leaf_Level_1");
      var leafLevel2Folder = new FolderElement("Leaf_Level_2");
      rootFolder.Add(leafLevel1Folder);
      leafLevel1Folder.Add(leafLevel2Folder);

      // act
      var actualChildrenCount = rootFolder.Children.Count;

      Assert.AreEqual(2, actualChildrenCount);
      Assert.AreSame(rootFile, rootFolder.Children[0]);
      Assert.AreSame(leafLevel1Folder, rootFolder.Children[1]);

      Assert.AreEqual(1, leafLevel1Folder.Children.Count);
      Assert.AreSame(leafLevel2Folder, leafLevel1Folder.Children[0]);
    }

    [TestMethod]
    public void Children_AddFileTo2LevelFoldersAndThenRemoveTheSubLevelFolder_TheChildrenAreUpdated()
    {
      var rootFolder = new FolderElement("root");
      var rootFile = AddNewFile(rootFolder, "rootFile", 100);

      var leafLevel1Folder = new FolderElement("Leaf_Level_1");
      var leafLevel2Folder = new FolderElement("Leaf_Level_2");
      rootFolder.Add(leafLevel1Folder);
      leafLevel1Folder.Add(leafLevel2Folder);

      AddNewFile(leafLevel2Folder, "leaf2", 1000);

      // act
      rootFolder.Remove(leafLevel1Folder);

      Assert.AreEqual(1, rootFolder.Children.Count);
      Assert.AreSame(rootFile, rootFolder.Children[0]);
    }

    #endregion

    #region Test for ShortName

    [TestMethod]
    public void ShortName_TestForNormalPath_TheLastFolderNameIsTheShortName()
    {
      var testPath = @"C:\Folder1\Folder2";
      var folderElement = new FolderElement(testPath);

      var expected = "Folder2";
      var actual = folderElement.ShortName;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ShortName_TestForOnlyNormalPathEndWithSlash_TheLastFolderNameIsTheShortName()
    {
      var testPath = @"C:\Folder1\";
      var folderElement = new FolderElement(testPath);

      var expected = @"Folder1";
      var actual = folderElement.ShortName;

      Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void ShortName_TestForOnlyRootPathEndWithSlash_TheRootPathWithSlashIsTheShortName()
    {
      var testPath = @"C:\";
      var folderElement = new FolderElement(testPath);

      var expected = @"C:\";
      var actual = folderElement.ShortName;

      Assert.AreEqual(expected, actual);
    }

    #endregion
  }
}