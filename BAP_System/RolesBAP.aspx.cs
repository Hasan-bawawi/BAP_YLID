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
using Microsoft.Identity.Client;
using static AjaxControlToolkit.AsyncFileUpload.Constants;

namespace BAP_System
{
    public partial class RolesBAP : BasePageBAP  /*System.Web.UI.Page*/
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            if (!IsPostBack)
            {

                GetDataRole("View");
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                    btNewRL.Visible = access.CanCreate;
                }

            }

        }

        private void GetDataRole(string name)
        {
            // Ambil data dari database

            ResultValue Result = new ResultValue();


            RMA rma = new RMA();

            Result = Sp_Role(name, rma);

            DataTable dtb = Result.Data;

            ViewState["myViewStatemodule"] = dtb;
            TableRole.DataSource = dtb;
            TableRole.DataBind();

            TableRole.UseAccessibleHeader = true;
            TableRole.HeaderRow.TableSection = TableRowSection.TableHeader;



        }


        private void GetDataRoleRMA(string name)
        {
            // Ambil data dari database

            ResultValue Result = new ResultValue();
            RMA rma = new RMA();

            Result = Sp_Role(name, rma);
            DataTable dtb = Result.Data;

            ViewState["myViewStateRMA"] = dtb;
            TableRMA.DataSource = dtb;
            TableRMA.DataBind();

            TableRMA.UseAccessibleHeader = true;
            TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;



        }


        public ResultValue Sp_Role(string Actionname, RMA rma)
        {

            var result = new ResultValue();

            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("Sp_Role_Management_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@Role_id", string.IsNullOrEmpty(Role_id.Value) ? (object)DBNull.Value : Guid.Parse(Role_id.Value));
                        cmd.Parameters.AddWithValue("@Accesss_id", string.IsNullOrEmpty(Access_id.Value) ? (object)DBNull.Value : Guid.Parse(Access_id.Value));
                        cmd.Parameters.AddWithValue("@Menu_id", string.IsNullOrEmpty(Menu_id.Value) ? (object)DBNull.Value : Guid.Parse(Menu_id.Value));

                        cmd.Parameters.AddWithValue("@CanCreate", rma.CanCreate);
                        cmd.Parameters.AddWithValue("@CanEdit", rma.CanEdit);
                        cmd.Parameters.AddWithValue("@CanView", rma.CanView);
                        
                        cmd.Parameters.AddWithValue("@RoleName", txtRoleName.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString() /*"TES"*/); 
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["nik"].ToString() /* "TES"*/);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(result.Data);
                        }

                        if (result.Data.Rows.Count > 0)
                        {
                            DataRow lastRow = result.Data.Rows[result.Data.Rows.Count - 1];
                            if (result.Data.Columns.Contains("IsSuccess") && result.Data.Columns.Contains("Scop_identity") && result.Data.Columns.Contains("ErrorRet") )
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

        protected void btnview_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViews').modal();", true);

            GetDataRole("View");

            LinkButton btn = (LinkButton)sender;
            GridViewRow row = (GridViewRow)btn.NamingContainer;


            int rowIndex = row.RowIndex;
            Guid Roleid = Guid.Empty;
            object key = TableRole.DataKeys[rowIndex].Value;

            if (key != null && Guid.TryParse(key.ToString(), out Guid parsedGuid))
            {
                Roleid = parsedGuid;
            }

            Label lblRole = (Label)row.FindControl("lblRoleNmae");
        

            Role_id.Value = Roleid.ToString();
            txtheader.InnerText = lblRole.Text.ToString();

            GetDataRoleRMA("ChekDetail");



        }



        protected void btnCloseModalView_Click(object sender, EventArgs e)
        {
            Response.Redirect("RolesBAP.aspx");
        }


        protected void btNewRL_Click(object sender, EventArgs e)
        {
                  
                    
            DataTable dt = ViewState["myViewStateRMA"] as DataTable;

           
            ViewState["originalRMA"] = dt.Copy();
          

            // Jika null atau belum ada kolom, inisialisasi tabel dan kolom
            if (dt == null || dt.Columns.Count == 0)
            {
                dt = new DataTable("Table1");
                dt.Columns.Add("Access_id", typeof(Guid));
                dt.Columns.Add("Menu_id", typeof(string));
                dt.Columns.Add("CanCreate", typeof(bool));
                dt.Columns.Add("CanEdit", typeof(bool));
                dt.Columns.Add("CanView", typeof(bool));
                dt.Columns.Add("CretedDateRMA", typeof(DateTime));
            }

            // Cek apakah sudah ada baris kosong
            bool hasEmptyRow = dt.AsEnumerable().Any(row =>
                string.IsNullOrWhiteSpace(row["Menu_id"]?.ToString())
                && row.Field<bool?>("CanCreate") != true
                && row.Field<bool?>("CanEdit") != true
                && row.Field<bool?>("CanView") != true

            );

            if (!hasEmptyRow)
            {
                DataRow newRow = dt.NewRow();
                newRow["Access_id"] = Guid.Empty;
                newRow["Menu_id"] = Guid.Empty;
                newRow["CanCreate"] = false;
                newRow["CanEdit"] = false;
                newRow["CanView"] = false;
                newRow["CretedDateRMA"] = DateTime.Now;

                dt.Rows.InsertAt(newRow, 0);
            }

            // Simpan dan binding ke GridView
            ViewState["myViewStateRMA"] = dt;
            TableRMA.EditIndex = 0;
            TableRMA.DataSource = dt;
            TableRMA.DataBind();


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"
                    $('#mdlViews').modal();
                    disableEditButtons();", true);

            GetDataRole("View");



            TableRMA.UseAccessibleHeader = true;
            TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;
        }



        protected void btnDelete_Click(object sender, EventArgs e)
        {
            string Message = "";
            ResultValue Result = new ResultValue();
            RMA rma = new RMA();
            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableRMA.Rows[rowIndex];

            Guid Accessid = SafeParseGuid(TableRMA.DataKeys[rowIndex].Values["Access_id"]);

            Access_id.Value = Accessid.ToString();
            Result = Sp_Role("DeleteRMA",rma);

            GetDataRoleRMA("ChekDetail");
            GetDataRole("View");

            TableRMA.UseAccessibleHeader = true;
            TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;

            string errorMessage = "Delete Success";
            Message = $@"
                        setTimeout(function() {{
                            FuncSaveRMA('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                        }}, 500);
                        $('#mdlViews').modal('show');";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);





        }


        protected void TableRole_RowDataBound(object sender, GridViewRowEventArgs e)
        {


            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");

                if (btnEdit != null) btnEdit.Visible = access.CanEdit;
                if (btnView != null) btnView.Visible = access.CanView;
            }




        }

        protected void TableRole_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableRole_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TableRole_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {


            int rowIndex = e.RowIndex;
            ResultValue Result = new ResultValue();
            RMA rma = new RMA();

            GridViewRow row = TableRole.Rows[rowIndex];

            Guid roleid = SafeParseGuid(TableRole.DataKeys[e.RowIndex].Values["Role_id"]);
            System.Web.UI.WebControls.TextBox rolename = (System.Web.UI.WebControls.TextBox)row.FindControl("txtRoleName");


            DataTable dgl = ViewState["myViewStatemodule"] as DataTable;

            Role_id.Value = roleid.ToString();
            txtRoleName.Value = rolename.Text.ToString();
           


            if (roleid == Guid.Empty)
            {

                Result = Sp_Role("Save",rma);
            }
            else
            {
                Result = Sp_Role("Update",rma);

            }

            if (Result.IsSuccess == true)

            {
                TableRole.EditIndex = -1;


                GetDataRole("View");

                TableRole.UseAccessibleHeader = true;
                TableRole.HeaderRow.TableSection = TableRowSection.TableHeader;



                if (roleid == Guid.Empty)
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
                    TableRole.EditIndex = (int)ViewState["EditIndex"];
                }

                // Restore data if needed
                if (ViewState["original"] != null)
                {
                    ViewState["myViewStatemodule"] = ((DataTable)ViewState["original"]).Copy();
                }


                GetDataRole("View");


                TableRole.UseAccessibleHeader = true;
                TableRole.HeaderRow.TableSection = TableRowSection.TableHeader;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }


        }

        protected void TableRole_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "enableEditButtons();", true);


            TableRole.EditIndex = -1;
            ViewState["EditIndex"] = -1;

            GetDataRole("View");


            TableRole.UseAccessibleHeader = true;
            TableRole.HeaderRow.TableSection = TableRowSection.TableHeader;


        }

        protected void TableRole_RowEditing(object sender, GridViewEditEventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"disableEditButtons();", true);


            TableRole.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;

            if (ViewState["myViewStatemodule"] != null)
            {
                TableRole.DataSource = ViewState["myViewStatemodule"];
                TableRole.DataBind();
            }
            else
            {
                GetDataRole("View");

            }


            TableRole.UseAccessibleHeader = true;
            TableRole.HeaderRow.TableSection = TableRowSection.TableHeader;


        }


        protected void btNew_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["myViewStatemodule"] as DataTable;

            ViewState["original"] = dt.Copy();


            // Jika null atau belum ada kolom, inisialisasi tabel dan kolom
            if (dt == null || dt.Columns.Count == 0)
            {
                dt = new DataTable("Table1");
                dt.Columns.Add("RoleName", typeof(string));
                
            }

            // Cek apakah sudah ada baris kosong (semua kolom kosong)
            bool hasEmptyRow = dt.AsEnumerable().Any(row =>
                string.IsNullOrWhiteSpace(row["RoleName"]?.ToString()) 
               
            );

            if (!hasEmptyRow)
            {
                // Tambah baris baru kosong
                DataRow newRow = dt.NewRow();
                newRow["RoleName"] = ""; 
                dt.Rows.InsertAt(newRow, 0); 
            }

            // Simpan ke ViewState dan binding
            ViewState["myViewStatemodule"] = dt;
            TableRole.EditIndex = 0;
            TableRole.DataSource = dt;
            TableRole.DataBind();

            // Buka modal jika perlu
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"disableEditButtons();", true);

            //GetDataRole();

            TableRole.UseAccessibleHeader = true;
            TableRole.HeaderRow.TableSection = TableRowSection.TableHeader;


        }

        private Guid SafeParseGuid(object value)
        {
            if (value == null) return Guid.Empty;

            var str = value.ToString();
            return !string.IsNullOrWhiteSpace(str) && Guid.TryParse(str, out var result)
                ? result
                : Guid.Empty;
        }


       
        protected void TableRMA_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            RMA rma = new RMA();

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEditGL = (LinkButton)e.Row.FindControl("btnEditGL");
                LinkButton btnDeleteGL = (LinkButton)e.Row.FindControl("btnDeleteGL");

                if (btnEditGL != null) btnEditGL.Visible = access.CanEdit;
                if (btnDeleteGL != null) btnDeleteGL.Visible = access.CanEdit;

                if (TableRMA.EditIndex == e.Row.RowIndex)
                {
                    ResultValue result = Sp_Role("Getmenu", rma);

                    DropDownList ddlModuleName = (DropDownList)e.Row.FindControl("ddlModuleName");
                    if (ddlModuleName != null)
                    {
                        ddlModuleName.Items.Clear();
                        ddlModuleName.Items.Add(new System.Web.UI.WebControls.ListItem("<Select Module>", "")); 

                        foreach (DataRow dr in result.Data.Rows)
                        {
                            ddlModuleName.Items.Add(new System.Web.UI.WebControls.ListItem(
                                dr["ModuleName"].ToString(),
                                dr["Menu_id"].ToString() 
                            ));
                        }

                        string menuId = DataBinder.Eval(e.Row.DataItem, "Menu_id")?.ToString();
                        bool isNewRow = string.IsNullOrWhiteSpace(menuId) || menuId == Guid.Empty.ToString();


                        if (isNewRow)
                        {
                            ddlModuleName.SelectedValue = ""; // Pilih default
                            ddlModuleName.Enabled = true;
                        }
                        else
                        {
                          
                            System.Web.UI.WebControls.ListItem item = ddlModuleName.Items.FindByValue(menuId);
                            if (item != null)
                            {
                                ddlModuleName.SelectedValue = menuId;
                            }
                            ddlModuleName.Enabled = false;
                        }
                    }
                }


            }
        }


        protected void TableRMA_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableRMA_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void TableRMA_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int rowIndex = e.RowIndex;
            ViewState["EditIndex"] = rowIndex;


            string message = "";
            ResultValue Result = new ResultValue();
            RMA rma = new RMA();

            GridViewRow row = TableRMA.Rows[rowIndex];

            Guid Accessid = SafeParseGuid(TableRMA.DataKeys[e.RowIndex].Values["Access_id"]);

               rma.CanCreate = ((System.Web.UI.WebControls.CheckBox)row.FindControl("CanCreate")).Checked;
               rma.CanEdit =  ((System.Web.UI.WebControls.CheckBox)row.FindControl("CanEdit")).Checked;
               rma.CanView =  ((System.Web.UI.WebControls.CheckBox)row.FindControl("CanView")).Checked;
               DropDownList ddlModuleName_ = (DropDownList)row.FindControl("ddlModuleName");
               Guid menuid = Guid.Parse(ddlModuleName_.SelectedValue);



            DataTable dgl = ViewState["myViewStateRMA"] as DataTable;



            Access_id.Value = Accessid.ToString();
            Menu_id.Value = menuid.ToString();
           

            if (Accessid == Guid.Empty)
            {

                Result = Sp_Role("SaveRMA", rma);
            }
            else
            {
                Result = Sp_Role("UpdateRMA", rma);

            }

            if (Result.IsSuccess == true)

            {
                TableRMA.EditIndex = -1;


                GetDataRoleRMA("ChekDetail");

                GetDataRole("View");

                TableRMA.UseAccessibleHeader = true;
                TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;

                string succses = "Data Successfully Submit or Update";
                message = $@"
                            setTimeout(function() {{
                                FuncSaveRMA('{HttpUtility.JavaScriptStringEncode(succses)}');
                            }}, 500);
                            $('#mdlViews').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", message, true);


            }
            else
            {

                // Keep edit index so row stays in edit mode
                if (ViewState["EditIndex"] != null)
                {
                    TableRMA.EditIndex = (int)ViewState["EditIndex"];
                }

                // Restore data if needed
                if (ViewState["originalRMA"] != null)
                {
                    ViewState["myViewStateRMA"] = ((DataTable)ViewState["originalRMA"]).Copy();
                }


                GetDataRole("View");


                TableRMA.UseAccessibleHeader = true;
                TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;

                string Errormsg = Result.ErrorRet;
                message = $@"
                            setTimeout(function() {{
                                ErrorsaveRMA('{HttpUtility.JavaScriptStringEncode(Errormsg)}');
                            }}, 500);
                            $('#mdlViews').modal('show'); ";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "disableEditButtons();", true);





            }

        }

        protected void TableRMA_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "enableEditButtons();", true);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViews').modal();", true);

            TableRMA.EditIndex = -1;
            ViewState["EditIndex"] =  null;

            
            GetDataRoleRMA("ChekDetail");
            GetDataRole("View");

            TableRMA.UseAccessibleHeader = true;
            TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        






        protected void TableRMA_RowEditing(object sender, GridViewEditEventArgs e)
        {

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViews').modal();  disableEditButtons();", true);


            //TableRMA.EditIndex = e.NewEditIndex;
            //ViewState["EditIndex"] = e.NewEditIndex;

            //if (ViewState["myViewStateRMA"] != null)
            //{
            //    TableRMA.DataSource = ViewState["myViewStateRMA"];
            //    TableRMA.DataBind();
            //}
            //else
            //{
            //    GetDataRoleRMA("ChekDetail");


            //}

            //GetDataRole("View");

            //TableRMA.UseAccessibleHeader = true;
            //TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;


            TableRMA.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;
           



            DataTable dt = ViewState["myViewStateRMA"] as DataTable;

            if (dt == null)
            {
              
                GetDataRoleRMA("ChekDetail");
              
            }

            TableRMA.DataSource = dt;
            TableRMA.DataBind();

            GetDataRole("View");

            TableRMA.UseAccessibleHeader = true;
            TableRMA.HeaderRow.TableSection = TableRowSection.TableHeader;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal_edit_" + Guid.NewGuid().ToString("N"),"$('#mdlViews').modal(); disableEditButtons();", true);


        }

        }
}