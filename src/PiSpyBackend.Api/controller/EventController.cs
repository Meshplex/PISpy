using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using PiSpyBackend.Application;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class  EventController : ControllerBase
    {
        private readonly EventService _eventService;
        private readonly IHubContext<AlarmHub> _hubContext;

        public EventController(EventService eventService, IHubContext<AlarmHub> hubContext)
        {
            _eventService = eventService;
            _hubContext = hubContext;
        }

        [HttpGet("getEvents")]
        public Result<Event[]> GetEvents()
        {
            return Result.Ok(_eventService.GetEvents().Value);
        }

        [HttpPost("registerEvent")]
        public void RegisterEvent(string description, string keyId, int userId)
        {
            _eventService.AddEvent(keyId, userId, description);
        }
    }
}