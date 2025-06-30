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


namespace BAP_System
{
    public partial class Transaksi_ImportBAP : /*System.Web.UI.Page*/ BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetDataSummary();
            }

        }


        protected void GetDataSummary()

        {
            var Result = Sp_Import("View",GetEmptyTvp());
            DataTable dtb = Result.data;

            ViewState["myViewState"] = dtb;
            TableImportBAP.DataSource = dtb;
            TableImportBAP.DataBind();

            TableImportBAP.UseAccessibleHeader = true;
            TableImportBAP.HeaderRow.TableSection = TableRowSection.TableHeader;

        } 



        protected void btNImport_Click(object sender, EventArgs e)
        {
            
            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "modal", "$('#mdlImport').modal();", true);

            GetDataSummary();


        }


        protected void btnCloseModalNew_Click (object sender, EventArgs e)
        {

            Response.Redirect("Transaksi_ImportBAP.aspx");

        }


        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string Message = "";

            if (!FileUpload1.HasFile)
            {

                string errorMessage = "Pilih File terlebih dahulu";
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

            }

            string ext = Path.GetExtension(FileUpload1.FileName).ToLower();
            if (ext != ".xlsx" && ext != ".xls" )
            {

                string errorMessage = "Periksa file Extensi Excel, wajib (.xlsx atau .xls)";

                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

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

                var Result = Sp_Import("IMPORT", dt);

                if (Result.isSuccess == true)
                {
                    
                    string errorMessage = "Import Berhasil," + "Dengan Jumlah data : " + Result.counting;
                    Message = $@"
                            setTimeout(function() {{
                                FuncSave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);

                }
                else
                {

                    GetDataSummary();

                    TableImportBAP.UseAccessibleHeader = true;
                    TableImportBAP.HeaderRow.TableSection = TableRowSection.TableHeader;


                    string errorMessage = Result.ErrorRet;
                    Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);


                }

            }
            catch (Exception ex)
            {

                string errorMessage = "Gagal Import";
                Message = $@"
                            setTimeout(function() {{
                                Errorsave('{HttpUtility.JavaScriptStringEncode(errorMessage)}');
                            }}, 500);
                            $('#mdlImport').modal('show');";

                ScriptManager.RegisterStartupScript(this, this.GetType(), "showErrorWithModal", Message, true);


            }


            GetDataSummary();



        }



        protected void btnDownloadTemplate_Click(object sender, EventArgs e)
        {

            using (XLWorkbook wb = new XLWorkbook())
            {
                var ws = wb.Worksheets.Add("Template");

                // Header sesuai kolom TVP
                string[] headers = new string[]
                {
                    "Company","Account", "Type", "Description" ,"Br", "Dept",
                    "Activity", "Direction", "Mode","Year", "Periode", "Amount in Period",
                    "Currency"
                };

                for (int i = 0; i < headers.Length; i++)
                {
                    var cell = ws.Cell(1, i + 1);
                    cell.Value = headers[i];
                    cell.Style.Font.Bold = true;
                    cell.Style.Fill.BackgroundColor = XLColor.Gray;
                    cell.Style.Border.OutsideBorder = XLBorderStyleValues.Thin; 
                    cell.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                    ws.Column(i + 1).AdjustToContents();
                  

                }

              
                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("content-disposition", "attachment;filename=Template_Import_BAP.xlsx");

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

        protected void btNMenu_Click(object sender, EventArgs e)
        {
            Response.Redirect("Financial_Accounting_ReportBAP.aspx");

        }

        protected void TableImportBAP_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void TableImportBAP_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void TableImportBAP_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        public (bool isSuccess, string ErrorRet, string counting ,DataTable data) Sp_Import(string Actionname, DataTable xcl)
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
                    using (SqlCommand cmd = new SqlCommand("sp_Import_TB", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        
                        cmd.Parameters.AddWithValue("@CreateBy", Session["nik"].ToString() ?? (object)DBNull.Value);
                      
                        cmd.Parameters.AddWithValue("@TvpImBAP", xcl);

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


        private DataTable GetEmptyTvp()
        {
            var table = new DataTable();
            table.Columns.Add("Company" ,  typeof(string)); 
            table.Columns.Add("Account", typeof(string));
            table.Columns.Add("Type", typeof(string));
            table.Columns.Add("Description" , typeof(string));
            table.Columns.Add("Br", typeof(string));
            table.Columns.Add("Dept", typeof(string));
            table.Columns.Add("Activity", typeof(string));
            table.Columns.Add("Direction", typeof(string));
            table.Columns.Add("Mode", typeof(string));
            table.Columns.Add("Year", typeof(string));
            table.Columns.Add("Periode", typeof(string));
            table.Columns.Add("Amountperiode", typeof(string));
            table.Columns.Add("Currency", typeof(string));

            return table;
        }


    }
}