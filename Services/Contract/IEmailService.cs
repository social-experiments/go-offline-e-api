namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IEmailService" />.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// The SendAsync.
        /// </summary>
        /// <param name="emailRequest">The emailRequest<see cref="EmailRequest"/>.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task SendAsync(EmailRequest emailRequest);
    }
}
