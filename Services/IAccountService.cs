using goOfflineE.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IAccountService
    {
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);
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
