using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;

namespace MovieFinder.Controllers
{
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
                return NotFound(); 
            }

            var users = new Users(usersDto);

            _unitOfWork.Users.Add(users);
            _unitOfWork.SaveChanges(); 

            return Ok(users);
        } 
    }
}
