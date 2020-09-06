using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace eOrder
{
    public partial class Default : System.Web.UI.Page
    {
        private static OrderingSystem OSys;
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    PageSetup();
                }
            }
            catch (Exception)
            {
                SucessNotify.Visible = false;
                FailureNotify.Visible = true;
                ValidationSummaryDiv.Visible = false;
            }
        }
        private void PageSetup()
        {
            try
            {

                Reset();
                SucessNotify.Visible = false;
                FailureNotify.Visible = false;
                ValidationSummaryDiv.Visible = false;
            }
            catch
            {
                SucessNotify.Visible = false;
                FailureNotify.Visible = true;
                ValidationSummaryDiv.Visible = false;
            }
        }
        private void Reset()
        {
            Authorized.Visible = false;
            VideoSelection.Visible = false;
            MembershipTypeSelection.Visible = false;
            OrderValidator.Visible = false;
            packingPanel.Visible = false;
            PackingSlipMain.Visible = false;
            duplicatePackingSlip.Visible = false;
            NotificationPanel.Visible = false;
            NotifyActivationPanel.Visible = false;
            NotifyUpgradePanel.Visible = false;
            OSys = new OrderingSystem();
            OSys.Setup();

        }

        protected void NotificationButton_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "" && e.CommandArgument != "")
                {
                    switch (e.CommandName)
                    {
                        case "Cancel":
                            if (e.CommandArgument == "FailureDiv")
                            {
                                FailureNotify.Visible = false;
                            }
                            else if (e.CommandArgument == "SucessDiv")
                            {
                                SucessNotify.Visible = false;
                            }
                            else
                            {
                                FailureNotify.Visible = false;
                                SucessNotify.Visible = false;
                            }
                            break;
                    }
                }
                else
                {
                    FailureNotify.Visible = true;
                }
            }
            catch (Exception)
            {
                FailureNotify.Visible = true;
            }
        }
        protected void SaveButton_Command(object sender, CommandEventArgs e)
        {
            try
            {
                if (e.CommandName != "")
                {

                    switch (e.CommandName)
                    {
                        case "Save":
                            if (Session["AuthUser"].ToString() != null)
                            {
                                //Validations
                                string Validation = "<h4>Correct the errors below</h4> <br>";
                                bool IsValid = true;

                                //Give input back or Save
                                if (IsValid)
                                {


                                }
                                else
                                {
                                    ValidationMessage.Text = Validation;
                                    ValidationSummaryDiv.Visible = true;
                                }
                            }
                            else
                            {
                                FailureNotify.Visible = true;
                                OrderDiv.Visible = true;
                                Authorized.Visible = false;
                                ValidationSummaryDiv.Visible = false;
                            }
                            break;
                        case "Edit":
                            int recId = Convert.ToInt32(e.CommandArgument);

                            break;
                    }
                }
            }
            catch (Exception)
            {

                FailureNotify.Visible = true;
                SucessNotify.Visible = false;
            }
        }
        protected void OrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string o = OrderType.SelectedValue;
                if (o == "M")
                {
                    Reset();
                    MembershipTypeSelection.Visible = true;
                }
                else if (o == "V")
                {
                    Reset();
                    VideoSelection.Visible = true;
                }
                else
                {
                    Reset();
                }
            }
            catch (Exception)
            {
                FailureNotify.Visible = true;
                SucessNotify.Visible = false;
            }
        }
        protected void OrderButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateOrder())  //Validate Order
                {
                    List<OrderItem> itemList = new List<OrderItem>();
                    switch (OrderType.SelectedValue)
                    {
                        case "P":

                            itemList.Add(new OrderItem(1, "A Great Solid Product", 1, Convert.ToDouble(PaymentAmount.Text)));
                            Order newProductorder = new Order(eOrder.OrderType.PhysicalProduct, itemList, Convert.ToDouble(PaymentAmount.Text));
                            if (OSys.AddOrder(newProductorder))
                            {
                                ShowOrderPage(newProductorder);
                            }
                            else
                            {
                                FailureNotify.Visible = true;
                                SucessNotify.Visible = false;
                            }
                            break;
                        case "B":
                            itemList.Add(new OrderItem(1, "A Great Book", 1, Convert.ToDouble(PaymentAmount.Text)));
                            Order newBookorder = new Order(eOrder.OrderType.Book, itemList, Convert.ToDouble(PaymentAmount.Text));
                            if (OSys.AddOrder(newBookorder))
                            {
                                ShowOrderPage(newBookorder);
                            }
                            else
                            {
                                FailureNotify.Visible = true;
                                SucessNotify.Visible = false;
                            }
                            break;
                        case "M":
                            //Check if Activation or Upgrade
                            if (ActivationOption.Checked)
                            {
                                itemList.Add(new OrderItem(1, "Membership Activation", 1, Convert.ToDouble(PaymentAmount.Text)));
                                Order newMembershiporder = new Order(eOrder.OrderType.MembershipActivation, itemList, Convert.ToDouble(PaymentAmount.Text));
                                if (OSys.AddOrder(newMembershiporder))
                                {
                                    ShowOrderPage(newMembershiporder);
                                }
                                else
                                {
                                    FailureNotify.Visible = true;
                                    SucessNotify.Visible = false;
                                }
                            }
                            else if (UpgradeOption.Checked)
                            {
                                itemList.Add(new OrderItem(1, "Membership Upgrade", 1, Convert.ToDouble(PaymentAmount.Text)));
                                Order newMembershiporder = new Order(eOrder.OrderType.MembershipUpgrade, itemList, Convert.ToDouble(PaymentAmount.Text));
                                if (OSys.AddOrder(newMembershiporder))
                                {
                                    ShowOrderPage(newMembershiporder);
                                }
                                else
                                {
                                    FailureNotify.Visible = true;
                                    SucessNotify.Visible = false;
                                }
                            }
                            break;
                        case "V":
                            itemList.Add(new OrderItem(1, VideoLibraryDropdown.SelectedItem.ToString(), 1, Convert.ToDouble(PaymentAmount.Text)));
                            Order newVideoorder = new Order(eOrder.OrderType.Video, itemList, Convert.ToDouble(PaymentAmount.Text));
                            if (OSys.AddOrder(newVideoorder))
                            {
                                ShowOrderPage(newVideoorder);
                            }
                            else
                            {
                                FailureNotify.Visible = true;
                                SucessNotify.Visible = false;
                            }
                            break;
                        default:
                            FailureNotify.Visible = true;
                            SucessNotify.Visible = false;
                            break;
                    }

                }
                else
                {
                    FailureNotify.Visible = true;
                    SucessNotify.Visible = false;
                }
            }
            catch (Exception)
            {
                FailureNotify.Visible = true;
                SucessNotify.Visible = false;
            }
        }
        private bool ValidateOrder()
        {
            try
            {
                //Add backend validation checks or fetch from server
                if (OrderType.SelectedValue == "P" || OrderType.SelectedValue == "B" ||
                    OrderType.SelectedValue == "M" || OrderType.SelectedValue == "V")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                FailureNotify.Visible = true;
                SucessNotify.Visible = false;
                return false;
            }
        }
        protected void ResetButton_Click(object sender, EventArgs e)
        {
            Reset();
            OrderDiv.Visible = true;
            Authorized.Visible = false;
            OrderType.SelectedIndex = 0;
        }
        private void ShowOrderPage(Order ord) {
            try
            {
                //Handle Order Details Panel
                List<OrderDetail> OrderDetails = new List<OrderDetail>();
                OrderDetails.Add(new OrderDetail() { parameter = "Order ID", value = ord.OrderId.ToString() });
                OrderDetails.Add(new OrderDetail() { parameter = "Order Date", value = ord.OrderDate.ToString() });
                OrderDetails.Add(new OrderDetail() { parameter = "Order Type", value = ord.orderType.ToString() });
                
                OrderDetails.Add(new OrderDetail() { parameter = "Current Status", value = ord.orderState.ToString() });
                OrderDetails.Add(new OrderDetail() { parameter = "Total Items", value = ord.orderItems.Count().ToString() });
                switch (ord.orderType)
                {
                    case eOrder.OrderType.PhysicalProduct:
                        OrderDetails.Add(new OrderDetail() { parameter = "Product Name", value = ord.orderItems[0].Name.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Shipping Cost", value = ord.shippingRate.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Agent Commission", value = ord.AgentComission.ToString() + " INR" });
                        break;
                    case eOrder.OrderType.Book:
                        OrderDetails.Add(new OrderDetail() { parameter = "Book Name", value = ord.orderItems[0].Name.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Shipping Cost", value = ord.shippingRate.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Agent Commission", value = ord.AgentComission.ToString() + " INR" });
                        break;
                    case eOrder.OrderType.MembershipActivation:
                        OrderDetails.Add(new OrderDetail() { parameter = "Member ID (if applicable)", value = ord.membership.MembershipId.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Membership Active", value = ord.membership.IsMembershipActive.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Premium Member", value = ord.membership.IsMembershipPremium.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Membership Status", value = "Membership Activated on " + DateTime.Now.ToString() });
                        break;
                    case eOrder.OrderType.MembershipUpgrade:
                        OrderDetails.Add(new OrderDetail() { parameter = "Member ID (if applicable)", value = ord.membership.MembershipId.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Membership Active", value = ord.membership.IsMembershipActive.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Premium Member", value = ord.membership.IsMembershipPremium.ToString() });
                        OrderDetails.Add(new OrderDetail() { parameter = "Membership Status", value = "Upgraded to Premium on " + DateTime.Now.ToString() });
                        break;
                    case eOrder.OrderType.Video:
                        OrderDetails.Add(new OrderDetail() { parameter = "Video Name", value = ord.orderItems[0].Name.ToString() });
                        if (ord.rule.IsVideoFreebieApplicable && ord.orderItems[0].Name == "Learning to Ski")
                        {
                            OrderDetails.Add(new OrderDetail() { parameter = "Special Offer Freebie", value = ord.orderItems[1].Name.ToString() });
                        }
                        OrderDetails.Add(new OrderDetail() { parameter = "Agent Commission", value = ord.AgentComission.ToString() + " INR" });
                        break;
                }
                OrderDetails.Add(new OrderDetail()
                {
                    parameter = "Payment",
                    value = ord.payment.Amount.ToString()
                                                    + " " + ord.payment.Currency.ToString()
                });
                OrderDetails.Add(new OrderDetail() { parameter = "Tax", value = ord.taxRate.ToString() + " INR" });
                OrderDetailRepeater.DataSource = OrderDetails;
                OrderDetailRepeater.DataBind();

                //Handle the Packing Slip Panel

                switch (ord.orderType)
                {
                    case eOrder.OrderType.PhysicalProduct:
                        packingPanel.Visible = true;
                        PackingSlipMain.Visible = true;
                        break;
                    case eOrder.OrderType.Book:
                        packingPanel.Visible = true;
                        PackingSlipMain.Visible = true;
                        duplicatePackingSlip.Visible = true;
                        break;
                    case eOrder.OrderType.MembershipActivation:
                        packingPanel.Visible = false;
                        break;
                    case eOrder.OrderType.MembershipUpgrade:
                        packingPanel.Visible = false;
                        break;
                    case eOrder.OrderType.Video:
                        packingPanel.Visible = true;
                        PackingSlipMain.Visible = true;
                        break;
                    default:
                        packingPanel.Visible = false;
                        PackingSlipMain.Visible = false;
                        duplicatePackingSlip.Visible =false;
                        break;
                }

                if (ord.rule.IsPackingSlipApplicable)
                {
                    //Packing Slip Details
                    orderRefLabel.Text = ord.OrderId.ToString();
                    orderRef1Label.Text = orderRefLabel.Text;
                    orderDateLabel.Text = ord.OrderDate.ToShortDateString() + " " + ord.OrderDate.ToShortTimeString();
                    orderDate1Label.Text = orderDateLabel.Text;
                    shippingFromLabel.Text = ord.packingSlips[0].FromAddress;
                    shippingFrom1Label.Text = shippingFromLabel.Text;
                    shippingToLabel.Text = ord.packingSlips[0].ToAddress;
                    shippingTo1Label.Text = shippingToLabel.Text;
                    subTotalLabel.Text = ord.orderItems.Sum(x => x.Cost).ToString();
                    subTotal1Label.Text = subTotalLabel.Text;
                    taxLabel.Text = ord.taxRate.ToString();
                    tax1Label.Text = taxLabel.Text;
                    shippingLabel.Text = ord.shippingRate.ToString();
                    shipping1Label.Text = shippingLabel.Text;

                    //Fill Items
                    List<itemDetail> itemlist = new List<itemDetail>();
                    foreach (var item in ord.orderItems)
                    {
                        itemDetail i = new itemDetail();
                        i.Category = ord.orderType.ToString();
                        i.Item = item.Name;
                        i.Quantity = item.Quantity.ToString();
                        i.Cost = item.Cost.ToString();
                        itemlist.Add(i);
                    }

                    itemsRepeater.DataSource = itemlist;
                    itemsRepeater.DataBind();
                    ItemsRepeater2.DataSource = itemlist;
                    ItemsRepeater2.DataBind();

                    GrandTotalLabel.Text = (ord.orderItems.Sum(x => x.Cost) + ord.taxRate + ord.shippingRate).ToString() + " " + ord.payment.Currency.ToString();
                    GrandTotal1Label.Text = GrandTotalLabel.Text;
                }

                //Handle the Notification Panel
                switch (ord.orderType)
                {
                    case eOrder.OrderType.MembershipActivation:
                        NotificationPanel.Visible = true;
                        ActivationMessage.InnerHtml = ord.membership.EmailNotificationMessage;
                        NotifyActivationPanel.Visible = true;
                        NotifyUpgradePanel.Visible = false;
                        break;
                    case eOrder.OrderType.MembershipUpgrade:
                        NotificationPanel.Visible = true;
                        UpgradeMessage.InnerHtml = ord.membership.EmailNotificationMessage;
                        NotifyActivationPanel.Visible = false;
                        NotifyUpgradePanel.Visible = true;
                        break;
                    default:
                        NotificationPanel.Visible = false;
                        NotifyActivationPanel.Visible = false;
                        NotifyUpgradePanel.Visible = false;
                        break;
                }

                //Handle BRE Panel
                List<BREDetail> rulelist = new List<BREDetail>();
                switch (ord.orderType)
                {
                    case eOrder.OrderType.PhysicalProduct:
                        rulelist.Add(new BREDetail() {
                                                        Rule = "If the payment is for a physical product, generate a packing slip for shipping",
                                                        Applicability = ord.rule.IsPackingSlipApplicable.ToString()                              
                                                    });
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a physical product or a book, generate a commission payment to the agent",
                            Applicability = ord.rule.IsCommissionApplicable.ToString()
                        });
                        break;
                    case eOrder.OrderType.Book:
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a physical product, generate a packing slip for shipping",
                            Applicability = ord.rule.IsPackingSlipApplicable.ToString()
                        });
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a book, create a duplicate packing slip for the royalty department",
                            Applicability = ord.rule.IsDuplicatePackingSlipRequired.ToString()
                        });
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a physical product or a book, generate a commission payment to the agent",
                            Applicability = ord.rule.IsCommissionApplicable.ToString()
                        });
                        break;
                    case eOrder.OrderType.MembershipActivation:
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a membership, activate that membership",
                            Applicability = ord.rule.IsMembershipActivation.ToString()
                        });
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a membership or upgrade, email the owner and inform them of the activation",
                            Applicability = ord.rule.IsMembershipNotificationRequired.ToString()
                        });
                        break;
                    case eOrder.OrderType.MembershipUpgrade:
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is an upgrade to a membership, apply the upgrade",
                            Applicability = ord.rule.IsMembershipUpgradation.ToString()
                        });
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a membership or upgrade, email the owner and inform them of the upgradation",
                            Applicability = ord.rule.IsMembershipNotificationRequired.ToString()
                        });
                        break;
                    case eOrder.OrderType.Video:
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a physical product, generate a packing slip for shipping",
                            Applicability = ord.rule.IsPackingSlipApplicable.ToString()
                        });
                        if (ord.rule.IsVideoFreebieApplicable && ord.orderItems[0].Name == "Learning to Ski")
                        {
                            rulelist.Add(new BREDetail()
                            {
                                Rule = "If the payment is for a video 'Learning to Ski', add a free 'First Aid' video to the packing slip (the result of a court decision in 1997). ",
                                Applicability = ord.rule.IsVideoFreebieApplicable.ToString()
                            });

                        }
                        rulelist.Add(new BREDetail()
                        {
                            Rule = "If the payment is for a physical product or a book, generate a commission payment to the agent",
                            Applicability = ord.rule.IsCommissionApplicable.ToString()
                        });

                        break;
                }
                BRERepeater.DataSource = rulelist;
                BRERepeater.DataBind();

                OrderDiv.Visible = false;
                Authorized.Visible = true;
            }
            catch (Exception)
            {
                FailureNotify.Visible = true;
                SucessNotify.Visible = false;
            }

        }
    }

    

}