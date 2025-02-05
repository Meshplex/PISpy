using FluentResults;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Infrastructure
{
    public class DbEventRepository : IDbEventRepository
    {
        private AppDbContext context; 
        public DbEventRepository(AppDbContext dbContext)
        {
            this.context = dbContext;
        }

        public Result<Event[]> GetAllEvents()
        {
            return Result.Ok(context.Events.ToArray());
        }

        public Result RegisterEvent(Event newEvent)
        {
            context.Events.Add(newEvent);
            return Result.Ok();
        }
    }
}