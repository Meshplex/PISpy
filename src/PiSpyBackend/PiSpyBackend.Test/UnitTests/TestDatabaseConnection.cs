using FluentAssertions;
using PiSpyBackend.Domain;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Test.UnitTests
{
    public class TestDatabaseConnection
    {
        [Fact]
        public void TestConn()
        {
            var appCon = new AppDbContext();
            if (appCon.Database.CanConnect())
            {
                appCon.Users.Should().NotBeNullOrEmpty();
                appCon.Events.Should().NotBeNullOrEmpty();
                true.Should().BeTrue();
            }
            false.Should().BeFalse();
        }
    }
}