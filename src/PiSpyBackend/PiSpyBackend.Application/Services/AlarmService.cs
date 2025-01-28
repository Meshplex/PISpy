using FluentResults;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Application
{
    class AlarmService
    {
        public bool AlarmState { get; private set; }
        private AppDbContext Context { get; set; }

        public AlarmService(AppDbContext context)
        {
            this.Context = context;
        }

        public void ActivateAlarm(string sensorName)
        {
            if (AlarmState == true)
            {
                var eventService = new EventService(Context);
                var eventDescription = $"Alarm ausgelöst der Sensor: {sensorName} hat Alarm gegeben!";
                eventService.AddEvent(description: eventDescription, keyId: null, userId: 0);

                // TODO: Pieper Angehen lassen
            }
        }

        public Result TurnOnAlarm()
        {
            if (AlarmState == true)
            {
                return Result.Fail("Der Alarm ist bereits Scharfgestellt");
            }
            AlarmState = true;
            return Result.Ok();
        }

        public Result TurnOffAlarm()
        {
            if (AlarmState == false)
            {
                return Result.Fail("Der Alarm ist bereits Aus");
            }
            AlarmState = false;
            return Result.Ok();
        }
    }    
}