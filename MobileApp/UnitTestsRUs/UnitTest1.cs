using FluentAssertions;
using NSubstitute;
using System.Runtime.CompilerServices;
using TicketClassLib.Services;

namespace UnitTestsRUs
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public async Task ChangingDatabaseTests()
        {
            Assert.True(true);
        }

        [Test]
        public async Task NotChangingDatabaseTests()
        {
            Assert.True(true);
        }
    }
}