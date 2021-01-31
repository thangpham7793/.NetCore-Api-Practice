using System;
using Xunit;
using CommandAPI.Models;

namespace CommandAPI.Tests
{
    public class CommandTests : IDisposable
    {
        Command testCmd;

        // this is run before each test?
        public CommandTests()
        {
            testCmd = new Command
            {
                HowTo = "Run test",
                Platform = "Xunit",
                CLI = "dotnet test"
            };
        }
        [Fact]
        public void CanChangeHowTo()
        {
            testCmd.HowTo = "Run unit tests";
            Assert.Equal("Run unit tests", testCmd.HowTo);
        }

        [Fact]
        public void CanChangePlatform()
        {
            testCmd.Platform = "Linux";
            Assert.Equal("Linux", testCmd.Platform);
        }

        public void Dispose()
        {
            testCmd = null;
        }
    }


}