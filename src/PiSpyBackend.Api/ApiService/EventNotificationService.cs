using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiSpyBackend.Application;
using PiSpyBackend.Domain;

namespace PiSpyBackend.Api.Services
{
    public class EventNotificationService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IHubContext<AlarmHub> _hubContext;
        private Event[] _lastEvents = Array.Empty<Event>();

        public EventNotificationService(IServiceScopeFactory serviceScopeFactory, IHubContext<AlarmHub> hubContext)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var eventService = scope.ServiceProvider.GetRequiredService<EventService>();
                    var currentEvents = eventService.GetEvents().Value;

                    var newEvents = currentEvents.Except(_lastEvents).ToArray();
                    if (newEvents.Length > 0)
                    {
                        _lastEvents = currentEvents;
                        await _hubContext.Clients.All.SendAsync("NewEvents", newEvents);
                    }
                }

                await Task.Delay(5000, stoppingToken); // Alle 5 Sekunden prüfen
            }
        }
    }
}
