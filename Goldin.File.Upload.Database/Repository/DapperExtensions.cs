using Dapper;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Database.Repository
{
    public static class DapperExtensions
    {
        public static SqlMapper.ICustomQueryParameter AsTableValuedParameter(this DataTable dataTable, string typeName)
        {
            return dataTable.AsTableValuedParameter(typeName);
        }
    }
}
