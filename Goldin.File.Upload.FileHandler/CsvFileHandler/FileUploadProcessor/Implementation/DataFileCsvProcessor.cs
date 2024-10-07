using Goldin.File.Upload.Common;
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
        private readonly ILogger<DataFileCsvProcessor> _logger;
        private readonly IDataFileCsvUploader _dataFileCsvUploader;

        public DataFileCsvProcessor(ILogger<DataFileCsvProcessor> logger, IDataFileCsvUploader dataFileCsvUploader) 
        {
            _logger = logger;
            _dataFileCsvUploader = dataFileCsvUploader;
        }

        /// <summary>
        /// This implements the reading of a csv file into a string of contents.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A string of the file.</returns>
        public async Task<Tuple<bool,string>> ValidateAndProcessCsvAsync(IFormFile file) 
        {
            try 
            {
                //Read the file.
                string content = await _dataFileCsvUploader.ReadFileAsync(file);

                if (content == null) 
                {
                    _logger.LogInformation(string.Format(string.Format("{0} - {1} - {2} - file is empty."), LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvProcessor), nameof(ValidateAndProcessCsvAsync)));
                    throw new ArgumentNullException(nameof(content));
                }

            }
            catch (Exception ex) 
            {

            }
            return new Tuple<bool, string>(true, "");
        }
    }
}
