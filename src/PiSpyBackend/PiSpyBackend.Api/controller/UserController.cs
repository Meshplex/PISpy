using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiSpyBackend.Application;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Api.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtTokenService _jwtTokenService;

        public UserController(UserService userService, JwtTokenService jwtTokenService)
        {
            _userService = userService;
            _jwtTokenService = jwtTokenService;
        }

        [HttpPut("register")]
        [Authorize]
        public Result RegisterUser(string username, string password)
        {
            var userToAdd = new User()
            {
                Username = username,
                Password = password
            };
            var result = _userService.CreateUser(userToAdd);
            if (result.IsFailed)
            {
                return Result.Fail("Failed to create user");
            }
            return Result.Ok();
        }
        
        [HttpPost("login")]
        public Result<string> LoginUser(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return Result.Fail("Empty input");
            }

            var result = _userService.LoginUser(username, password);
            if (result.IsFailed)
            {
                return Result.Fail("Es wurde kein Benutzer mit den angegebenen Daten gefunden");
            }
            return Result.Ok(_jwtTokenService.GenerateToken(result.Value.Username!, result.Value.Id));
        }
        
        [HttpPost("update")]
        [Authorize]
        public Result UpdateUser(int userId, string newPassword)
        {
            if (userId <= 0 || string.IsNullOrEmpty(newPassword))
            {
                return Result.Fail("Empty input");
            }
            var result = _userService.ChangeUserPassword(userId, newPassword);
            if (result.IsFailed)
            {
                return Result.Fail("Empty input");
            }
            return Result.Ok();
        }
        
        [HttpDelete("delete")]
        [Authorize]
        public Result DeleteUser(int userId)
        {
            if (userId <= 0)
            {
                return Result.Fail("Empty input");
            }
            var result = _userService.DeleteUser(userId);
            if (result.IsFailed)
            {
                return Result.Fail("Empty input");
            }
            return Result.Ok();
        }
    }
}