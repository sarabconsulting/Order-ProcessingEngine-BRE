using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using eOrder;
using System.Collections.Generic;
using System.Linq;

namespace eOrder
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void OrderingSystemInitilizationTest()
        {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            Assert.AreEqual(os.RulesCount(), 5);
        }

        [TestMethod]
        public void PhysicalProductRuleTest() {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            List<Rule> rules =  os.GetCurrentRules();
            var r = (from rule in rules
                     where rule.orderType == OrderType.PhysicalProduct
                     select rule).FirstOrDefault();
            Assert.AreEqual(r.IsPackingSlipApplicable, true);
            Assert.AreEqual(r.IsMembershipApplicable, false);
            Assert.AreEqual(r.IsCommissionApplicable, true);
            Assert.AreEqual(r.IsMembershipActivation, false);
            Assert.AreEqual(r.IsMembershipNotificationRequired, false);
            Assert.AreEqual(r.IsVideoFreebieApplicable, false);
        }
        //Add more tests for each order type


        [TestMethod]
        public void AddRemoveOrderTest() {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            Order neworder = new Order(OrderType.PhysicalProduct, 100.00);
            Assert.AreEqual(os.AddOrder(neworder), true);
            Assert.AreEqual(os.RemoveOrder(neworder.OrderId), true); 
        }


    }
}
