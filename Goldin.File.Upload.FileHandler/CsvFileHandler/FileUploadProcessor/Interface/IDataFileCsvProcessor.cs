using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface
{
    /// <summary>
    /// This interface is responsible to handle all csv file related processing.  
    /// </summary>
    public interface IDataFileCsvProcessor
    {
        /// <summary>
        /// This is responsible to validate and process a file that is being uploaded. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A tuple containing with a bool successful or not with associated message. </returns>
        Task<Tuple<bool, string, string[]?>> ValidateAndProcessCsvAsync(IFormFile file);
    }
}
