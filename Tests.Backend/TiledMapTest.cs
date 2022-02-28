using Avace.Backend.Map;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tests.Utils;

namespace Tests.Backend
{
    [TestClass]
    public class TiledMapTest
    {
        [TestMethod]
        public void ParseTiledMap()
        {
            TiledMap.FromPath(TestUtils.GetTestFile("map.tmx")).ShouldBeEquivalentToDumpFile(TestUtils.GetTestFile("map.json"));
        }
    }
}
