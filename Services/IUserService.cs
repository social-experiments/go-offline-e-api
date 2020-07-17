using Educati.Azure.Function.Api.Entites;
using Educati.Azure.Function.Api.Models;
using System.Collections.Generic;

namespace Educati.Azure.Function.Api.Services
{
    public interface IUserService
    {
        AuthenticateResponse Authenticate(AuthenticateRequest model);
        IEnumerable<User> GetAll();
    }
}
