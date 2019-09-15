using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;

namespace MovieFinder.Controllers
{
    [Route("{controller}")]
    public class UsersController : Controller 
    {
        private readonly UnitOfWork _unitOfWork;
        
        public UsersController(MovieFinderContext movieFinderContext)
        {
            _unitOfWork = new UnitOfWork(movieFinderContext); 
        }

        [HttpPost]
        public IActionResult CreateAccount([FromBody] UsersDto usersDto)
        {
            if (usersDto == null)
            {
                return BadRequest(); 
            }

            var users = _unitOfWork.Users.GetAll();

            if(LoginValidator.EmailExists(usersDto.Email, users))
            {
                return BadRequest("Sorry, that email already is taken"); 
            }

            if (!LoginValidator.PasswordMeetsCriteria(usersDto.Password))
            {
                return BadRequest("Sorry, Password must be 8 characters long and contain at least one number");
            }

            var user = new Users(usersDto);

            _unitOfWork.Users.Add(user);
            _unitOfWork.SaveChanges(); 

            return Ok(user);
        }
    }
}
