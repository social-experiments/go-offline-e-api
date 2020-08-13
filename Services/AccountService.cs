using Aducati.Azure.TableStorage.Repository;
using AutoMapper;
using goOfflineE.Entites;
using goOfflineE.Helpers;
using goOfflineE.Models;
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

namespace goOfflineE.Services
{
    public class AccountService : IAccountService
    {
        private readonly ITableStorage _tableStorage;
        private readonly ISchoolService _schoolService;
        private readonly IMapper _mapper;
        public AccountService(ITableStorage tableStorage, ISchoolService schoolService,IMapper mapper)
        {
            _tableStorage = tableStorage;
            _schoolService = schoolService;
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

            var schools = account.Role == Role.Teacher.ToString()? await _schoolService.GetAll(account.PartitionKey) : await _schoolService.GetAll();

            var response = new AuthenticateResponse
            {
                Id = account.RowKey,
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Role = account.Role,
                Schools = schools,
                ForceChangePasswordNextLogin = account.ForceChangePasswordNextLogin,
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
            var userId = model.Id ?? Guid.NewGuid().ToString();
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
                ForceChangePasswordNextLogin = true
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

        public async Task ResetPassword(ResetPasswordRequest model)
        {
            // validate
            TableQuery<User> query = new TableQuery<User>()
                   .Where(TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, model.UserId));
            var users = await _tableStorage.QueryAsync<User>("User", query);
            var currentUser = users.SingleOrDefault();

            // Validate password and confirmed passwrd are same.
            if (!String.IsNullOrEmpty(model.Password) && !String.IsNullOrEmpty(model.Password))
            {
                if (model.Password != model.ConfirmPassword)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent(string.Format("Password and confirm password are not same!")),

                    };
                    throw new HttpResponseException(resp);
                }
                currentUser.PasswordHash = BC.HashPassword(model.Password);
            }
            else
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(string.Format("Password and confirm password are required to change!")),

                };
                throw new HttpResponseException(resp);
            }

            currentUser.ForceChangePasswordNextLogin = false;
            currentUser.PasswordReset = DateTime.UtcNow;
            currentUser.Verified = DateTime.UtcNow;
            currentUser.UpdatedBy = model.UserId;

            try
            {
                await _tableStorage.UpdateAsync("User", currentUser);
            }
            catch (Exception ex)
            {
                throw new AppException("User password reset error ", ex.InnerException);
            }

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
