using System;
using FileSizeCounter.MicroMvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest.MicroMvvm
{
    #region Generic tests

    [TestClass]
    public class GenericRelayCommandTests
    {
        private bool _IsCommandExecuted;

        [TestInitialize]
        public void TestInit()
        {
            _IsCommandExecuted = false;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ActionIsNull_ThrowException()
        {
            var command = new RelayCommand<bool>(null);
        }

        [TestMethod]
        public void CanExecute_NotSpecifyInCtor_DefaultIsTrue()
        {
            var command = new RelayCommand<bool>(OnCommandExecuted);

            var canExecute = command.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void CanExecute_SpecifyInCtor_ReturnCorrectResult()
        {
            var command = new RelayCommand<bool>(OnCommandExecuted, CanExecute);

            var canExecute = command.CanExecute(false);

            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void Execute_AssignActionInCtor_TheActionIsExecuted()
        {
            var command = new RelayCommand<bool>(OnCommandExecuted);

            command.Execute(true);

            Assert.IsTrue(_IsCommandExecuted);
        }

        private void OnCommandExecuted(bool arg)
        {
            _IsCommandExecuted = true;
        }

        private bool CanExecute(bool arg)
        {
            return arg;
        }
    }

    #endregion

    #region Non-generic tests

    [TestClass]
    public class RelayCommandTests
    {
        private bool _IsCommandExecuted;

        [TestInitialize]
        public void TestInit()
        {
            _IsCommandExecuted = false;
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void Constructor_ActionIsNull_ThrowException()
        {
            var command = new RelayCommand(null);
        }

        [TestMethod]
        public void CanExecute_NotSpecifyInCtor_DefaultIsTrue()
        {
            var command = new RelayCommand(OnCommandExecuted);

            var canExecute = command.CanExecute(null);

            Assert.IsTrue(canExecute);
        }

        [TestMethod]
        public void CanExecute_SpecifyInCtor_ReturnCorrectResult()
        {
            var command = new RelayCommand(OnCommandExecuted, CanExecute);

            var canExecute = command.CanExecute(null);

            Assert.IsFalse(canExecute);
        }

        [TestMethod]
        public void Execute_AssignActionInCtor_TheActionIsExecuted()
        {
            var command = new RelayCommand(OnCommandExecuted);

            command.Execute(null);

            Assert.IsTrue(_IsCommandExecuted);
        }

        private void OnCommandExecuted()
        {
            _IsCommandExecuted = true;
        }

        private bool CanExecute()
        {
            return false;
        }
    }

    #endregion

}
