using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Users
    {
        public Users()
        {

        }

        public Users(RegisterDto registerDto)
        {
            if (registerDto.FirstName == null)
            {
                throw new ArgumentException("First name must have characters");
            }

            if (registerDto.LastName == null)
            {
                throw new ArgumentException("Last name must have characters");
            }

            if (registerDto.Email == null)
            {
                throw new ArgumentException("Email must have characters");
            }

            if (registerDto.Password == null)
            {
                throw new ArgumentException("Password must have characters");
            }

            FirstName = registerDto.FirstName;
            LastName = registerDto.LastName;
            Email = registerDto.Email;
            UserName = registerDto.Email;
        }
    }
}
