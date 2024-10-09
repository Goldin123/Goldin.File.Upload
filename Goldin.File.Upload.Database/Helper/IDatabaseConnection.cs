using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Database.Helper
{
    public interface IDatabaseConnection
    {
        /// <summary>
        /// This is used to create a connection.
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateConnection();
        Task<IDbConnection> CreateConnectionAsync();

    }
}
