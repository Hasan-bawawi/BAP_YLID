using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DocumentFormat.OpenXml.Wordprocessing;
using DocumentFormat.OpenXml.Bibliography;
using System.EnterpriseServices;
using BAP_System.Models;
using DocumentFormat.OpenXml.Math;
using Microsoft.Identity.Client;
using System.Security.Principal;

namespace BAP_System
{
    public partial class Master_GeneralLedgerBAP : BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetdataGLDetail();
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                }


            }

    
        
        }

        protected void TableGLaccount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEditGL = (LinkButton)e.Row.FindControl("btnEditGL");
                LinkButton btnDeleteGL = (LinkButton)e.Row.FindControl("btnDeleteGL");
                if (btnEditGL != null) btnEditGL.Visible = access.CanEdit;
                if (btnDeleteGL != null) btnDeleteGL.Visible = access.CanDelete;


                if (TableGLaccount.EditIndex == e.Row.RowIndex)
                {

                    DropDownList ddlTypeGL = (DropDownList)e.Row.FindControl("ddlTypeGL");

                    if (ddlTypeGL != null)
                    {
                        string currentValue = DataBinder.Eval(e.Row.DataItem, "Type_GL").ToString();


                        if (ddlTypeGL.Items.FindByValue(currentValue) != null)
                        {
                            ddlTypeGL.SelectedValue = currentValue;
                        }
                    }

                }

            }

        }

        protected void TableGLaccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableGLaccount_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_GeneralLedgerBAP.aspx");
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            ResultValue Result = new ResultValue(); 
            string Message = "";

            txtTypeGL.Value = ddlTypeGL.SelectedValue.ToString(); 

            Result = Sp_GL("Save");
            if (Result.IsSuccess == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

            }
            else
            {
                string errorMessage = Result.ErrorRet;
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500); ";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddGL').modal();", true);

            }
            GetdataGLDetail();
        }

        protected void btNew_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddGL').modal();", true);

            GetdataGLDetail();

            // Untuk keperluan rendering header yang konsisten
            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //string Message = "";
            ResultValue Result = new ResultValue();

            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableGLaccount.Rows[rowIndex];

            Guid glid = SafeParseGuid(TableGLaccount.DataKeys[rowIndex].Values["GL_id"]);

            GL_id.Value = glid.ToString();
            Result = Sp_GL("DeleteGL");

            GetdataGLDetail();

            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;


            Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "Deletesave();", true);

            //string errorMessage = "Delete Success";
            //Message = $@"
            //            setTimeout(function() {{
            //                Deletesave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
            //            }}, 500);";

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
        }



        protected void TableGLaccount_RowEditing(object sender, GridViewEditEventArgs e)
        {

            TableGLaccount.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;


            DataTable dt = ViewState["myViewStateGL"] as DataTable;

            if (dt == null)
            {

                GetdataGLDetail();

            }

            TableGLaccount.DataSource = dt;
            TableGLaccount.DataBind();

            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);

        }





        protected void TableGLaccount_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int rowIndex = e.RowIndex;
            ViewState["EditIndex"] = rowIndex;

            string Message = "";
            ResultValue Result = new ResultValue();

            GridViewRow row = TableGLaccount.Rows[rowIndex];

            Guid glid = SafeParseGuid(TableGLaccount.DataKeys[e.RowIndex].Values["GL_id"]);
           

            System.Web.UI.WebControls.TextBox glname = (System.Web.UI.WebControls.TextBox)row.FindControl("txtGLName");
            System.Web.UI.WebControls.TextBox glaccount = (System.Web.UI.WebControls.TextBox)row.FindControl("txtGLAccount");
            DropDownList typegl = (DropDownList)row.FindControl("ddlTypeGL");

           

            DataTable dgl = ViewState["myViewStateGL"] as DataTable;
            var glacnt = dgl.AsEnumerable().FirstOrDefault(r => r.Field<Guid>("GL_id") == glid);
            
            
            txtGLAccountbrf.Value = glacnt.Field<string>("GL_Accouunt").ToString();
            GL_id.Value = glid.ToString();
            txtGLName.Value = glname.Text.ToString();
            txtGLAccount.Value = glaccount.Text.ToString();
            txtTypeGL.Value = typegl.SelectedValue.ToString();

            


            Result = Sp_GL("Update");
           

            if (Result.IsSuccess == true)

            {
                TableGLaccount.EditIndex = -1;


                GetdataGLDetail();

                TableGLaccount.UseAccessibleHeader = true;
                TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);


            }
            else
            {

                // Keep edit index so row stays in edit mode
                if (ViewState["EditIndex"] != null)
                {
                    TableGLaccount.EditIndex = (int)ViewState["EditIndex"];
                }

                // Restore data if needed
                if (ViewState["originalGL"] != null)
                {
                    ViewState["myViewStateGL"] = ((DataTable)ViewState["originalGL"]).Copy();
                }


                TableGLaccount.UseAccessibleHeader = true;
                TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

                string errorMessage = Result.ErrorRet;
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500); ";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "disableEditButtons();", true);

            }

        }


        private Guid SafeParseGuid(object value)
        {
            if (value == null) return Guid.Empty;

            var str = value.ToString();
            return !string.IsNullOrWhiteSpace(str) && Guid.TryParse(str, out var result)
                ? result
                : Guid.Empty;
        }




        protected void TableGLaccount_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "enableEditButtons();", true);


            TableGLaccount.EditIndex = -1;
            ViewState["EditIndex"] = null;

           
            GetdataGLDetail();
            
            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        private void GetdataGLDetail()
        {
            // Ambil data dari database
            ResultValue Result = new ResultValue();

            Result = Sp_GL("View");
            DataTable dtb = Result.Data;

            ViewState["myViewStateGL"] = dtb;
            TableGLaccount.DataSource = dtb;
            TableGLaccount.DataBind();

            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public ResultValue Sp_GL(string Actionname)
        {

            var result = new ResultValue();

            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Master_GeneralLedger_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@GL_id", string.IsNullOrEmpty(GL_id.Value) ? (object)DBNull.Value : Guid.Parse(GL_id.Value));
                        cmd.Parameters.AddWithValue("@GL_Name", txtGLName.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GL_Account", txtGLAccount.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GL_Accountbefore", txtGLAccountbrf.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Type_GL", txtTypeGL.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString());
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["nik"].ToString());

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(result.Data);
                        }

                        if (result.Data.Rows.Count > 0)
                        {
                            DataRow lastRow = result.Data.Rows[result.Data.Rows.Count - 1];
                            if (result.Data.Columns.Contains("IsSuccess") && result.Data.Columns.Contains("ErrorRet") && result.Data.Columns.Contains("Scop_identity"))
                            {
                                result.IsSuccess = lastRow["IsSuccess"].ToString() == "true";
                                result.ErrorRet = lastRow["ErrorRet"].ToString();
                                result.Identity = lastRow["Scop_identity"].ToString();

                                result.Data.Rows.Remove(lastRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Identity = "0";
                result.ErrorRet = ex.Message;
            }

            return result;
        }

    }


}