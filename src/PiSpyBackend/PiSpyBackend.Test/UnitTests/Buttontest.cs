using FluentAssertions;
using PiSpyBackend.Application;
using Xunit.Abstractions;

namespace PiSpyBackend.Test.UnitTests
{
    public class Buttontest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public Buttontest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestButton()
        {
            // Arrange
            var service = new ButtonService();
            
            // Act
            var result = (bool)service.Run();
            
            // Assert
            _testOutputHelper.WriteLine("Button Pressed: " + result);
            result.Should().BeTrue();
        }
    }
}