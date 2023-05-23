using Microsoft.AspNetCore.Identity;

namespace MbcApi.Core.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
