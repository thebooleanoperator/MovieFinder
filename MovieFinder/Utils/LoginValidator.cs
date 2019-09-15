using MovieFinder.Models;
using System.Collections.Generic;
using System.Linq;

namespace MovieFinder.Utils
{
    public static class LoginValidator
    {
        public static bool EmailExists(string email, IEnumerable<Users> users)
        {
            return users.Any(u => u.Email == email); 
        }

        public static bool PasswordMeetsCriteria(string password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            if (!password.Any(char.IsDigit))
            {
                return false; 
            }

            return true;
        }

    }
}
