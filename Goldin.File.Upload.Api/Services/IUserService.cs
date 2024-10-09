using Goldin.File.Upload.Api.Entities;
using Goldin.File.Upload.Api.Models;

namespace Goldin.File.Upload.Api.Services
{
    public interface IUserService
    {
        /// <summary>
        /// This is used to authenticate a user.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        AuthenticateResponse? Authenticate(AuthenticateRequest model);     
        /// <summary>
        /// This gets a user by a user id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        User? GetById(int id);
    }
}