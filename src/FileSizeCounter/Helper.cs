using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace FileSizeCounter
{
  public static class Helper
  {
    public static bool CompareOrdinal(this string source, string target, bool ignoreCase = false)
    {
      var comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

      return string.Compare(source, target, comparison) == 0;
    }
    /// <summary>
    /// If the text is a valid numeric value
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static bool IsValidNumeric(string text)
    {
      Regex regex = new Regex(@"^([0-9]+(?:\.[0-9]*)?)$");
      var isValidNumber = regex.IsMatch(text);
      return isValidNumber;
    }

    [DllImport("shell32.dll", CharSet = CharSet.Unicode)]
    private static extern IntPtr ILCreateFromPathW(string pszPath);

    [DllImport("shell32.dll")]
    private static extern int SHOpenFolderAndSelectItems(IntPtr pidlFolder, int cild, IntPtr apidl, int dwFlags);

    [DllImport("shell32.dll")]
    private static extern void ILFree(IntPtr pidl);
    /// <summary>
    /// Open the specified file/path in windows explorer and set focus on the file/path
    /// </summary>
    /// <param name="filePath"></param>
    public static void OpenFolderAndSelectFile(string filePath)
    {
      if (filePath == null)
        throw new ArgumentNullException("filePath");

      IntPtr pidl = ILCreateFromPathW(filePath);
      SHOpenFolderAndSelectItems(pidl, 0, IntPtr.Zero, 0);
      ILFree(pidl);
    }
  }
}