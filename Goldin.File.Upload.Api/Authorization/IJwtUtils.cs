using Goldin.File.Upload.Api.Entities;

namespace Goldin.File.Upload.Api.Authorization
{
    public interface IJwtUtils
    {
        /// <summary>
        /// This is used to generate a jwt token.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>JWT Token.</returns>
        public string GenerateJwtToken(User user);
        /// <summary>
        /// This is used to validate the token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>A user id.</returns>
        public int? ValidateJwtToken(string? token);
    }
}
