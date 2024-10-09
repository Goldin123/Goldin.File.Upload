// Ignore Spelling: Goldin

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Goldin.File.Upload.Common
{
    public static class Notification
    {
        public static string GeneralExceptionMessage = "An unexpected error occurred. Please try again later.";
        public static string JWTNoSecretMessage = "JWT secret not configured";
        public static string FileExceptionMessage = "An unexpected error occurred while reading the file. Please try again later.";
        public static string GeneralSqlExceptionMessage = "An error occurred while processing data to the database. Please try again later.";
        public static string UserUnauthorizedMessage = "You are not authorized to access this endpoint, please login to get the auth token.";
        public static string UserPasswordIncorrectMessage = "Username or password is incorrect.";

    }
}
