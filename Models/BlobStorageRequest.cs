namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="BlobStorageRequest" />.
    /// </summary>
    public class BlobStorageRequest
    {
        /// <summary>
        /// Gets or sets the StorageUri.
        /// </summary>
        public string StorageUri { get; set; }

        /// <summary>
        /// Gets or sets the StorageAccessToken.
        /// </summary>
        public string StorageAccessToken { get; set; }
    }
}
