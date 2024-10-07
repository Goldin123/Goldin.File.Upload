using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.CustomException
{
    public class CsvValidationException : Exception
    {
        public int RowNumber { get; }
        public string ColumnName { get; }

        // Constructors for different ways to initialize the exception
        public CsvValidationException(string message) : base(message) { }

        public CsvValidationException(string message, Exception innerException)
            : base(message, innerException) { }

        public CsvValidationException(int rowNumber, string message)
            : base($"Line {rowNumber}: {message}")
        {
            RowNumber = rowNumber;
        }

        public CsvValidationException(int rowNumber, string columnName, string message)
            : base($"Line {rowNumber}, Column '{columnName}': {message}")
        {
            RowNumber = rowNumber;
            ColumnName = columnName;
        }
    }
}
