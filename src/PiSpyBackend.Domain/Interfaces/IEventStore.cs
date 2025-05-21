namespace PiSpyBackend.Domain.Interfaces
{
  public interface IEventStore
  {
    public void Add(Event evt);
    public IEnumerable<Event> GetAll();
  }
}
