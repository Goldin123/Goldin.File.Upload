using Goldin.File.Upload.Manager.UseCases.DataFileManagement.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goldin.File.Upload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataFileController : ControllerBase
    {
        private readonly IDataFileManager _dataFileManager;
        public DataFileController(IDataFileManager dataFileManager) 
        {
            _dataFileManager = dataFileManager;
        }
        [HttpGet]
        [Route("GetAllDataFiles")]
        public async Task<IActionResult> GetAllDataFiles() => Ok(await _dataFileManager.GetAllDataFilesAsync());
    }
}
