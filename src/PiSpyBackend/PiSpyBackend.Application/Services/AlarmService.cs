using FluentResults;
using PiSpyBackend.Domain.Interfaces;
using PiSpyBackend.Infrastructure;
using System.Threading;
using System.Threading.Tasks;

namespace PiSpyBackend.Application
{
    class AlarmService 
    {
        public bool AlarmState { get; private set; }
        private AppDbContext Context { get; set; }
        private MotionService MotionService { get; set; }
        private ButtonService ButtonService { get; set; }
        private RfidService RfidService { get; set; }
        private LedRingService LedRingService { get; set; }
        private CancellationTokenSource cancellationTokenSource;

        public AlarmService(AppDbContext context)
        {
            this.Context = context;
            this.RfidService = new RfidService();
            this.LedRingService = new LedRingService();
        }

        public void ActivateAlarm(string sensorName)
        {
            if (AlarmState == true)
            {
                var eventService = new EventService(Context);
                var eventDescription = $"Alarm ausgelöst der Sensor: {sensorName} hat Alarm gegeben!";
                eventService.AddEvent(description: eventDescription, keyId: null, userId: 0);
            }
        }

        public Result TurnOnAlarm()
        {
            if (AlarmState == true)
            {
                return Result.Fail("Der Alarm ist bereits Scharfgestellt");
            }
            AlarmState = true;
            StartSensors();
            return Result.Ok();
        }

        public Result TurnOffAlarm()
        {
            if (AlarmState == false)
            {
                return Result.Fail("Der Alarm ist bereits Aus");
            }
            AlarmState = false;
            StopSensors();
            return Result.Ok();
        }

        private void StartSensors()
        {
            cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            MotionService = new MotionService();
            ButtonService = new ButtonService();

            Task.Run(() => RunSensor(MotionService, token), token);
            Task.Run(() => RunSensor(ButtonService, token), token);
        }

        private void StopSensors()
        {
            cancellationTokenSource?.Cancel();
        }

        private async Task RunSensor(ISensorService sensorService, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var result = sensorService.Run();
                ProcessSensorResult(sensorService, result);
                await Task.Delay(1000); // Adjust the delay as needed
            }
        }

        private void ProcessSensorResult(ISensorService sensorService, object result)
        {
            // Verarbeiten Sie hier die Rückgabewerte der Sensoren
            if (sensorService is MotionService)
            {
                // Verarbeiten Sie die Rückgabewerte des Bewegungssensors
                var distance = (int)result;
                if (distance < 100) // Beispielbedingung
                {
                    ActivateAlarm("MotionSensor");
                }
            }
            else if (sensorService is RfidService)
            {
                // Verarbeiten Sie die Rückgabewerte des RFID-Sensors
                var rfid = (string)result;
                // Beispiel: Überprüfen Sie die RFID-ID
                if (rfid == "123456")
                {
                    ActivateAlarm("RfidSensor");
                }
            }
            // Fügen Sie hier weitere Sensoren hinzu
        }
    }
}