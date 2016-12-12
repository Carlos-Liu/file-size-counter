using System.ComponentModel;
using FileSizeCounter.MicroMvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest.MicroMvvm
{
    [TestClass]
    public class ObservableObjectTests
    {
        private class TestableObservableObject : ObservableObject
        {
            private string _Property1;

            public string Property1
            {
                get { return _Property1; }
                set
                {
                    _Property1 = value;
                    RaisePropertyChanged();
                }
            }

            public void TriggerPropertyChangedByName()
            {
                RaisePropertyChangedByName("Property1");
            }

            public void TriggerPropertyChangedByExpression()
            {
                RaisePropertyChanged(() => Property1);
            }
        }

        [TestMethod]
        public void PropertyChanged_CallRaisePropertyChanged_EventIsTriggeredWithCorrectArgs()
        {
            // Arrange
            bool eventWasRaised = false;
            PropertyChangedEventArgs eventArgs = null;
            var observableObject = new TestableObservableObject();
            observableObject.PropertyChanged += (sender, args) =>
            {
                eventWasRaised = true;
                eventArgs = args;
            }; 

            // Act
            observableObject.Property1 = "New Value";

            // Assert
            Assert.IsTrue(eventWasRaised);
            Assert.AreEqual("Property1", eventArgs.PropertyName);
        }

        [TestMethod]
        public void PropertyChanged_CallRaisePropertyChangedByName_EventIsTriggeredWithCorrectArgs()
        {
            // Arrange
            bool eventWasRaised = false;
            PropertyChangedEventArgs eventArgs = null;
            var observableObject = new TestableObservableObject();
            observableObject.PropertyChanged += (sender, args) =>
            {
                eventWasRaised = true;
                eventArgs = args;
            };

            // Act
            observableObject.TriggerPropertyChangedByName();

            // Assert
            Assert.IsTrue(eventWasRaised);
            Assert.AreEqual("Property1", eventArgs.PropertyName);
        }

        [TestMethod]
        public void PropertyChanged_CallRaisePropertyChangedByExpression_EventIsTriggeredWithCorrectArgs()
        {
            // Arrange
            bool eventWasRaised = false;
            PropertyChangedEventArgs eventArgs = null;
            var observableObject = new TestableObservableObject();
            observableObject.PropertyChanged += (sender, args) =>
            {
                eventWasRaised = true;
                eventArgs = args;
            };

            // Act
            observableObject.TriggerPropertyChangedByExpression();

            // Assert
            Assert.IsTrue(eventWasRaised);
            Assert.AreEqual("Property1", eventArgs.PropertyName);
        }
    }
}
