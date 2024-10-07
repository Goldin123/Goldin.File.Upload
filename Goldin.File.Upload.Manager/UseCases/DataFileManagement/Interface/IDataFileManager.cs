using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Manager.UseCases.DataFileManagement.Interface
{
    /// <summary>
    /// This manager is responsible to process all data file related requests and responses.
    /// </summary>
    public interface IDataFileManager
    {
        /// <summary>
        /// This manages the retrieval of all data files uploaded on the database.
        /// </summary>
        /// <returns>A list of data files.</returns>
        Task<IEnumerable<Goldin.File.Upload.Model.DataFile>> GetAllDataFilesAsync();
    }
}
