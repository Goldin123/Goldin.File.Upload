using Goldin.File.Upload.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Interface
{
    /// <summary>
    /// This is an interface that is responsible to define all csv rules needed to be validated during file validations.
    /// </summary>
    public interface IDataFileCsvValidationRules
    {
        /// <summary>
        /// Check if the content is line delimited by CR or CRLF using regex.
        /// </summary>
        /// <param name="content"></param>
        /// <returns>Successful or not.</returns>
        bool IsValidCsvContent(string content);

        /// <summary>
        /// Check if the headers are 5 and have all needed fields.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns>Successful or not.</returns>
        bool IsValidHeader(string[] headers);

        /// <summary>
        /// Check if the number field is searchable.
        /// </summary>
        /// <param name="dataFileRow"></param>
        /// <returns>Successful or not.</returns>
        bool IsNumberSearchable(DataFileRow dataFileRow);


    }
}
