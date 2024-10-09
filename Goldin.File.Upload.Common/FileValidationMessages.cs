using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Common
{
    public static class FileValidationMessages
    {
        public static string FileValidatorCsvFormat = "The file you have imported cannot be recognized. Please import a CSV file format.";
        public static string FileEmpty = "The file is empty. Please upload a valid CSV file.";
        public static string FileTooLarge = "The file is more than 100 lines. Please reduce the size of the CSV file.";
        public static string InvalidHeader = "CSV Header line not found or invalid.";
    }
}
