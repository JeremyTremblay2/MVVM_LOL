using System;
using HelloWorldLib;
using Xunit;

namespace HelloWordLib_UnitTests
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            Class1 c = new Class1();
            Assert.NotNull(c);
        }
    }
}
