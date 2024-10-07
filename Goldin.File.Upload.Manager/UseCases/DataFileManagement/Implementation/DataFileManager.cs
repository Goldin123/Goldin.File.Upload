using Goldin.File.Upload.Common;
using Goldin.File.Upload.Database.Interface;
using Goldin.File.Upload.Database.Repository;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface;
using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Manager.UseCases.DataFileManagement.Implementation
{
    /// <summary>
    /// This is the implementation of all IDataFileManager
    /// </summary>
    public class DataFileManager : IDataFileManager
    {
        private readonly ILogger<DataFileManager> _logger;
        private readonly IDataFile _dataFile;
        private readonly IDataFileCsvProcessor _dataFileCsvProcessor;
        /// <summary>
        /// Dependency injects the logger, dataFile, dataFileCsvProcessor interfaces.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataFile"></param>
        public DataFileManager(ILogger<DataFileManager> logger, IDataFile dataFile, IDataFileCsvProcessor dataFileCsvProcessor) 
        {
            _logger = logger;
            _dataFile = dataFile;
            _dataFileCsvProcessor = dataFileCsvProcessor;
        }

        /// <summary>
        /// This implements the manager process to get all files.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Goldin.File.Upload.Model.DataFile>> GetAllDataFilesAsync() 
        {
            try 
            {
                _logger.LogInformation(string.Format("{0} - {1} - {2} - attempting to get all files.", LogMessage.GeneralLogMessage, nameof(DataFileManager), nameof(GetAllDataFilesAsync)));
                return await _dataFile.GetAllDataFilesAsync();

            }
            catch (Exception ex) 
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileManager), nameof(GetAllDataFilesAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        /// <summary>
        /// This implements the logic to validate and process and uploaded file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<Tuple<bool, string, string[]?>> ValidateAndProcessCsvAsync(IFormFile file) 
        {
            try 
            {
                _logger.LogInformation(string.Format("{0} - {1} - {2} - attempting to validate and process the file.", LogMessage.GeneralLogMessage, nameof(DataFileManager), nameof(GetAllDataFilesAsync)));
                return await _dataFileCsvProcessor.ValidateAndProcessCsvAsync(file);
            }
            catch (Exception ex) 
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileManager), nameof(ValidateAndProcessCsvAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

    }
}
