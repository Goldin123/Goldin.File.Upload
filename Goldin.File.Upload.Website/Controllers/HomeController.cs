using Goldin.File.Upload.Website.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Goldin.File.Upload.Website.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpClientFactory _clientFactory;

        public HomeController(ILogger<HomeController> logger , IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _clientFactory = clientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile file)
        {
            var viewModel = new FileUploadViewModel();

            if (file == null || file.Length == 0)
            {
                viewModel.ErrorMessage = "Please select a file to upload.";
                return View("Index", viewModel);
            }

            try
            {
                // Create an HttpClient to call the Web API
                var client = _clientFactory.CreateClient();

                // Prepare the content to send (file as form-data)
                using var content = new MultipartFormDataContent();
                using var fileStream = file.OpenReadStream();
                using var streamContent = new StreamContent(fileStream);
                content.Add(streamContent, "file", file.FileName);

                // Call the API (replace "http://apiurl" with your actual API endpoint)
                var response = await client.PostAsync("http://localhost:5000/api/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    viewModel.IsSuccess = true;
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    viewModel.ErrorMessage = errorMessage;
                }
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error calling the Web API");
                viewModel.ErrorMessage = "An error occurred while uploading the file. Please try again.";
            }

            return View("Index", viewModel);
        }
    }
}
