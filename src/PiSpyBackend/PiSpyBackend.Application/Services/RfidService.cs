using System;
using System.Device.Gpio;
using System.Device.Spi;
using Iot.Device.Mfrc522;
using Iot.Device.Rfid;
using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Application
{
    public class RfidService : ISensorService
    {
        private const int SpiBusId = 0;
        private const int ChipSelectLine = 0; // CE0 (Pin 24)
        private const int ResetPin = 25;      // GPIO25 (Pin 22)
        
        public object Run()
        {
            try
            {
                // Erstelle den GPIO-Controller in einem using-Block, damit er korrekt entsorgt wird.
                using var gpio = new GpioController(PinNumberingScheme.Logical);
                
                var spiSettings = new SpiConnectionSettings(SpiBusId, ChipSelectLine)
                {
                    ClockFrequency = 10_000_000,
                    Mode = SpiMode.Mode0
                };

                // Erstelle das SPI-Gerät.
                using var spi = SpiDevice.Create(spiSettings);
                
                // Initialisiere den RFID-Leser.
                using var reader = new MfRc522(spi, ResetPin, gpio, false);

                // Antennenkonfiguration
                reader.Gain = Gain.G38dB; // Entspricht ~36dB (Wertbereich 0x00-0x70)
                reader.Enabled = true;

                // Prüfe, ob eine Karte vorhanden ist.
                // Hier wird ein Array der erwarteten Länge (2 Byte für ATQA) verwendet.
                var atqa = new byte[2];
                if (reader.IsCardPresent(atqa, false))
                {
                    // Versuche, die UID der Karte auszulesen.
                    if (reader.ListenToCardIso14443TypeA(out Data106kbpsTypeA cardData, TimeSpan.FromSeconds(2)))
                    {
                        // UID in einen String umwandeln (z. B. "04A3B2C1")
                        return BitConverter.ToString(cardData.NfcId).Replace("-", "");
                    }
                }
            }
            catch (Exception ex)
            {
                // In produktiven Anwendungen sollte hier ein Logging-Framework (z.B. Serilog, NLog, etc.) verwendet werden.
                Console.WriteLine($"Fehler: {ex.Message}");
            }

            return null;
        }
    }
}
