using Microsoft.AspNetCore.SignalR;
using PiSpyBackend.Application;
using PiSpyBackend.Domain;

public class AlarmHub : Hub
{
    private readonly AlarmService _alarmService;

    public AlarmHub(AlarmService alarmService)
    {
        _alarmService = alarmService;
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
