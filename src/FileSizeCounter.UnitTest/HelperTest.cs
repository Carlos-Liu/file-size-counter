using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest
{
  [TestClass]
  public class HelperTest
  {
    [TestMethod]
    public void CompareOrdinal_TheStringsOnlyDifferInCaseButDoNotIgnoreCaseWhenCompare_TheStringsAreNotEqual()
    {
      string source = "String";
      string compared = "string";

      var isEqual = source.CompareOrdinal(compared);
      Assert.IsFalse(isEqual);
    }

    [TestMethod]
    public void CompareOrdinal_TheStringsOnlyDifferInCaseButIgnoreCaseWhenCompare_TheStringsAreEqual()
    {
      string source = "String";
      string compared = "string";

      var isEqual = source.CompareOrdinal(compared, true);
      Assert.IsTrue(isEqual);
    }
  }
}
