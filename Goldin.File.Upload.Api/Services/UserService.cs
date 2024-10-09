using Goldin.File.Upload.Api.Authorization;
using Goldin.File.Upload.Api.Entities;
using Goldin.File.Upload.Api.Models;
using Goldin.File.Upload.Common;

namespace Goldin.File.Upload.Api.Services
{
    public class UserService : IUserService
    {
        private List<User> _users = new List<User>
           {
            new User { Id = 1, FirstName = "Test", LastName = "User", Username = "test", Password = "test" }
           }; // Dummy user data, this should be stored in a secure place and will be available for retrieval.

        private readonly IJwtUtils _jwtUtils;

        public UserService(IJwtUtils jwtUtils)
        {
            _jwtUtils = jwtUtils;
        }

        /// <summary>
        /// Thisreturn null if user not found and authentication successful so generate jwt token
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public AuthenticateResponse? Authenticate(AuthenticateRequest model)
        {
            try
            {
                var user = _users.SingleOrDefault(x => x.Username == model.Username && x.Password == model.Password);
                if (user == null) return null;
                var token = _jwtUtils.GenerateJwtToken(user);

                return new AuthenticateResponse(user, token);
            }
            catch (Exception ex) 
            {
                throw new Exception(Notification.GeneralExceptionMessage);
            }
        }

        #region Dummy users services
        public IEnumerable<User> GetAll()=> _users;       

        public User? GetById(int id) => _users.FirstOrDefault(x => x.Id == id);

        #endregion
    }
}
