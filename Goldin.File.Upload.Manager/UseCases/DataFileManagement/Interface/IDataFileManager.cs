using Microsoft.AspNetCore.Http;
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

        /// <summary>
        /// This manages the validations conducted to a file and the processing there after.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A tuple with validated fields and related messages associated to the file.</returns>
        Task<Tuple<bool, string, string[]?>> ValidateAndProcessCsvAsync(IFormFile file);

        /// <summary>
        /// This manages the way file information is retrieved by using a filename.
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        Task<IEnumerable<Goldin.File.Upload.Model.DataFile>> GetDataFileByFilenameAsync(string filename);
    }
}
