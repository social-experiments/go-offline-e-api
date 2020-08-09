using Educati.Azure.Function.Api.Entites;
using Educati.Azure.Function.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Educati.Azure.Function.Api.Services
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
        Task<object> Register(RegisterRequest model);
        Task VerifyEmail(string token);
        Task ForgotPassword(ForgotPasswordRequest model);
        Task ResetPassword(ResetPasswordRequest model);
        Task<IEnumerable<AccountResponse>> GetAll();
        Task<AccountResponse> GetById(string id);
        Task<AccountResponse> Create(CreateRequest model);
        Task<AccountResponse> Update(string id, UpdateRequest model);
        Task Delete(string id);
    }
}
