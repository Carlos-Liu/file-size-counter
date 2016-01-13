using System;

namespace FileSizeCounter
{
  public static class Helper
  {
    public static bool CompareOrdinal(this string source, string target, bool ignoreCase = false)
    {
      StringComparison comparison = ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal;

      return string.Compare(source, target, comparison) == 0;
    }
  }
}
