﻿using MovieFinder.DtoModels;
using System;

namespace MovieFinder.Models
{
    public partial class Users
    {
        public Users()
        {

        }

        public Users(CreateAccountDto createAccountDto)
        {
            if (createAccountDto.FirstName == null)
            {
                throw new ArgumentException("First name must have characters");
            }

            if (createAccountDto.LastName == null)
            {
                throw new ArgumentException("Last name must have characters");
            }

            if (createAccountDto.Email == null)
            {
                throw new ArgumentException("Email must have characters");
            }

            if (createAccountDto.Password == null)
            {
                throw new ArgumentException("Password must have characters");
            }

            FirstName = createAccountDto.FirstName;
            LastName = createAccountDto.LastName;
            UserName = createAccountDto.Email;
        }

        public static Users CreateUser(CreateAccountDto createAccountDto)
        {
            if (createAccountDto == null)
            {
                throw new ArgumentException("CreateAccountDto must not be empty.");
            }

            var user = new Users(createAccountDto);

            return user;
        }
    }
}
