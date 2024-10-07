using Goldin.File.Upload.Common;
using Goldin.File.Upload.Database.Interface;
using Goldin.File.Upload.Database.Repository;
using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Interface;
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

        /// <summary>
        /// Dependency injects the logger, dataFile interface.
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="dataFile"></param>
        public DataFileManager(ILogger<DataFileManager> logger, IDataFile dataFile) 
        {
            _logger = logger;
            _dataFile = dataFile;
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
                _logger.LogInformation(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileManager), nameof(GetAllDataFilesAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

    }
}
