using System;
using FileSizeCounter.MicroMvvm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest.MicroMvvm
{
    [TestClass]
    public class PropertySupportTests
    {
        private class MyClass
        {
            public string Property1 { get { return "property"; } }

            public string GetName()
            {
                return "non-static method";
            }

            public static string StaticMethod()
            {
                return "static method";
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ExtractPropertyName_ArgumentIsNull_ThrowException()
        {
            PropertySupport.ExtractPropertyName<string>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtractPropertyName_CallNonStaticMethod_ThrowException()
        {
            PropertySupport.ExtractPropertyName(() => new MyClass().GetName());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExtractPropertyName_CallStaticMethod_ThrowException()
        {
            PropertySupport.ExtractPropertyName(() => MyClass.StaticMethod());
        }

        [TestMethod]
        public void ExtractPropertyName_GetProperty1Name_NameIsCorrect()
        {
            var actual = PropertySupport.ExtractPropertyName(() => new MyClass().Property1);

            Assert.AreEqual("Property1", actual);
        }
    }
}
