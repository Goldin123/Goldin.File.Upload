using Goldin.File.Upload.Api.Entities;
using Goldin.File.Upload.Api.Models;

namespace Goldin.File.Upload.Api.Services
{
    public interface IUserService
    {
        AuthenticateResponse? Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
        User? GetById(int id);
    }
}