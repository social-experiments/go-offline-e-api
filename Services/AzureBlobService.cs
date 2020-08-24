using goOfflineE.Helpers;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using Azure.Storage.Blobs;
using Azure.Storage;
using Azure.Storage.Sas;
using Azure.Storage.Blobs.Specialized;
using goOfflineE.Models;

namespace goOfflineE.Services
{
    public class AzureBlobService : IAzureBlobService
    {
        public async Task<BlobStorageRequest> GetSasUri(string containerName)
        {
          
            // connect to our storage account and create a blob client
            var connectionString = SettingConfigurations.AzureWebJobsStorage;
            var storageAccount = CloudStorageAccount.Parse(connectionString);
            var blobClient = storageAccount.CreateCloudBlobClient();
            StorageSharedKeyCredential credential = new StorageSharedKeyCredential(storageAccount.Credentials.AccountName, SettingConfigurations.AccountKey);

            // get a reference to the container
            var blobContainer = blobClient.GetContainerReference(containerName);
            await blobContainer.CreateIfNotExistsAsync();

            var sasBuilder = new AccountSasBuilder()
            {
                StartsOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(5),
                Services = AccountSasServices.Blobs,
                ResourceTypes = AccountSasResourceTypes.All,
                Protocol = SasProtocol.Https
            };
            sasBuilder.SetPermissions(AccountSasPermissions.All);

            var sasToken = sasBuilder.ToSasQueryParameters(credential).ToString();
            return new BlobStorageRequest { StorageAccessToken = sasToken, StorageUri = blobContainer.ServiceClient.StorageUri.PrimaryUri.AbsoluteUri };
        }

    }
}
