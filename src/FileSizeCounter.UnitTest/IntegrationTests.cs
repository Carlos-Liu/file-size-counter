using System;
using FileSizeCounter.Common;
using FileSizeCounter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FileSizeCounter.UnitTest
{
    [TestClass]
    public class IntegrationTests
    {
        private SizeCounterViewModel _SizeCounterViewModel;
        private IBusyIndicatorWindow _BusyIndicatorWindowStub;
        private const long OneMb = 1024 * 1024;

        private FolderElement _RootFolder;
        private IElement _RootFile1;
        private IElement _RootFile2;
        private FolderElement _SubFolder1;
        private IElement _SubFile1;
        private IElement _SubFile2;
        private FolderElement _SubFolder2;
        private IElement _SubFile3;
        private IElement _SubFile4;

        [TestInitialize]
        public void TestInit()
        {
            _BusyIndicatorWindowStub = Substitute.For<IBusyIndicatorWindow>();
            _SizeCounterViewModel = new SizeCounterViewModel(_BusyIndicatorWindowStub);
            InitialElementList();
        }

        // Initialize the directories as below
        // - root
        //    - root-file1.text     1 MB
        //    - root-file2.text     3 MB
        //    - sub-1
        //        - sub file1.txt   2 MB + 1 byte
        //        - sub file2.txt   2 MB - 1 byte
        //    - sub-2
        //        - sub file3.txt   1 MB
        //        - sub file4.txt   1 MB
        private void InitialElementList()
        {

            _RootFolder = new FolderElement("root");
            _RootFile1 = new FileElement("root-file1.text", OneMb);
            _RootFile2 = new FileElement("root-file1.text", 3 * OneMb);

            _SubFolder1 = new FolderElement("sub-1");
            _SubFile1 = new FileElement("sub file1.txt", 2 * OneMb + 1);
            _SubFile2 = new FileElement("sub file2.txt", 2 * OneMb - 1);
            _SubFolder1.Add(_SubFile1);
            _SubFolder1.Add(_SubFile2);

            _SubFolder2 = new FolderElement("sub-2");
            _SubFile3 = new FileElement("sub file3.txt", OneMb);
            _SubFile4 = new FileElement("sub file4.txt", OneMb);
            _SubFolder2.Add(_SubFile3);
            _SubFolder2.Add(_SubFile4);

            _RootFolder.Add(_RootFile1);
            _RootFolder.Add(_RootFile2);
            _RootFolder.Add(_SubFolder1);
            _RootFolder.Add(_SubFolder2);

            _SizeCounterViewModel.ElementList.Add(_RootFolder);
        }

        [TestMethod]
        public void IsVisible_ByDefault_ALlElementsAreVisible()
        {
            // set filter size as 5 MB
            _SizeCounterViewModel.SizeFilterValue = "5";

            foreach (var element in _SizeCounterViewModel.ElementList)
            {
                Assert.IsTrue(element.IsVisible);
            }
        }

        [TestMethod]
        public void ShouldBeHighlighted_ByDefault_ALlElementsAreNotHighlighted()
        {
            // set filter size as 0.1 MB
            _SizeCounterViewModel.SizeFilterValue = "0.1";

            foreach (var element in _SizeCounterViewModel.ElementList)
            {
                Assert.IsFalse(element.ShouldBeHighlighted);
            }
        }

        [TestMethod]
        public void HideSmallerElements_SetFilterSizeAs5MB_ALlElementsAreNotVisibleExceptTheRoot()
        {
            _SizeCounterViewModel.HideSmallerElements = true;
            // set filter size as 100 MB
            _SizeCounterViewModel.SizeFilterValue = "100";

            Assert.IsTrue(_RootFolder.IsVisible);
            var elements = _SizeCounterViewModel.ElementList;
            for (int i = 1; i < elements.Count; i++)
            {
                var element = elements[i];
                Assert.IsFalse(element.IsVisible);
            }
        }

        [TestMethod]
        public void ShouldBeHighlighted_SetFilterSizeAs1MB_ALlElementsWillBeHighlightedExceptTheRoot()
        {
            _SizeCounterViewModel.HighlightElements = true;
            // set filter size as 1 MB
            _SizeCounterViewModel.SizeFilterValue = "1";

            Assert.IsFalse(_RootFolder.ShouldBeHighlighted);
            var elements = _SizeCounterViewModel.ElementList;
            for (int i = 1; i < elements.Count; i++)
            {
                var element = elements[i];
                Assert.IsTrue(element.ShouldBeHighlighted);
            }
        }

        [TestMethod]
        public void HideSmallerElements_SetFilterSizeAs2MB_ElementSmallerThan2MBWillBeHidden()
        {
            // Arrange
            // set filter size as 2 MB
            _SizeCounterViewModel.SizeFilterValue = "2";
            
            // Act
            _SizeCounterViewModel.HideSmallerElements = true;

            // Assert - the elements' size smaller than 2 MB will be hidden
            Assert.IsTrue(_RootFolder.IsVisible);
            Assert.IsFalse(_RootFile1.IsVisible);
            Assert.IsTrue(_RootFile2.IsVisible);
            Assert.IsTrue(_SubFolder1.IsVisible);
            Assert.IsTrue(_SubFile1.IsVisible);
            Assert.IsFalse(_SubFile2.IsVisible);
            Assert.IsTrue(_SubFolder2.IsVisible);
            Assert.IsFalse(_SubFile3.IsVisible);
            Assert.IsFalse(_SubFile4.IsVisible);
        }

        [TestMethod]
        public void ShouldBeHighlighted_SetFilterSizeAs2_ElementLargerThan2MBWillBeHighlighted()
        {
            // Arrange
            // set filter size as 2 MB
            _SizeCounterViewModel.SizeFilterValue = "2";

            // Act
            _SizeCounterViewModel.HighlightElements = true;

            // Assert - the elements' size >= 2 MB will be highlighted

            // the root will never be highlighted
            Assert.IsFalse(_RootFolder.ShouldBeHighlighted);

            Assert.IsFalse(_RootFile1.ShouldBeHighlighted);
            Assert.IsTrue(_RootFile2.ShouldBeHighlighted);
            Assert.IsTrue(_SubFolder1.ShouldBeHighlighted);
            Assert.IsTrue(_SubFile1.ShouldBeHighlighted);
            Assert.IsFalse(_SubFile2.ShouldBeHighlighted);
            Assert.IsTrue(_SubFolder2.ShouldBeHighlighted);
            Assert.IsFalse(_SubFile3.ShouldBeHighlighted);
            Assert.IsFalse(_SubFile4.ShouldBeHighlighted);
        }

        [TestMethod]
        public void InspectFor2ndTime_PreviousSettingsWereKept()
        {
            // Arrange

            // settings base on the initial element list
            _SizeCounterViewModel.SizeFilterValue = "2";
            _SizeCounterViewModel.HighlightElements = true;
            _SizeCounterViewModel.HideSmallerElements = true;

            // settings for mocking the inspecting result
            var rootFolder = new FolderElement("root");
            var fileElement1 = new FileElement("file1.log", OneMb);
            var fileElement2 = new FileElement("file2.log", 3*OneMb);

            rootFolder.Add(fileElement1);
            rootFolder.Add(fileElement2);
            rootFolder.IsExpanded = false;

            _BusyIndicatorWindowStub.ExecuteAndWait(Arg.Any<string>(), Arg.Any<Func<FolderElement>>())
                .Returns(rootFolder);
            _BusyIndicatorWindowStub.IsSuccessfullyExecuted.Returns(true);

            // Act
            _SizeCounterViewModel.StartCmd.Execute(null);

            // Assert
            Assert.AreSame(rootFolder, _SizeCounterViewModel.ElementList[0]);
            Assert.IsTrue(rootFolder.IsExpanded);

            // Verify the previous settings
            Assert.IsTrue(rootFolder.IsVisible);
            Assert.IsFalse(rootFolder.ShouldBeHighlighted);

            Assert.IsFalse(fileElement1.IsVisible);
            Assert.IsFalse(fileElement1.ShouldBeHighlighted);

            Assert.IsTrue(fileElement2.IsVisible);
            Assert.IsTrue(fileElement2.ShouldBeHighlighted);
        }
    }
}
