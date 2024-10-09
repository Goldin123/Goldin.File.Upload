using Goldin.File.Upload.Api.Entities;
using Goldin.File.Upload.Api.Helpers;
using Goldin.File.Upload.Common;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Goldin.File.Upload.Api.Authorization
{
        public class JwtUtils : IJwtUtils
    {
        private readonly AppSettings _appSettings;

        public JwtUtils(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

            if (string.IsNullOrEmpty(_appSettings.Secret))
                throw new Exception(Notification.JWTNoSecretMessage);
        }

        /// <summary>
        /// This generates a Token that expires in 60 minutes.
        /// </summary>
        /// <param name="user"></param>
        /// <returns>A JWT Token.</returns>
        /// <exception cref="Exception"></exception>
        public string GenerateJwtToken(User user)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[] { new Claim("id", user.Id.ToString()) }),
                    Expires = DateTime.UtcNow.AddMinutes(60),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                return tokenHandler.WriteToken(token);
            }
            catch (Exception ex) 
            {
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        /// <summary>
        /// This method is used to validate the JWT token and also set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later). 
        /// </summary>
        /// <param name="token"></param>
        /// <returns>user id from JWT token if validation successful</returns>
        public int? ValidateJwtToken(string? token)
        {
            if (token == null)
                return null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret!);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = int.Parse(jwtToken.Claims.First(x => x.Type == "id").Value);

                return userId;
            }
            catch (Exception ex)
            {
               throw new Exception (Notification.GeneralExceptionMessage);
            }
        }
    }
}
