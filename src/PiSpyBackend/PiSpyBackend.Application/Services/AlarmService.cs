using PiSpyBackend.Domain.Interfaces;
using PiSpyBackend.Infrastructure;
using System.Numerics;

namespace PiSpyBackend.Application
{
    public class AlarmService 
    {
        public bool AlarmState { get; private set; }
        private AppDbContext Context { get; set; }
        private MotionService MotionService { get; set; }
        private ButtonService ButtonService { get; set; }
        private RfidService RfidService { get; set; }
        private LedRingService LedRingService { get; set; }
        private MapKeyToUserService KeyService { get; set; }
        private EventService EventService { get; set; }
        private CancellationTokenSource cancellationTokenSource;

        public AlarmService(AppDbContext context)
        {   
            this.cancellationTokenSource = new CancellationTokenSource();
            this.Context = context;
            this.EventService = new EventService(Context);
            this.KeyService = new MapKeyToUserService(this.Context);
            this.RfidService = new RfidService();
            this.MotionService = new MotionService();
            this.ButtonService = new ButtonService();
            this.LedRingService = new LedRingService();
            StartRfidService();
            LedRingService.ActivateGreenLed();
        }

        private void StartRfidService()
        {
            Task.Run(() =>
            {
                while (!cancellationTokenSource.Token.IsCancellationRequested)
                {
                    var result = (BigInteger)RfidService.Run();
                    if (result == 0)
                    {
                        continue;
                    }
                    var findUserFromKey = KeyService.FindUserFromKey(result.ToString());
                    if (findUserFromKey.IsFailed || String.IsNullOrEmpty(findUserFromKey.Value.Username))
                    {
                        continue;
                    }
                    if (AlarmState)
                    {
                        TurnOffAlarm(findUserFromKey.Value.Username, findUserFromKey.Value.Id);
                    }
                    else {
                        TurnOnAlarm(findUserFromKey.Value.Username, findUserFromKey.Value.Id);
                    }
                }
            });
        }

        private void ActivateAlarm(string sensorName)
        {
            if (AlarmState == true)
            {
                var eventDescription = $"Alarm ausgelöst der Sensor: {sensorName} hat Alarm gegeben!";
                EventService.AddEvent(description: eventDescription, keyId: null, userId: 0);
            }
        }

        public void TurnOnAlarm(string username, int userId)
        {
            AlarmState = true;
            var eventDescription = $"Alarm wurde eingeschalten von: {username}";
            EventService.AddEvent(description: eventDescription, keyId: null, userId: userId);
            StartSensors();
            LedRingService.ActivateRedLed();
        }

        public void TurnOffAlarm(string username, int userId)
        {
            AlarmState = false;
            var eventDescription = $"Alarm wurde ausgeschalten von: {username}";
            EventService.AddEvent(description: eventDescription, keyId: null, userId: userId);
            StopSensors();
            LedRingService.ActivateGreenLed();
        }

        private void StartSensors()
        {
            var token = cancellationTokenSource.Token;
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
                await Task.Delay(250, cancellationToken: token);
            }
        }

        private void ProcessSensorResult(ISensorService sensorService, object result)
        {
            if (sensorService is MotionService)
            {
                var motionDetected = (bool)result;
                if (motionDetected)
                {
                    ActivateAlarm("Bewegungsmelder");
                }
            }
            else if (sensorService is ButtonService)
            {
                var buttonPressed = (bool)result;
                if (buttonPressed)
                {
                    ActivateAlarm("Fensterbruchsensor");
                }
            }
        }
    }
}