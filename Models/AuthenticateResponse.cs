using System.Collections.Generic;

namespace goOfflineE.Models
{
    public class AuthenticateResponse
    {
        public AuthenticateResponse()
        {
            this.Schools = new List<School>();
        }
        public string Id { get; set; }
        public IEnumerable<School> Schools { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
        public bool ForceChangePasswordNextLogin { get; set; }

    }
}