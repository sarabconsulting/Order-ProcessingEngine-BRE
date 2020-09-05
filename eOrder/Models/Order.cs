using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eOrder
{
    enum OrderType {
        PhysicalProduct,
        Book,
        MembershipActivation,
        MembershipUpgrade,
        Video
    }
    enum OrderState {
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
        static List<Order> CurrentOrders {get;set;}
        static List<Order> OrdersHistory { get; set; }
        static List<Rule> BusinessRules { get; set; }


        public int RulesCount() {
            return BusinessRules.Count();
        }


        public void Setup()
        {
            CurrentOrders = new List<Order>();
            OrdersHistory = new List<Order>();
            BusinessRules = new List<Rule>();
            //Create Rules as per Problem Statement / Fetch Rules from Database
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
    }

    class Order
    {
        [Required]
        private int OrderId { get; set; }
        [Required]
        private DateTime OrderDate { get; set; }
        [Required]
        private OrderType orderType { get; set; }
        [Required]
        private OrderState orderState { get; set; }
        [Required]
        private List<OrderItem> orderItems { get; set; }

        [Required]
        private Payment payment { get; set; }
        private Rule rule { get; set; }
        private PackingSlip packingSlip { get; set; }
        private Membership membership { get; set; }

        Order(OrderType ot, double paymentAmount) {
            orderType = ot;
            OrderDate = DateTime.Now;
            orderState = OrderState.Pending;




        }
    }

    class OrderItem {
        private int SrN { get; set; }
        private string Name { get; set; }
        private int Quantity { get; set; }
        private double Cost { get; set; }

    }
    
    class Membership {
        [Required]
        private MembershipType membershipType{ get; set; }
        public Membership(MembershipType mt) {
            membershipType = mt;
        }
    }
    class Payment {
        [Required]
        private double Amount { get; set; }
        private string Currency { get; set; }
        public Payment(double amount)
        {
            Amount = amount;
            Currency = "INR";
        }
    }
    class PackingSlip {

       public PackingSlip() {

        }
    }


    class Rule
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
