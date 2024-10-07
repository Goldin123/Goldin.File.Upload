using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Common
{
    public static class LogMessage
    {
        public static string GeneralLogMessage = string.Format("{0}", DateTime.UtcNow);
        public static string SqlLogMessage = string.Format("{0} - SQL Exception:", DateTime.Now);
        public static string GeneralExceptionLogMessage = string.Format("{0} - General Exception:", DateTime.Now);
        public static string FileExceptionLogMessage = string.Format("{0} - File Exception:", DateTime.Now);
    }
}
