using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Database.Helper
{
    public class DatabaseConnection: IDatabaseConnection
    {
        private readonly string _connectionString;
        /// <summary>
        /// Dependency injects the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        public DatabaseConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// This implements the creation of the MS SQL connection.
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}
