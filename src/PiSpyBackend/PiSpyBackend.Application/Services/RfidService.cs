using System.Device.Gpio;
using System.Device.Spi;
using System.Numerics;
using Iot.Device.Mfrc522;
using Iot.Device.Rfid;
using PiSpyBackend.Domain.Interfaces;

namespace PiSpyBackend.Application
{
    public class RfidService : ISensorService
    {
        private const int SpiBusId = 0;
        private const int ChipSelectLine = 0;
        private const int ResetPin = 25;

        public object Run()
        {
            try
            {
                using var gpio = new GpioController(PinNumberingScheme.Logical);
                var spiSettings = new SpiConnectionSettings(SpiBusId, ChipSelectLine)
                {
                    ClockFrequency = 10_000_000,
                    Mode = SpiMode.Mode0
                };
                using var spi = SpiDevice.Create(spiSettings);
                using var reader = new MfRc522(spi, ResetPin, gpio, false);
                reader.Gain = Gain.G38dB;
                reader.Enabled = true;
                var atqa = new byte[2];
                if (reader.IsCardPresent(atqa, false))
                {
                    if (reader.ListenToCardIso14443TypeA(out Data106kbpsTypeA cardData, TimeSpan.FromSeconds(2)))
                    {
                        byte[] uidBytes = cardData.NfcId;
                        Array.Reverse(uidBytes);
                        BigInteger uidDecimal = new BigInteger(uidBytes, isUnsigned: true, isBigEndian: true);
                        return uidDecimal.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler: {ex.Message}");
            }

            return new BigInteger(0);
        }
    }
}
