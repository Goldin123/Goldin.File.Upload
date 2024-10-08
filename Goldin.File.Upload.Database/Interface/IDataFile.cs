using Goldin.File.Upload.Database.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Database.Interface
{
    /// <summary>
    /// This interface is responsible to facilitate all retrievals and additions of DataFile database table.
    /// </summary>
    public interface IDataFile
    {
        /// <summary>
        /// This is used to get all the data files stored in the MS SQL database.
        /// </summary>
        /// <returns>A collections of all uploaded data files.</returns>
        Task<IEnumerable<Goldin.File.Upload.Model.DataFile>> GetAllDataFilesAsync();

        /// <summary>
        /// This is used to add a data file record one at a time. 
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="name"></param>
        /// <param name="type"></param>
        /// <param name="search"></param>
        /// <param name="libraryFilter"></param>
        /// <param name="visible"></param>
        /// <returns>Successful or Not</returns>
        Task<bool> AddDataFileRecordAsync(string fileName, string name, string type, bool search, bool libraryFilter, bool visible);

        /// <summary>
        /// This is used to bulk add a list of data file's data.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="lines"></param>
        /// <returns>Successful or Not</returns>
        Task<Tuple<bool, string, string[]?>> BulkSaveCsvDataAsync(string fileName, string[] lines);
    }
}
