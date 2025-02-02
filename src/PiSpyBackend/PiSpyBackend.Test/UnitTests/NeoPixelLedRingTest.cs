using System;
using System.Device.Spi;
using System.Drawing;
using Iot.Device.Ws28xx; // Namespace aus dem Iot.Device.Bindings-Paket
using Xunit;

namespace PiSpyBackend.Test.UnitTests
{
    public class NeoPixelLedRingTests
    {
        private readonly Ws2812b _ledRing;
        private const int LedCount = 24;

        public NeoPixelLedRingTests()
        {
            var spiSettings = new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 2400000,
                Mode = SpiMode.Mode0,
                DataBitLength = 8,
            };

            var spiDevice = SpiDevice.Create(spiSettings);
            _ledRing = new Ws2812b(spiDevice, LedCount);
        }

        [Fact]
        public void SetAllLedsToRedTest()
        {
            for (int i = 0; i < LedCount; i++)
            {
                _ledRing.Image.SetPixel(i, 0, Color.Red);
            }
            _ledRing.Update();

            // Warte etwas, damit du den Farbwechsel am Ring beobachten kannst.
            Thread.Sleep(2000);

            // Erreichen wir diesen Punkt ohne Exception, gilt der Test als bestanden.
            Assert.True(true);
        }

        [Fact]
        public void CycleColorsTest()
        {
            // Eine Auswahl an Farben zum Durchlaufen.
            Color[] colors = new Color[] 
            { 
                Color.Red, 
                Color.Green, 
                Color.Blue, 
                Color.Yellow, 
                Color.Purple, 
                Color.Cyan, 
                Color.White 
            };

            foreach (Color color in colors)
            {
                for (int i = 0; i < LedCount; i++)
                {
                    _ledRing.Image.SetPixel(i, 0, color);
                }
                _ledRing.Update();
                Thread.Sleep(500);
            }

            Assert.True(true);
        }
    }
}
