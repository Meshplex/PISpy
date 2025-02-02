
using rpi_ws281x;
using System.Device.Spi;
using System.Drawing;

namespace PiSpyBackend.Test.UnitTests
{
    public class NeoPixelLedRingTests
    {

        [Fact]
        public void SetAllLedsToRedTest()
        {
            var settings = Settings.CreateDefaultSettings();
        
            // Kanal für die LEDs konfigurieren
            settings.Channels[0] = new Channel(
                ledCount: 24,       // Anzahl der LEDs
                gpioPin: 19,        // GPIO 21
                brightness: 255,    // Helligkeit (0-255)
                invert: false,      // Signal nicht invertieren
                stripType: StripType.WS2812_STRIP // Typ der LEDs
            );

        // LED-Controller erstellen
            var controller = new WS281x(settings);

        // Beispiel: Erste LED auf Rot setzen
            controller.SetLEDColor(0, 23, Color.Red);
            controller.Render(); // Änderungen anzeigen
        }
    }
}
