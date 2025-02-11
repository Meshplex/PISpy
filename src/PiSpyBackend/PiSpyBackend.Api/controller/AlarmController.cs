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
        public void ActivateAlarm()
        {
            var username = HttpContext.User.FindFirst("sub")?.Value;
            var userId = HttpContext.User.FindFirst("nameid")?.Value;
            if (username == null || userId == null)
            {
                return;
            }
            if (int.TryParse(userId, out int id))
            {
                _alarmService.TurnOnAlarm(username, id);
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