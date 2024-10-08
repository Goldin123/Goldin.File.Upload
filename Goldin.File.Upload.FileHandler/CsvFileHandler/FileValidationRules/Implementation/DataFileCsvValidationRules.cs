using Goldin.File.Upload.Common;
using Goldin.File.Upload.FileHandler.CsvFileHandler.CustomException;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Interface;
using Goldin.File.Upload.Model;
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
                _logger.LogInformation($"{DateTime.Now} - Verify CR or CRL using Regex: " + lineDelimiterPattern);
                var isValid = Regex.IsMatch(content, lineDelimiterPattern);

                if (isValid) 
                    _logger.LogInformation(string.Format("{0} - {1} - {2} - content is valid.",LogMessage.GeneralLogMessage,nameof(DataFileCsvValidationRules),nameof(IsValidCsvContent)));
                else
                    _logger.LogWarning(string.Format("{0} - {1} - {2} - content is not valid.", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidCsvContent)));

                return isValid;
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
                var isValid =  headers.Length >= 5 &&
                       headers.Contains("Name") &&
                       headers.Contains("Type") &&
                       headers.Contains("Search") &&
                       headers.Contains("Library Filter") &&
                       headers.Contains("Visible");

                if (isValid)
                    _logger.LogInformation(string.Format("{0} - {1} - {2} - headers are valid.", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidHeader)));
                else
                    _logger.LogWarning(string.Format("{0} - {1} - {2} - headers are not valid.", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidHeader)));


                return isValid;
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvValidationRules), nameof(IsValidHeader), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        /// <summary>
        /// This implements the rule for numbers not to be searchable.
        /// </summary>
        /// <param name="dataFileRow"></param>
        /// <param name="rowNumber"></param>
        /// <exception cref="DataFileCsvCustomException"></exception>
        public bool IsNumberSearchable(DataFileRow dataFileRow)
        {
            try
            {
                if (dataFileRow.Type == "Number" && dataFileRow.Search == "Yes")
                {
                    _logger.LogError(string.Format("{0} - {1} - {2} - number is searchable.", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsNumberSearchable)));
                    return true;
                }
                else if (dataFileRow.Type == "Number" && dataFileRow.Search == "No")
                {
                    _logger.LogWarning(string.Format("{0} - {1} - {2} - number field found but is not searchable.", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsNumberSearchable)));
                    return false;
                }
                else
                {
                    _logger.LogInformation(string.Format("{0} - {1} - {2} - no number field found.", LogMessage.GeneralLogMessage, nameof(DataFileCsvValidationRules), nameof(IsNumberSearchable)));
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format("{0} - {1} - {2} - {3}", LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvValidationRules), nameof(IsNumberSearchable), ex.Message));
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }
    }
}
