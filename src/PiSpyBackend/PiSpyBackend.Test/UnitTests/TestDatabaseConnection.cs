using FluentAssertions;
using PiSpyBackend.Infrastructure;

namespace PiSpyBackend.Test.UnitTests
{
    public class TestDatabaseConnection
    {
        [Fact]
        public void TestConn()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("TESTTEST TEST TEST TEST");
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            var appCon = new AppDbContext();
            if (appCon.Database.CanConnect())
            {
                Console.Write("Datenbank hat sich verbunden");
            }
        }
    }
}