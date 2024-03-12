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
        public void ChangingDatabaseTests()
        {
            Assert.True(false);
        }

        [Test]
        public void NotChangingDatabaseTests()
        {
            Assert.True(true);
        }
    }
}