﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieFinder.DtoModels;
using MovieFinder.Models;
using MovieFinder.Repository;
using MovieFinder.Utils;

namespace MovieFinder.Controllers
{
    [Route("[controller]")]
    public class UserSearchHistoryController : Controller
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly Session _sessionVars; 

        public UserSearchHistoryController(MovieFinderContext movieFinderContext, IHttpContextAccessor httpContextAccessor)
        {
            _sessionVars = new Session(httpContextAccessor.HttpContext.User);
            _unitOfWork = new UnitOfWork(movieFinderContext);
        }

        /// <summary>
        /// Creates a new UserSearchHistory entity.
        /// </summary>
        /// <param name="userSearchHistoryDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public IActionResult Create([FromBody] UserSearchHistoryDto userSearchHistoryDto)
        {
            if (userSearchHistoryDto == null)
            {
                return BadRequest("User search dto is required.");
            }

            var user = _unitOfWork.Users.Get(userSearchHistoryDto.UserId);

            if (user == null)
            {
                return BadRequest("User not found.");
            }

            var userSearchHistory = new UserSearchHistory(userSearchHistoryDto);

            _unitOfWork.UserSearchHistory.Add(userSearchHistory);
            _unitOfWork.SaveChanges();

            return Ok(userSearchHistory);
        }
    }
}
