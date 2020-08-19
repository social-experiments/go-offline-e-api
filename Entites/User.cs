using System;

namespace goOfflineE.Entites
{
    public class User : BaseEntity
    {
        public User(string schoolId, string userId)
        {
            this.PartitionKey = schoolId;
            this.RowKey = userId;
        }

        public User() { }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public string Role { get; set; }
        public DateTime? Verified { get; set; }
        public bool IsVerified => Verified.HasValue || PasswordReset.HasValue;
        public DateTime? PasswordReset { get; set; }
        public string ProfileStoragePath { get; set; }
        public bool ForceChangePasswordNextLogin { get; set; }
    }
}
