using FluentResults;
using Microsoft.AspNetCore.Mvc;
using PiSpyBackend.Application;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class  EventController : ControllerBase
    {
        private readonly EventService _eventService;

        public EventController(EventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet("getEvents")]
        public Result<Event[]> GetEvents()
        {
            return Result.Ok(_eventService.GetEvents().Value);
        }
    }
}