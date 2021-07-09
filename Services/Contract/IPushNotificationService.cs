namespace goOfflineE.Services
{
    using goOfflineE.Models;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="IPushNotification" />.
    /// </summary>
    public interface IPushNotificationService
    {
        /// <summary>
        /// The SendAsync.
        /// </summary>
        /// <param name="notification">The notification<see cref="PushNotification"/>.</param>
        /// <returns>The <see cref="Task{bool}"/>.</returns>
        Task<bool> SendAsync(PushNotification notification);
    }
}
