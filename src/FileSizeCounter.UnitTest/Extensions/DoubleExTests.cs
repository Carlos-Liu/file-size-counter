using FileSizeCounter.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest.Extensions
{
    [TestClass]
    public class DoubleExTests
    {
        [TestMethod]
        public void AlmostEqualTo_TwoValuesAreExactlyTheSame_ReturnTrue()
        {
            const double value1 = 0.0000001;
            const double value2 = 0.0000001;

            bool isAlmostEqual = value1.AlmostEqualTo(value2);

            Assert.IsTrue(isAlmostEqual);
        }

        [TestMethod]
        public void AlmostEqualTo_TwoValuesAreDiffsWithinTolerance_ReturnTrue()
        {
            const double value1 = 0;
            const double value2 = 0.00000001;

            bool isAlmostEqual = value1.AlmostEqualTo(value2);

            Assert.IsTrue(isAlmostEqual);
        }

        [TestMethod]
        public void AlmostEqualTo_TwoValuesAreDiffsExceedingTolerance_ReturnFalse()
        {
            const double value1 = 0.0000001;
            const double value2 = 0.00000021;

            bool isAlmostEqual = value1.AlmostEqualTo(value2);

            Assert.IsFalse(isAlmostEqual);
        }

        [TestMethod]
        public void AlmostEqualTo_TwoValuesAreDiffsEqualToTolerance_ReturnFalse()
        {
            const double value1 = 0.0000001;
            const double value2 = 0;

            bool isAlmostEqual = value1.AlmostEqualTo(value2);

            Assert.IsFalse(isAlmostEqual);
        }
    }
}
