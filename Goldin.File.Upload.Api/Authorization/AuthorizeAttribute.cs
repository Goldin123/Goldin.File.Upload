using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Goldin.File.Upload.Api.Entities;
using Goldin.File.Upload.Common;

namespace Goldin.File.Upload.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        /// <summary>
        /// This method handles the Authorize attribute and adds a check is the user is logged in.
        /// </summary>
        /// <param name="context"></param>
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
                if (allowAnonymous)
                    return;

                var user = (User?)context.HttpContext.Items["User"];
                if (user == null)
                {
                    context.Result = new JsonResult(new { message = Notification.UserUnauthorizedMessage }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
            }
            catch (Exception ex) 
            {
                new Exception(Notification.GeneralExceptionMessage);
            }
        }
    }
}
