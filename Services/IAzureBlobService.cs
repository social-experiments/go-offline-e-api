using goOfflineE.Models;
using System.Threading.Tasks;

namespace goOfflineE.Services
{
    public interface IAzureBlobService
    {
        Task<BlobStorageRequest> GetSasUri(string containerName);
    }
}
