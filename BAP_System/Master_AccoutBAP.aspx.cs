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
using System.Drawing;
using Microsoft.Identity.Client;

namespace BAP_System
{
    public partial class Master_AccoutBAP : /*System.Web.UI.Page*/ BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        
        {
            if (!IsPostBack)
            {

                GetDataAccount();
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                    //btNewGL.Visible = access.CanCreate;
                }


                string showModal = Request.QueryString["showModal"];
                if (showModal == "1")
                {
                    #region  untuk delete reload


                    Account_id.Value = Request.QueryString["accountId"].ToString();

                    GetDataPL();
                    GetDataAccount();

                    TablePLaccount.UseAccessibleHeader = true;
                    TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

                    string script = @"
                                    $(document).ready(function() {
                                        $('#mdlViewsPL').modal('show');
                                        swal({
                                            title: 'Remove Success',
                                            text: 'Successfully Removed',
                                            timer: 3000,
                                            type: 'success',
                                            showConfirmButton: false,
                                            html: true
                                        });

                                        // Remove query string after load
                                        if (window.history.replaceState) {
                                            window.history.replaceState(null, null, window.location.pathname);
                                        }
                                    });";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalAfterDelete", script, true);



                    #endregion
                }
                else if (showModal == "2")
                {
                    #region untuk add new reload BS
                    if (showModal == "2")
                    {
                        Account_id.Value = Request.QueryString["accountId"].ToString();
                       
                        GetDataBs();
                        GetDataAccount();
                        
                        TableGLaccount.UseAccessibleHeader = true;
                        TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
                        string script = @"
                                    $(document).ready(function() {
                                        $('#mdlViewsBS').modal('show');
                                        swal({
                                            title: 'Remove Success',
                                            text: 'Successfully Removed',
                                            timer: 3000,
                                            type: 'success',
                                            showConfirmButton: false,
                                            html: true
                                        });
                                        // Remove query string after load
                                        if (window.history.replaceState) {
                                            window.history.replaceState(null, null, window.location.pathname);
                                        }
                                    });";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalAfterDelete", script, true);
                    }
                    #endregion
                }
                else if (showModal == "3")
                {
                    #region Untuk delete reload add/edit

                    string accountId = Request.QueryString["accountId"];
                    string modul = Request.QueryString["modul"];

                    Account_id.Value = accountId;

                    string modalId = "";
                    if (modul == "gl")
                    {
                        GetDataBs();
                        GetDataAccount();
                        TableGLaccount.UseAccessibleHeader = true;
                        TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
                        modalId = "mdlViewsBS";
                    }
                    else if (modul == "pl")
                    {
                        GetDataPL();
                        GetDataAccount();
                        TablePLaccount.UseAccessibleHeader = true;
                        TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
                        modalId = "mdlViewsPL";
                    }

                    if (!string.IsNullOrEmpty(modalId))
                    {
                        string script = $@"
                            $(document).ready(function() {{
                                $('#{modalId}').modal('show');
                                swal({{
                                    title: 'Success',
                                    text: 'Successfully save',
                                    timer: 3000,
                                    type: 'success',
                                    showConfirmButton: false,
                                    html: true
                                }});

                                if (window.history.replaceState) {{
                                    window.history.replaceState(null, null, window.location.pathname);
                                }}
                            }});";

                        ScriptManager.RegisterStartupScript(this, this.GetType(), "ShowModalAfterSuccess", script, true);
                    }

                    #endregion
                }





            }
        }


        protected void TableAccount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                LinkButton btnView = (LinkButton)e.Row.FindControl("btnView");
                LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");

                if (btnEdit != null) btnEdit.Visible = access.CanEdit;
                if (btnView != null) btnView.Visible = access.CanView;
                if (btnDelete != null) btnDelete.Visible = access.CanDelete;

                if (TableAccount.EditIndex == e.Row.RowIndex)
                {

                    DropDownList ddlTypeAcc = (DropDownList)e.Row.FindControl("ddlTypeAcc");

                    if (ddlTypeAcc != null)
                    {
                        string currentValue = DataBinder.Eval(e.Row.DataItem, "TypeAccount").ToString();


                        if (ddlTypeAcc.Items.FindByValue(currentValue) != null)
                        {
                            ddlTypeAcc.SelectedValue = currentValue;
                        }
                    }

                    DropDownList ddlTypeReport = (DropDownList)e.Row.FindControl("ddlTypeReport");

                    if (ddlTypeReport != null)
                    {
                        string currentValue = DataBinder.Eval(e.Row.DataItem, "TypeReport").ToString();


                        if (ddlTypeReport.Items.FindByValue(currentValue) != null)
                        {
                            ddlTypeReport.SelectedValue = currentValue;
                        }
                    }

                }

            }

        }

        protected void TableAccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableAccount_SelectedIndexChanged (object sender, EventArgs e)
        {

        }





        protected void TableGLaccount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ResultValue Result = new ResultValue();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEditGL = (LinkButton)e.Row.FindControl("btnEditGL");

                if (btnEditGL != null) btnEditGL.Visible = access.CanEdit;


                if (TableGLaccount.EditIndex == e.Row.RowIndex)
                {

                    DropDownList ddlGLbs = (DropDownList)e.Row.FindControl("ddlGLbs");
                    if (ddlGLbs != null)
                    {
                        Guid IdBS_ = SafeParseGuid(TableGLaccount.DataKeys[e.Row.RowIndex].Values["IdBS"]);

                        IdBS.Value = IdBS_.ToString();

                        Result = Sp_Account("Getbs");
                        ddlGLbs.Items.Clear();
                        ddlGLbs.Items.Add(new System.Web.UI.WebControls.ListItem("<Select GL>", "0"));
                        foreach (DataRow dr in Result.Data.Rows)
                        {
                            ddlGLbs.Items.Add(new System.Web.UI.WebControls.ListItem(dr["GL_Accouunt"].ToString(), dr["GL_id"].ToString()));
                        }

                        string selected = DataBinder.Eval(e.Row.DataItem, "GL_id")?.ToString();
                        if (!string.IsNullOrEmpty(selected) && ddlGLbs.Items.FindByValue(selected) != null)
                        ddlGLbs.SelectedValue = selected;
                        
                    }

                }

            }

        }


        protected void TablePLaccount_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            ResultValue Result = new ResultValue();
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var access = GetAccessForPage();

                LinkButton btnEditGL = (LinkButton)e.Row.FindControl("btnEditGL");

                if (btnEditGL != null) btnEditGL.Visible = access.CanEdit;


                if (TablePLaccount.EditIndex == e.Row.RowIndex)
                {

                    DropDownList ddlGLbs = (DropDownList)e.Row.FindControl("ddlGLbs");
                    if (ddlGLbs != null)
                    {
                        Guid IdBS_ = SafeParseGuid(TablePLaccount.DataKeys[e.Row.RowIndex].Values["IdBS"]);

                        IdBS.Value = IdBS_.ToString();

                        Result = Sp_Account("GetPL");
                        ddlGLbs.Items.Clear();
                        ddlGLbs.Items.Add(new System.Web.UI.WebControls.ListItem("<Select GL>", "0"));
                        foreach (DataRow dr in Result.Data.Rows)
                        {
                            ddlGLbs.Items.Add(new System.Web.UI.WebControls.ListItem(dr["GL_Accouunt"].ToString(), dr["GL_id"].ToString()));
                        }

                        string selected = DataBinder.Eval(e.Row.DataItem, "GL_id")?.ToString();
                        if (!string.IsNullOrEmpty(selected) && ddlGLbs.Items.FindByValue(selected) != null)
                            ddlGLbs.SelectedValue = selected;

                    }


                    DropDownList ddlbranch = (DropDownList)e.Row.FindControl("ddlbranch");
                    if (ddlbranch != null)
                    {

                        Result = Sp_Account("GetBranch");
                        ddlbranch.Items.Clear();
                        ddlbranch.Items.Add(new System.Web.UI.WebControls.ListItem("<Select Branch>", "0"));
                        foreach (DataRow dr in Result.Data.Rows)
                        {
                            ddlbranch.Items.Add(new System.Web.UI.WebControls.ListItem(dr["CodeBranch"].ToString(), dr["Branch_id"].ToString()));
                        }

                        string selected = DataBinder.Eval(e.Row.DataItem, "Branch_id")?.ToString();
                        if (!string.IsNullOrEmpty(selected) && ddlbranch.Items.FindByValue(selected) != null)
                            ddlbranch.SelectedValue = selected;

                    }

                    DropDownList ddldept = (DropDownList)e.Row.FindControl("ddldept");
                    if (ddldept != null)
                    {


                        Result = Sp_Account("GetDept");
                        ddldept.Items.Clear();
                        ddldept.Items.Add(new System.Web.UI.WebControls.ListItem("<Select Dept>", "0"));
                        foreach (DataRow dr in Result.Data.Rows)
                        {
                            ddldept.Items.Add(new System.Web.UI.WebControls.ListItem(dr["NamaDept"].ToString(), dr["Dept_id"].ToString()));
                        }

                        string selected = DataBinder.Eval(e.Row.DataItem, "Dept_id")?.ToString();
                        if (!string.IsNullOrEmpty(selected) && ddldept.Items.FindByValue(selected) != null)
                            ddldept.SelectedValue = selected;

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

        protected void TablePLaccount_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TablePLaccount_SelectedIndexChanged(object sender, EventArgs e)
        {

        }






        protected void btNew_Click(object sender, EventArgs e)
        {
            //GetDataAccount();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewAccount').modal();", true);

            GetDataAccount();

            // Untuk keperluan rendering header yang konsisten
            TableAccount.UseAccessibleHeader = true;
            TableAccount.HeaderRow.TableSection = TableRowSection.TableHeader;

        }




        //protected void btNewGL_Click(object sender, EventArgs e)
        //{
        //    DataTable dt = ViewState["myViewStateGL"] as DataTable;

        //    ViewState["originalGL"] = dt.Copy();



        //    if (dt == null || dt.Columns.Count == 0)
        //    {
        //        dt = new DataTable("Table1");
        //        dt.Columns.Add("GL_Name", typeof(string));
        //        dt.Columns.Add("GL_Accouunt", typeof(string));
        //        dt.Columns.Add("Type_GL", typeof(string));
        //        dt.Columns.Add("CreateDateGL", typeof(DateTime));
        //    }

        //    // Cek apakah sudah ada baris kosong (semua kolom kosong)
        //    bool hasEmptyRow = dt.AsEnumerable().Any(row =>
        //        string.IsNullOrWhiteSpace(row["GL_Name"]?.ToString()) &&
        //        string.IsNullOrWhiteSpace(row["GL_Accouunt"]?.ToString()) &&
        //        string.IsNullOrWhiteSpace(row["Type_GL"]?.ToString())
        //    );

        //    if (!hasEmptyRow)
        //    {
        //        // Tambah baris baru kosong
        //        DataRow newRow = dt.NewRow();
        //        newRow["GL_Name"] = ""; 
        //        newRow["GL_Accouunt"] = ""; 
        //        newRow["Type_GL"] = ""; 
        //        newRow["CreateDateGL"] = DateTime.Now;
        //        dt.Rows.InsertAt(newRow, 0); 
        //    }

        //    // Simpan ke ViewState dan binding
        //    ViewState["myViewStateGL"] = dt;
        //    TableGLaccount.EditIndex = 0;
        //    TableGLaccount.DataSource = dt;
        //    TableGLaccount.DataBind();

        //    // Buka modal jika perlu
        //    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"
        //            $('#mdlViews').modal();
        //            disableEditButtons();", true);

        //    GetDataAccount();

        //    // Untuk keperluan rendering header yang konsisten
        //    TableGLaccount.UseAccessibleHeader = true;
        //    TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        //}


        protected void TableGLaccount_RowEditing(object sender, GridViewEditEventArgs e)
        {

            TableGLaccount.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;


            DataTable dt = ViewState["myViewStateGLBS"] as DataTable;

            if (dt == null)
            {

                GetDataBs();

            }

            TableGLaccount.DataSource = dt;
            TableGLaccount.DataBind();



            GetDataAccount();

            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewsBS').modal();  disableEditButtons();", true);

        }

        protected void TablePLaccount_RowEditing(object sender, GridViewEditEventArgs e)
        {

            TablePLaccount.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;


            DataTable dt = ViewState["myViewStateGLPL"] as DataTable;

            if (dt == null)
            {

                GetDataPL();

            }

            TablePLaccount.DataSource = dt;
            TablePLaccount.DataBind();



            GetDataAccount();

            TablePLaccount.UseAccessibleHeader = true;
            TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewsPL').modal();  disableEditButtons();", true);

        }



        protected void TableGLaccount_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int rowIndex = e.RowIndex;
            ViewState["EditIndex"] = rowIndex;

            string Message = "";
            ResultValue Result = new ResultValue();

            GridViewRow row = TableGLaccount.Rows[rowIndex];

            Guid IdBS_ = SafeParseGuid(TableGLaccount.DataKeys[e.RowIndex].Values["IdBS"]);
            Guid accountId = SafeParseGuid(TableAccount.DataKeys[e.RowIndex].Values["Account_id"]);

            DropDownList glbs = (DropDownList)row.FindControl("ddlGLbs");


            DataTable dgl = ViewState["myViewStateGLBS"] as DataTable;


            IdBS.Value = IdBS_.ToString();
            GL_id.Value = glbs.SelectedValue.ToString();



            if (IdBS_ == Guid.Empty)
            {

                Result = Sp_Account("SaveBS");
            }
            else
            {
                Result = Sp_Account("UpdateBS");


            }

            if (Result.IsSuccess == true)

            {
                TableGLaccount.EditIndex = -1;


                //GetDataBs();
                //GetDataAccount();

                //TableGLaccount.UseAccessibleHeader = true;
                //TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

                //string errorMessage = "Success";
                //Message = $@"
                //            setTimeout(function() {{
                //                FuncSavegl('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                //            }}, 500);
                //            $('#mdlViewsBS').modal('show');";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                string redirectUrl = "Master_AccoutBAP.aspx?showModal=3&modul="+"gl"+"&accountId=" + Server.UrlEncode(accountId.ToString());
                Response.Redirect(redirectUrl);

            }
            else
            {

                // Keep edit index so row stays in edit mode
                if (ViewState["EditIndex"] != null)
                {
                    TableGLaccount.EditIndex = (int)ViewState["EditIndex"];
                }

                // Restore data if needed
                if (ViewState["originalBS"] != null)
                {
                    ViewState["myViewStateGLBS"] = ((DataTable)ViewState["originalBS"]).Copy();
                }



                GetDataAccount();


                TableGLaccount.UseAccessibleHeader = true;
                TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

                string errorMessage = Result.ErrorRet;
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlViewsBS').modal('show'); ";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "disableEditButtons();", true);

            }

        }


        protected void TablePLaccount_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            int rowIndex = e.RowIndex;
            ViewState["EditIndex"] = rowIndex;

            string Message = "";
            ResultValue Result = new ResultValue();

            GridViewRow row = TablePLaccount.Rows[rowIndex];

            Guid IdBS_ = SafeParseGuid(TablePLaccount.DataKeys[e.RowIndex].Values["IdBS"]);
            Guid accountId = SafeParseGuid(TableAccount.DataKeys[e.RowIndex].Values["Account_id"]);

            DropDownList glbs = (DropDownList)row.FindControl("ddlGLbs");
            DropDownList glbranch = (DropDownList)row.FindControl("ddlbranch");
            DropDownList gldept = (DropDownList)row.FindControl("ddldept");

            DataTable dgl = ViewState["myViewStateGLPL"] as DataTable;


            IdBS.Value = IdBS_.ToString();
            GL_id.Value = glbs.SelectedValue.ToString();
            Dept_id.Value = gldept.SelectedValue.ToString();
            Branch_id.Value =glbranch.SelectedValue.ToString();


            if (IdBS_ == Guid.Empty)
            {
                Result = Sp_Account("SavePL");
            }
            else
            {
                Result = Sp_Account("UpdatePL");

            }


            if (Result.IsSuccess == true)

            {
                TablePLaccount.EditIndex = -1;


                //GetDataPL();
                //GetDataAccount();

                //TablePLaccount.UseAccessibleHeader = true;
                //TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

                //string errorMessage = "Success";
                //Message = $@"
                //            setTimeout(function() {{
                //                FuncSavegl('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                //            }}, 500);
                //            $('#mdlViewsPL').modal('show');";

                //ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);


                string redirectUrl = "Master_AccoutBAP.aspx?showModal=3&modul="+"pl"+"&accountId=" + Server.UrlEncode(accountId.ToString());
                Response.Redirect(redirectUrl);



            }
            else
            {

                // Keep edit index so row stays in edit mode
                if (ViewState["EditIndex"] != null)
                {
                    TablePLaccount.EditIndex = (int)ViewState["EditIndex"];
                }


                // Restore data if needed
                if (ViewState["originalPL"] != null)
                {
                    ViewState["myViewStateGLPL"] = ((DataTable)ViewState["originalPL"]).Copy();
                }

                GetDataAccount();


                TablePLaccount.UseAccessibleHeader = true;
                TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;

                string errorMessage = Result.ErrorRet;
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlViewsPL').modal('show'); ";

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

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewsBS').modal();", true);

            TableGLaccount.EditIndex = -1;
            ViewState["EditIndex"] = null;

           
            GetDataBs();

            GetDataAccount();

            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void TablePLaccount_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "enableEdits", "enableEditButtons();", true);

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewsPL').modal();", true);

            TablePLaccount.EditIndex = -1;
            ViewState["EditIndex"] = null;


            GetDataPL();

            GetDataAccount();

            TablePLaccount.UseAccessibleHeader = true;
            TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        }



        protected void btnview_Click(object sender, EventArgs e)
        {

            LinkButton btn = (LinkButton)sender;
            string typeAccount = btn.CommandArgument;
            GridViewRow row = (GridViewRow)btn.NamingContainer;



            GetDataAccount();

            int rowIndex = row.RowIndex;
            Guid accountId = Guid.Empty;
            object key = TableAccount.DataKeys[rowIndex].Value;

            if (key != null && Guid.TryParse(key.ToString(), out Guid parsedGuid))
            {
                accountId = parsedGuid;
            }

            Account_id.Value = accountId.ToString();


            if (typeAccount == "BSH")
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewsBS').modal();", true);
                GetDataBs();

            }
            else {

                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlViewsPL').modal();", true);
                GetDataPL();

            }
        
           
        }

        private void GetDataBs()
        {
            // Ambil data dari database
            ResultValue Result = new ResultValue();

            Result = Sp_Account("ViewBS");
            DataTable dtb = Result.Data;

            ViewState["myViewStateGLBS"] = dtb;
            TableGLaccount.DataSource = dtb;
            TableGLaccount.DataBind();

            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        }


        private void GetDataPL()
        {
            // Ambil data dari database
            ResultValue Result = new ResultValue();

            Result = Sp_Account("ViewPL");
            DataTable dtb = Result.Data;

            ViewState["myViewStateGLPL"] = dtb;
            TablePLaccount.DataSource = dtb;
            TablePLaccount.DataBind();

            TablePLaccount.UseAccessibleHeader = true;
            TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_AccoutBAP.aspx");
        }



        protected void btnCloseModalViewbs_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_AccoutBAP.aspx");
        }


        protected void btnCloseModalViewPL_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_AccoutBAP.aspx");
        }


        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string Message = "";
            ResultValue Result = new ResultValue();

            TxtTypeAcc.Value = ddlTypeAcc.SelectedValue.ToString();
            TxtTypeReport.Value = ddlTypeReport.SelectedValue.ToString();
            

            Result = Sp_Account("Save");
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
                            }}, 500);
                            $('#mdlAddNewAccount').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);


            }

            GetDataAccount();
            // }
        }




        protected void TableAccount_RowEditing(object sender, GridViewEditEventArgs e)
        {
          
            TableAccount.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;



            if (ViewState["myViewState"] != null)
            {
                TableAccount.DataSource = ViewState["myViewState"];
                TableAccount.DataBind();
            }
            else
            {
                GetDataAccount();
            }



            TableAccount.UseAccessibleHeader = true;
            TableAccount.HeaderRow.TableSection = TableRowSection.TableHeader;


            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);

        }

        protected void TableAccount_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {


            TableAccount.EditIndex = -1;

            // Coba ambil dari ViewState (kalau ada)
            if (ViewState["myViewState"] != null)
            {
                TableAccount.DataSource = ViewState["myViewState"];
                TableAccount.DataBind();
            }
            else
            {
                GetDataAccount();
            }

            TableAccount.UseAccessibleHeader = true;
            TableAccount.HeaderRow.TableSection = TableRowSection.TableHeader;



        }

        protected void TableAccount_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            string Message = "";
            ResultValue Result = new ResultValue();

            int rowIndex = e.RowIndex;
            GridViewRow row = TableAccount.Rows[rowIndex];
            Guid accountId = (Guid)TableAccount.DataKeys[e.RowIndex].Value;

            System.Web.UI.WebControls.TextBox accountCode = (System.Web.UI.WebControls.TextBox)row.FindControl("txtAccountCode");
            System.Web.UI.WebControls.TextBox accountTitle = (System.Web.UI.WebControls.TextBox)row.FindControl("txtAccountTitle");

            DropDownList TxtTypeAcc_ = (DropDownList)row.FindControl("ddlTypeAcc");
            DropDownList TxtTypeReport_ = (DropDownList)row.FindControl("ddlTypeReport");

            Account_id.Value = accountId.ToString();
            txtaccount.Value = accountCode.Text.ToString();
            txttitle.Value = accountTitle.Text.ToString();
            TxtTypeAcc.Value = TxtTypeAcc_.SelectedValue.ToString();
            TxtTypeReport.Value = TxtTypeReport_.SelectedValue.ToString();



            Result = Sp_Account("Update");
            if (Result.IsSuccess == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);

            }
            else
            {

                if (ViewState["EditIndex"] != null)
                {
                    TableAccount.EditIndex = (int)ViewState["EditIndex"];
                }

                TableAccount.DataSource = ViewState["myViewState"];
                TableAccount.DataBind();

                TableAccount.UseAccessibleHeader = true;
                TableAccount.HeaderRow.TableSection = TableRowSection.TableHeader;



                string errorMessage = Result.ErrorRet;
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);";
                 
                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }



            TableAccount.EditIndex = -1;
            GetDataAccount();


        }



        private void GetDataAccount()
        {
            // Ambil data dari database

           ResultValue Result = new ResultValue();


            Result = Sp_Account("View");
            DataTable dtb = Result.Data;

            ViewState["myViewState"] = dtb;
            TableAccount.DataSource = dtb;
            TableAccount.DataBind();

            TableAccount.UseAccessibleHeader = true;
            TableAccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        
        
        
        }



        //private void GetdataGLDetail()
        //{
        //    // Ambil data dari database
        //    ResultValue Result = new ResultValue();

        //    Result = Sp_Account("ViewGLDetail");
        //    DataTable dtb = Result.Data;

        //    ViewState["myViewStateGL"] = dtb;
        //    TableGLaccount.DataSource = dtb;
        //    TableGLaccount.DataBind();

        //    TableGLaccount.UseAccessibleHeader = true;
        //    TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;
        //}


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ResultValue Result = new ResultValue();

            string Message = "";

            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableAccount.Rows[rowIndex];
          

            Guid dirid = SafeParseGuid(TableAccount.DataKeys[rowIndex].Values["Account_id"]);
            string TxtTypeAcc_ = TableAccount.DataKeys[rowIndex].Values["TypeAccount"].ToString();

            
            //DropDownList TxtTypeAcc_ = (DropDownList)row.FindControl("ddlTypeAcc");

            Account_id.Value = dirid.ToString();
            TxtTypeAcc.Value = TxtTypeAcc_;

            Sp_Account("Delete");



            
            GetDataAccount();

            TableAccount.UseAccessibleHeader = true;
            TableAccount.HeaderRow.TableSection = TableRowSection.TableHeader;


            ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "FuncRemove();", true);

        }




        protected void btnDeleteBS_Click(object sender, EventArgs e)
        {
            ResultValue Result = new ResultValue();


            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableGLaccount.Rows[rowIndex];


            Guid dirid = SafeParseGuid(TableGLaccount.DataKeys[rowIndex].Values["IdBS"]);
            Guid accountId = SafeParseGuid(TablePLaccount.DataKeys[rowIndex].Values["Account_idBS"]);



            IdBS.Value = dirid.ToString();
            Sp_Account("DeleteBS");




            //Message = $@"
            //                setTimeout(function() {{
            //                    FuncRemove();
            //                }}, 500);
            //                $('#mdlViewsBS').modal('show');";

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

            string redirectUrl = "Master_AccoutBAP.aspx?showModal=2&accountId=" + Server.UrlEncode(accountId.ToString());
            Response.Redirect(redirectUrl);

            //ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modalAndError", @"$('#mdlViewsBS').modal(); FuncRemove();", true);

        }


        protected void btnDeletePL_Click(object sender, EventArgs e)
        {
            ResultValue Result = new ResultValue();

            //string Message = "";

            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TablePLaccount.Rows[rowIndex];


            Guid dirid = SafeParseGuid(TablePLaccount.DataKeys[rowIndex].Values["IdBS"]);
            Guid accountId = SafeParseGuid(TablePLaccount.DataKeys[rowIndex].Values["Account_idBS"]);



            IdBS.Value = dirid.ToString();

            Sp_Account("DeletePL");



            //Message = $@"
            //                setTimeout(function() {{
            //                    FuncRemove();
            //                }}, 500);
            //                $('#mdlViewsPL').modal('show');";

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

            string redirectUrl = "Master_AccoutBAP.aspx?showModal=1&accountId=" + Server.UrlEncode(accountId.ToString());
            Response.Redirect(redirectUrl);



        }





        protected void btNewBS_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["myViewStateGLBS"] as DataTable;

            ViewState["originalBS"] = dt.Copy();


            // Jika null atau belum ada kolom, inisialisasi tabel dan kolom
            if (dt == null || dt.Columns.Count == 0)
            {
                dt = new DataTable("Table1");
                dt.Columns.Add("IdBS", typeof(Guid));
                dt.Columns.Add("GL_id", typeof(Guid));
             
            }

            // Cek apakah sudah ada baris kosong
            bool hasEmptyRow = dt.AsEnumerable().Any(row =>
                string.IsNullOrWhiteSpace(row["GL_id"]?.ToString())
                
            );

            if (!hasEmptyRow)
            {
                DataRow newRow = dt.NewRow();
                newRow["IdBS"] = Guid.Empty;
                newRow["GL_id"] = Guid.Empty;
              

                dt.Rows.InsertAt(newRow, 0);
            }

            // Simpan ke ViewState dan binding
            ViewState["myViewStateGLBS"] = dt;
            TableGLaccount.EditIndex = 0;
            TableGLaccount.DataSource = dt;
            TableGLaccount.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"
                    $('#mdlViewsBS').modal();
                    disableEditButtons();", true);

            GetDataAccount();

            TableGLaccount.UseAccessibleHeader = true;
            TableGLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;


        }



        protected void btNewPL_Click(object sender, EventArgs e)
        {

            DataTable dt = ViewState["myViewStateGLPL"] as DataTable;

            ViewState["originalPL"] = dt.Copy();


            // Jika null atau belum ada kolom, inisialisasi tabel dan kolom
            if (dt == null || dt.Columns.Count == 0)
            {
                dt = new DataTable("Table1");
                dt.Columns.Add("IdBS", typeof(Guid));
                dt.Columns.Add("GL_id", typeof(Guid));
                dt.Columns.Add("Branch_id", typeof(Guid));
                dt.Columns.Add("Dept_id", typeof(Guid));

            }

            // Cek apakah sudah ada baris kosong
            bool hasEmptyRow = dt.AsEnumerable().Any(row =>
                string.IsNullOrWhiteSpace(row["GL_id"]?.ToString())

            );

            if (!hasEmptyRow)
            {
                DataRow newRow = dt.NewRow();
                newRow["IdBS"] = Guid.Empty;
                newRow["GL_id"] = Guid.Empty;
                newRow["Branch_id"] = Guid.Empty;
                newRow["Dept_id"] = Guid.Empty;

                dt.Rows.InsertAt(newRow, 0);
            }

            // Simpan ke ViewState dan binding
            ViewState["myViewStateGLPL"] = dt;
            TablePLaccount.EditIndex = 0;
            TablePLaccount.DataSource = dt;
            TablePLaccount.DataBind();

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", @"
                    $('#mdlViewsPL').modal();
                    disableEditButtons();", true);

            GetDataAccount();

            TablePLaccount.UseAccessibleHeader = true;
            TablePLaccount.HeaderRow.TableSection = TableRowSection.TableHeader;


        }




        public ResultValue Sp_Account(string Actionname)
        {
           
            var result = new ResultValue();

            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Master_Account_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@IdBS", string.IsNullOrEmpty(IdBS.Value) ? (object)DBNull.Value : Guid.Parse(IdBS.Value));
                        cmd.Parameters.AddWithValue("@GL_id", string.IsNullOrEmpty(GL_id.Value) ? (object)DBNull.Value : Guid.Parse(GL_id.Value));
                        cmd.Parameters.AddWithValue("@Branch_id", string.IsNullOrEmpty(Branch_id.Value) ? (object)DBNull.Value : Guid.Parse(Branch_id.Value));
                        cmd.Parameters.AddWithValue("@Dept_id", string.IsNullOrEmpty(Dept_id.Value) ? (object)DBNull.Value : Guid.Parse(Dept_id.Value));
                        cmd.Parameters.AddWithValue("@Account_id", string.IsNullOrEmpty(Account_id.Value) ? (object)DBNull.Value: Guid.Parse(Account_id.Value));                       
                        cmd.Parameters.AddWithValue("@AccountCode", txtaccount.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@AccountTitle", txttitle.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TypeAccount", TxtTypeAcc.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TypeReport", TxtTypeReport.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString());
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["nik"].ToString());

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(result.Data);
                        }

                        if (result.Data.Rows.Count > 0)
                        {
                            DataRow lastRow = result.Data.Rows[result.Data.Rows.Count - 1];
                            if (result.Data.Columns.Contains("IsSuccess") && result.Data.Columns.Contains("ErrorRet")  && result.Data.Columns.Contains("Scop_identity"))
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

            return result ;
        }


    }


}