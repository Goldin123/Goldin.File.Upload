using Goldin.File.Upload.Common;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Implementation
{
    public class DataFileCsvValidationRules: IDataFileCsvValidationRules
    {
        private readonly ILogger<DataFileCsvValidationRules> _logger;

        public DataFileCsvValidationRules(ILogger<DataFileCsvValidationRules> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This is used to validate using Regex to check if the content is line delimited by CR or CRLF
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public bool IsValidCsvContent(string content)
        {
            try
            {
                if (string.IsNullOrEmpty(content))
                    return false;
                
                string lineDelimiterPattern = @"(\r\n|\r|\n)";
                _logger.LogInformation(string.Format("{0} - {1} - {2} - checking {3} using regex {4}", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidCsvContent), content, lineDelimiterPattern));
                return Regex.IsMatch(content, lineDelimiterPattern);

            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidCsvContent), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        /// <summary>
        /// This implements the validations of headers.
        /// </summary>
        /// <param name="headers"></param>
        /// <returns>Successful or not.</returns>
        /// <exception cref="Exception"></exception>
        public bool IsValidHeader(string[] headers)
        {
            try
            {
                if (headers.Length == 0 || headers == null)
                    return false;

                _logger.LogInformation(string.Format("{0} - {1} - {2} - validating headers.", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidHeader)));
                return headers.Length >= 5 &&
                       headers.Contains("Name") &&
                       headers.Contains("Type") &&
                       headers.Contains("Search") &&
                       headers.Contains("Library Filter") &&
                       headers.Contains("Visible");
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidHeader), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }
    }
}
