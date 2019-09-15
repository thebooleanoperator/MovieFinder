using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Users
    {
        public Users(UsersDto usersDto)
        {
            if (usersDto.FirstName == null)
            {
                throw new ArgumentException("First name must have characters");
            }

            if (usersDto.LastName == null)
            {
                throw new ArgumentException("Last name must have characters");
            }

            if (usersDto.Email == null)
            {
                throw new ArgumentException("Email must have characters");
            }

            if (usersDto.Password == null)
            {
                throw new ArgumentException("Password must have characters");
            }

            FirstName = usersDto.FirstName;
            LastName = usersDto.LastName;
            Email = usersDto.Email;
            Password = usersDto.Password;
        }
    }
}
