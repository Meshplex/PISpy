using System.Device.Spi;
using System.Drawing;
using rpi_ws281x;

namespace PiSpyBackend.Test.UnitTests
{
    public class NeoPixelLedRingTests
    {

        [Fact]
        public void SetAllLedsToRedTest()
        {
            var settings = Settings.CreateDefaultSettings(false);
            var controller = settings.AddController(16, Pin.Gpio19, StripType.WS2812_STRIP, ControllerType.PWM0, 255, false);

            using (var rpi = new WS281x(settings))
            {
                controller.SetLED(0, Color.Blue);
                controller.SetLED(1, Color.Red);
                rpi.Render();
            };
        }
    }
}
