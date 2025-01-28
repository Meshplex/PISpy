using FluentResults;
using PiSpyBackend.Domain;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Application
{
    class EventService
    {
        private IDbEventRepository repo { get; set; }

        public EventService(AppDbContext context)
        {
            this.repo = new DbEventRepository(context);
        }

        public Result AddEvent(string? keyId, int userId, string description)
        {
            var newEvent = new Event
            {
                KeyId = keyId ?? "",
                UserId = userId,
                Description = description,
                Timestamp = DateTime.Now
            };
            repo.RegisterEvent(newEvent);
            return Result.Ok();
        }
    }
}