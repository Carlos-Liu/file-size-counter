using System;
using FileSizeCounter.Common;
using FileSizeCounter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace FileSizeCounter.UnitTest.Model
{
    [TestClass]
    public class SizeCounterViewModelTest
    {
        private SizeCounterViewModel _SizeCounterViewModel;
        private IBusyIndicatorWindow _BusyIndicatorWindowStub;

        [TestInitialize]
        public void TestInit()
        {
            _BusyIndicatorWindowStub = Substitute.For<IBusyIndicatorWindow>();
            _SizeCounterViewModel = new SizeCounterViewModel(_BusyIndicatorWindowStub);
        }

        [TestMethod]
        public void TargetDirectory_DeDefault_IsPartitionC()
        {
            Assert.AreEqual(@"C:\", _SizeCounterViewModel.TargetDirectory);
        }

        [TestMethod]
        public void FilterSize_DeDefault_IsMaxDoubleValue()
        {
            Assert.AreEqual(double.MaxValue, _SizeCounterViewModel.FilterSize);
        }

        [TestMethod]
        public void HideSmallerElements_ByDefault_IsFalse()
        {
            Assert.IsFalse(_SizeCounterViewModel.HideSmallerElements);
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
        public void CanStart_SizeFilterValueIsInvalid_CannotStart()
        {
            _SizeCounterViewModel.SizeFilterValue = "invalid";
            var canStart = _SizeCounterViewModel.CanStart();
            Assert.IsFalse(canStart);
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

        [TestMethod]
        public void ProcessResult_ByDefault_DefaultValueIsNull()
        {
            Assert.IsNull(_SizeCounterViewModel.ProcessResult);
        }

        [TestMethod]
        public void ProcessDetailedErrors_ByDefault_DefaultValueIsNull()
        {
            Assert.IsNull(_SizeCounterViewModel.ProcessDetailedErrors);
        }

        [TestMethod]
        public void ProcessDetailedErrors_ErrorIsNull_ProcessResultShowSuccessful()
        {
            _SizeCounterViewModel.ProcessDetailedErrors = null;
            Assert.AreEqual("Inspect successful.", _SizeCounterViewModel.ProcessResult);
        }

        [TestMethod]
        public void ProcessDetailedErrors_ErrorIsNotNull_ProcessResultShowErrorPrompt()
        {
            _SizeCounterViewModel.ProcessDetailedErrors = "fake error.";
            Assert.AreEqual(
                "Error occurred during the inspecting process. Please see the tooltip from the left icon to get detailed information.",
                _SizeCounterViewModel.ProcessResult);
        }

        [TestMethod]
        public void SizeFilterValue_SetEmptyValue_FilterSizeIsSetAsDefaultValue()
        {
            _SizeCounterViewModel.SizeFilterValue = string.Empty;
            Assert.AreEqual(double.MaxValue, _SizeCounterViewModel.FilterSize);
        }

        [TestMethod]
        public void SizeFilterValue_SetValidValue_FilterSizeIsUpdated()
        {
            _SizeCounterViewModel.SizeFilterValue = "0.5";
            Assert.AreEqual(0.5, _SizeCounterViewModel.FilterSize);
        }

        [TestMethod]
        public void SizeFilterValue_SetInvalidValue_FilterSizeIsNotUpdated()
        {
            _SizeCounterViewModel.FilterSize = 1.5;
            _SizeCounterViewModel.SizeFilterValue = "0.5a";
            Assert.AreEqual(1.5, _SizeCounterViewModel.FilterSize);
        }

        [TestMethod]
        public void StartCommand_NoExceptionWhenInspect_InpectResultIsAddedToElementList()
        {
            // Arrange
            var returnFolderElement = new FolderElement("test");
            _BusyIndicatorWindowStub.ExecuteAndWait(Arg.Any<string>(), Arg.Any<Func<FolderElement>>())
                .Returns(returnFolderElement);

            _BusyIndicatorWindowStub.IsSuccessfullyExecuted.Returns(true);


            // Act
            _SizeCounterViewModel.StartCmd.Execute(null);

            // Assert
            Assert.AreEqual(1, _SizeCounterViewModel.ElementList.Count);
            Assert.AreSame(returnFolderElement, _SizeCounterViewModel.ElementList[0]);
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
