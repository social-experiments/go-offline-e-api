namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IProfileService" />.
    /// </summary>
    public interface IProfileService
    {
        /// <summary>
        /// The Register.
        /// </summary>
        /// <param name="model">The model<see cref="RegisterRequest"/>.</param>
        /// <returns>The <see cref="Task{object}"/>.</returns>
        Task<object> Register(RegisterRequest model);

        /// <summary>
        /// The UpdateProfile.
        /// </summary>
        /// <param name="model">The model<see cref="ProfileUpdateRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task UpdateProfile(ProfileUpdateRequest model);
    }
}
