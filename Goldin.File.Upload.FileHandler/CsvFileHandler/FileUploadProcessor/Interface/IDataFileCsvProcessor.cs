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
        Task<Tuple<bool, string>> ValidateAndProcessCsvAsync(IFormFile file);
    }
}
