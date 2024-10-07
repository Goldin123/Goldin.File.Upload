using Goldin.File.Upload.Database.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Database.Interface
{
    public interface IDataFile
    {
        /// <summary>
        /// This is used to get all the data files stored in the MS SQL database.
        /// </summary>
        /// <returns>A collections of all uploaded data files.</returns>
        Task<IEnumerable<Goldin.File.Upload.Model.DataFile>> GetAllDataFilesAsync();
    }
}
