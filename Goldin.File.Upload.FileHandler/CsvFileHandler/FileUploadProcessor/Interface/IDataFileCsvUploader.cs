using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface
{
    /// <summary>
    /// This is an interface that handles the reading or writing of the csv files being uploaded or downloaded.
    /// </summary>
    public interface IDataFileCsvUploader
    {
        /// <summary>
        /// This gets a file and converts the file into a string.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A string of the file content.</returns>
        Task<string> ReadFileAsync(IFormFile file);
    }
}
