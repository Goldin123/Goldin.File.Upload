using Goldin.File.Upload.Api.Models;
using Goldin.File.Upload.Api.Services;
using Goldin.File.Upload.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Goldin.File.Upload.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = Notification.UserPasswordIncorrectMessage });

            return Ok(response);
        }
    }
}
