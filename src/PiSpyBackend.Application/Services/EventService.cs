using FluentResults;
using PiSpyBackend.Domain;
using PiSpyBackend.Domain.Interfaces;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Application
{
    public class EventService
    {
        private IDbEventRepository repo { get; set; }
        private IEventStore eventStore { get; set; }

        public EventService(AppDbContext context, IEventStore eventStore)

        {
            this.repo = new DbEventRepository(context);
            this.eventStore = eventStore;
        }

        public Result<Event[]> GetEvents()
        {
            return Result.Ok(repo.GetAllEvents().Value);
        }

        public Result AddEvent(string? keyId, int userId, Eventtype eventtype)
        {
            var newEvent = new Event
            {
                KeyId = keyId ?? "",
                UserId = userId,
                EventType = eventtype,
                Timestamp = DateTime.Now
            };
            repo.RegisterEvent(newEvent);
            eventStore.Add(newEvent);
            return Result.Ok();
        }
    }
}