using Educati.Entites;
using Educati.Models;
using System.Collections.Generic;

namespace Educati.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
    }
}
