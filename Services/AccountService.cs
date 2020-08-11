using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using Educati.Azure.Function.Api.Entites;
using Educati.Azure.Function.Api.Helpers;
using Educati.Azure.Function.Api.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using BC = BCrypt.Net.BCrypt;

namespace Educati.Azure.Function.Api.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITableStorage _tableStorage;
        private readonly IMapper _mapper;
        public AccountService(ITableStorage tableStorage, IMapper mapper)
        {
            _tableStorage = tableStorage;
            _mapper = mapper;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model)
        {
            // validate
            TableQuery<User> query = new TableQuery<User>()
                   .Where(TableQuery.GenerateFilterCondition("Email", QueryComparisons.Equal, model.Email));
            var users = await _tableStorage.QueryAsync<User>("User", query);

            var account = users.SingleOrDefault();

            if (account == null || !account.IsVerified || !BC.Verify(model.Password, account.PasswordHash))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(string.Format("Username or password is incorrect!"))
                };
                throw new HttpResponseException(resp);
            }

            // authentication successful so generate jwt
            var jwtToken = GenerateToken(account.RowKey);

            var response = new AuthenticateResponse
            {
                Id = account.RowKey,
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Role = account.Role,
                Token = jwtToken
            };

            return response;
        }

        public async Task<object> Register(RegisterRequest model)
        {
            // validate
            var users = await _tableStorage.GetAllAsync<User>("User");

            if (users.Any(x => x.Email == model.Email))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.AlreadyReported)
                {
                    Content = new StringContent(string.Format($"User { model.Email} already registred!")),
                    ReasonPhrase = "Already registred!"
                };
                throw new HttpResponseException(resp);
            }

            var defaultPasswrod = model.Password ?? "p@ssw0rd";
            var userId = model.Id?? Guid.NewGuid().ToString();
            var schoolId = model.SchoolId ?? userId;

            var newUser = new User(schoolId, userId)
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PasswordHash = BC.HashPassword(defaultPasswrod),
                Role = model.Role,
                Active = true,
                Verified = DateTime.UtcNow,
                PasswordReset = DateTime.UtcNow,
                CreatedBy = userId,
                UpdatedOn = DateTime.UtcNow,
                UpdatedBy = userId,
            };

            try
            {
                return await _tableStorage.AddAsync("User", newUser);
            }
            catch (Exception ex)
            {
                throw new AppException("Registration Error: ", ex.InnerException);
            }
        }

        public Task VerifyEmail(string token)
        {
            throw new NotImplementedException();
        }

        public Task ForgotPassword(ForgotPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        public Task ResetPassword(ResetPasswordRequest model)
        {
            throw new NotImplementedException();
        }

        Task<IEnumerable<AccountResponse>> IAccountService.GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<AccountResponse> GetById(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<AccountResponse> Create(CreateRequest model)
        {
            throw new NotImplementedException();
        }

        public Task<AccountResponse> Update(string id, UpdateRequest model)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string id)
        {
            throw new NotImplementedException();
        }

        // helper methods
        public string GenerateToken(string userId)
        {
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(SettingConfigurations.IssuerToken));
            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = SettingConfigurations.Issuer,
                Audience = SettingConfigurations.Audience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
