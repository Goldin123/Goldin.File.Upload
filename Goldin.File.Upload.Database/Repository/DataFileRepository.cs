﻿using Dapper;
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
    /// <summary>
    /// This implements the interface responsible for retrieval, updating or additions to the DataFile Table. 
    /// </summary>
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
                using (var connection = await _databaseConnection.CreateConnectionAsync())
                {
                    var dataFiles = await connection.QueryAsync<Goldin.File.Upload.Model.DataFile>("sp_GetAllDataFiles", commandType: CommandType.StoredProcedure);
                    _logger.LogInformation(string.Format("{0} - {1} - {2}", LogMessage.GeneralLogMessage, nameof(DataFileRepository), nameof(GetAllDataFilesAsync)));
                    return dataFiles;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}"), LogMessage.SqlLogMessage, nameof(DataFileRepository), nameof(GetAllDataFilesAsync), ex.Message));
                throw new Exception(Notification.GeneralSqlExceptionMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileRepository), nameof(GetAllDataFilesAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        /// <summary>
        /// This is used to save a single record to the data file table.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="search"></param>
        /// <param name="libraryFilter"></param>
        /// <param name="visible"></param>
        /// <returns>Successful or not.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<bool> AddDataFileRecordAsync(string fileName, string name, string type, bool search, bool libraryFilter, bool visible)
        {
            try
            {
                using (var connection = await _databaseConnection.CreateConnectionAsync())
                {
                    await connection.ExecuteAsync("sp_AddDataFileRecord", new
                    {
                        FileName = fileName,
                        Name = name,
                        Type = type,
                        Search = search,
                        LibraryFilter = libraryFilter,
                        Visible = visible
                    }, commandType: System.Data.CommandType.StoredProcedure);
                    return true;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}"), LogMessage.SqlLogMessage, nameof(DataFileRepository), nameof(AddDataFileRecordAsync), ex.Message));
                throw new Exception(Notification.GeneralSqlExceptionMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileRepository), nameof(AddDataFileRecordAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        /// <summary>
        /// This is a method used to save bulk entries into the database.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lines"></param>
        /// <returns>A tuple with successful, message and lines. </returns>
        public async Task<Tuple<bool, string, string[]?>> BulkSaveCsvDataAsync(string fileName, string[] lines)
        {
            try
            {
                // Prepare the DataTable for bulk insert
                DataTable dataTable = CreateLinesDataTable(lines);

                if (dataTable == null)
                    return new Tuple<bool, string, string[]?>(false, "No data table", null);

                using (var connection = await _databaseConnection.CreateConnectionAsync())
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@FileName", fileName, DbType.String);
                    parameters.Add("@DataFileRows", dataTable.AsTableValuedParameter("dbo.DataFileType"));
                    await connection.ExecuteAsync("dbo.sp_SaveDataFile", parameters, commandType: CommandType.StoredProcedure);
                    return new Tuple<bool, string, string[]?>(true, string.Format("Successfully bulk uploaded records using unique filename: {0}", fileName), lines);
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}"), LogMessage.SqlLogMessage, nameof(DataFileRepository), nameof(BulkSaveCsvDataAsync), ex.Message));
                throw new Exception(Notification.GeneralSqlExceptionMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileRepository), nameof(BulkSaveCsvDataAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        /// <summary>
        /// This is a private method used to create a data table given a list of lines. 
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>A data table.</returns>
        private static DataTable CreateLinesDataTable(string[] lines)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add("Name", typeof(string));
            dataTable.Columns.Add("Type", typeof(string));
            dataTable.Columns.Add("Search", typeof(bool));
            dataTable.Columns.Add("LibraryFilter", typeof(bool));
            dataTable.Columns.Add("Visible", typeof(bool));

            // Populate the DataTable with rows from the CSV file.
            foreach (var line in lines.Skip(1)) // Skip the header.
            {
                var fields = line.Replace("\",\"", ";").Split(';');
                var row = dataTable.NewRow();
                row["Name"] = fields[0];
                row["Type"] = fields[1];
                row["Search"] = fields[2].ToLower() == "yes";
                row["LibraryFilter"] = fields[3].ToLower() == "yes";
                row["Visible"] = fields[4].ToLower() == "yes";
                dataTable.Rows.Add(row);
            }
            return dataTable;
        }

        /// <summary>
        /// This implements the logic to retrieve records based on a filename. 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>A collections of data files in the database based on a filename.</returns>
        /// <exception cref="Exception"></exception>
        public async Task<IEnumerable<Goldin.File.Upload.Model.DataFile>> GetDataFileByFilenameAsync(string filename)
        {
            try
            {
                using (var connection = await _databaseConnection.CreateConnectionAsync())
                {
                    var dataFiles = await connection.QueryAsync<Goldin.File.Upload.Model.DataFile>("sp_GetDataFileByFilename",new { FileName = filename }, commandType: CommandType.StoredProcedure);
                    _logger.LogInformation(string.Format("{0} - {1} - {2}", LogMessage.GeneralLogMessage, nameof(DataFileRepository), nameof(GetDataFileByFilenameAsync)));
                    return dataFiles;
                }
            }
            catch (SqlException ex)
            {
                _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}"), LogMessage.SqlLogMessage, nameof(DataFileRepository), nameof(GetDataFileByFilenameAsync), ex.Message));
                throw new Exception(Notification.GeneralSqlExceptionMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileRepository), nameof(GetDataFileByFilenameAsync), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }
    }
}
