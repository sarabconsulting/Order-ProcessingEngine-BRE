using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eOrder;

namespace eOrder
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void OrderingSystemInitilization()
        {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            Assert.AreEqual(os.RulesCount(), 5);
        }
    }
}
