using Goldin.File.Upload.Common;
using Goldin.File.Upload.FileHandler.CsvFileHandler.CustomException;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface;
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

        public DataFileCsvProcessor(ILogger<DataFileCsvProcessor> logger, IDataFileCsvUploader dataFileCsvUploader) 
        {
            _logger = logger;
            _dataFileCsvUploader = dataFileCsvUploader;
        }

        /// <summary>
        /// This implements the reading of a csv file into a string of contents.
        /// </summary>
        /// <param name="file"></param>
        /// <returns>A string of the file.</returns>
        public async Task<Tuple<bool,string>> ValidateAndProcessCsvAsync(IFormFile file) 
        {
            try 
            {
                //Read the file.
                string content = await _dataFileCsvUploader.ReadFileAsync(file);

                if (content == null) 
                {
                    _logger.LogInformation(string.Format(string.Format("{0} - {1} - {2} - file is empty."), LogMessage.GeneralExceptionLogMessage, nameof(DataFileCsvProcessor), nameof(ValidateAndProcessCsvAsync)));
                    throw new ArgumentNullException(nameof(content));
                }

                // Check if content is a text string and delimited by CR or CRLF
                if (!IsValidCsvContent(content))
                {
                    throw new CsvValidationException(FileValidationMessages.FileValidatorCsvFormat);
                }

                var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (!lines.Any())
                {
                    throw new CsvValidationException(FileValidationMessages.FileEmpty);
                }

                // Check if the header row exists and is valid
                string[] headers = lines[0].Split(',');
                if (!IsValidHeader(headers))
                {
                    throw new CsvValidationException(1, "CSV Header line not found or invalid");
                }

                // Validate each row (line) in the CSV
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Split(',');

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
                        var errorMessage = string.Join("; ", validationResults.Select(r => $"Line {i + 1}: {r.ErrorMessage}"));
                        throw new CsvValidationException(errorMessage);
                    }

                    // Additional complex validation rules
                    ValidateCustomRules(dataFileRow, i + 1);
                }

                // If validation passes, save the data
                //await SaveCsvDataAsync(file.FileName, lines);
                return new Tuple<bool, string> (true, string.Empty); // Return success
            }
            catch (CsvValidationException ex)
            {
                _logger.LogError(ex, "CSV validation error: {ErrorMessage}", ex.Message);
                return new Tuple<bool, string> (false, ex.Message); // Return specific validation error message
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error while processing CSV");
                return new Tuple<bool, string>(false, "An unexpected error occurred while processing the CSV file.");
            }
        }

        private bool IsValidCsvContent(string content)
        {
            // Regex to check if the content is line delimited by CR or CRLF
            string lineDelimiterPattern = @"(\r\n|\r|\n)";
            return Regex.IsMatch(content, lineDelimiterPattern);
        }

        private bool IsValidHeader(string[] headers)
        {
            return headers.Length >= 5 &&
                   headers.Contains("Name") &&
                   headers.Contains("Type") &&
                   headers.Contains("Search") &&
                   headers.Contains("Library Filter") &&
                   headers.Contains("Visible");
        }

        private void ValidateCustomRules(DataFileRow dataFileRow, int rowNumber)
        {
            // Custom rule validation for field types
            if (dataFileRow.Type == "Number" && dataFileRow.Search == "Yes")
            {
                throw new CsvValidationException(rowNumber, "Search", "Number fields cannot be searchable.");
            }
        }
    }
}
