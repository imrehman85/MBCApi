using Microsoft.AspNetCore.Identity;

namespace MbcApi.Core.Entities
{
    public class ApplicationUser:IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FatherName { get; set; }
        public string CNIC { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
    }
}
