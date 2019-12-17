using Microsoft.AspNetCore.Identity;

namespace MovieFinder.Models
{
    public partial class Users : IdentityUser
    {
        public Users()
        {

        }

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
