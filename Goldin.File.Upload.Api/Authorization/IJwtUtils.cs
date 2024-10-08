using Goldin.File.Upload.Api.Entities;

namespace Goldin.File.Upload.Api.Authorization
{
    public interface IJwtUtils
    {
        public string GenerateJwtToken(User user);
        public int? ValidateJwtToken(string? token);
    }
}
