using Mal.Net;
using Xunit.Abstractions;

namespace Mal.Net.Testing
{
    public class UserTests
    {
        public UserTests(ITestOutputHelper output)
        {
            Console.SetOut(new LogConverter(output));
        }
        
        [Fact]
        public void LogTest()
        {
            MalClient.Log("Hello World");

            Assert.True(true);
        }
    }
}