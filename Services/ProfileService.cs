using Aducati.Azure.TableStorage.Repository;
using goOfflineE.Entites;
using goOfflineE.Helpers;
using goOfflineE.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using BC = BCrypt.Net.BCrypt;


namespace goOfflineE.Services
{
    public class ProfileService : IProfileService
    {
        private readonly ITableStorage _tableStorage;

        public ProfileService(ITableStorage tableStorage)
        {
            _tableStorage = tableStorage;
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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
    }
}
