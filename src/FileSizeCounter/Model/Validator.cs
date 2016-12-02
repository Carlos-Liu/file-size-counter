using System.IO;

namespace FileSizeCounter.Model
{
    internal class Validator
    {
        public static string ValidateInspectDirectory(string directoryString)
        {
            bool doesDirectoryExist = Directory.Exists(directoryString);
            if (!doesDirectoryExist)
                return Res.Resources.Message_Error_InspectDirectoryInvalid;

            return string.Empty;
        }

        public static string ValidateSizeFilterValue(string filterValueString)
        {
            // empty string is treat as valid, and it means the filter value is 0
            if (!string.IsNullOrWhiteSpace(filterValueString))
            {
                double filterValue;
                bool parsedSuccessfully = double.TryParse(filterValueString, out filterValue);
                if (!parsedSuccessfully)
                {
                    return Res.Resources.Message_Error_SizeFilterValueInvalid;
                }
            }
            return string.Empty;
        }
    }
}
