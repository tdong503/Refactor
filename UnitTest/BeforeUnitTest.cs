using Kata.Refactor.Before;
using Xunit;

namespace UnitTest
{
    public class UnitTest
    {
        [Fact]
        public void Test()
        {
            new KeysFilter().Filter()
            Assert.True(true);
        }
    }
}