using PiSpyBackend.Application;

namespace PiSpyBackend.Tests.Integration
{
    public class RfidServiceIntegrationTests
    {
        [Fact]
        public void Run_WhenCalled_ReturnsUid()
        {
            // Arrange
            var service = new RfidService();
            
            // Act
            var result = service.Run();
            
            // Assert
            Console.WriteLine(result);
            Assert.NotNull(result);
        }
    }
}