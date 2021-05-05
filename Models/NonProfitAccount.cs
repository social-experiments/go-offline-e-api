namespace goOfflineE.Models
{
    /// <summary>
    /// Defines the <see cref="NonProfitAccount" />.
    /// </summary>
    public class NonProfitAccount
    {
        /// <summary>
        /// Gets or sets the Id.
        /// </summary>
        public string Id { get; set; }

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
        /// Gets or sets the Active.
        /// </summary>
        public bool? Active { set; get; }
    }
}
