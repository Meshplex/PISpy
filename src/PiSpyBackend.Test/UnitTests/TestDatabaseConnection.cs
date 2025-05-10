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
                User user = new()
                {
                    Username = "tercan",
                    Password = BCrypt.Net.BCrypt.HashPassword("123", workFactor: 12)
                };
                appCon.Users.Add(user);
                appCon.SaveChanges();
                appCon.Users.Should().NotBeNullOrEmpty();
                appCon.Events.Should().NotBeNullOrEmpty();
                true.Should().BeTrue();
            }
            false.Should().BeFalse();
        }
    }
}