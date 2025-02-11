using System;
using System.Numerics;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PiSpyBackend.Domain.Interfaces;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Application
{
    // Ableiten von BackgroundService sorgt dafür, dass der Service beim App-Start automatisch startet.
    public class AlarmService : BackgroundService 
    {
        public bool AlarmState { get; private set; }
        private MotionService MotionService { get; set; }
        private ButtonService ButtonService { get; set; }
        private RfidService RfidService { get; set; }
        private LedRingService LedRingService { get; set; }
        private MapKeyToUserService KeyService { get; set; }
        private IServiceScopeFactory ScopeFactory { get; set; }

        // Dieses CancellationTokenSource steuert alle sensorbezogenen Tasks.
        private CancellationTokenSource _sensorCts;

        public AlarmService(IServiceScopeFactory scopeFactory)
        {
            ScopeFactory = scopeFactory;
            _sensorCts = new CancellationTokenSource();

            // Erzeuge einen GPIO-Controller (hier mit Logischer Pin-Nummerierung)
            var gpioController = new GpioController(PinNumberingScheme.Logical);
            // Initialisiere die Services – ggf. auch über DI injizierbar machen
            RfidService = new RfidService();
            MotionService = new MotionService(gpioController);
            ButtonService = new ButtonService(gpioController);
            LedRingService = new LedRingService();
            var scope = ScopeFactory.CreateScope();
            KeyService = new MapKeyToUserService(scope.ServiceProvider.GetRequiredService<AppDbContext>());

            // Beim Start wird die grüne LED aktiviert.
            LedRingService.ActivateRedLed();
        }

        /// <summary>
        /// Diese Methode wird automatisch vom Host aufgerufen und startet den Hintergrund-Loop.
        /// </summary>
        /// <param name="stoppingToken">Token, das signalisiert, dass die Anwendung beendet wird.</param>
        /// <returns></returns>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Wir kombinieren das vom Host gelieferte Token mit unserem Sensor-Token.
            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(stoppingToken, _sensorCts.Token);
            var token = linkedCts.Token;

            // Starte den RFID-Loop als eigenständige Task.
            var rfidTask = StartRfidService(token);

            // Warten, bis das Token abgebrochen wird (beim Shutdown).
            await Task.WhenAny(rfidTask, Task.Delay(Timeout.Infinite, token));
        }

        /// <summary>
        /// Führt in einer Endlosschleife den RFID-Service aus.
        /// </summary>
        private async Task StartRfidService(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                // RfidService.Run() wird synchron ausgeführt und liefert ein Ergebnis, das zu BigInteger konvertiert wird.
                var result = (BigInteger)RfidService.Run();

                if (result == 0)
                {
                    // Kurze Pause, um einen Tight-Loop zu vermeiden.
                    await Task.Delay(100, token);
                    continue;
                }

                var findUserFromKey = KeyService.FindUserFromKey(result.ToString());
                if (findUserFromKey.IsFailed || string.IsNullOrEmpty(findUserFromKey.Value.Username))
                {
                    await Task.Delay(100, token);
                    continue;
                }

                // Schalte den Alarm um – je nach aktuellem Zustand.
                if (AlarmState)
                {
                    TurnOffAlarm(findUserFromKey.Value.Username, findUserFromKey.Value.Id);
                }
                else
                {
                    TurnOnAlarm(findUserFromKey.Value.Username, findUserFromKey.Value.Id);
                }
            }
        }

        /// <summary>
        /// Löst den Alarm aus (wird z. B. von einem Sensor aufgerufen).
        /// </summary>
        private void ActivateAlarm(string sensorName)
        {
            // Falls der Alarm aktiv ist, wird ein Event hinzugefügt.
            if (AlarmState)
            {
                using var scope = ScopeFactory.CreateScope();
                var eventService = scope.ServiceProvider.GetRequiredService<EventService>();
                var eventDescription = $"Alarm ausgelöst – Sensor '{sensorName}' hat Alarm gegeben!";
                eventService.AddEvent(description: eventDescription, keyId: null, userId: 0);
            }
        }

        /// <summary>
        /// Schaltet den Alarm ein, startet die Sensoren und aktiviert die rote LED.
        /// </summary>
        public void TurnOnAlarm(string username, int userId)
        {
            using var scope = ScopeFactory.CreateScope();
            var eventService = scope.ServiceProvider.GetRequiredService<EventService>();
            AlarmState = true;
            var eventDescription = $"Alarm wurde eingeschaltet von: {username}";
            eventService.AddEvent(description: eventDescription, keyId: null, userId: userId);
            StartSensors();
            LedRingService.ActivateRedLed();
        }

        /// <summary>
        /// Schaltet den Alarm aus, stoppt die Sensoren und aktiviert die grüne LED.
        /// </summary>
        public void TurnOffAlarm(string username, int userId)
        {
            using var scope = ScopeFactory.CreateScope();
            var eventService = scope.ServiceProvider.GetRequiredService<EventService>();
            AlarmState = false;
            var eventDescription = $"Alarm wurde ausgeschaltet von: {username}";
            eventService.AddEvent(description: eventDescription, keyId: null, userId: userId);
            StopSensors();
            LedRingService.ActivateGreenLed();
        }

        /// <summary>
        /// Startet die Tasks für die Sensoren (Bewegungsmelder und Fenstersensor).
        /// </summary>
        private void StartSensors()
        {
            var token = _sensorCts.Token;
            // Starte jeweils eine Task für Motion- und Button-Sensor
            Task.Run(() => RunSensor(MotionService, token), token);
            Task.Run(() => RunSensor(ButtonService, token), token);
        }

        /// <summary>
        /// Stoppt die Sensor-Tasks, indem das Cancellation-Token abgebrochen wird.
        /// </summary>
        private void StopSensors()
        {
            _sensorCts.Cancel();
            // Nach dem Stop kann ein neues Token für zukünftige Sensor-Starts erzeugt werden.
            _sensorCts = new CancellationTokenSource();
        }

        /// <summary>
        /// Führt den jeweiligen Sensor-Service in einer Schleife aus.
        /// </summary>
        private async Task RunSensor(ISensorService sensorService, CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var result = sensorService.Run();
                ProcessSensorResult(sensorService, result);
                await Task.Delay(250, token);
            }
        }

        /// <summary>
        /// Wertet das Ergebnis des Sensors aus und aktiviert ggf. den Alarm.
        /// </summary>
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

        /// <summary>
        /// Wird beim Herunterfahren der Anwendung aufgerufen, um laufende Tasks zu beenden.
        /// </summary>
        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _sensorCts.Cancel();
            await base.StopAsync(cancellationToken);
        }
    }
}
