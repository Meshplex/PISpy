using PiSpyBackend.Application;
using Xunit.Abstractions;

namespace PiSpyBackend.Tests.Integration
{
    public class RfidServiceIntegrationTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public RfidServiceIntegrationTests(ITestOutputHelper testOutputHelper)
        {     
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Run_WhenCalled_ReturnsUid()
        {
            // Arrange
            var service = new RfidService();
            
            // Act
            var result = service.Run();
            
            // Assert
            _testOutputHelper.WriteLine((string)result);
            Assert.NotNull(result);
        }
    }
}