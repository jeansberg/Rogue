using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rogue;
using System;

namespace MapGeneratorTests
{
    [TestClass]
    public class DevelopmentTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var generator = new MapGenerator();

            var map = generator.GenerateMap();

            map.SetCellProperties(10, 10, true, true);

            Console.WriteLine(map.ToString());
        }
    }
}
