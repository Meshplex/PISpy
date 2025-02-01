using Xunit;
using PiSpyBackend.Application;
using PiSpyBackend.Domain.Interfaces;
using System.Device.Gpio;
using System.Device.Spi;

namespace PiSpyBackend.Tests.Integration
{
    public class RfidServiceIntegrationTests : IDisposable
    {
        private readonly RfidService _rfidService;
        private readonly GpioController _gpioController;
        private readonly SpiDevice _spiDevice;

        public RfidServiceIntegrationTests()
        {
            // Hardware-Initialisierung (nur auf einem Raspberry Pi mit SPI/GPIO-Zugriff möglich)
            _gpioController = new GpioController(PinNumberingScheme.Logical);
            var spiSettings = new SpiConnectionSettings(0, 0)
            {
                ClockFrequency = 10_000_000,
                Mode = SpiMode.Mode0
            };
            _spiDevice = SpiDevice.Create(spiSettings);
            
            // Service mit echter Hardware erstellen
            _rfidService = new RfidService();
        }

        [Fact]
        public void Run_WhenCardPresent_ReturnsValidUid()
        {
            // Arrange
            // Halte eine RFID-Karte vor das Lesegerät, bevor der Test läuft
            
            // Act
            var result = _rfidService.Run();
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<string>(result);
            Assert.Matches("^[0-9A-F]{8}$", (string)result); // Beispiel: "04A3B2C1"
        }

        [Fact]
        public void Run_WhenNoCardPresent_ReturnsNull()
        {
            // Arrange
            // Stelle sicher, dass keine Karte am Lesegerät liegt
            
            // Act
            var result = _rfidService.Run();
            
            // Assert
            Assert.Null(result);
        }

        public void Dispose()
        {
            _spiDevice?.Dispose();
            _gpioController?.Dispose();
        }
    }
}