using Dapper;
using Goldin.File.Upload.Common;
using Goldin.File.Upload.Database.Helper;
using Goldin.File.Upload.Database.Interface;
using Goldin.File.Upload.Database.Table;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Database.Repository
{
    public class DataFileRepository : IDataFile
    {
        private readonly ILogger<DataFileRepository> _logger;
        private readonly IDatabaseConnection _databaseConnection;

        /// <summary>
        /// Dependency injects the logger, database connection 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="databaseConnection"></param>
        public DataFileRepository(ILogger<DataFileRepository> logger, IDatabaseConnection databaseConnection)
        {
            _logger = logger;
            _databaseConnection = databaseConnection;
        }

        /// <summary>
        /// This implements the retrieval of data files in the database.
        /// </summary>
        /// <returns>A collections of data files in the database. </returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Goldin.File.Upload.Model.DataFile>> GetAllDataFilesAsync()
        {
            try
            {
                using (var connection = _databaseConnection.CreateConnection())
                {
                    var dataFiles = await connection.QueryAsync<Goldin.File.Upload.Model.DataFile>("sp_GetAllDataFiles", commandType: CommandType.StoredProcedure);
                    _logger.LogInformation(string.Format("{0} - {1} - {2}", LogMessage.GeneralLogMessage, nameof(DataFileRepository), nameof(GetAllDataFilesAsync)));
                    return dataFiles;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogInformation(string.Format(string.Format("{0} - {1} - {2} - {3}"), LogMessage.SqlLogMessage, nameof(DataFileRepository), nameof(GetAllDataFilesAsync), ex.Message));
                throw new Exception(Notification.GeneralSqlExceptionMessage);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileRepository), nameof(GetAllDataFilesAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }
    }
}
