using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOrder
{
   public enum OrderType {
        PhysicalProduct,
        Book,
        MembershipActivation,
        MembershipUpgrade,
        Video
    }
   public enum OrderState {
        Pending,
        Processed,
        Failed,
        Deleted
    }
    enum MembershipType {
        Basic,
        Premium
    }
    public class OrderingSystem
    {
        static private List<Order> CurrentOrders {get;set;}
        static private List<Order> OrdersHistory { get; set; }
        static private List<Rule> BusinessRules;
        public List<Rule> GetCurrentRules() {
            return BusinessRules;
        }
        public List<Order> GetCurrentOrders()
        {
            return CurrentOrders;
        }
        public List<Order> GetOrdersHistory()
        {
            return OrdersHistory;
        }
        public int RulesCount() {
            return BusinessRules.Count();
        }
        public void Setup()
        {
            CurrentOrders = new List<Order>();
            OrdersHistory = new List<Order>();
            BusinessRules = new List<Rule>();
            //Create Rules as per Problem Statement / or Fetch Saved Rules from Database table
            Rule physicalProductRule = new Rule(OrderType.PhysicalProduct,true,false,false,false,false,false,false,true);
            Rule bookRule = new Rule(OrderType.Book, true, true, false, false, false, false, false, true);
            Rule membershipActivationRule = new Rule(OrderType.MembershipActivation, false, false, true, true, false, true, false, false);
            Rule membershipUpgradeRule = new Rule(OrderType.MembershipUpgrade, false, false, true, false, true, true, false, false);
            Rule videoRule = new Rule(OrderType.Video, true, false, false, false, false, false, true, false);
            //Add
            BusinessRules.Add(physicalProductRule);
            BusinessRules.Add(bookRule);
            BusinessRules.Add(membershipActivationRule);
            BusinessRules.Add(membershipUpgradeRule);
            BusinessRules.Add(videoRule);
        }

        //CRUD Functions
        public bool AddOrder(Order o) {
            try
            {
                //Assign Rule to order
                var r = (from rule in BusinessRules
                         where rule.orderType == o.orderType
                         select rule).FirstOrDefault();
                if (r!=null)
                {
                    o.rule = r;
                    CurrentOrders.Add(o);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
           
        }
        public bool RemoveOrder(int OrderIdentifier)
        {
            try
            {
                //Check current order list or fetch from database
                var order = (from o in CurrentOrders
                             where o.OrderId == OrderIdentifier
                             select o).FirstOrDefault();
                if (order!=null)
                {
                    CurrentOrders.Remove(order);
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool ProcessOrders() {
            try
            {
                var orders = (from order in CurrentOrders
                              where order.orderState == OrderState.Pending
                              select order).ToList();
                if (orders != null && orders.Count() > 0)
                {
                    foreach (var ord in orders)
                    {
                        //Steps to execute based on the Rule

                    }
                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception)
            {
                return false;
            }
        }
    }
    public class Order
    {
        [Required]
        public int OrderId { get; set; }
        [Required]
        private DateTime OrderDate { get; set; }
        [Required]
        public OrderType orderType { get; set; }
        [Required]
        public OrderState orderState { get; set; }
        [Required]
        public List<OrderItem> orderItems { get; set; }

        [Required]
        private Payment payment { get; set; }
        private double shippingRate { get; set; }
        private double taxRate { get; set; }

        public Rule rule { get; set; }
        public List<PackingSlip> packingSlips { get; set; }
        public Membership membership { get; set; }

        public Order(OrderType ot, double paymentAmount) {
            Random r = new Random();
            OrderId = r.Next(1, 99999);
            orderType = ot;
            OrderDate = DateTime.Now;
            orderState = OrderState.Pending;
            payment = new Payment(paymentAmount);
            taxRate = 0.05;        //Or fetch from database
            shippingRate = 50.00;  //Or fetch from database
        }
    }

   public class OrderItem {
        private int SrN { get; set; }
        private string Name { get; set; }
        private int Quantity { get; set; }
        private double Cost { get; set; }

    }
    
   public class Membership {
        [Required]
        private MembershipType membershipType{ get; set; }
        private bool IsMembershipActive { get; set; }
        private bool IsMembershipUpgraded { get; set; }

        Membership(MembershipType mt, bool activateMembership, bool upgradeMembership) {
            membershipType = mt;
            IsMembershipActive = activateMembership;
            IsMembershipUpgraded = upgradeMembership;
        }
    }
    class Payment {
        [Required]
        private double Amount { get; set; }
        private string Currency { get; set; }
        public Payment(double amount)
        {
            Amount = amount;
            Currency = "INR";   // or fetch from local culture / database
        }
    }
   public class PackingSlip {
        [Required]
        private int OrderRef { get; set; }
        [Required]
        private DateTime OrderDate { get; set; }
        private string FromAddress { get; set; }
        private string ToAddress { get; set; }
        public bool IsDuplicate { get; set; }
        public PackingSlip(int orderRef) {
            OrderDate = DateTime.Now;
            FromAddress = "Sarabpreet Singh, Retail Manager, Largest eRetailer Company, Punjab, India";
            ToAddress = "Sundar Sharma, GT Road Mahilpur, Jalandhar, Punjab India - +919288837238";
        }
    }
    public class Rule
    {
        [Required]
        public OrderType orderType { get; private set; }
        [Required]
        public bool IsPackingSlipApplicable { get; private set; }
        [Required]
        public bool IsDuplicatePackingSlipRequired { get; private set; }
        [Required]
        public bool IsMembershipApplicable { get; private set; }
        [Required]
        public bool IsMembershipActivation { get; private set; }
        [Required]
        public bool IsMembershipUpgradation { get; private set; }
        [Required]
        public bool IsMembershipNotificationRequired { get; private set; }
        [Required]
        public bool IsVideoFreebieApplicable { get; private set; }
        [Required]
        public bool IsCommissionApplicable { get; private set; }
        public Rule(OrderType ot, bool packingSlipNeeded, bool duplicatePackingSlipNeeded, bool membershipApplicable, bool membershipActivation, bool membershipUpgrade, bool membershipNotification, bool videoFreebieCheckRequired, bool agentcommission)
        {
                orderType = ot;
            IsPackingSlipApplicable = packingSlipNeeded;
            IsDuplicatePackingSlipRequired = duplicatePackingSlipNeeded;
            IsMembershipApplicable = membershipApplicable;
            IsMembershipActivation = membershipActivation;
            IsMembershipUpgradation = membershipUpgrade;
            IsMembershipNotificationRequired = membershipNotification;
            IsVideoFreebieApplicable = videoFreebieCheckRequired;
            IsCommissionApplicable = agentcommission;
        }
    }
}
