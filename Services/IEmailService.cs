using goOfflineE.Models;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IEmailService
    {
        Task SendAsync(EmailRequest emailRequest);
    }
}
