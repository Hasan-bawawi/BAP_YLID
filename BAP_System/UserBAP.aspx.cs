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
using System.DirectoryServices.ActiveDirectory;
using DocumentFormat.OpenXml.Spreadsheet;

namespace BAP_System
{
    public partial class UserBAP : BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetDataUser();
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                }

            }

        }

        private void GetDataUser()
        {
            // Ambil data dari database

            ResultValue Result = new ResultValue();
            Result = Sp_User("View");
            DataTable dtb = Result.Data;

            ViewState["myViewStatemodule"] = dtb;
            TableUser.DataSource = dtb;
            TableUser.DataBind();

            TableUser.UseAccessibleHeader = true;
            TableUser.HeaderRow.TableSection = TableRowSection.TableHeader;

        }


      



        public ResultValue Sp_User(string Actionname)
        {

            var result = new ResultValue();

            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_User_Management_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@User_id", string.IsNullOrEmpty(User_id.Value) ? (object)DBNull.Value : Guid.Parse(User_id.Value));
                        cmd.Parameters.AddWithValue("@Employessid", string.IsNullOrEmpty(Employees_id.Value) ? (object)DBNull.Value : Guid.Parse(Employees_id.Value));
                        cmd.Parameters.AddWithValue("@Roleid", string.IsNullOrEmpty(Role_id.Value) ? (object)DBNull.Value : Guid.Parse(Role_id.Value));
                        cmd.Parameters.AddWithValue("@username", txtusername.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@useridAd", txtad_id.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString());
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["nik"].ToString());

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(result.Data);
                        }

                        if (result.Data.Rows.Count > 0)
                        {
                            DataRow lastRow = result.Data.Rows[result.Data.Rows.Count - 1];
                            if (result.Data.Columns.Contains("IsSuccess") && result.Data.Columns.Contains("Scop_identity"))
                            {
                                result.IsSuccess = lastRow["IsSuccess"].ToString() == "true";
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
            }

            return result;
        }





      

        protected void TableUser_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            ResultValue Result = new ResultValue();


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                //LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");

                if (btnEdit != null) btnEdit.Visible = access.CanEdit;
                //if (btnView != null) btnView.Visible = access.CanView;
                if (TableUser.EditIndex == e.Row.RowIndex)
                {
                    bool isNewRow = false;

                    string username = DataBinder.Eval(e.Row.DataItem, "username")?.ToString();
                    string adId = DataBinder.Eval(e.Row.DataItem, "ad_id")?.ToString();


                    if (string.IsNullOrWhiteSpace(username) && string.IsNullOrWhiteSpace(adId))
                    {
                        isNewRow = true;
                    }

                    DropDownList ddlEmployees = (DropDownList)e.Row.FindControl("ddlemployees");
                    if (ddlEmployees != null)
                    {
                        Result = Sp_User("GetEmployees");
                        ddlEmployees.Items.Clear();
                        ddlEmployees.Items.Add(new System.Web.UI.WebControls.ListItem("<Select Employeess>", "0"));
                        foreach (DataRow dr in Result.Data.Rows)
                        {
                            ddlEmployees.Items.Add(new System.Web.UI.WebControls.ListItem(dr["fullname"].ToString(), dr["Employees_id"].ToString()));
                        }

                        if (isNewRow)
                        {
                            ddlEmployees.SelectedValue = "0"; // Default select
                        }
                        else
                        {
                            string selected = DataBinder.Eval(e.Row.DataItem, "Employees_id")?.ToString();
                            if (!string.IsNullOrEmpty(selected) && ddlEmployees.Items.FindByValue(selected) != null)
                                ddlEmployees.SelectedValue = selected;
                        }
                    }

                    DropDownList ddlRole = (DropDownList)e.Row.FindControl("ddlRolename");
                    if (ddlRole != null)
                    {
                        Result = Sp_User("GetDataRole");
                        ddlRole.Items.Clear();
                        ddlRole.Items.Add(new System.Web.UI.WebControls.ListItem("<Select Role>", "0"));
                        foreach (DataRow dr in Result.Data.Rows)
                        {
                            ddlRole.Items.Add(new System.Web.UI.WebControls.ListItem(dr["RoleName"].ToString(), dr["Role_id"].ToString()));
                        }

                        if (isNewRow)
                        {
                            ddlRole.SelectedValue = "0"; // Default select
                        }
                        else
                        {
                            string selected = DataBinder.Eval(e.Row.DataItem, "Role_id")?.ToString();
                            if (!string.IsNullOrEmpty(selected) && ddlRole.Items.FindByValue(selected) != null)
                                ddlRole.SelectedValue = selected;
                        }
                    }



                }

            }
        }

        protected void TableUser_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableUser_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TableUser_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            int rowIndex = e.RowIndex;
            ResultValue Result = new ResultValue();

            GridViewRow row = TableUser.Rows[rowIndex];

            Guid userid = SafeParseGuid(TableUser.DataKeys[e.RowIndex].Values["User_id"]);
            System.Web.UI.WebControls.TextBox username = (System.Web.UI.WebControls.TextBox)row.FindControl("txtusername");
            System.Web.UI.WebControls.TextBox adid = (System.Web.UI.WebControls.TextBox)row.FindControl("txtad_id");


            DropDownList ddlEmplyoees_= (DropDownList)row.FindControl("ddlemployees");
            DropDownList ddlRole_ = (DropDownList)row.FindControl("ddlRolename");

            Guid Employeesid = Guid.Parse(ddlEmplyoees_.SelectedValue);
            Guid Roleid = Guid.Parse(ddlRole_.SelectedValue);


            DataTable dgl = ViewState["myViewStatemodule"] as DataTable;


            User_id.Value = userid.ToString();
            Employees_id.Value = Employeesid.ToString();
            Role_id.Value = Roleid.ToString();

            txtusername.Value = username.Text.ToString();
            txtad_id.Value = adid.Text.ToString();


            if (userid == Guid.Empty)
            {
                Result = Sp_User("Save");
            }
            else
            {
                Result = Sp_User("Update");

            }

            if (Result.IsSuccess == true)

            {
                TableUser.EditIndex = -1;


                GetDataUser();

                TableUser.UseAccessibleHeader = true;
                TableUser.HeaderRow.TableSection = TableRowSection.TableHeader;



                if (userid == Guid.Empty)
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);

                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "enableEditButtons();", true);

            }
            else
            {

                // Keep edit index so row stays in edit mode
                if (ViewState["EditIndex"] != null)
                {
                    TableUser.EditIndex = (int)ViewState["EditIndex"];
                }

                // Restore data if needed
                if (ViewState["original"] != null)
                {
                    ViewState["myViewStatemodule"] = ((DataTable)ViewState["original"]).Copy();
                }


                GetDataUser();


                TableUser.UseAccessibleHeader = true;
                TableUser.HeaderRow.TableSection = TableRowSection.TableHeader;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }


        }

        protected void TableUser_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "enableEditButtons();", true);


            TableUser.EditIndex = -1;
            ViewState["EditIndex"] = -1;

            GetDataUser();


            TableUser.UseAccessibleHeader = true;
            TableUser.HeaderRow.TableSection = TableRowSection.TableHeader;


        }

        protected void TableUser_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"disableEditButtons();", true);


            TableUser.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;

            if (ViewState["myViewStatemodule"] != null)
            {
                TableUser.DataSource = ViewState["myViewStatemodule"];
                TableUser.DataBind();
            }
            else
            {
                GetDataUser();

            }


            TableUser.UseAccessibleHeader = true;
            TableUser.HeaderRow.TableSection = TableRowSection.TableHeader;


        }


        protected void btNew_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["myViewStatemodule"] as DataTable;

            ViewState["original"] = dt.Copy();


            // Jika null atau belum ada kolom, inisialisasi tabel dan kolom
            if (dt == null || dt.Columns.Count == 0)
            {
                dt = new DataTable("Table1");
                dt.Columns.Add("username", typeof(string));
                dt.Columns.Add("ad_id", typeof(string));
            }

            // Cek apakah sudah ada baris kosong (semua kolom kosong)
            bool hasEmptyRow = dt.AsEnumerable().Any(row =>
                string.IsNullOrWhiteSpace(row["username"]?.ToString()) &&
                string.IsNullOrWhiteSpace(row["ad_id"]?.ToString()) 
            );

            if (!hasEmptyRow)
            {
                // Tambah baris baru kosong
                DataRow newRow = dt.NewRow();
                newRow["username"] = "";
                newRow["ad_id"] = "";
                newRow["Employees_id"] = Guid.Empty; // Default untuk ddlemployees
                newRow["Role_id"] = Guid.Empty;    // Default untuk ddlRolename

                // -->  untuk ddlemployees, ddlRolename ambill all karena add baru 

                dt.Rows.InsertAt(newRow, 0);
            }

            // Simpan ke ViewState dan binding
            ViewState["myViewStatemodule"] = dt;
            TableUser.EditIndex = 0;
            TableUser.DataSource = dt;
            TableUser.DataBind();

            // Buka modal jika perlu
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"disableEditButtons();", true);

            //GetDataUser();

            TableUser.UseAccessibleHeader = true;
            TableUser.HeaderRow.TableSection = TableRowSection.TableHeader;


        }

        private Guid SafeParseGuid(object value)
        {
            if (value == null) return Guid.Empty;

            var str = value.ToString();
            return !string.IsNullOrWhiteSpace(str) && Guid.TryParse(str, out var result)
                ? result
                : Guid.Empty;
        }





    }
}