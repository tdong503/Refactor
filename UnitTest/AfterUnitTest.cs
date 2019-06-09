using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Kata.Refactor.After;
using Xunit;
using Moq;

namespace UnitTest
{
    public class AfterUnitTest
    {
        private Mock<ISessionService> _sessionSerivce = new Mock<ISessionService>();
        
        private List<string> _goldenKeys = new List<string>()
        {
            "GD01ABC001",
            "GD02ABC002",
            "GD01ABC002",
            "GD02ABC004",
            "GD01ABC005",
            "GD03ABC006",
        };
        
        private List<string> _silverKeys = new List<string>()
        {
            "SV01ABC001",
            "SV01ABC002",
            "SV02ABC003",
            "SV02ABC004",
            "SV03ABC005",
        };
        
        private List<string> _copperKeys = new List<string>()
        {
            "CP02ABC001",
            "CP03ABC002",
            "CP01ABC003",
            "CP02ABC004",
            "CP04ABC005",
        };
        
        [Fact]
        public void TestGoldenKeys()
        {
            _sessionSerivce.Setup(x => x.Get<List<string>>("GoldenKey")).Returns(_goldenKeys);
            var keysFilter = new KeysFilter(_sessionSerivce.Object);

            var testData = new List<string>()
            {
                "GD01ABC001",
                "GD02ABC002",
                "GD01ABC002",
                "GD02ABC004",
                "GD03ABC005",
                "GD03ABC006",
                "GD03ABC005FAKE"
            };

            var actual = keysFilter.FilterGoldenKeys(testData);
            Assert.Equal(5, actual.Count);
            Assert.Equal("GD01ABC001", actual.First());
            Assert.Equal("GD02ABC002", actual[1]);
            Assert.Equal("GD01ABC002", actual[2]);
            Assert.Equal("GD03ABC005FAKE", actual.Last());
        }
        
        [Fact]
        public void TestNonGoldenKeys()
        {
            _sessionSerivce.Setup(x => x.Get<List<string>>("SilverKey")).Returns(_silverKeys);
            _sessionSerivce.Setup(x => x.Get<List<string>>("CopperKey")).Returns(_copperKeys);
            var keysFilter = new KeysFilter(_sessionSerivce.Object);

            var testData = new List<string>()
            {
                "SV01ABC001",
                "SV01ABC002",
                "SV04ABC005",
                "SV03ABC005FAKE",
                "CP02ABC001",
                "CP03ABC002",
                "CP01ABC006",
            };

            var actual = keysFilter.FilterNonGoldenKeys(testData);
            Assert.Equal(5, actual.Count);
            Assert.Equal("SV01ABC001", actual.First());
            Assert.Equal("SV01ABC002", actual[1]);
            Assert.Equal("SV03ABC005FAKE", actual[2]);
        }
    }
}