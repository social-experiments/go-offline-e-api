namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IAccountService" />.
    /// </summary>
    public interface IAccountService
    {
        /// <summary>
        /// The Authenticate.
        /// </summary>
        /// <param name="model">The model<see cref="AuthenticateRequest"/>.</param>
        /// <returns>The <see cref="Task{AuthenticateResponse}"/>.</returns>
        Task<AuthenticateResponse> Authenticate(AuthenticateRequest model);

        /// <summary>
        /// The Authenticate.
        /// </summary>
        /// <param name="model">The model<see cref="StudentAuthenticateRequest"/>.</param>
        /// <returns>The <see cref="Task{AuthenticateResponse}"/>.</returns>
        Task<AuthenticateResponse> Authenticate(StudentAuthenticateRequest model);

        /// <summary>
        /// The VerifyEmail.
        /// </summary>
        /// <param name="email">The email<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> VerifyEmail(string email);

        /// <summary>
        /// The ForgotPassword.
        /// </summary>
        /// <param name="model">The model<see cref="ForgotPasswordRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task ForgotPassword(ForgotPasswordRequest model);

        /// <summary>
        /// The ResetPassword.
        /// </summary>
        /// <param name="model">The model<see cref="ResetPasswordRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task ResetPassword(ResetPasswordRequest model);

        /// <summary>
        /// The GetAll.
        /// </summary>
        /// <returns>The <see cref="Task{IEnumerable{AccountResponse}}"/>.</returns>
        Task<IEnumerable<AccountResponse>> GetAll();

        /// <summary>
        /// The GetById.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AccountResponse}"/>.</returns>
        Task<AccountResponse> GetById(string id);

        /// <summary>
        /// The Create.
        /// </summary>
        /// <param name="model">The model<see cref="CreateRequest"/>.</param>
        /// <returns>The <see cref="Task{AccountResponse}"/>.</returns>
        Task<AccountResponse> Create(CreateRequest model);

        /// <summary>
        /// The Update.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <param name="model">The model<see cref="UpdateRequest"/>.</param>
        /// <returns>The <see cref="Task{AccountResponse}"/>.</returns>
        Task<AccountResponse> Update(string id, UpdateRequest model);

        /// <summary>
        /// The Delete.
        /// </summary>
        /// <param name="id">The id<see cref="string"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete(string id);

        /// <summary>
        /// The RefreshPushNotificationToken.distrubuted.
        /// </summary>
        /// <param name="token">The token<see cref="PushNotificationToken"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task RefreshPushNotificationToken(PushNotificationToken token);

        /// <summary>
        /// The SyncData.
        /// </summary>
        /// <param name="schoolId">The schoolId<see cref="string"/>.</param>
        /// <param name="classId">The classId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{AuthenticateResponse}"/>.</returns>
        Task<AuthenticateResponse> SyncData(string schoolId = null, string classId = null);

        /// <summary>
        /// The NonProfitAccountRegistration.
        /// </summary>
        /// <param name="nonProfitAccount">The nonProfitAccount<see cref="NonProfitAccount"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task NonProfitAccountRegistration(NonProfitAccount nonProfitAccount);

        /// <summary>
        /// The OTPNonProfitVerification.
        /// </summary>
        /// <param name="nonProfitAccount">The nonProfitAccount<see cref="NonProfitAccount"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> OTPNonProfitVerification(NonProfitAccount nonProfitAccount);
    }
}
