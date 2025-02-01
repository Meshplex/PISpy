using Microsoft.AspNetCore.Mvc;

namespace PiSpyBackend.Api.Controllers
{
    [Route("api/[controller]")]
    public class AlarmController : ControllerBase
    {
        [HttpPost("activate")]
        public void ActivateAlarm(){}

        [HttpPost("deactivate")]
        public void DeactivateAlarm(){}
    }
}