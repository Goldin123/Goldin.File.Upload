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
    public class DataFileCsvUploader : IDataFileCsvUploader
    {
        private readonly ILogger<DataFileCsvUploader> _logger;
        public DataFileCsvUploader(ILogger<DataFileCsvUploader> logger)
        {
            _logger = logger;
        }

        public async Task<string> ReadFileAsync(IFormFile file)
        {
            try
            {
                if (file == null)
                {
                    _logger.LogInformation(string.Format(string.Format("{0} - {1} - {2} - no file found."), LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvUploader), nameof(ReadFileAsync)));
                    throw new ArgumentNullException(nameof(file));
                }
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            catch (IOException ex)
            {
                _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}"), LogMessage.FileExceptionLogMessage, nameof(DataFileCsvUploader), nameof(ReadFileAsync), ex.Message));
                throw new Exception(Notification.FileExceptionMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvUploader), nameof(ReadFileAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }
    }
}
