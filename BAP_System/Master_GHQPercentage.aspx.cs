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
using static BAP_System.Master_ImportFixAmount;

namespace BAP_System
{
    public partial class Master_GHQPercentage : BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetDataGHQ();

            }

        }


        protected void TableGHQGen_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                //LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");

                if (btnEdit != null) btnEdit.Visible = access.CanEdit;
                //if (btnView != null) btnView.Visible = access.CanView;
            }

        }



        protected void TableGHQGen_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TableGHQGen.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;



            if (ViewState["myViewState"] != null)
            {
                TableGHQGen.DataSource = ViewState["myViewState"];
                TableGHQGen.DataBind();
            }
            else
            {
                GetDataGHQ();
            }



            TableGHQGen.UseAccessibleHeader = true;
            TableGHQGen.HeaderRow.TableSection = TableRowSection.TableHeader;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);



        }

        protected void TableGHQGen_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            TableGHQGen.EditIndex = -1;

            // Coba ambil dari ViewState (kalau ada)
            if (ViewState["myViewState"] != null)
            {
                TableGHQGen.DataSource = ViewState["myViewState"];
                TableGHQGen.DataBind();
            }
            else
            {
                GetDataGHQ();
            }

            TableGHQGen.UseAccessibleHeader = true;
            TableGHQGen.HeaderRow.TableSection = TableRowSection.TableHeader;

        }



        protected void TableGHQGen_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow row = TableGHQGen.Rows[rowIndex];
            Guid id = (Guid)TableGHQGen.DataKeys[e.RowIndex].Value;
            string Message = "";

            System.Web.UI.WebControls.TextBox Ghqarm_ = (System.Web.UI.WebControls.TextBox)row.FindControl("txtGHQarm");
            System.Web.UI.WebControls.TextBox Global_ = (System.Web.UI.WebControls.TextBox)row.FindControl("txtGlobalHQ");

            //decimal amount2 = Decimal.Parse(Amount_.Text, new CultureInfo("id-ID"));

            Id.Value = id.ToString();

            GHQarm.Value = Ghqarm_.Text.ToString();
            GlobalHQ.Value = Global_.Text.ToString();


            bool Spchek = Sp_GHQ("Update").isSuccess;
            if (Spchek == true)
            {

                string errorMessage = "Successfully Updated";
                Message = $@"
                            setTimeout(function() {{
                                FuncSave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);


                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);

            }
            else
            {
                if (ViewState["EditIndex"] != null)
                {
                    TableGHQGen.EditIndex = (int)ViewState["EditIndex"];
                }

                TableGHQGen.DataSource = ViewState["myViewState"];
                TableGHQGen.DataBind();

                TableGHQGen.UseAccessibleHeader = true;
                TableGHQGen.HeaderRow.TableSection = TableRowSection.TableHeader;


                string errorMessage = "Update Failed!";

                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }


        }
        protected void TableGHQGen_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableGHQGen_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void GetDataGHQ()
        {
            // Ambil data dari database

            var Result = Sp_GHQ("View");
            DataTable dtb = Result.data;

            ViewState["myViewState"] = dtb;

            TableGHQGen.DataSource = dtb;
            TableGHQGen.DataBind();



            TableGHQGen.UseAccessibleHeader = true;
            TableGHQGen.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        public (bool isSuccess, string identity, DataTable data) Sp_GHQ(string Actionname)
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
                    using (SqlCommand cmd = new SqlCommand("sp_Master_GHQPercentage", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@Id", string.IsNullOrEmpty(Id.Value) ? (object)DBNull.Value : Guid.Parse(Id.Value));
                        cmd.Parameters.AddWithValue("@GHQarm", GHQarm.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@GlobalHQ", GlobalHQ.Value ?? (object)DBNull.Value);
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