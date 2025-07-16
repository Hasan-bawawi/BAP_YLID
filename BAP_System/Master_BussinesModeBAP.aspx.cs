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
    public partial class Master_BussinesModeBAP : /*System.Web.UI.Page*/ BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetDataBussinesmode();
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                }
            }

        }


        private void GetDataBussinesmode()
        {
            // Ambil data dari database

            var Result = Sp_bussinesmode("View");
            DataTable dtb = Result.data;

            ViewState["myViewState"] = dtb;

            TableBussinesMode.DataSource = dtb;
            TableBussinesMode.DataBind();



            TableBussinesMode.UseAccessibleHeader = true;
            TableBussinesMode.HeaderRow.TableSection = TableRowSection.TableHeader;
        }




        protected void TableBussinesMode_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
                //LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");

                if (btnEdit != null) btnEdit.Visible = access.CanEdit;
                if (btnDelete != null) btnDelete.Visible = access.CanDelete;
            }


        }

        protected void TableBussinesMode_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableBussinesMode_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void btNew_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewBussines').modal();", true);

            GetDataBussinesmode();

            TableBussinesMode.UseAccessibleHeader = true;
            TableBussinesMode.HeaderRow.TableSection = TableRowSection.TableHeader;


        }


        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_BussinesModeBAP.aspx");
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool Spchek = Sp_bussinesmode("Save").isSuccess;
            if (Spchek == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewBussines').modal();", true);

            }
            GetDataBussinesmode();
        }
        protected void TableBussinesMode_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TableBussinesMode.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;



            if (ViewState["myViewState"] != null)
            {
                TableBussinesMode.DataSource = ViewState["myViewState"];
                TableBussinesMode.DataBind();
            }
            else
            {
                GetDataBussinesmode();
            }



            TableBussinesMode.UseAccessibleHeader = true;
            TableBussinesMode.HeaderRow.TableSection = TableRowSection.TableHeader;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);

        }

        protected void TableBussinesMode_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            TableBussinesMode.EditIndex = -1;

            // Coba ambil dari ViewState (kalau ada)
            if (ViewState["myViewState"] != null)
            {
                TableBussinesMode.DataSource = ViewState["myViewState"];
                TableBussinesMode.DataBind();
            }
            else
            {
                GetDataBussinesmode();
            }

            TableBussinesMode.UseAccessibleHeader = true;
            TableBussinesMode.HeaderRow.TableSection = TableRowSection.TableHeader;

        }



        protected void TableBussinesMode_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow row = TableBussinesMode.Rows[rowIndex];
            Guid BussinesModeid = (Guid)TableBussinesMode.DataKeys[e.RowIndex].Value;


            System.Web.UI.WebControls.TextBox BussinesName = (System.Web.UI.WebControls.TextBox)row.FindControl("txtBussinesName");
            System.Web.UI.WebControls.TextBox CodeName = (System.Web.UI.WebControls.TextBox)row.FindControl("txtCodeName");



            BussinesMode_id.Value = BussinesModeid.ToString();
            txtBussinesNameadd.Value = BussinesName.Text.ToString();
            txtCodeNameadd.Value = CodeName.Text.ToString();


            bool Spchek = Sp_bussinesmode("Update").isSuccess;
            if (Spchek == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);

            }
            else
            {
                if (ViewState["EditIndex"] != null)
                {
                    TableBussinesMode.EditIndex = (int)ViewState["EditIndex"];
                }

                TableBussinesMode.DataSource = ViewState["myViewState"];
                TableBussinesMode.DataBind();

                TableBussinesMode.UseAccessibleHeader = true;
                TableBussinesMode.HeaderRow.TableSection = TableRowSection.TableHeader;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }


        }


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ResultValue Result = new ResultValue();


            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableBussinesMode.Rows[rowIndex];

            Guid dirid = SafeParseGuid(TableBussinesMode.DataKeys[rowIndex].Values["BussinesMode_id"]);

            BussinesMode_id.Value = dirid.ToString();

            Sp_bussinesmode("Delete");

            GetDataBussinesmode();

            TableBussinesMode.UseAccessibleHeader = true;
            TableBussinesMode.HeaderRow.TableSection = TableRowSection.TableHeader;


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

        public (bool isSuccess, string identity, DataTable data) Sp_bussinesmode(string Actionname)
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
                    using (SqlCommand cmd = new SqlCommand("sp_Master_BussinesMode_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        //cmd.Parameters.AddWithValue("@Account_id", Account_id.Value.ToString());
                        cmd.Parameters.AddWithValue("@BussinessMode_id", string.IsNullOrEmpty(BussinesMode_id.Value) ? (object)DBNull.Value : Guid.Parse(BussinesMode_id.Value));

                        cmd.Parameters.AddWithValue("@BussinesName", txtBussinesNameadd.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CodeName", txtCodeNameadd.Value ?? (object)DBNull.Value);
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