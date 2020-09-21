namespace goOfflineE.Services
{
    using goOfflineE.Entites;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using goOfflineE.Repository;
    using System;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using BC = BCrypt.Net.BCrypt;

    /// <summary>
    /// Defines the <see cref="ProfileService" />.
    /// </summary>
    public class ProfileService : IProfileService
    {
        /// <summary>
        /// Defines the _tableStorage.
        /// </summary>
        private readonly ITableStorage _tableStorage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProfileService"/> class.
        /// </summary>
        /// <param name="tableStorage">The tableStorage<see cref="ITableStorage"/>.</param>
        public ProfileService(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        /// <summary>
        /// Update user profile.
        /// </summary>
        /// <param name="model">.</param>
        /// <returns>.</returns>
        public async Task UpdateProfile(ProfileUpdateRequest model)
        {
            var users = await _tableStorage.GetAllAsync<User>("User");
            var currentUser = users.SingleOrDefault(user => user.RowKey == model.Id);

            // Validate email id availabilty if user update.
            if (!String.IsNullOrEmpty(model.Email))
            {
                var isEmailAvailable = users.Any(user => user.RowKey != model.Id && user.Email == model.Email);
                if (isEmailAvailable)
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new StringContent(string.Format("Email Id does not avalable to update!")),

                    };
                    throw new HttpResponseException(resp);
                }
                currentUser.Email = model.Email;

            }

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


            if (String.IsNullOrEmpty(model.FirstName))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(string.Format("First name required!")),

                };
                throw new HttpResponseException(resp);
            }

            if (String.IsNullOrEmpty(model.LastName))
            {
                var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(string.Format("Last name required!")),

                };
                throw new HttpResponseException(resp);
            }

            currentUser.FirstName = model.FirstName;
            currentUser.LastName = model.LastName;

            try
            {
                await _tableStorage.UpdateAsync("User", currentUser);
            }
            catch (Exception ex)
            {
                throw new AppException("User profile update rrror: ", ex.InnerException);
            }
        }

        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="model">The model<see cref="RegisterRequest"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
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
    }
}
