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
using System.Security.Principal;
using ClosedXML.Excel;
using BAP_System.Models;


namespace BAP_System
{
    public partial class Master_BranchBAP : /*System.Web.UI.Page*/ BasePageBAP
    {


        protected void Page_Load(object sender, EventArgs e)

        {
            if (!IsPostBack)
            {
                GetDataBranch();

                //string currentPage = Path.GetFileName(Request.Path);
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                }
            }
        }

        private void GetDataBranch()
        {
            // Ambil data dari database

            var Result = Sp_Branch("View");
            DataTable dtb = Result.data;

            ViewState["myViewState"] = dtb;

            TableBranch.DataSource = dtb;
            TableBranch.DataBind();

            

            TableBranch.UseAccessibleHeader = true;
            TableBranch.HeaderRow.TableSection = TableRowSection.TableHeader;
        }




        protected void TableBranch_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
                //LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");

                if (btnEdit != null) btnEdit.Visible = access.CanEdit;
                if (btnDelete != null) btnDelete.Visible = access.CanDelete;
                //if (btnView != null) btnView.Visible = access.CanView;
            }


        }

        protected void TableBranch_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableBranch_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void btNew_Click(object sender, EventArgs e)
        {
           
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewBranch').modal();", true);

            GetDataBranch();

            // Untuk keperluan rendering header yang konsisten
            TableBranch.UseAccessibleHeader = true;
            TableBranch.HeaderRow.TableSection = TableRowSection.TableHeader;

        }


        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_BranchBAP.aspx");
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool Spchek = Sp_Branch("Save").isSuccess;
            if (Spchek == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewAccount').modal();", true);

            }
            GetDataBranch();
        }
        protected void TableBranch_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TableBranch.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;



            if (ViewState["myViewState"] != null)
            {
                TableBranch.DataSource = ViewState["myViewState"];
                TableBranch.DataBind();
            }
            else
            {
                GetDataBranch();
            }



            TableBranch.UseAccessibleHeader = true;
            TableBranch.HeaderRow.TableSection = TableRowSection.TableHeader;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);



        }

        protected void TableBranch_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            
            TableBranch.EditIndex = -1;

            // Coba ambil dari ViewState (kalau ada)
            if (ViewState["myViewState"] != null)
            {
                TableBranch.DataSource = ViewState["myViewState"];
                TableBranch.DataBind();
            }
            else
            {
                GetDataBranch();
            }

            TableBranch.UseAccessibleHeader = true;
            TableBranch.HeaderRow.TableSection = TableRowSection.TableHeader;

        }



        protected void TableBranch_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow row = TableBranch.Rows[rowIndex];
            Guid Branchid = (Guid)TableBranch.DataKeys[e.RowIndex].Value;


            System.Web.UI.WebControls.TextBox Branchname = (System.Web.UI.WebControls.TextBox)row.FindControl("txtBranchName");
            System.Web.UI.WebControls.TextBox CodeBranch = (System.Web.UI.WebControls.TextBox)row.FindControl("txtCodeBranch");
            System.Web.UI.WebControls.TextBox Address = (System.Web.UI.WebControls.TextBox)row.FindControl("txtAddress");

            

            Branch_id.Value = Branchid.ToString();
            txtBranchNameadd.Value = Branchname.Text.ToString();
            txtCodeBranchadd.Value = CodeBranch.Text.ToString();
            txtAddressadd.Value = Address.Text.ToString();


            bool Spchek = Sp_Branch("Update").isSuccess;
            if (Spchek == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);

            }
            else
            {
                if (ViewState["EditIndex"] != null)
                {
                    TableBranch.EditIndex = (int)ViewState["EditIndex"];
                }

                TableBranch.DataSource = ViewState["myViewState"];
                TableBranch.DataBind();

                TableBranch.UseAccessibleHeader = true;
                TableBranch.HeaderRow.TableSection = TableRowSection.TableHeader;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }


        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ResultValue Result = new ResultValue();

            string Message = "";

            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableBranch.Rows[rowIndex];

            Guid dirid = SafeParseGuid(TableBranch.DataKeys[rowIndex].Values["Branch_id"]);

            Branch_id.Value = dirid.ToString();

            Sp_Branch("Delete");

            GetDataBranch();

            TableBranch.UseAccessibleHeader = true;
            TableBranch.HeaderRow.TableSection = TableRowSection.TableHeader;


            ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "FuncRemove();", true);

        }

        private Guid SafeParseGuid(object value)
        {
            if (value == null) return Guid.Empty;

            var str = value.ToString();
            return !string.IsNullOrWhiteSpace(str) && Guid.TryParse(str, out var result)
                ? result
                : Guid.Empty;
        }

        public (bool isSuccess, string identity, DataTable data) Sp_Branch(string Actionname)
        {
            bool isSuccess = false;
            string scopeIdentity = "";
            DataTable dt = new DataTable();

            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Master_Branch_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        //cmd.Parameters.AddWithValue("@Account_id", Account_id.Value.ToString());
                        cmd.Parameters.AddWithValue("@Branch_id", string.IsNullOrEmpty(Branch_id.Value) ? (object)DBNull.Value : Guid.Parse(Branch_id.Value));

                        cmd.Parameters.AddWithValue("@NameBranch", txtBranchNameadd.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CodeBranch", txtCodeBranchadd.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Address", txtAddressadd.Value ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString());
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["nik"].ToString());
                        //cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                            if (dt.Columns.Contains("IsSuccess") && dt.Columns.Contains("Scop_identity"))
                            {
                                isSuccess = lastRow["IsSuccess"].ToString() == "true";
                                scopeIdentity = lastRow["Scop_identity"].ToString();

                                dt.Rows.Remove(lastRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                scopeIdentity = "0";
            }

            return (isSuccess, scopeIdentity, dt);
        }

      


    }
}