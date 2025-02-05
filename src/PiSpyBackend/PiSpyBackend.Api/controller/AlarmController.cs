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
        public void ActivateAlarm(string username, int userId)
        {
            _alarmService.TurnOnAlarm(username, userId);
        }

        [HttpPut("deactivate")]
        public void DeactivateAlarm(string username, int userId)
        {
            _alarmService.TurnOffAlarm(username, userId);
        }
    }
}