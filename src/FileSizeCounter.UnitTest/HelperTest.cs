using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest
{
  [TestClass]
  public class HelperTest
  {
    [TestMethod]
    public void CompareOrdinal_TheStringsOnlyDifferInCaseButDoNotIgnoreCaseWhenCompare_TheStringsAreNotEqual()
    {
      var source = "String";
      var compared = "string";

      var isEqual = source.CompareOrdinal(compared);
      Assert.IsFalse(isEqual);
    }

    [TestMethod]
    public void CompareOrdinal_TheStringsOnlyDifferInCaseButIgnoreCaseWhenCompare_TheStringsAreEqual()
    {
      var source = "String";
      var compared = "string";

      var isEqual = source.CompareOrdinal(compared, true);
      Assert.IsTrue(isEqual);
    }
  }
}