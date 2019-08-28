using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Users
    {
        public Users(UsersDto usersDto)
        {
            if (usersDto.FirstName.Length <= 0)
            {
                throw new ArgumentException("First name must have characters"); 
            }

            if (usersDto.LastName.Length <= 0)
            {
                throw new ArgumentException("Last name must have characters");
            }

            FirstName = usersDto.FirstName;
            LastName = usersDto.LastName; 
        }
    }
}
