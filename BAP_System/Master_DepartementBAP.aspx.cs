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
using BAP_System.Models;


namespace BAP_System
{
    public partial class Master_DepartementBAP : /*System.Web.UI.Page*/ BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDept();
                GetBus("all");
                var access = GetAccessForPage();

                if (access != null)
                {
                    btNew.Visible = access.CanCreate;
                }
            }

        }

        protected DataTable GetBus(string menu)
        {
            ddlBussines.Items.Clear();
            var resultTuple = Sp_Dept("Getbus");
            DataTable Result = resultTuple.data;


            if (menu == "all")
            {
                ListItem newItem = new ListItem();
                newItem.Text = "<Select Bussines>";
                newItem.Value = "0";
                ddlBussines.Items.Add(newItem);

                foreach (DataRow dr in Result.Rows)
                {
                    newItem = new ListItem();
                    newItem.Text = dr["BussinesName"].ToString();
                    newItem.Value = dr["BussinesMode_id"].ToString();
                    ddlBussines.Items.Add(newItem);
                }

                return Result;

            }
            else

            {
                Guid businessModeId;
                string selectedValue = Request.Form[ddlBussines.UniqueID];
                if (Guid.TryParse(selectedValue, out businessModeId))
                {
                    var hasil = Result.AsEnumerable()
                        .Where(row => row.Field<Guid>("BussinesMode_id") == businessModeId);
                    
                    
                    
                    DataRow dr = hasil.FirstOrDefault();
                    Session.Add("BussinesMode_id", dr["BussinesMode_id"].ToString());

                }
                else
                {
                    // Tangani jika nilai tidak valid
                    Console.WriteLine("Format GUID tidak valid dari dropdown.");
                }

                return Result;



            }

               

        }



        private void GetDept()
        {
            // Ambil data dari database

            var Result = Sp_Dept("View");
            DataTable dtb = Result.data;

            ViewState["myViewState"] = dtb;

            TableDepartement.DataSource = dtb;
            TableDepartement.DataBind();


            TableDepartement.UseAccessibleHeader = true;
            TableDepartement.HeaderRow.TableSection = TableRowSection.TableHeader;
        }




        protected void TableDepartement_RowDataBound(object sender, GridViewRowEventArgs e)
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

                if (e.Row.RowIndex == TableDepartement.EditIndex)
                {
                    DropDownList ddl = (DropDownList)e.Row.FindControl("ddlBussines");
                    if (ddl != null)
                    {
                        DataTable dt = GetBus("all");

                        // Tambahkan <Select Bussines>
                        ddl.Items.Clear();
                        ddl.Items.Add(new ListItem("<Select Bussines>", "0"));

                        foreach (DataRow dr in dt.Rows)
                        {
                            ListItem item = new ListItem(dr["BussinesName"].ToString(), dr["BussinesMode_id"].ToString());
                            ddl.Items.Add(item);
                        }

                        // Set nilai yang sesuai dengan data baris
                        string currentValue = DataBinder.Eval(e.Row.DataItem, "BussinesMode_id").ToString();
                        if (ddl.Items.FindByValue(currentValue) != null)
                        {
                            ddl.SelectedValue = currentValue;
                        }
                    }


                }
            }
             

        }

        protected void TableDepartement_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableDepartement_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void ddlBussines_SelectedIndexChanged(object sender, EventArgs e)
        {
            GetBus("select");
            Busid.Value = Session["BussinesMode_id"].ToString();
        }

        protected void btNew_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewDept').modal();", true);
            GetDept();

            TableDepartement.UseAccessibleHeader = true;
            TableDepartement.HeaderRow.TableSection = TableRowSection.TableHeader;

        }


        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("Master_DepartementBAP.aspx");
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool Spchek = Sp_Dept("Save").isSuccess;
            if (Spchek == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncSave();", true);

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlAddNewDept').modal();", true);

            }
            GetDept();
        }
        protected void TableDepartement_RowEditing(object sender, GridViewEditEventArgs e)
        {
          
            TableDepartement.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;

           
            GetDept();
            GetBus("all");

            TableDepartement.UseAccessibleHeader = true;
            TableDepartement.HeaderRow.TableSection = TableRowSection.TableHeader;
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


        }

        protected void TableDepartement_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            TableDepartement.EditIndex = -1;

            // Coba ambil dari ViewState (kalau ada)
            if (ViewState["myViewState"] != null)
            {
                TableDepartement.DataSource = ViewState["myViewState"];
                TableDepartement.DataBind();
            }
            else
            {
                GetDept();
            }

            TableDepartement.UseAccessibleHeader = true;
            TableDepartement.HeaderRow.TableSection = TableRowSection.TableHeader;

        }



        protected void TableDepartement_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow row = TableDepartement.Rows[rowIndex];
            Guid Departementid = (Guid)TableDepartement.DataKeys[e.RowIndex].Values[0];
           
            System.Web.UI.WebControls.TextBox NameDept = (System.Web.UI.WebControls.TextBox)row.FindControl("txtNamaDept");
            System.Web.UI.WebControls.TextBox CodeDept = (System.Web.UI.WebControls.TextBox)row.FindControl("txtCodeDept");
            //System.Web.UI.WebControls.CheckBox HavesubEdit = (System.Web.UI.WebControls.CheckBox)row.FindControl("havesubEdit");

            DropDownList ddlBusinessName = (DropDownList)row.FindControl("ddlBussines");

            Guid businessNameId = Guid.Parse(ddlBusinessName.SelectedValue);
            Departement_id.Value = Departementid.ToString();
            txtNamaDeptadd.Value = NameDept.Text.ToString();
            txtCodeDeptadd.Value = CodeDept.Text.ToString();
            Busid.Value = businessNameId.ToString();
            //havesub.Checked = HavesubEdit.Checked;

            bool Spchek = Sp_Dept("Update").isSuccess;
            if (Spchek == true)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "text", "FuncUpdate();", true);

            }
            else
            {
                if (ViewState["EditIndex"] != null)
                {
                    TableDepartement.EditIndex = (int)ViewState["EditIndex"];
                }

                TableDepartement.DataSource = ViewState["myViewState"];
                TableDepartement.DataBind();

                TableDepartement.UseAccessibleHeader = true;
                TableDepartement.HeaderRow.TableSection = TableRowSection.TableHeader;


                ScriptManager.RegisterStartupScript(this, this.GetType(), "error", "Errorsave();", true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }


        }



        public (bool isSuccess, string identity, DataTable data) Sp_Dept(string Actionname)
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
                    using (SqlCommand cmd = new SqlCommand("sp_Master_Departement_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        
                        cmd.Parameters.AddWithValue("@Departement_id", string.IsNullOrEmpty(Departement_id.Value) ? (object)DBNull.Value : Guid.Parse(Departement_id.Value));
                        cmd.Parameters.AddWithValue("@BussinesMode_id", string.IsNullOrEmpty(Busid.Value) ? (object)DBNull.Value : Guid.Parse(Busid.Value));

                        cmd.Parameters.AddWithValue("@NamaDept", txtNamaDeptadd.Value ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CodeDept", txtCodeDeptadd.Value ?? (object)DBNull.Value);

                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString());
                        cmd.Parameters.AddWithValue("@ModifiedBy", Session["nik"].ToString());
                        //cmd.Parameters.AddWithValue("@IsHavesub", havesub.Checked);
                       
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


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            ResultValue Result = new ResultValue();

            string Message = "";


            LinkButton btn = (LinkButton)sender;
            int rowIndex = int.Parse(btn.CommandArgument);

            GridViewRow row = TableDepartement.Rows[rowIndex];

            Guid dirid = SafeParseGuid(TableDepartement.DataKeys[rowIndex].Values["Department_id"]);

            Departement_id.Value = dirid.ToString();

            Sp_Dept("Delete");

            GetDept();

            TableDepartement.UseAccessibleHeader = true;
            TableDepartement.HeaderRow.TableSection = TableRowSection.TableHeader;


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

    }
}