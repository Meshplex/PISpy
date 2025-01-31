using System;
using System.Device.Gpio;
using Xunit;

namespace SensorTests
{
    public class PiezoSensorTests : IDisposable
    {
        private readonly GpioController _controller;
        private const int BuzzerPin = 18; // BCM 18

        public PiezoSensorTests()
        {
            // Erzeugt einen GPIO-Controller
            _controller = new GpioController();
            // Buzzer-Pin als Ausgang setzen
            _controller.OpenPin(BuzzerPin, PinMode.Output);
        }

        [Fact]
        public void BuzzerShouldBeep()
        {
            // Schaltet den Buzzer ein (High)
            _controller.Write(BuzzerPin, PinValue.High);

            // Lässt ihn 2 Sekunden piepen
            Console.WriteLine("Buzzer an (aktiver Piezo). Jetzt sollte es piepen!");
            System.Threading.Thread.Sleep(500);

            // Schaltet wieder aus (Low)
            _controller.Write(BuzzerPin, PinValue.Low);
            Console.WriteLine("Buzzer aus. Piepen sollte gestoppt sein.");

            // Für den Test kein echter 'Assert'; du könntest z. B. nur kontrollieren,
            // dass dein Code ohne Fehler durchläuft.
            Assert.True(true);
        }

        public void Dispose()
        {
            // Pin aufräumen
            _controller.ClosePin(BuzzerPin);
            _controller.Dispose();
        }
    }
}
