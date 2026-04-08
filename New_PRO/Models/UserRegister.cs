using Microsoft.AspNetCore.Identity;

namespace New_PRO.Models
{
    public class UserRegister :IdentityUser
    {

        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
