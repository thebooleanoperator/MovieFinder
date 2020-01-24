using MovieFinder.Models;
using System;

namespace MovieFinder.DtoModels
{
    public class UsersDto
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }

        public UsersDto()
        {

        }

        public UsersDto(Users user)
        {
            if (user == null)
            {
                throw new ArgumentException("User cannot be null");
            }

            if (user.FirstName == null)
            {
                throw new ArgumentException("First name cannot be null");
            }

            if (user.LastName == null)
            {
                throw new ArgumentException("Last name cannot be null");
            }

            if (user.UserId < 0 || user.UserId == null)
            {
                throw new ArgumentException("User Id cannot be null or less than zero.");
            }

            if (user.Email == null)
            {
                throw new ArgumentException("Email cannot be null.");
            }

            UserId = user.UserId;
            Email = user.Email;
            FirstName = user.FirstName;
            LastName = user.LastName;
        }
    }
}
