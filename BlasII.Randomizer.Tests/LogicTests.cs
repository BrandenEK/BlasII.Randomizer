using BlasII.Randomizer.Items;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BlasII.Randomizer.Tests
{
    [TestClass]
    public class LogicTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var inventory = new Blas2Inventory();

            Assert.IsNotNull(inventory);
        }
    }
}