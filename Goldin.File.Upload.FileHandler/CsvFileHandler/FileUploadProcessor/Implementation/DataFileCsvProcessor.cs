using Goldin.File.Upload.Common;
using Goldin.File.Upload.FileHandler.CsvFileHandler.CustomException;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Implementation;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileValidationRules.Interface;
using Goldin.File.Upload.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Implementation
{
    /// <summary>
    /// This implements all functionality needed for IDataFileCsvProcessor
    /// </summary>
    public class DataFileCsvProcessor : IDataFileCsvProcessor
    {
        private readonly ILogger<DataFileCsvProcessor> _logger;
        private readonly IDataFileCsvUploader _dataFileCsvUploader;
        private readonly IDataFileCsvValidationRules _dataFileCsvValidationRules;
        public DataFileCsvProcessor(ILogger<DataFileCsvProcessor> logger, IDataFileCsvUploader dataFileCsvUploader, IDataFileCsvValidationRules dataFileCsvValidationRules)
        {
            _logger = logger;
            _dataFileCsvUploader = dataFileCsvUploader;
            _dataFileCsvValidationRules = dataFileCsvValidationRules;
        }

        /// <summary>
        /// This implements the reading of a csv file into a string of contents.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A string of the file.</returns>
        public async Task<Tuple<bool, string, string[]?>> ValidateCsvFileAsync(IFormFile file)
        {
            try
            {
                //Read the file.
                string content = await _dataFileCsvUploader.ReadFileAsync(file);

                _logger.LogInformation(string.Format("{0} - {1} - {2} - file content: \n {3}.",LogMessage.GeneralLogMessage,nameof(DataFileCsvProcessor),nameof(ValidateCsvFileAsync) ,content));

                if (content == null)
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}.", LogMessage.GeneralExceptionLogMessage,
                                     nameof(DataFileCsvProcessor), nameof(ValidateCsvFileAsync), FileValidationMessages.FileEmpty)));
                    throw new DataFileCsvCustomException(FileValidationMessages.FileEmpty);
                }

                // Check if content is a text string and delimited by CR or CRLF
                if (!_dataFileCsvValidationRules.IsValidCsvContent(content))
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}.", LogMessage.GeneralExceptionLogMessage,
                              nameof(DataFileCsvProcessor), nameof(ValidateCsvFileAsync), FileValidationMessages.FileValidatorCsvFormat)));
                    throw new DataFileCsvCustomException(FileValidationMessages.FileValidatorCsvFormat);
                }

                var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (!lines.Any())
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}.", LogMessage.GeneralExceptionLogMessage,
                                nameof(DataFileCsvProcessor), nameof(ValidateCsvFileAsync), FileValidationMessages.FileEmpty)));
                    throw new DataFileCsvCustomException(FileValidationMessages.FileEmpty);
                }

                // Check if the header row exists and is valid
                string[] headers = lines[0].Replace("\",\"", ";").Split(';');

                if (!_dataFileCsvValidationRules.IsValidHeader(headers))
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}.", LogMessage.GeneralExceptionLogMessage,
                               nameof(DataFileCsvProcessor), nameof(ValidateCsvFileAsync), FileValidationMessages.InvalidHeader)));
                    throw new DataFileCsvCustomException(0, FileValidationMessages.InvalidHeader);
                }

                // Validate each data row (line) excluding the header in the CSV
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Replace("\",\"", ";").Split(';');

                    // Check if the line contains more than 5 properties
                    if (values.Length < 5) 
                    {
                        _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - Only contains {3} properties - should be at least 5.", LogMessage.GeneralExceptionLogMessage,
                               nameof(DataFileCsvProcessor), nameof(ValidateCsvFileAsync), values.Length)));
                        throw new DataFileCsvCustomException(i, string.Format("Only contains {0} properties - should be at least 5.", values.Length));
                    }
                    else
                        _logger.LogInformation(string.Format("{0} - {1} - {2} - data row line {3} length is greater then 5 properties.", LogMessage.GeneralLogMessage, nameof(DataFileCsvProcessor), nameof(ValidateCsvFileAsync), i));



                    // Map CSV line to DataFileRow model
                    DataFileRow dataFileRow = new DataFileRow
                    {
                        Name = values[0],
                        Type = values[1],
                        Search = values[2],
                        LibraryFilter = values[3],
                        Visible = values[4]
                    };

                    // Validate the model using DataAnnotations
                    var validationResults = new List<ValidationResult>();
                    var context = new ValidationContext(dataFileRow);
                    bool isValid = Validator.TryValidateObject(dataFileRow, context, validationResults, true);

                    if (!isValid)
                    {
                        var errorMessage = string.Join("; ", validationResults.Select(r => $"Line {i}: {r.ErrorMessage}"));
                        throw new DataFileCsvCustomException(errorMessage);
                    }

                    // number is searchable validation rules

                    if (_dataFileCsvValidationRules.IsNumberSearchable(dataFileRow)) 
                    {
                        throw new DataFileCsvCustomException(i , "Search", "Number fields cannot be searchable.");
                    }
                    
                }

                return new Tuple<bool, string, string[]?>(true, string.Empty, lines); // Return success
            }
            catch (DataFileCsvCustomException ex)
            {
                _logger.LogError(ex, "CSV validation error: {ErrorMessage}", ex.Message);
                return new Tuple<bool, string, string[]?>(false, ex.Message, null); // Return specific validation error message
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while processing CSV");
                return new Tuple<bool, string, string[]?>(false, "An unexpected error occurred while processing the CSV file.", null);
            }
        }
    }
}
