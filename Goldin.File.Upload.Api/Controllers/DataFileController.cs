using Goldin.File.Upload.Api.Authorization;
using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goldin.File.Upload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DataFileController : ControllerBase
    {
        private readonly IDataFileManager _dataFileManager;
        public DataFileController(IDataFileManager dataFileManager)
        {
            _dataFileManager = dataFileManager;
        }
        [HttpGet]
        [Route("get-all-data-files")]
        public async Task<IActionResult> GetAllDataFilesAsync() => Ok(await _dataFileManager.GetAllDataFilesAsync());

        [HttpPost]
        [Route("upload-data-file")]
        public async Task<IActionResult> UploadCsvFileAsync(IFormFile file) => Ok(await _dataFileManager.ValidateAndProcessCsvAsync(file));

        [HttpGet]
        [Route("get-file-by-name")]
        public async Task<IActionResult> GetFileByNameAsync(string filename) => Ok(await _dataFileManager.GetDataFileByFilenameAsync(filename));
    }
}
