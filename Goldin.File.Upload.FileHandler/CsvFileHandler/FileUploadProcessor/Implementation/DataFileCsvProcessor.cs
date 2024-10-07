using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Implementation
{
    /// <summary>
    /// This implements all functionality needed for IDataFileCsvProcessor
    /// </summary>
    public class DataFileCsvProcessor : IDataFileCsvProcessor
    {
        private readonly ILogger<DataFileCsvProcessor> logger;
  
        public DataFileCsvProcessor(ILogger<DataFileCsvProcessor> logger) 
        {
            this.logger = logger;
        }

        public async Task<Tuple<bool,string>> ValidateAndProcessCsvAsync(IFormFile file) 
        {
            return new Tuple<bool, string>(true, "");
        }
    }
}
