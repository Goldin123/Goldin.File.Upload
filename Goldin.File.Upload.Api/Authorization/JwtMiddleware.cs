using Goldin.File.Upload.Api.Services;
using Goldin.File.Upload.Common;

namespace Goldin.File.Upload.Api.Authorization
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// This is a thread responsible for validating the jwt token.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="userService"></param>
        /// <param name="jwtUtils"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, IUserService userService, IJwtUtils jwtUtils)
        {
            try
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var userId = jwtUtils.ValidateJwtToken(token);
                if (userId != null)
                {
                    context.Items["User"] = userService.GetById(userId.Value);
                }

                await _next(context);
            }
            catch (Exception ex) 
            {
                new Exception(Notification.GeneralExceptionMessage);
            }
        }
    }
}
