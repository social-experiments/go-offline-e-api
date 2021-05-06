namespace goOfflineE.Entites
{
    using System;

    /// <summary>
    /// Defines the <see cref="NonProfitAccount" />.
    /// </summary>
    public class NonProfitAccount : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NonProfitAccount"/> class.
        /// </summary>
        /// <param name="registrationNo">The registrationNo<see cref="string"/>.</param>
        /// <param name="accountId">The accountId<see cref="string"/>.</param>
        public NonProfitAccount(string registrationNo, string accountId)
        {
            this.PartitionKey = registrationNo;
            this.RowKey = accountId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NonProfitAccount"/> class.
        /// </summary>
        public NonProfitAccount()
        {
        }

        /// <summary>
        /// Gets or sets the NameOfNGO.
        /// </summary>
        public string NameOfNGO { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNo.
        /// </summary>
        public string PhoneNo { get; set; }

        /// <summary>
        /// Gets or sets the Location.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the RegistrationNo.
        /// </summary>
        public string RegistrationNo { get; set; }

        /// <summary>
        /// Gets or sets the Address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the TaxRegistrationNo.
        /// </summary>
        public string TaxRegistrationNo { get; set; }

        /// <summary>
        /// Gets or sets the OperationalLocations.
        /// </summary>
        public string OperationalLocations { get; set; }

        /// <summary>
        /// Gets or sets the OTP.
        /// </summary>
        public string OTP { get; set; }

        /// <summary>
        /// Gets or sets the OTPDate.
        /// </summary>
        public DateTime? OTPDate { get; set; }
    }
}
