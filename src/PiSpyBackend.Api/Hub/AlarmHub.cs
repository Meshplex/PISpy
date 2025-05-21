using Microsoft.AspNetCore.SignalR;
using PiSpyBackend.Api.ApiService;
using PiSpyBackend.Application;
using PiSpyBackend.Domain.Interfaces;

public class AlarmHub : Hub
{
    private readonly AlarmService _alarmService;
    private readonly EventService EventService;

    public AlarmHub(AlarmService alarmService, EventService eventService)
    {
        _alarmService = alarmService;
        EventService = eventService;
    }

    public override async Task OnConnectedAsync()
    {
        var existingEvents = EventService.GetEvents().Value.AsEnumerable(); ;
        await Clients.Caller.SendAsync("ReceiveAllEvents", existingEvents);
        await base.OnConnectedAsync();
    }

    public async Task TogglePropertyStatus(bool newStatus, string username, int userId)
    {
        if (newStatus)
        {
            _alarmService.TurnOnAlarm(username, userId);
        }
        else
        {
            _alarmService.TurnOffAlarm(username, userId);
        }
        await Clients.All.SendAsync("ReceivePropertyStatus", newStatus);
    }
}
