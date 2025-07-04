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
using System.Diagnostics.Eventing.Reader;
using System.Security.Principal;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Bibliography;
using System.Globalization;

namespace BAP_System
{
    public partial class Master_ImportFixAmount : BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetDataFix();
            }

        }


        protected void GetDataFix()

        {
            var Result = Sp_Import("View", GetEmptyTvp(new List<ImportRow>()));
            DataTable dtb = Result.data;

            ViewState["myViewState"] = dtb;
            TableImportFIX.DataSource = dtb;
            TableImportFIX.DataBind();

            TableImportFIX.UseAccessibleHeader = true;
            TableImportFIX.HeaderRow.TableSection = TableRowSection.TableHeader;

        }

        private DataTable GetEmptyTvp(IEnumerable<ImportRow> importedRows)
        {
            var table = new DataTable();

            table.Columns.Add("RowNum", typeof(int));
            table.Columns.Add("BussinesMode", typeof(string));
            table.Columns.Add("Direction", typeof(string));
            table.Columns.Add("Description", typeof(string));
            table.Columns.Add("Amount", typeof(string));
            table.Columns.Add("GHQarmprcnt", typeof(string));
            table.Columns.Add("GlobalGHQprcnt", typeof(string));

            int rowNum = 1;
            foreach (var row in importedRows)
            {
                table.Rows.Add(
                    rowNum++,

                    row.BussinesMode ?? string.Empty,
                    row.Direction ?? string.Empty,
                    row.Description ?? string.Empty,
                    row.Amount ?? string.Empty,
                    row.GHQarmprcnt ?? string.Empty,
                    row.GlobalGHQprcnt ?? string.Empty
                );
            }

            return table;

        }


        public class ImportRow
        {

            public string BussinesMode { get; set; }
            public string Direction { get; set; }
            public string Description { get; set; }
            public string Amount { get; set; }
            public string GHQarmprcnt { get; set; }
            public string GlobalGHQprcnt { get; set; }
        }



        protected void btNImport_Click(object sender, EventArgs e)
        {

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlImport').modal();", true);
        
            GetDataFix();
     


        }

        protected void TableImportFIX_RowDataBound(object sender, GridViewRowEventArgs e)
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



        protected void TableImportFIX_RowEditing(object sender, GridViewEditEventArgs e)
        {
            TableImportFIX.EditIndex = e.NewEditIndex;
            ViewState["EditIndex"] = e.NewEditIndex;



            if (ViewState["myViewState"] != null)
            {
                TableImportFIX.DataSource = ViewState["myViewState"];
                TableImportFIX.DataBind();
            }
            else
            {
                GetDataFix();
            }



            TableImportFIX.UseAccessibleHeader = true;
            TableImportFIX.HeaderRow.TableSection = TableRowSection.TableHeader;

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);



        }

        protected void TableImportFIX_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

            TableImportFIX.EditIndex = -1;

            // Coba ambil dari ViewState (kalau ada)
            if (ViewState["myViewState"] != null)
            {
                TableImportFIX.DataSource = ViewState["myViewState"];
                TableImportFIX.DataBind();
            }
            else
            {
                GetDataFix();
            }

            TableImportFIX.UseAccessibleHeader = true;
            TableImportFIX.HeaderRow.TableSection = TableRowSection.TableHeader;

        }



        protected void TableImportFIX_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int rowIndex = e.RowIndex;
            GridViewRow row = TableImportFIX.Rows[rowIndex];
            Guid id = (Guid)TableImportFIX.DataKeys[e.RowIndex].Value;
            string Message = "";

            System.Web.UI.WebControls.TextBox Amount_ = (System.Web.UI.WebControls.TextBox)row.FindControl("txtAmount");
            //decimal amount2 = Decimal.Parse(Amount_.Text, new CultureInfo("id-ID"));

            Id.Value = id.ToString();

            Amount.Value = Amount_.Text.ToString();


            bool Spchek = Sp_Import("Update", GetEmptyTvp(new List<ImportRow>())).isSuccess;
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
                    TableImportFIX.EditIndex = (int)ViewState["EditIndex"];
                }

                TableImportFIX.DataSource = ViewState["myViewState"];
                TableImportFIX.DataBind();

                TableImportFIX.UseAccessibleHeader = true;
                TableImportFIX.HeaderRow.TableSection = TableRowSection.TableHeader;


                string errorMessage = "Update Failed!";

                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }


        }






        protected void TableImportFIX_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableImportFIX_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Import Fixed Amount & GHQ %");

                // Header
                ws.Cell("A1").Value = "Bussines mode";
                ws.Cell("B1").Value = "Direction";
                ws.Cell("C1").Value = "Description";
                ws.Cell("D1").Value = "Amount/Monthly";
                ws.Cell("E1").Value = "GHQ arm Percentage";
                ws.Cell("F1").Value = "Global GHQ Percentage";

                // Format Header
                ws.Range("A1:F1").Style.Fill.BackgroundColor = XLColor.AshGrey;
                ws.Range("A1:F1").Style.Font.Bold = true;
                ws.Columns("A:C").Width = 30;
                ws.Columns("D").Width = 20;
                ws.Columns("E:F").Width = 25;

                // Sample Data
                string[,] data = new string[,]
                {
                { "Air Freight Forwarding", "Export", "indirect", "" },
                { "Air Freight Forwarding", "Export", "Interest expense allocation/IFRS16", "" },
                { "Air Freight Forwarding", "Import", "indirect", "" },
                { "Air Freight Forwarding", "Import", "Interest expense allocation/IFRS16", "" },
                { "Ocean Freight Forwarding", "Export", "indirect", "" },
                { "Ocean Freight Forwarding", "Export", "Interest expense allocation/IFRS16", "" },
                { "Ocean Freight Forwarding", "Import", "indirect", "" },
                { "Ocean Freight Forwarding", "Import", "Interest expense allocation/IFRS16", "" },
                { "SCS", "Origin Cargo Management", "indirect", "" },
                { "SCS", "Origin Cargo Management", "Interest expense allocation/IFRS16", "" },
                { "SCS", "LLP/4PL", "indirect", "" },
                { "SCS", "LLP/4PL", "Interest expense allocation/IFRS16", "" },
                { "Air Freight Forwarding", "Export", "GHQ", "" },
                { "Air Freight Forwarding", "Import", "GHQ", "" },
                { "Ocean Freight Forwarding", "Export", "GHQ", "" },
                { "Ocean Freight Forwarding", "Import", "GHQ", "" },
                { "SCS", "Origin Cargo Management", "GHQ", "" },
                { "SCS", "LLP/4PL", "GHQ", "" },
                { "GHQ arm", "Other", "GHQ", "" },
                { "Global HQ (GHQ)", "Other", "GHQ", "" },
                };

                // Isi data dan pengaturan kolom
                for (int i = 0; i < data.GetLength(0); i++)
                {
                    for (int j = 0; j < data.GetLength(1); j++)
                    {
                        var cell = ws.Cell(i + 2, j + 1);

                        if (j == 3) // Kolom D (Amount)
                        {
                            cell.Value = 0;
                            cell.Style.NumberFormat.Format = "0";
                           
                            cell.Style.Protection.SetLocked(false);

                            var val = cell.DataValidation;
                            val.WholeNumber.EqualOrGreaterThan(0); // ✅ Hanya angka >= 0, bebas besar
                            val.ErrorMessage = "Only numeric input is allowed.";
                            val.ShowErrorMessage = true;
                        }                        
                        else
                        {
                            string value = data[i, j].Trim();
                            cell.Value = value;

                            if (!string.IsNullOrEmpty(value))
                                cell.Style.Fill.BackgroundColor = XLColor.SkyBlue;
                        }
                    }
                }

                // Isi kolom GHQ arm dan Global GHQ (kolom E dan F)
                ws.Cell("E2").Value = 0;
                ws.Cell("E2").Style.NumberFormat.Format = "0";
                ws.Cell("E2").Style.Protection.SetLocked(false);
                var e2Val = ws.Cell("E2").DataValidation;
                e2Val.WholeNumber.EqualOrGreaterThan(0);
                e2Val.ErrorMessage = "Only numeric input is allowed.";
                e2Val.ShowErrorMessage = true;

                ws.Cell("F2").Value = 0;
                ws.Cell("F2").Style.NumberFormat.Format = "0";
                ws.Cell("F2").Style.Protection.SetLocked(false);
                var f2Val = ws.Cell("F2").DataValidation;
                f2Val.WholeNumber.EqualOrGreaterThan(0);
                f2Val.ErrorMessage = "Only numeric input is allowed.";
                f2Val.ShowErrorMessage = true;

                // Lock kolom E dan F selain baris 2
                for (int i = 3; i <= data.GetLength(0) + 1; i++)
                {
                    ws.Cell(i, 5).Clear(); // E3 -> E21 kosong
                    ws.Cell(i, 5).Style.Protection.SetLocked(true);

                    ws.Cell(i, 6).Clear(); // F3 -> F21 kosong
                    ws.Cell(i, 6).Style.Protection.SetLocked(true);
                }

                // Freeze kolom A–C
                ws.SheetView.FreezeColumns(3);

                // Border untuk kolom A–D
                int dataRowCount = data.GetLength(0);
                int lastRow = dataRowCount + 1;
                var mainRange = ws.Range("A1:D" + lastRow);
                mainRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                mainRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Border hanya E1:F2
                var ghqHeaderRange = ws.Range("E1:F2");
                ghqHeaderRange.Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                ghqHeaderRange.Style.Border.InsideBorder = XLBorderStyleValues.Thin;

                // Protect setelah pengaturan SetLocked
                ws.Protect("password123");

                // Export file
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Template_Import_FixedAmount.xlsx");

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    wb.SaveAs(memoryStream);
                    memoryStream.WriteTo(Response.OutputStream);
                }
                Response.Flush();
                Response.End();

            }

            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlImport').modal();", true);
        }
        protected void btnUpload_Click(object sender, EventArgs e)
        {


            string Message = "";

            if (!FileUpload1.HasFile)
            {

                GetDataFix();

                string errorMessage = "Select a file before import.";
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }

            string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext != ".xlsx" && ext != ".xls")
            {

                GetDataFix();

                string errorMessage = "Invalid file extension. Only .xlsx or .xls Excel files are accepted.";

                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


            }

            try
            {
                DataTable dt = new DataTable();

                using (var stream = FileUpload1.PostedFile.InputStream)
                using (var workbook = new ClosedXML.Excel.XLWorkbook(stream))
                {
                    var worksheet = workbook.Worksheet(1);
                    bool firstRow = true;

                    foreach (var row in worksheet.RowsUsed())
                    {
                        if (firstRow)
                        {
                            foreach (var cell in row.Cells())
                                dt.Columns.Add(cell.Value.ToString());
                            firstRow = false;
                        }
                        else
                        {
                            DataRow dr = dt.NewRow();
                            int i = 0;
                            foreach (var cell in row.Cells(1, dt.Columns.Count))
                            {
                                dr[i++] = cell.Value.ToString();
                            }
                            dt.Rows.Add(dr);
                        }
                    }


                }


                DataTable tvp = new DataTable();
                tvp.Columns.Add("RowNum", typeof(int));
                tvp.Columns.Add("BussinesMode", typeof(string));
                tvp.Columns.Add("Direction", typeof(string));
                tvp.Columns.Add("Description", typeof(string));
                tvp.Columns.Add("Amount", typeof(string));
                tvp.Columns.Add("GHQarmprcnt", typeof(string));
                tvp.Columns.Add("GlobalGHQprcnt", typeof(string));


                string defaultGHQarm = dt.Rows[0][4]?.ToString();
                string defaultGlobalGHQ = dt.Rows[0][5]?.ToString();

                int rowNum = 1;
                foreach (DataRow row in dt.Rows)
                {
                    string ghqarm = string.IsNullOrWhiteSpace(row[4]?.ToString()) ? defaultGHQarm : row[4]?.ToString();
                    string globalghq = string.IsNullOrWhiteSpace(row[5]?.ToString()) ? defaultGlobalGHQ : row[5]?.ToString();

                    tvp.Rows.Add(
                        rowNum++,
                        row[0]?.ToString(), // BussinesMode
                        row[1]?.ToString(), // Direction
                        row[2]?.ToString(), // Description
                        row[3]?.ToString(), // Amount
                        ghqarm,
                        globalghq
                    );
                }


                if(dt.Rows.Count > 20)
                {

                    GetDataFix();

                    string errorMessage = "Invalid file format";

                    Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);


                }


                // ✅ Kirim ke SP
                var Result = Sp_Import("IMPORT", tvp);




                if (Result.isSuccess == true)
                {

                    string errorMessage = "Fixed Amount & GHQ Percentage import completed successfully," + "Number of records : " + Result.counting;
                    Message = $@"
                            setTimeout(function() {{
                                FuncSave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);


                }
                else
                {

                    GetDataFix();

                    TableImportFIX.UseAccessibleHeader = true;
                    TableImportFIX.HeaderRow.TableSection = TableRowSection.TableHeader;


                    string errorMessage = Result.ErrorRet;
                    Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);



                }

            }
            catch (Exception ex)
            {
                GetDataFix();

                string errorMessage = "Import Failed";
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "disableEditButtons();", true);



            }


            GetDataFix();


        }


        protected void btnCloseModalNew_Click(object sender, EventArgs e)
        {

            Response.Redirect("Master_ImportFixAmount.aspx");

        }



        public (bool isSuccess, string ErrorRet, string counting, DataTable data) Sp_Import(string Actionname, DataTable xcl)
        {
            bool isSuccess = false;
            string ErrorRet = "";
            string Counting = "";
            DataTable dt = new DataTable();


            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_Import_Fixed", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@Id", string.IsNullOrEmpty(Id.Value) ? (object)DBNull.Value : Guid.Parse(Id.Value));      
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString() ?? (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Amount", Amount.Value ?? (Object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@TvpImFIXD", xcl);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        if (dt.Rows.Count > 0)
                        {
                            DataRow lastRow = dt.Rows[dt.Rows.Count - 1];
                            if (dt.Columns.Contains("IsSuccess") && dt.Columns.Contains("ErrorRet") && dt.Columns.Contains("Counting"))
                            {
                                isSuccess = lastRow["IsSuccess"].ToString() == "true";
                                ErrorRet = lastRow["ErrorRet"].ToString();
                                Counting = lastRow["Counting"].ToString();

                                dt.Rows.Remove(lastRow);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                isSuccess = false;
                ErrorRet = "";
                Counting = "0";
            }

            return (isSuccess, ErrorRet, Counting, dt);
        }


    }
}