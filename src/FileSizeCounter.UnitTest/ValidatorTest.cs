using FileSizeCounter.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileSizeCounter.UnitTest
{
  [TestClass]
  public class ValidatorTest
  {
    [TestMethod]
    public void ValidateInspectDirectory_DirectoryExists_ReturnEmptyErrorString()
    {
      string error = Validator.ValidateInspectDirectory(@"c:\windows");
      Assert.IsTrue(string.IsNullOrEmpty(error));
    }

    [TestMethod]
    public void ValidateInspectDirectory_DirectoryIsInvalid_ReturnErrorString()
    {
      string error = Validator.ValidateInspectDirectory(@"invalid directory");
      Assert.IsFalse(string.IsNullOrEmpty(error));
    }

    [TestMethod]
    public void ValidateInspectDirectory_DirectoryDoesNotExist_ReturnErrorString()
    {
      string error = Validator.ValidateInspectDirectory(@"C:\ShouldNot Exist - Carlos Liu - 2016");
      Assert.IsFalse(string.IsNullOrEmpty(error));
    }

    [TestMethod]
    public void ValidateSizeFilterValue_ValueIsInteger_NoErrorMessage()
    {
      string error = Validator.ValidateSizeFilterValue("0");
      Assert.IsTrue(string.IsNullOrEmpty(error));
    }

    [TestMethod]
    public void ValidateSizeFilterValue_ValueIsEmpty_NoErrorMessage()
    {
      string error = Validator.ValidateSizeFilterValue("");
      Assert.IsTrue(string.IsNullOrEmpty(error));
    }

    [TestMethod]
    public void ValidateSizeFilterValue_ValueIsFloat_NoErrorMessage()
    {
      string error = Validator.ValidateSizeFilterValue("0.5");
      Assert.IsTrue(string.IsNullOrEmpty(error));
    }

    [TestMethod]
    public void ValidateSizeFilterValue_ValueIsInvalid_ReturnErrorMessage()
    {
      string error = Validator.ValidateSizeFilterValue("0.a");
      Assert.IsFalse(string.IsNullOrEmpty(error));
    }
  }
}
