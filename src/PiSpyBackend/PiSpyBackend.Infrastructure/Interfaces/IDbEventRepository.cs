using FluentResults;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Infrastructure
{
    public interface IDbEventRepository
    {
        public Result<Event[]> GetAllEvents();
        public Result RegisterEvent(Event newEvent);
    }
}