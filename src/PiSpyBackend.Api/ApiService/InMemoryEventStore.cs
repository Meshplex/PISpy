using Microsoft.AspNetCore.SignalR;
using PiSpyBackend.Application;
using PiSpyBackend.Domain;
using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Api.ApiService
{
  public class InMemoryEventStore : IEventStore
  {
    private static readonly List<Event> _events = new();
    private readonly IHubContext<AlarmHub> _hubContext;

    public InMemoryEventStore(IHubContext<AlarmHub> hubContext)
    {
      _hubContext = hubContext;
    }

    public void Add(Event evt)
    {
      lock (_events)
      {
        _events.Add(evt);
      }

      _hubContext.Clients.All.SendAsync("ReceiveEvent", evt);
    }

    public IEnumerable<Event> GetAll()
    {
      lock (_events)
      {
        return _events.ToList();
      }
    }
  }
}