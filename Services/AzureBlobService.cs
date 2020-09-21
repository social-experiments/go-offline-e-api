namespace goOfflineE.Services
{
    using Azure.Storage;
    using Azure.Storage.Sas;
    using goOfflineE.Common.Constants;
    using goOfflineE.Helpers;
    using goOfflineE.Models;
    using Microsoft.WindowsAzure.Storage;
    using System;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="AzureBlobService" />.
    /// </summary>
    public class AzureBlobService : IAzureBlobService
    {
        /// <summary>
        /// The GetSasUri.
        /// </summary>
        /// <param name="containerName">The containerName<see cref="string"/>.</param>
        /// <returns>The <see cref="Task{BlobStorageRequest}"/>.</returns>
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
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(7),
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
