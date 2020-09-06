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
        [TestMethod]
        public void BookRuleTest()
        {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            List<Rule> rules = os.GetCurrentRules();
            var r = (from rule in rules
                     where rule.orderType == OrderType.Book
                     select rule).FirstOrDefault();
            Assert.AreEqual(r.IsPackingSlipApplicable, true);
            Assert.AreEqual(r.IsMembershipApplicable, false);
            Assert.AreEqual(r.IsCommissionApplicable, true);
            Assert.AreEqual(r.IsMembershipActivation, false);
            Assert.AreEqual(r.IsMembershipNotificationRequired, false);
            Assert.AreEqual(r.IsVideoFreebieApplicable, false);
        }
        [TestMethod]
        public void MembershipActivationRuleTest()
        {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            List<Rule> rules = os.GetCurrentRules();
            var r = (from rule in rules
                     where rule.orderType == OrderType.MembershipActivation
                     select rule).FirstOrDefault();
            Assert.AreEqual(r.IsPackingSlipApplicable, false);
            Assert.AreEqual(r.IsMembershipApplicable, true);
            Assert.AreEqual(r.IsCommissionApplicable, false);
            Assert.AreEqual(r.IsMembershipActivation, true);
            Assert.AreEqual(r.IsMembershipUpgradation, false);
            Assert.AreEqual(r.IsMembershipNotificationRequired, true);
            Assert.AreEqual(r.IsVideoFreebieApplicable, false);
        }
        [TestMethod]
        public void MembershipUpgradeRuleTest()
        {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            List<Rule> rules = os.GetCurrentRules();
            var r = (from rule in rules
                     where rule.orderType == OrderType.MembershipUpgrade
                     select rule).FirstOrDefault();
            Assert.AreEqual(r.IsPackingSlipApplicable, false);
            Assert.AreEqual(r.IsMembershipApplicable, true);
            Assert.AreEqual(r.IsCommissionApplicable, false);
            Assert.AreEqual(r.IsMembershipActivation, false);
            Assert.AreEqual(r.IsMembershipUpgradation, true);
            Assert.AreEqual(r.IsMembershipNotificationRequired, true);
            Assert.AreEqual(r.IsVideoFreebieApplicable, false);
        }
        [TestMethod]
        public void VideoRuleTest()
        {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            List<Rule> rules = os.GetCurrentRules();
            var r = (from rule in rules
                     where rule.orderType == OrderType.Video
                     select rule).FirstOrDefault();
            Assert.AreEqual(r.IsPackingSlipApplicable, true);
            Assert.AreEqual(r.IsMembershipApplicable, false);
            Assert.AreEqual(r.IsCommissionApplicable, true);
            Assert.AreEqual(r.IsMembershipActivation, false);
            Assert.AreEqual(r.IsMembershipNotificationRequired, false);
            Assert.AreEqual(r.IsVideoFreebieApplicable, true);
        }

        [TestMethod]
        public void AddRemoveOrderTest() {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            List<OrderItem> itemList = new List<OrderItem>();
            itemList.Add(new OrderItem(1, "Membership Activation", 1, 100));
            Order neworder = new Order(OrderType.MembershipActivation, itemList, 100.00);
            Assert.AreEqual(os.AddOrder(neworder), true);
            Assert.AreEqual(os.RemoveOrder(neworder.OrderId), true); 
        }

        [TestMethod]
        public void AgentCommission() {
            OrderingSystem os = new OrderingSystem();
            os.Setup();
            List<OrderItem> pitemList = new List<OrderItem>();

            foreach (OrderType ot in (OrderType[])Enum.GetValues(typeof(OrderType)))
            {
                switch (ot)
                {
                    case OrderType.PhysicalProduct:
                        pitemList.Add(new OrderItem(1, "Physical Product", 1, 100));
                        Order pOrder = new Order(OrderType.PhysicalProduct, pitemList, 100.00);
                        Assert.AreEqual(os.AddOrder(pOrder), true);
                        Assert.AreEqual(pOrder.rule.IsCommissionApplicable, true);
                        break;
                    case OrderType.Book:
                        pitemList.Add(new OrderItem(1, "Great Book", 1, 100));
                        Order bOrder = new Order(OrderType.PhysicalProduct, pitemList, 100.00);
                        Assert.AreEqual(os.AddOrder(bOrder), true);
                        Assert.AreEqual(bOrder.rule.IsCommissionApplicable, true);
                        break;
                    case OrderType.MembershipActivation:
                        pitemList.Add(new OrderItem(1, "Membership Activation", 1, 100));
                        Order maOrder = new Order(OrderType.MembershipActivation, pitemList, 100.00);
                        Assert.AreEqual(os.AddOrder(maOrder), true);
                        Assert.AreEqual(maOrder.rule.IsCommissionApplicable, false);
                        break;
                    case OrderType.MembershipUpgrade:
                        pitemList.Add(new OrderItem(1, "Membership Upgrade", 1, 100));
                        Order muOrder = new Order(OrderType.MembershipUpgrade, pitemList, 100.00);
                        Assert.AreEqual(os.AddOrder(muOrder), true);
                        Assert.AreEqual(muOrder.rule.IsCommissionApplicable, false);
                        break;
                    case OrderType.Video:
                        pitemList.Add(new OrderItem(1, "Life on the rocks!", 1, 100));
                        Order vOrder = new Order(OrderType.Video, pitemList, 100.00);
                        Assert.AreEqual(os.AddOrder(vOrder), true);
                        Assert.AreEqual(vOrder.rule.IsCommissionApplicable, true);
                        break;
                }
            }
        }

    }
}
