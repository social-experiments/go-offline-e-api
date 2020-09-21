namespace goOfflineE.Repository
{
    /// <summary>
    /// Defines the <see cref="BatchOperationOptions" />.
    /// </summary>
    public class BatchOperationOptions
    {
        /// <summary>
        /// Gets or sets the BatchInsertMethod.
        /// </summary>
        public BatchInsertMethod BatchInsertMethod { get; set; }
    }

    /// <summary>
    /// Defines the BatchInsertMethod.
    /// </summary>
    public enum BatchInsertMethod
    {
        /// <summary>
        /// Defines the Insert.
        /// </summary>
        Insert,

        /// <summary>
        /// Defines the InsertOrReplace.
        /// </summary>
        InsertOrReplace,

        /// <summary>
        /// Defines the InsertOrMerge.
        /// </summary>
        InsertOrMerge
    }
}
