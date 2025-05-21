using System.Security.Principal;
using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiSpyBackend.Application;
using PiSpyBackend.Domain;
using PiSpyBackend.Domain.Models;

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

        [HttpGet("getall")]
        [Authorize]
        public Result<UserDto[]> GetAllUsers()
        {
            var result = _userService.GetAllUsers();
            Console.WriteLine(result.Value);
            if (result.IsFailed)
            {
                return Result.Fail<UserDto[]>("Failed to get all users");
            }
            return Result.Ok(result.Value);
        }

        [HttpPut("register/{username}/{password}")]
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
        public IActionResult LoginUser([FromBody] Domain.Models.LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Username) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(new { Message = "Empty input" });
            }

            var result = _userService.LoginUser(loginRequest.Username, loginRequest.Password);
            if (result.IsFailed)
            {
                return Unauthorized(new { Message = "Es wurde kein Benutzer mit den angegebenen Daten gefunden" });
            }
            return Ok(new { Token = _jwtTokenService.GenerateToken(result.Value.Username!, result.Value.Id) });
        }

        [HttpPost("update/username")]
        [Authorize]
        public Result UpdateUsername([FromBody] UpdateUserRequest request)
        {
            if (request.UserId <= 0 || string.IsNullOrEmpty(request.NewUsername))
            {
                return Result.Fail("Empty input");
            }
            var result = _userService.ChangeUsername(request.UserId, request.NewUsername);
            if (result.IsFailed)
            {
                return Result.Fail("Empty input");
            }
            return Result.Ok();
        }

        [HttpPost("update/password")]
        [Authorize]
        public Result UpdatePassword([FromBody] UpdateUserRequest request)
        {
            if (request.UserId <= 0 || string.IsNullOrEmpty(request.NewPassword))
            {
                return Result.Fail("Empty input");
            }
            var result = _userService.ChangeUserPassword(request.UserId, request.NewPassword);
            if (result.IsFailed)
            {
                return Result.Fail("Empty input");
            }
            return Result.Ok();
        }

        [HttpDelete("delete")]
        [Authorize]
        public Result DeleteUser([FromBody] int userId)
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