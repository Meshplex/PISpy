using FluentAssertions;
using PiSpyBackend.Application;
using Xunit.Abstractions;

namespace PiSpyBackend.Test.UnitTests
{
    public class MotionTest
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public MotionTest(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestMotion()
        {
            // Arrange
            var service = new MotionService();
            
            // Act
            var result = (bool)service.Run();
            
            // Assert
            _testOutputHelper.WriteLine("Motion detected: " + result);
            result.Should().BeTrue();
        }
    }
}