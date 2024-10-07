﻿using Goldin.File.Upload.Common;
using Goldin.File.Upload.FileHandler.CsvFileHandler.CustomException;
using Goldin.File.Upload.FileHandler.CsvFileHandler.FileUploadProcessor.Interface;
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
        public async Task<Tuple<bool, string, string[]?>> ValidateAndProcessCsvAsync(IFormFile file)
        {
            try
            {
                //Read the file.
                string content = await _dataFileCsvUploader.ReadFileAsync(file);

                if (content == null)
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}."), LogMessage.GeneralExceptionLogMessage,
                                     nameof(DataFileCsvProcessor), nameof(ValidateAndProcessCsvAsync), FileValidationMessages.FileEmpty));
                    throw new DataFileCsvCustomException(FileValidationMessages.FileEmpty);
                }

                // Check if content is a text string and delimited by CR or CRLF
                if (!_dataFileCsvValidationRules.IsValidCsvContent(content))
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}."), LogMessage.GeneralExceptionLogMessage,
                              nameof(DataFileCsvProcessor), nameof(ValidateAndProcessCsvAsync), FileValidationMessages.FileValidatorCsvFormat));
                    throw new DataFileCsvCustomException(FileValidationMessages.FileValidatorCsvFormat);
                }

                var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                if (!lines.Any())
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}."), LogMessage.GeneralExceptionLogMessage,
                                nameof(DataFileCsvProcessor), nameof(ValidateAndProcessCsvAsync), FileValidationMessages.FileEmpty));
                    throw new DataFileCsvCustomException(FileValidationMessages.FileEmpty);
                }

                // Check if the header row exists and is valid
                string[] headers = lines[0].Split(',');
                if (!_dataFileCsvValidationRules.IsValidHeader(headers))
                {
                    _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - {3}."), LogMessage.GeneralExceptionLogMessage,
                               nameof(DataFileCsvProcessor), nameof(ValidateAndProcessCsvAsync), FileValidationMessages.InvalidHeader));
                    throw new DataFileCsvCustomException(1, FileValidationMessages.InvalidHeader);
                }

                // Validate each row (line) in the CSV
                for (int i = 1; i < lines.Length; i++)
                {
                    var values = lines[i].Split(',');

                    if (values.Length < 5)
                    {
                        _logger.LogError(string.Format(string.Format("{0} - {1} - {2} - Only contains {3} properties - should be at least 5."), LogMessage.GeneralExceptionLogMessage,
                               nameof(DataFileCsvProcessor), nameof(ValidateAndProcessCsvAsync), values.Length));
                        throw new DataFileCsvCustomException(i + 1, string.Format("Only contains {0} properties - should be at least 5.", values.Length));
                    }

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
                        throw new DataFileCsvCustomException(errorMessage);
                    }

                    // Additional complex validation rules
                    ValidateCustomRules(dataFileRow, i + 1);
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

        private void ValidateCustomRules(DataFileRow dataFileRow, int rowNumber)
        {
            // Custom rule validation for field types
            if (dataFileRow.Type == "Number" && dataFileRow.Search == "Yes")
            {
                throw new DataFileCsvCustomException(rowNumber, "Search", "Number fields cannot be searchable.");
            }
        }
    }
}
