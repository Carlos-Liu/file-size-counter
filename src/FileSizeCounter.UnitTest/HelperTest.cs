using System;
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

    [TestMethod]
    public void IsValidNumeric_TextIsIntegerValue_ReturnTrue()
    {
      bool isValid = Helper.IsValidNumeric("2");
      Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValidNumeric_TextIsFloatValue_ReturnTrue()
    {
      bool isValid = Helper.IsValidNumeric("0.5");
      Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValidNumeric_TextIsCharacter_ReturnFalse()
    {
      bool isValid = Helper.IsValidNumeric("a");
      Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValidNumeric_TextIsIntegerValueWithDot_ReturnTrue()
    {
      bool isValid = Helper.IsValidNumeric("2.");
      Assert.IsTrue(isValid);
    }

    [TestMethod]
    public void IsValidNumeric_TextIsFloatValueWithExtraDot_ReturnFalse()
    {
      bool isValid = Helper.IsValidNumeric("2.5.0");
      Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValidNumeric_TextIsFloatValueWithoutLeadingZero_ReturnFalse()
    {
      bool isValid = Helper.IsValidNumeric(".2");
      Assert.IsFalse(isValid);
    }

    [TestMethod]
    public void IsValidNumeric_TextIsIntegerValueWithCharacter_ReturnFalse()
    {
      bool isValid = Helper.IsValidNumeric("2.k");
      Assert.IsFalse(isValid);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void OpenFolderAndSelectFile_FilePathIsNull_ExceptionIsThrown()
    {
        Helper.OpenFolderAndSelectFile(null);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void OpenFolderAndSelectFile_FilePathIsWhitespaceOnly_ExceptionIsThrown()
    {
        Helper.OpenFolderAndSelectFile("   ");
    }
  }
}