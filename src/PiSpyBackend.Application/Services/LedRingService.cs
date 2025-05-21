using System;
using System.Drawing;
using System.Device.Spi;
using Iot.Device.Ws28xx;

namespace PiSpyBackend.Application.Services
{
    /// <summary>
    /// Dienst zur Ansteuerung eines 24-LED WS2812B-Rings über SPI (SPI0.1).
    /// </summary>
    public class LedRingService : IDisposable
    {
        private const int LedCount = 24;          // Anzahl der LEDs im Ring
        private readonly SpiDevice _spiDevice;
        private readonly Ws2812b _ledDevice;
        private bool _disposed = false;

        /// <summary>
        /// Initialisiert SPI0.1 (CE1) mit 2.4 MHz, Mode0 und erstellt das Ws2812b-Objekt.
        /// </summary>
        public LedRingService()
        {
            try
            {
                Console.WriteLine("Initialisiere SPI für LED-Ring...");
                var settings = new SpiConnectionSettings(busId: 0, chipSelectLine: 1)
                {
                    ClockFrequency = 2_400_000,  // 2.4 MHz Takt für WS2812B (SPI Mode0):contentReference[oaicite:5]{index=5}
                    Mode = SpiMode.Mode0,
                    DataBitLength = 8
                };
                _spiDevice = SpiDevice.Create(settings);
                Console.WriteLine("SPI für LED-Ring initialisiert." + _spiDevice);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler bei der SPI-Initialisierung: {ex.Message}");
                throw new InvalidOperationException("SPI-Initialisierung für LED-Ring fehlgeschlagen.", ex);
            }

            // Erstelle das WS2812B-Gerät mit 24 LEDs (Breite=24, Höhe=1)
            _ledDevice = new Ws2812b(_spiDevice, LedCount, 1);
        }

        /// <summary>
        /// Alle LEDs dauerhaft rot leuchten lassen.
        /// </summary>
        public void SetRed()
        {
            SetColor(Color.Red);
        }

        /// <summary>
        /// Alle LEDs dauerhaft grün leuchten lassen.
        /// </summary>
        public void SetGreen()
        {
            SetColor(Color.Green);
        }

        /// <summary>
        /// Alle LEDs für die angegebene Dauer rot blinken (an/aus ~500ms).
        /// </summary>
        public void BlinkRed(TimeSpan blinkDuration)
        {
            // Blink-Intervall ~500ms
            TimeSpan interval = TimeSpan.FromMilliseconds(500);
            var endTime = DateTime.UtcNow + blinkDuration;

            while (DateTime.UtcNow < endTime)
            {
                // LED-Ring einschalten (rot)
                SetColor(Color.Red);
                System.Threading.Thread.Sleep(interval);

                // LED-Ring ausschalten (alle schwarz)
                _ledDevice.Image.Clear();    // Alle Pixel auf Schwarz
                _ledDevice.Update();         // An Treiber senden:contentReference[oaicite:6]{index=6}
                System.Threading.Thread.Sleep(interval);
            }

            // Nach dem Blinken sicherheitshalber ausschalten
            _ledDevice.Image.Clear();
            _ledDevice.Update();
        }

        /// <summary>
        /// Hilfsmethode: Setzt alle LEDs auf die angegebene Farbe.
        /// </summary>
        private void SetColor(Color color)
        {
            // Fülle das Bild mit der Farbe
            var image = _ledDevice.Image;    // BitmapImage mit 24x1 Pixel:contentReference[oaicite:7]{index=7}
            image.Clear();
            for (int i = 0; i < LedCount; i++)
            {
                image.SetPixel(i, 0, color);
            }
            _ledDevice.Update();             // Bild an den LED-Treiber senden:contentReference[oaicite:8]{index=8}
        }

        /// <summary>
        /// Gibt SPI-Device und LED-Treiber frei.
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
                return;

            _spiDevice?.Dispose();
            _disposed = true;
        }
    }
}
