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
    public enum MembershipType {
        Basic,
        Premium
    }
    public class OrderingSystem
    {
        static private List<Order> CurrentOrders { get; set; }
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
            Rule physicalProductRule = new Rule(OrderType.PhysicalProduct, true, false, false, false, false, false, false, true);
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
                if (r != null)
                {
                    o.rule = r;
                    CurrentOrders.Add(o);
                    ProcessOrder();
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
                if (order != null)
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
        public bool ProcessOrder() {
            try
            {
                var orders = (from order in CurrentOrders
                              where order.orderState == OrderState.Pending
                              select order).ToList();
                if (orders != null && orders.Count() > 0)
                {
                    foreach (var ord in orders)
                    {
                        //Steps to execute based on Rules
                        //IsPackingSlipApplicable
                        if (ord.rule.IsPackingSlipApplicable)
                        {
                            PackingSlip ps = new PackingSlip(ord.OrderId, false);
                            ord.packingSlips.Add(ps);
                        }
                        //IsDuplicatePackingSlipRequired
                        if (ord.rule.IsDuplicatePackingSlipRequired)
                        {
                            PackingSlip psd = new PackingSlip(ord.OrderId, true);
                            ord.packingSlips.Add(psd);
                        }
                        //IsMembershipApplicable
                        if (ord.rule.IsMembershipApplicable)
                        {
                            Membership ms = new Membership(MembershipType.Basic, "MBR001");
                            ms.ActivateMembership("ABC", true);  //Talk to server for Encrypted Token / validation params
                            ord.membership = ms;
                        }
                        //IsMembershipActivation
                        if (ord.rule.IsMembershipActivation)
                        {
                            if (ord.membership != null)
                            {
                                ord.membership.ActivateMembership("ABC", true); //Talk to server for Encrypted Token / validation params
                            }
                            else
                            {
                                Membership ms = new Membership(MembershipType.Basic, "MBR001");
                                ms.ActivateMembership("ABC", true);  //Talk to server for Encrypted Token / validation params
                                ord.membership = ms;
                            }
                        }
                        //IsMembershipUpgradation
                        if (ord.rule.IsMembershipUpgradation)
                        {
                            if (ord.membership != null)
                            {
                                ord.membership.ActivateMembership("ABC", true); //Talk to server for Encrypted Token / validation params
                                ord.membership.UpgradeMembership("DEF", true); //Talk to server for Encrypted Token / validation params
                            }
                            else
                            {
                                Membership ms = new Membership(MembershipType.Premium, "MBR001");
                                ord.membership.ActivateMembership("ABC", true); //Talk to server for Encrypted Token / validation params
                                ord.membership.UpgradeMembership("DEF", true); //Talk to server for Encrypted Token / validation params
                            }
                        }
                        //IsMembershipNotificationRequired
                        if (ord.rule.IsMembershipNotificationRequired)
                        {
                            if (!ord.membership.EmailNotificationSent)
                            {
                                ord.membership.ProcessEmailNotificaiton(ord.membership.membershipType);
                            }
                            else {
                                //Log email notification  sending error / raise internal event
                            }
                        }
                        //IsVideoFreebieApplicable
                        if (ord.rule.IsVideoFreebieApplicable)
                        {
                            //Implement Court Decision in 1997 on Learning to Ski Video
                            bool freebieflag = false;
                            foreach (var item in ord.orderItems)
                            {
                                if (item.Name =="Learning to Ski")
                                {
                                    freebieflag = true;
                                }
                            }
                            if (freebieflag)
                            {
                                ord.orderItems.Add(new OrderItem(2, "First Aid", 1, 0));
                            }
                        }
                        //IsCommissionApplicable
                        if (ord.rule.IsCommissionApplicable)
                        {
                            ord.AgentComission = 10;
                        }

                        ord.orderState = OrderState.Processed;

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
        public DateTime OrderDate { get; set; }
        [Required]
        public OrderType orderType { get; set; }
        [Required]
        public OrderState orderState { get; set; }
        [Required]
        public List<OrderItem> orderItems { get; set; }

        [Required]
        public Payment payment { get; set; }
        public double shippingRate { get; set; }
        public double taxRate { get; set; }
        public double AgentComission { get; set; }

        public Rule rule { get; set; }
        public List<PackingSlip> packingSlips { get; set; }
        public Membership membership { get; set; }

        public Order(OrderType ot, List<OrderItem> orderItemsLists, double paymentAmount) {
            Random r = new Random();
            OrderId = r.Next(1, 99999);
            orderType = ot;
            orderItems = orderItemsLists;
            OrderDate = DateTime.Now;
            orderState = OrderState.Pending;
            payment = new Payment(paymentAmount);
            taxRate = 5.00;        //Or fetch from database
            shippingRate = 50.00;  //Or fetch from database
            packingSlips = new List<PackingSlip>();
        }
    }
    public class OrderItem {
        private int SrN { get; set; }
        public string Name { get; set; }
        private int Quantity { get; set; }
        private double Cost { get; set; }
        public OrderItem(int srn, string name, int qty, double cost) {
            SrN = srn;
            Name = name;
            Quantity = qty;
            Cost = cost;
        }
    }
   

    public class Membership {
        [Required]
        public MembershipType membershipType { get; set; }
        public string MembershipId { get; set; }
        public bool IsMembershipActive { get; set; }
        public bool IsMembershipPremium { get; set; }
        public string EmailNotificationMessage { get; private set; }
        public bool EmailNotificationSent { get; private set; }

        public bool ActivateMembership(string EncryptedAuthToken, bool ApprovedForActivation)
        {
            try
            {
                if (EncryptedAuthToken == "ABC" && ApprovedForActivation)  //For serverside token validation ()
                {
                    IsMembershipActive = ApprovedForActivation;
                    ProcessEmailNotificaiton(MembershipType.Basic);
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
        public bool UpgradeMembership(string EncryptedAuthToken, bool ApprovedForUpgrade)
        {
            try
            {
                if (EncryptedAuthToken == "DEF" && ApprovedForUpgrade)  //For server-side token validation ()
                {
                    ActivateMembership("ABC", true); //Just in-case if it is inactive
                    IsMembershipPremium = ApprovedForUpgrade;
                    ProcessEmailNotificaiton(MembershipType.Premium);
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
        public void ProcessEmailNotificaiton(MembershipType membershipType)
        {
            switch (membershipType)
            {
                case MembershipType.Basic:
                    EmailNotificationMessage = @"Dear Sir/Madam,
                                               You will be happy to know that your membership of " + membershipType.ToString() + " plan has been            activated on date " + DateTime.Now.ToString() +@". We hope you will truly enjoy our product and services. 
                                                Best Wishes,
                                                Customer Support
                                                Largest eRetailer Company";

                    break;
                case MembershipType.Premium:
                    EmailNotificationMessage = @"Dear Sir/Madam,
                                               You will be happy to know that your Basic membership upgrade request to " + membershipType.ToString() + " plan has been processed on date " +      DateTime.Now.ToString() + @". We hope you will truly enjoy our product and services. 
                                                Best Wishes,
                                                Customer Support
                                                Largest eRetailer Company";
                    break;
            }

            //Get email address of the Member from server + Send the message to Notification Service
            EmailNotificationSent = SendEmailbyExternalService("member@emailprovider.com", EmailNotificationMessage);
        }

        public bool SendEmailbyExternalService(string emailAddress, string Message)
        {
            try
            {
                //Call external emailer service
                //SendHTTPEMail(emailAddress,Message);
                return true;  //Assuming all went fine.
            }
            catch (Exception)
            {
                return false;
            }
        }

        public Membership(MembershipType mt, string membershipId) {
            membershipType = mt;
            MembershipId = membershipId;
            //Restore membership status from server
            IsMembershipActive = false;
            IsMembershipPremium = false;
        }
    }
    public class Payment {
        [Required]
        public double Amount { get; set; }
        public string Currency { get; set; }
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
        public PackingSlip(int orderRef, bool isDuplicate) {
            OrderRef = orderRef;
            OrderDate = DateTime.Now;
            FromAddress = "Sarabpreet Singh, Retail Manager, Largest eRetailer Company, Punjab, India";
            ToAddress = "Sundar Sharma, GT Road Mahilpur, Jalandhar, Punjab India - +919288837238";
            IsDuplicate = isDuplicate;
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
