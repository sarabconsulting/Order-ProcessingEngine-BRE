<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="eOrder.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width,initial-scale=1.0,maximum-scale=1" />
    <title>eOrder Processing | Largest eRetailer Company Private Limited</title>
    <link href="css/bootstrap-theme.min.css" rel="stylesheet" type="text/css" />
    <link href="css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="css/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server" style="margin: 20px;">
        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
            <h1 class="text-center">eOrder system</h1>
            <hr />
        </div>
        <div id="SucessNotify" runat="server" class="col-lg-12 col-md-12 col-sm-12 col-xs-12 label label-success"
            style="margin-bottom: 20px; padding: 30px; font-size: small;">
            Last action was successful!
        <asp:Button ID="CancelFailure" runat="server" Text="X" CssClass="btn btn-danger btn-xs pull-right"
            OnCommand="NotificationButton_Command" CommandName="Cancel" CommandArgument="SuccessDiv"></asp:Button>
        </div>
        <div id="FailureNotify" runat="server" class="col-lg-12 col-md-12 col-sm-12 col-xs-12 label label-danger"
            style="margin-bottom: 20px; padding: 30px; font-size: small;">
            Last action was Unuccessful!
        <asp:Button ID="CancelSuccess" runat="server" Text="X" CssClass="btn btn-danger btn-xs pull-right"
            OnCommand="NotificationButton_Command" CommandName="Cancel" CommandArgument="FailureDiv"></asp:Button>
        </div>

        <div id="OrderDiv" runat="server" class="row" style="width: 500px; margin: 0 auto; background: #eee;">
            <div id="OrderValidator" runat="server" class="text-left" style="margin-bottom: 0px; padding: 50px; font-size: medium;">
                <asp:Label ID="OrderValidationSummary" runat="server" Text=""
                    CssClass="col-lg-12 col-md-12 col-sm-12 col-xs-12 label label-danger"></asp:Label>
                <br />
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12 text-center" style="padding: 50px;">

                <h4 class="text-center">Please make a choice..</h4>
                <asp:DropDownList ID="OrderType" runat="server" CssClass="form-control" Style="background: #0a9087; color: White;"
                    OnSelectedIndexChanged="OrderType_SelectedIndexChanged" AutoPostBack="true">
                    <asp:ListItem Value="P">Physical Product</asp:ListItem>
                    <asp:ListItem Value="B">Book</asp:ListItem>
                    <asp:ListItem Value="M">Membership</asp:ListItem>
                    <asp:ListItem Value="V">Digital Video</asp:ListItem>
                </asp:DropDownList>


                <br />
                <div id="VideoSelection" runat="server">
                    <hr />
                    <h4 class="text-center">Select a video from library</h4>
                    <asp:DropDownList ID="VideoLibraryDropdown" runat="server" CssClass="form-control" Style="background: #ffd800; color: Black;"
                        ValidationGroup="Ordering">
                        <asp:ListItem Value="LOTR">Life on the rocks!</asp:ListItem>
                        <asp:ListItem Value="LTS">Learning to Ski</asp:ListItem>
                        <asp:ListItem Value="WOC">Wisdom of Crowds</asp:ListItem>
                        <asp:ListItem Value="DR">Digital Revolution</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <br />

                <div id="MembershipTypeSelection" runat="server" class="form-check form-check-inline">
                    <h4 class="text-center">Select a membership options</h4>
                    <asp:TextBox ID="MembershipId" runat="server" placeholder="Enter your membership Id here.. e.g. MBR0001" CssClass="form-control" Text="MBR0001"
                        ValidationGroup="Ordering" TextMode="SingleLine" Enabled="false"></asp:TextBox>
                    <br />
                    <input class="form-check-input" name="MembershipOptions" type="radio" id="ActivationOption" runat="server" value="Activation" checked />
                    <label class="form-check-label" for="ActivationOption">Activation</label>
                    <input class="form-check-input" name="MembershipOptions" type="radio" id="UpgradeOption" runat="server" value="Upgrade" />
                    <label class="form-check-label" for="UpgradeOption">Upgrade</label>
                    <hr />
                </div>

                <h4 class="text-center">Payment Amount (INR)</h4>
                <asp:TextBox ID="PaymentAmount" runat="server" placeholder="Enter payment here.." CssClass="form-control" Text="100.00"
                    ValidationGroup="Ordering" TextMode="Number"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Above cannot be left empty!"
                    ControlToValidate="PaymentAmount" Display="Dynamic" CssClass="label label-danger"
                    ValidationGroup="Ordering"></asp:RequiredFieldValidator>
                <br />
                <asp:Button ID="OrderButton" runat="server" Text="Submit Order" CssClass="btn btn-success btn-block" OnClick="OrderButton_Click"
                    ValidationGroup="Ordering" />
            </div>
        </div>
        <div id="Authorized" class="row" runat="server">
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="pull-right">
                    <asp:Button ID="ResetButton" runat="server" Text="Reset" CssClass="btn btn-danger btn-xs" OnClick="ResetButton_Click" />
                </div>
                <strong>Welcome, </strong><span>Master</span>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="margin-bottom: 20px;">
                <h3 style="background: #0a9087; color: White; padding: 10px; border-radius: 3px;">Current Order</h3>
                <div id="ValidationSummaryDiv" runat="server" class="text-left" style="margin-bottom: 20px; padding: 10px; font-size: medium;">
                    <asp:Label ID="ValidationMessage" runat="server" Text="There seems to be some error"
                        CssClass="col-lg-12 col-md-12 col-sm-12 col-xs-12 label label-danger"></asp:Label>
                    <br />
                </div>
                <div class="row">
                    <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                        <h4 class="text-center" style="background: lightcyan; color: black; padding: 10px; border-radius: 7px;">Order Details</h4>
                        <table class="table table-responsive table-condensed">
                            <thead style="font-size: small;">
                                <tr>
                                    <th>Parameter
                                    </th>
                                    <th>Value
                                    </th>
                                </tr>
                            </thead>
                            <tbody style="font-size: small;">

                                <asp:Repeater ID="OrderDetailRepeater" runat="server">
                                    <ItemTemplate>

                                        <tr>
                                            <td>
                                                <%# ((eOrder.OrderDetail)Container.DataItem).parameter %>
                                            </td>
                                            <td>
                                                <%# ((eOrder.OrderDetail)Container.DataItem).value%>
                                            </td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                    <div id="packingPanel" runat="server" class="col-lg-4 col-md-3 col-sm-6 col-xs-12">

                        <div id="PackingSlipMain" runat="server" class="row" style="background: White; color: gray; border-style: dotted;">
                            <h4 class="text-center" style="background: lightcyan; color: black; padding: 10px; border-radius: 7px;">Packing Slip</h4>
                            <div class="col-ld-12 text-center">
                                <h4><strong>Largest eRetailer Company Private Limited</strong></h4>
                                <h5 style="font-size: smaller">Quickest Retail Services </h5>
                            </div>

                            <div class="col-lg-6">
                                OrderRef : <span><asp:Label ID="orderRefLabel" runat="server" Text="--"></asp:Label></span>
                            </div>
                            <div class="col-lg-6">
                                Dated: <span><asp:Label ID="orderDateLabel" runat="server" Text="--"></asp:Label></span>
                            </div>


                            <div class="col-lg-6">
                                <h5><strong>Shipping From</strong></h5>
                                <p><asp:Label ID="shippingFromLabel" runat="server" Text="--"></asp:Label></p>
                            </div>
                            <div class="col-lg-6">
                                <h5><strong>Shipping To</strong></h5>
                                <p><asp:Label ID="shippingToLabel" runat="server" Text="--"></asp:Label></p>
                            </div>
                            <div class="col-lg-12">
                                <table class="table table-responsive table-condensed">
                                    <thead style="font-size: small;">
                                        <tr>
                                            <th>Category
                                            </th>
                                            <th>Item
                                            </th>
                                            <th>Quantity
                                            </th>
                                            <th>Cost
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: small;">
                                <asp:Repeater ID="itemsRepeater" runat="server">
                                    <ItemTemplate>

                                        <tr>
                                            <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Category %>
                                            </td>
                                            <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Item%>
                                            </td>
                                             <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Quantity%>
                                            </td>
                                             <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Cost%>
                                            </td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col-lg-12 text-right">
                                <h5>Sub-Total:<span><asp:Label ID="subTotalLabel" runat="server" Text="0.00"></asp:Label></span> </h5>
                                <h5>Tax (flat rate): <span><asp:Label ID="taxLabel" runat="server" Text="0.00"></asp:Label></span> </h5>
                                <h5>Shipping: <span><asp:Label ID="shippingLabel" runat="server" Text="0.00"></asp:Label></span> </h5>
                                <h3>Grand Total: <span><asp:Label ID="GrandTotalLabel" runat="server" Text="0.00"></asp:Label></span> </h3>
                            </div>
                        </div>
                        <div id="duplicatePackingSlip" runat="server" class="row" style="background: White; color: gray; border-style: dotted;">
                            <h4 class="text-center" style="background: lightcyan; color: black; padding: 10px; border-radius: 7px;">Duplicate Copy (cc to royalty dept.)</h4>
                            <div class="col-ld-12 text-center">
                                <h4><strong>Largest eRetailer Company Private Limited</strong></h4>
                                <h5 style="font-size: smaller">Quickest Retail Services </h5>
                                <h3 style="font-size: smaller"><strong>Duplicate Copy</strong> </h3>
                            </div>

                            <div class="col-lg-6">
                                OrderRef : <span>
                                    <asp:Label ID="orderRef1Label" runat="server" Text="--"></asp:Label></span>
                            </div>
                            <div class="col-lg-6">
                                Dated: <span><asp:Label ID="orderDate1Label" runat="server" Text="--"></asp:Label></span>
                            </div>
                            <div class="col-lg-6">
                                <h5><strong>Shipping From</strong></h5>
                                <p><asp:Label ID="shippingFrom1Label" runat="server" Text="--"></asp:Label></p>
                            </div>
                            <div class="col-lg-6">
                                <h5><strong>Shipping To</strong></h5>
                                <p><asp:Label ID="shippingTo1Label" runat="server" Text="--"></asp:Label></p>
                            </div>
                            <div class="col-lg-12">
                                <table class="table table-responsive table-condensed">
                                    <thead style="font-size: small;">
                                        <tr>
                                            <th>Category
                                            </th>
                                            <th>Item
                                            </th>
                                            <th>Quantity
                                            </th>
                                            <th>Cost
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody style="font-size: small;">
                                        <asp:Repeater ID="ItemsRepeater2" runat="server">
                                    <ItemTemplate>

                                        <tr>
                                            <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Category %>
                                            </td>
                                            <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Item%>
                                            </td>
                                             <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Quantity%>
                                            </td>
                                             <td>
                                                <%# ((eOrder.itemDetail)Container.DataItem).Cost%>
                                            </td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:Repeater>
                                    </tbody>
                                </table>
                            </div>
                            <div class="col-lg-12 text-right">
                                <h5>Sub-Total:<span><asp:Label ID="subTotal1Label" runat="server" Text="0.00"></asp:Label></span> </h5>
                                <h5>Tax (flat rate): <span><asp:Label ID="tax1Label" runat="server" Text="0.00"></asp:Label></span> </h5>
                                <h5>Shipping: <span><asp:Label ID="shipping1Label" runat="server" Text="0.00"></asp:Label></span> </h5>
                                <h3>Grand Total: <span><asp:Label ID="GrandTotal1Label" runat="server" Text="0.00 INR"></asp:Label></span> </h3>
                            </div>
                        </div>
                    </div>
                    <div id="NotificationPanel" runat="server" class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                        <div id="NotifyActivationPanel" runat="server" class="row">
                            <h4 class="text-center" style="background: lightcyan; color: black; padding: 10px; border-radius: 7px;">Membership Activated Email Notification</h4>
                            <div id="ActivationMessage" runat="server" class="col-lg-12">
                            </div>
                        </div>

                        
                        <div id="NotifyUpgradePanel" runat="server" class="row">
                            <h4 class="text-center" style="background: lightcyan; color: black; padding: 10px; border-radius: 7px;">Membership Upgraded Email Notification</h4>
                            <div id="UpgradeMessage" runat="server" class="col-lg-12">
                            </div>
                        </div>
                    </div>



                    <div class="col-lg-2 col-md-3 col-sm-6 col-xs-12">
                        <h4 class="text-center" style="background: green; color: White; padding: 10px; border-radius: 7px;">Business Rules Applicable</h4>
                        <table class="table table-responsive table-condensed">
                            <thead style="font-size: small;">
                                <tr>
                                    <th>Rule
                                    </th>
                                    <th>Applicable
                                    </th>
                                </tr>
                            </thead>
                            <tbody style="font-size: small;">
                                
                                  <asp:Repeater ID="BRERepeater" runat="server">
                                    <ItemTemplate>
                                        <tr>
                                            <td>
                                                <%# ((eOrder.BREDetail)Container.DataItem).Rule %>
                                            </td>
                                            <td>
                                                <%# ((eOrder.BREDetail)Container.DataItem).Applicability%>
                                            </td>
                                        </tr>

                                    </ItemTemplate>
                                </asp:Repeater>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div class="row text-center">
            All Rights Reserved. &copy; Largest eRetailer Company Private Limited 2020-21
        </div>
    </form>
</body>
<script src="js/bootstrap.js" type="text/javascript"></script>
<script src="js/bootstrap.min.js" type="text/javascript"></script>
</html>
