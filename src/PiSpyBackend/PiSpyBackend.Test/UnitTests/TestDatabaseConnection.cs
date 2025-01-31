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
            appCon.Users.Add(new User {Username = "test", Password = "test"});
            appCon.SaveChanges();
            if (appCon.Database.CanConnect())
            {
                true.Should().BeTrue();
            }
        }
    }
}