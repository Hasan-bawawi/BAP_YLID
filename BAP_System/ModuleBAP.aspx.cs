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
namespace BAP_System
{
    public partial class ModuleBAP : /*System.Web.UI.Page*/ BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                GetDataModule();
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                }

            }

        }


        private void GetDataModule()
        {
            // Ambil data dari database

            ResultValue Result = new ResultValue();
            Submenu sub = new Submenu();

            Result = Sp_Module("View",sub);
            DataTable dtb = Result.Data;

            ViewState["myViewStatemodule"] = dtb;
            TableModule.DataSource = dtb;
            TableModule.DataBind();

            TableModule.UseAccessibleHeader = true;
            TableModule.HeaderRow.TableSection = TableRowSection.TableHeader;

            

        }



        public ResultValue Sp_Module(string Actionname, Submenu sub)
        {

            var result = new ResultValue();

            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Module_Management_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@Menu_id", string.IsNullOrEmpty(menu_id.Value) ? (object)DBNull.Value : Guid.Parse(menu_id.Value));
                        cmd.Parameters.AddWithValue("@Parent_id", string.IsNullOrEmpty(Parent_id.Value) ? (object)DBNull.Value : Guid.Parse(Parent_id.Value));

                        cmd.Parameters.AddWithValue("@ModuleName", txtmoduleName.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ModuleASPXID", txtModuleASPXid.Value ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@submenu1", sub.Submenu1);
                        cmd.Parameters.AddWithValue("@submenu2", sub.Submenu2);
                        cmd.Parameters.AddWithValue("@submenu3", sub.Submenu3);



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

        protected void TableModule_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ResultValue Result = new ResultValue();
            Submenu sub = new Submenu();


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                //LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");

                if (btnEdit != null) btnEdit.Visible = access.CanEdit;
                //if (btnView != null) btnView.Visible = access.CanView;

                if (TableModule.EditIndex == e.Row.RowIndex)
                {

                    bool isNewRow = false;

                    string Modulename = DataBinder.Eval(e.Row.DataItem, "moduleName")?.ToString();
                    string Moduleaspxid = DataBinder.Eval(e.Row.DataItem, "ModuleASPXid")?.ToString();

                    if (string.IsNullOrWhiteSpace(Modulename) && string.IsNullOrWhiteSpace(Moduleaspxid))
                    {
                        isNewRow = true;
                    }

                    DropDownList ddlParent = (DropDownList)e.Row.FindControl("ddlParent");
                    if (ddlParent != null)
                    {
                        Result = Sp_Module("View", sub);
                        ddlParent.Items.Clear();
                        ddlParent.Items.Add(new System.Web.UI.WebControls.ListItem("<Select Parent>", "0"));
                        foreach (DataRow dr in Result.Data.Rows)
                        {
                            ddlParent.Items.Add(new System.Web.UI.WebControls.ListItem(dr["ModuleName"].ToString(), dr["Menu_id"].ToString()));
                        }

                        if (isNewRow)
                        {
                            ddlParent.SelectedValue = "0"; // Default select
                        }
                        else
                        {
                            string selected = DataBinder.Eval(e.Row.DataItem, "Parent_id")?.ToString();
                            if (!string.IsNullOrEmpty(selected) && ddlParent.Items.FindByValue(selected) != null)
                                ddlParent.SelectedValue = selected;
                        }
                    }



                }
            }

        }

        protected void TableModule_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableModule_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TableModule_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            int rowIndex = e.RowIndex;
            ResultValue Result = new ResultValue();
            Submenu sub = new Submenu();

            GridViewRow row = TableModule.Rows[rowIndex];
            Guid Parentid = Guid.Empty;
            Guid moduleid = SafeParseGuid(TableModule.DataKeys[e.RowIndex].Values["menu_id"]);
            System.Web.UI.WebControls.TextBox modulename = (System.Web.UI.WebControls.TextBox)row.FindControl("txtmoduleName");
            System.Web.UI.WebControls.TextBox aspxid = (System.Web.UI.WebControls.TextBox)row.FindControl("txtModuleASPXid");

            

            sub.Submenu1 = ((System.Web.UI.WebControls.CheckBox)row.FindControl("Submenu1")).Checked;
            sub.Submenu2 = ((System.Web.UI.WebControls.CheckBox)row.FindControl("Submenu2")).Checked;
            sub.Submenu3 = ((System.Web.UI.WebControls.CheckBox)row.FindControl("Submenu3")).Checked;


            DropDownList ddlParent = (DropDownList)row.FindControl("ddlParent");
            if (ddlParent.SelectedValue == "0")
            {
                Parentid = Guid.Empty;

            }
            else
            {

                Parentid = Guid.Parse(ddlParent.SelectedValue);

            }


            DataTable dgl = ViewState["myViewStatemodule"] as DataTable;

   


            menu_id.Value = moduleid.ToString();
            txtmoduleName.Value = modulename.Text.ToString();
            txtModuleASPXid.Value = aspxid.Text.ToString();
            Parent_id.Value = Parentid.ToString();    
            //txtType.Value = type.Text.ToString();

           
            if (moduleid == Guid.Empty)
            {

                Result = Sp_Module("Save",sub);
            }
            else
            {
                Result = Sp_Module("Update",sub);

            }

            if (Result.IsSuccess == true)

            {
                TableModule.EditIndex = -1;


                GetDataModule();

                TableModule.UseAccessibleHeader = true;
                TableModule.HeaderRow.TableSection = TableRowSection.TableHeader;



                if (moduleid == Guid.Empty)
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
                    TableModule.EditIndex = (int)ViewState["EditIndex"];
                }

                // Restore data if needed
                if (ViewState["original"] != null)
                {
                    ViewState["myViewStatemodule"] = ((DataTable)ViewState["original"]).Copy();
                }


                GetDataModule();


                TableModule.UseAccessibleHeader = true;
                TableModule.HeaderRow.TableSection = TableRowSection.TableHeader;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setupCheckbox", @"bindCheckboxBehavior();", true);



            }


        }

        protected void TableModule_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "enableEditButtons();", true);


            TableModule.EditIndex = -1;
            ViewState["EditIndex"] = -1;

            GetDataModule();
           

            TableModule.UseAccessibleHeader = true;
            TableModule.HeaderRow.TableSection = TableRowSection.TableHeader;


        }

        protected void TableModule_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"disableEditButtons();", true);


            TableModule.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;

            if (ViewState["myViewStatemodule"] != null)
            {
                TableModule.DataSource = ViewState["myViewStatemodule"];
                TableModule.DataBind();
            }
            else
            {
                GetDataModule();

            }
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setupCheckbox", @"bindCheckboxBehavior();", true);


            TableModule.UseAccessibleHeader = true;
            TableModule.HeaderRow.TableSection = TableRowSection.TableHeader;


        }   


        protected void btNew_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["myViewStatemodule"] as DataTable;

            ViewState["original"] = dt.Copy();


            // Jika null atau belum ada kolom, inisialisasi tabel dan kolom
            if (dt == null || dt.Columns.Count == 0)
            {
                dt = new DataTable("Table1");
                dt.Columns.Add("moduleName", typeof(string));
                dt.Columns.Add("ModuleASPXid", typeof(string));

            }

            // Cek apakah sudah ada baris kosong (semua kolom kosong)
            bool hasEmptyRow = dt.AsEnumerable().Any(row =>
                string.IsNullOrWhiteSpace(row["moduleName"]?.ToString()) &&
                string.IsNullOrWhiteSpace(row["ModuleASPXid"]?.ToString()) 
           
                );

            if (!hasEmptyRow)
            {
                // Tambah baris baru kosong
                DataRow newRow = dt.NewRow();
                newRow["moduleName"] = ""; 
                newRow["ModuleASPXid"] = "";
                newRow["Parent_id"] = Guid.Empty;
                newRow["Submenu1"] = false;
                newRow["Submenu2"] = false;
                newRow["Submenu3"] = false;

                dt.Rows.InsertAt(newRow, 0); 
            }

            // Simpan ke ViewState dan binding
            ViewState["myViewStatemodule"] = dt;
            TableModule.EditIndex = 0;
            TableModule.DataSource = dt;
            TableModule.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"disableEditButtons();", true);
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "setupCheckbox", @"bindCheckboxBehavior();", true);

            //GetDataModule();

            TableModule.UseAccessibleHeader = true;
            TableModule.HeaderRow.TableSection = TableRowSection.TableHeader;


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