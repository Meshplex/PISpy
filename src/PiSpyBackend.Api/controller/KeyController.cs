using FluentResults;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PiSpyBackend.Application;

namespace PiSpyBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class KeyController : ControllerBase
    {
        private readonly MapKeyToUserService _service;
        public KeyController(MapKeyToUserService service)
        {
            _service = service;
        }

        [HttpPost("mapkeytouser")]
        [Authorize]
        public Result MapKeyToUser(string keyId)
        {
            var userId = HttpContext.User.FindFirst("nameid")?.Value;
            if (userId == null)
            {
                return Result.Fail("Failed to get the user id from the token");
            }
            _service.AddKey(keyId, int.Parse(userId), null);
            return Result.Ok();
        }

        [HttpGet("getkeysFromUser")]
        [Authorize]
        public Result<Domain.Key[]> GetKeysFromUser()
        {
            var userId = HttpContext.User.FindFirst("nameid")?.Value;
            if (userId == null)
            {
                return Result.Fail("Failed to get the user id from the token");
            }
            var keys = _service.GetKeysFromUser(int.Parse(userId));
            if (keys.IsFailed)
            {
                return Result.Fail("Failed to get the keys from the user");
            }
            return Result.Ok(keys.Value);
        }

    }

}