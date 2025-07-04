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
using BAP_System.Models;
namespace BAP_System
{
    public partial class Master_DirectionBAP : /*System.Web.UI.Page*/ BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetDirection();
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                }
            }

        }


        private void GetDirection()
        {

            // Ambil data dari database
            ResultValue Result = new ResultValue();
            Result =  Sp_bussinesmode("View");
            DataTable dtb = Result.Data;

            ViewState["myViewState"] = dtb;

            TableDirection.DataSource = dtb;
            TableDirection.DataBind();



            TableDirection.UseAccessibleHeader = true;
            TableDirection.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        protected void TableDirection_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void TableDirection_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableDirection_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected void btNew_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddDirection').modal();", true);
            GetDirection();

            TableDirection.UseAccessibleHeader = true;
            TableDirection.HeaderRow.TableSection = TableRowSection.TableHeader;

        }


        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_DirectionBAP.aspx");
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            ResultValue Result = new ResultValue();
            string Message = "";

            Result =  Sp_bussinesmode("Save");
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
                            }}, 500);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddDirection').modal();", true);

            }

            GetDirection();
        }
        protected void TableDirection_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TableDirection.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;



            if (ViewState["myViewState"] != null)
            {
                TableDirection.DataSource = ViewState["myViewState"];
                TableDirection.DataBind();
            }
            else
            {
                GetDirection();
            }



            TableDirection.UseAccessibleHeader = true;
            TableDirection.HeaderRow.TableSection = TableRowSection.TableHeader;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


        }

        protected void TableDirection_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            TableDirection.EditIndex = -1;

            // Coba ambil dari ViewState (kalau ada)
            if (ViewState["myViewState"] != null)
            {
                TableDirection.DataSource = ViewState["myViewState"];
                TableDirection.DataBind();
            }
            else
            {
                GetDirection();
            }

            TableDirection.UseAccessibleHeader = true;
            TableDirection.HeaderRow.TableSection = TableRowSection.TableHeader;

        }



        protected void TableDirection_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow row = TableDirection.Rows[rowIndex];
            Guid Directionid = (Guid)TableDirection.DataKeys[e.RowIndex].Value;


            System.Web.UI.WebControls.TextBox Directiontype = (System.Web.UI.WebControls.TextBox)row.FindControl("txtDirectionType");



            Direction_id.Value = Directionid.ToString();
            txtDirectionTypeadd.Value = Directiontype.Text.ToString();


            bool Spchek = Sp_bussinesmode("Update").IsSuccess;

            if (Spchek == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);

            }
            else
            {
                if (ViewState["EditIndex"] != null)
                {
                    TableDirection.EditIndex = (int)ViewState["EditIndex"];
                }

                TableDirection.DataSource = ViewState["myViewState"];
                TableDirection.DataBind();

                TableDirection.UseAccessibleHeader = true;
                TableDirection.HeaderRow.TableSection = TableRowSection.TableHeader;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }

            GetDirection();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            //ResultValue Result = new ResultValue();
            string Message = "";


            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableDirection.Rows[rowIndex];

            Guid dirid = SafeParseGuid(TableDirection.DataKeys[rowIndex].Values["Direction_id"]);

            Direction_id.Value = dirid.ToString();


            //Result = 
                
            Sp_bussinesmode("Delete");

            GetDirection();

            TableDirection.UseAccessibleHeader = true;
            TableDirection.HeaderRow.TableSection = TableRowSection.TableHeader;


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


        public ResultValue Sp_bussinesmode(string Actionname)
        {


            var result = new ResultValue();

            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Master_Direction_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        //cmd.Parameters.AddWithValue("@Account_id", Account_id.Value.ToString());
                        cmd.Parameters.AddWithValue("@Direction_id", string.IsNullOrEmpty(Direction_id.Value) ? (object)DBNull.Value : Guid.Parse(Direction_id.Value));

                        cmd.Parameters.AddWithValue("@DirectionType", txtDirectionTypeadd.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString());
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["nik"].ToString());
                        //cmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);

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
                                result.Identity = lastRow["Scop_identity"].ToString();
                                result.ErrorRet = lastRow["ErrorRet"].ToString();
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