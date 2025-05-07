using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PiSpyBackend.Application;

namespace PiSpyBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AlarmController : ControllerBase
    {
        private readonly AlarmService _alarmService;

        public AlarmController(AlarmService alarmService)
        {
            _alarmService = alarmService;
        }

        [HttpPut("activate")]
        public Result ActivateAlarm()
        {
            var username = HttpContext.User.FindFirst("sub")?.Value;
            var userId = HttpContext.User.FindFirst("nameid")?.Value;
            if (username == null || userId == null)
            {
                return Result.Fail("Failed to get the user id from the token");
            }
            if (int.TryParse(userId, out int id))
            {
                _alarmService.TurnOnAlarm(username, id);
                return Result.Ok();
            }
            else 
            {
                return Result.Fail("Failed to parse the user id");
            }

        }

        [HttpPut("deactivate")]
        public void DeactivateAlarm()
        {
            var username = HttpContext.User.FindFirst("sub")?.Value;
            var userId = HttpContext.User.FindFirst("nameid")?.Value;
            if (username == null || userId == null)
            {
                return;
            }
            if (int.TryParse(userId, out int id))
            {
                _alarmService.TurnOffAlarm(username, id);
            }
        }
    }
}