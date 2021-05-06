namespace goOfflineE.Functions
{
    using AzureFunctions.Extensions.Swashbuckle.Attribute;
    using goOfflineE.Helpers.Attributes;
    using goOfflineE.Models;
    using goOfflineE.Services;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Net;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AccountFunction" />.
    /// </summary>
    public class AccountFunction : AuthenticationFilter
    {
        /// <summary>
        /// Defines the _accountService.
        /// </summary>
        private readonly IAccountService _accountService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountFunction"/> class.
        /// </summary>
        /// <param name="accountService">The accountService<see cref="IAccountService"/>.</param>
        public AccountFunction(IAccountService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// The Login.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AccountLogin")]
        public async Task<IActionResult> Login(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "login")]
            [RequestBodyType(typeof(AuthenticateRequest), "User authentication request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            AuthenticateRequest requestData = JsonConvert.DeserializeObject<AuthenticateRequest>(requestBody);

            var response = await _accountService.Authenticate(requestData);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The StudentLogin.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AccountStudentLogin")]
        public async Task<IActionResult> StudentLogin(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "login/student")]
            [RequestBodyType(typeof(StudentAuthenticateRequest), "User authentication request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            StudentAuthenticateRequest requestData = JsonConvert.DeserializeObject<StudentAuthenticateRequest>(requestBody);

            var response = await _accountService.Authenticate(requestData);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The ResetPassword.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AccountResetPassword")]
        public async Task<IActionResult> ResetPassword(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "reset/password")]
            [RequestBodyType(typeof(ResetPasswordRequest), "Reset password request")] HttpRequest request)
        {
            var validateStatus = base.AuthorizationStatus(request);
            if (validateStatus != HttpStatusCode.Accepted)
            {
                return new BadRequestObjectResult(validateStatus);
            }
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            ResetPasswordRequest requestData = JsonConvert.DeserializeObject<ResetPasswordRequest>(requestBody);

            await _accountService.ResetPassword(requestData);

            return new OkObjectResult(new { message = "Reset password successfully." });
        }

        /// <summary>
        /// The VerifyEmail.
        /// </summary>
        /// <param name="req">The req<see cref="HttpRequest"/>.</param>
        /// <param name="emailId">The emailId<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("AccountVerifyEmail")]
        public async Task<IActionResult> VerifyEmail(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "verifyemail/{emailId}")] HttpRequest req, string emailId)
        {
            var validateStatus = base.AuthorizationStatus(req);
            if (validateStatus != System.Net.HttpStatusCode.Accepted || String.IsNullOrEmpty(emailId))
            {
                return new BadRequestObjectResult(validateStatus);
            }

            var response = await _accountService.VerifyEmail(emailId);

            return new OkObjectResult(response);
        }

        /// <summary>
        /// The RefreshPushNotification.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("RefreshPushNotification")]
        public async Task<IActionResult> RefreshPushNotification(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "refresh/notification/token")]
            [RequestBodyType(typeof(PushNotificationToken), "Refresh push notification token request")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            PushNotificationToken requestData = JsonConvert.DeserializeObject<PushNotificationToken>(requestBody);

            await _accountService.RefreshPushNotificationToken(requestData);

            return new OkObjectResult(new { message = "Refresh token successfully." });
        }

        /// <summary>
        /// The RegisterNonProfitAccount.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("NonProfitAccount")]
        public async Task<IActionResult> RegisterNonProfitAccount(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "register/account")]
            [RequestBodyType(typeof(NonProfitAccount), "Register New non-profit account")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            NonProfitAccount requestData = JsonConvert.DeserializeObject<NonProfitAccount>(requestBody);

            await _accountService.NonProfitAccountRegistration(requestData);

            return new OkObjectResult(new { message = "Registration successful." });
        }

        /// <summary>
        /// The OTPVerification.
        /// </summary>
        /// <param name="request">The request<see cref="HttpRequest"/>.</param>
        /// <returns>The <see cref="Task{IActionResult}"/>.</returns>
        [FunctionName("OTPVerification")]
        public async Task<IActionResult> OTPVerification(
           [HttpTrigger(AuthorizationLevel.Anonymous, "post",  Route = "otp/verification")]
            [RequestBodyType(typeof(NonProfitAccount), "OTP verification")] HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            NonProfitAccount requestData = JsonConvert.DeserializeObject<NonProfitAccount>(requestBody);

            if (String.IsNullOrEmpty(requestData.Email) && String.IsNullOrEmpty(requestData.OTP))
            {
                return new UnauthorizedResult();
            }

            var response = await _accountService.OTPNonProfitVerification(requestData);

            return new OkObjectResult(response);
        }
    }
}
