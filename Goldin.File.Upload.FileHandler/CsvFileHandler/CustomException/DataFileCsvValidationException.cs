using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.CustomException
{
    public class DataFileCsvCustomException : Exception
    {
        public int RowNumber { get; }
        public string ColumnName { get; }

        // Constructors for different ways to initialize the exception
        public DataFileCsvCustomException(string message) : base(message) { }

        public DataFileCsvCustomException(string message, Exception innerException)
            : base(message, innerException) { }

        public DataFileCsvCustomException(int rowNumber, string message)
            : base($"Line {rowNumber}: {message}")
        {
            RowNumber = rowNumber;
        }

        public DataFileCsvCustomException(int rowNumber, string columnName, string message)
            : base($"Line {rowNumber}, Column '{columnName}': {message}")
        {
            RowNumber = rowNumber;
            ColumnName = columnName;
        }
    }
}
