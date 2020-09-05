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
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                }
                else
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
                if (o=="M")
                {
                    Reset();
                    MembershipTypeSelection.Visible = true;
                }
                else if (o=="V")
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
                if (ValidateOrder())
                {
                    //Submit
                    OrderDiv.Visible = false;
                    Authorized.Visible = true;
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
                //Add validation checks
                return true;
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
            Authorized.Visible =false;
        }
    }
}