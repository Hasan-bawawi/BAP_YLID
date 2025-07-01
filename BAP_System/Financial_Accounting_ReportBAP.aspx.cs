using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.IO;
using System.Windows.Forms;
using WebSupergoo.ABCpdf;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Drawing;
using BAP_System.Models;
using WebGrease.Css.Ast;
using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using System.Xml;
using OfficeOpenXml.DataValidation;


namespace BAP_System
{
    public partial class Financial_Accounting_ReportBAP : /*System.Web.UI.Page*/ BasePageBAP
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                GetBranch("ALL", "ALL");
                Session["CodeBranch"] = "ALL";
                BranchCode.Value = "ALL";


                GetBranch2("ALL", "ALL");
                Session["CodeBranch2"] = "ALL";
                BranchCode2.Value = "ALL";

            }
        }

        protected void ddlBranch_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedBranch = ddlBranch.SelectedValue;

            Session["CodeBranch"] = selectedBranch;
            BranchCode.Value = selectedBranch;

        }

        protected void ddlBranch2_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedBranch = ddlBranch.SelectedValue;

            Session["CodeBranch2"] = selectedBranch;
            BranchCode2.Value = selectedBranch;

        }


        protected void ddReport_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        protected DataTable GetBranch(string menu, string selectedValue)
        {
            ddlBranch.Items.Clear();
            var resultTuple = Sp_Branch("GetBranch");
            DataTable Result = resultTuple.data;

          

            if (menu == "ALL")
            {
                ddlBranch.Items.Add(new ListItem("ALL", "ALL"));

                foreach (DataRow dr in Result.Rows)
                {
                    ListItem newItem = new ListItem
                    {
                        Text = dr["CodeBranch"].ToString(),
                        Value = dr["CodeBranch"].ToString()
                    };
                    ddlBranch.Items.Add(newItem);
                }

                // Pilih yang sesuai
                if (ddlBranch.Items.FindByValue(selectedValue) != null)
                    ddlBranch.SelectedValue = selectedValue;
            }


            return Result;
        }



        protected DataTable GetBranch2(string menu, string selectedValue)
        {
            ddlBranch2.Items.Clear();
            var resultTuple = Sp_Branch("GetBranch");
            DataTable Result = resultTuple.data;


            if (menu == "ALL")
            {
                ddlBranch2.Items.Add(new ListItem("ALL", "ALL"));

                foreach (DataRow dr in Result.Rows)
                {
                    ListItem newItem = new ListItem
                    {
                        Text = dr["CodeBranch"].ToString(),
                        Value = dr["CodeBranch"].ToString()
                    };
                    ddlBranch2.Items.Add(newItem);
                }

                // Pilih yang sesuai
                if (ddlBranch2.Items.FindByValue(selectedValue) != null)
                    ddlBranch2.SelectedValue = selectedValue;
            }


            return Result;
        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {




            #region new code 

            System.Web.UI.WebControls.Button btn = (System.Web.UI.WebControls.Button)sender;
            string reportType = btn.CommandArgument;




            string connectionString = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            string periode = "";
            string endperiode = "";
            string branch = "";
            string report = "";
            string sheettype = "";
        
            if (reportType == "PL")
            {
                if (txtDate2.Value != null && txtDate2.Value != "")
                {
                    string a = txtDate2.Value.Replace(" ", "");
                    string[] parts = a.Split('-');

                    periode = parts[1] + parts[0];

                }
                if (txtDate3.Value != null && txtDate3.Value != "")
                {
                    string a = txtDate3.Value.Replace(" ", "");
                    string[] parts = a.Split('-');

                    endperiode = parts[1] + parts[0];

                }
                 branch = BranchCode.Value;
                 sheettype = ddlReport.SelectedValue;
                 report = ddlReport.SelectedValue;

            }
            else
            {

                if (txtDate4.Value != null && txtDate4.Value != "")
                {
                    string a = txtDate4.Value.Replace(" ", "");
                    string[] parts = a.Split('-');

                    periode = parts[1] + parts[0];


                }

                branch = BranchCode2.Value;
                report = "BS";

                if (ddlReport.SelectedValue == "")
                {

                    sheettype = "BS";

                }

            }




            


            //string Reportype = (ddlReport.SelectedValue == "FIN_Seg_PL" || ddlReport.SelectedValue == "PL") ? "P&L" : "BSH" ;           
            DataTable dt = new DataTable();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("Report_BAP", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Periode", periode);
                    cmd.Parameters.AddWithValue("@Branch", branch);
                    cmd.Parameters.AddWithValue("@EndPeriod", endperiode);
                    cmd.Parameters.AddWithValue("@ReportType", report);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }

            string templatePath = "";

            if (report == "FIN_Seg_PL")
            {
                var result = Sp_chekperiode("CheckPeriode", periode, endperiode);

                if (!result.IsSuccess)
                {
                    Response.Cookies.Add(new HttpCookie("fileDownload", "error")
                    {
                        Path = "/",
                        Expires = DateTime.Now.AddMinutes(5)
                    });

                    string message = "Periksa kembali bulan yang dipilih, sesuaikan dengan periode yang aktif!";
                    string js = $"Errorsave('{message}');";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "periodeError", js, true);

                    return; // Sudahi eksekusi tapi tidak memaksa berhenti seperti Response.End
                }
                templatePath = Server.MapPath("~/Templates/Template_Report.xlsx");

            }
            else if (report == "PL")
            {
                templatePath = Server.MapPath("~/Templates/Template_PL.xlsx");

            }
            else if (report == "BS")
            {
                templatePath = Server.MapPath("~/Templates/Template_BS.xlsx");

            }


            FileInfo templateFile = new FileInfo(templatePath);

            using (ExcelPackage package = new ExcelPackage(templateFile))
            {
                // Aktifkan kalkulasi ulang otomatis untuk mendukung formula
                package.Workbook.CalcMode = ExcelCalcMode.Automatic;
                package.Workbook.FullCalcOnLoad = true;

                ExcelWorksheet sheet = package.Workbook.Worksheets[sheettype];

                #region FIN 

                if (sheettype == "FIN_Seg_PL")
                {
                   

                    //var depttype = Sp_dept("MappingByDtType");
                    //DataTable deptdt = depttype.Data;
                    string[] MappingByDtType = {"AIR", "SCS" ,"SEA" ,"CLT" };
                    var ignoreAccounts = new HashSet<string> { "449000", "499000", "500000", "527000", "599000", "600000" };

                    foreach (DataRow row in dt.Rows)
                    {
                        string accountCode = row["AccountCode"].ToString().Trim();
                        string businessName = row["BussinesName"].ToString().Trim();
                        string dtype = row["DtType"].ToString().Trim();
                        string BussCode = row["BussinesCode"].ToString().Trim();
                        decimal amount = Convert.ToDecimal(row["Amount"]);

                        bool isDtTypeMapping = MappingByDtType.Contains(BussCode);
                        int targetRow = -1;
                        int targetCol = -1;

                        if (ignoreAccounts.Contains(accountCode))
                        {
                            continue;
                        }

                        // Cari baris berdasarkan AccountCode di kolom H (kolom ke-8)
                        for (int i = 21; i <= sheet.Dimension.End.Row; i++)
                        {
                            string cellVal =sheet.Cells[i, 8].Text.Trim();
                            if (cellVal.Equals(accountCode, StringComparison.OrdinalIgnoreCase))
                            {
                                targetRow = i;
                                break;
                            }
                        }

                        if (isDtTypeMapping)
                        {
                            for (int j = 11; j <= sheet.Dimension.End.Column; j++)
                            {
                                string groupHeader = sheet.Cells[18, j].Text.Trim();
                                if (groupHeader.Equals(businessName, StringComparison.OrdinalIgnoreCase))
                                {
                                    for (int k = j; k <= sheet.Dimension.End.Column; k++)
                                    {
                                        string nextHeader = sheet.Cells[18, k].Text.Trim();
                                        if (!string.IsNullOrWhiteSpace(nextHeader) && k != j)
                                            break;

                                        string subHeader = sheet.Cells[20, k].Text.Trim();
                                        if (subHeader.Equals(dtype, StringComparison.OrdinalIgnoreCase))
                                        {
                                            targetCol = k;
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        { 
                            for (int j = 29; j <= sheet.Dimension.End.Column; j++)
                            {
                                string header = sheet.Cells[18, j].Text.Trim();
                                if (header.Equals(businessName, StringComparison.OrdinalIgnoreCase))
                                {
                                    targetCol = j;
                                    break;
                                }
                            }
                        }

                        if (targetRow != -1 && targetCol != -1)
                        {
                            var cell = sheet.Cells[targetRow, targetCol];
                            var bgColor = cell.Style.Fill.BackgroundColor;

                            if (bgColor.Theme != null && bgColor.Theme == "1" && Math.Abs((double)bgColor.Tint - 0.5) < 0.01)
                            {
                                continue;
                            }
                            else
                            {
                                cell.Value = amount;
                            }


                            //sheet.Cells[targetRow, targetCol].Value = amount;
                        }

                    }

                    // untuk mapping table baru 
                    var codetab2 = new[] { "449000", "499000", "500000", "527000", "599000", "600000" };
                    var dtfilter = dt.AsEnumerable().Where(x => codetab2.Contains(x.Field<string>("AccountCode"))).ToList();

                    foreach (DataRow row in dtfilter)
                    {

                        string accountCode = row["AccountCode"].ToString().Trim();
                        decimal amount = Convert.ToDecimal(row["Amount"]);
                        int targetRow2 = -1;

                        // baris 
                        for (int i = 21; i <= sheet.Dimension.End.Row; i++)
                        {
                            string cellValue = sheet.Cells[i, 38].Text.Trim();
                            if (cellValue.Equals(accountCode, StringComparison.OrdinalIgnoreCase))
                            {
                                targetRow2 = i;
                                break;
                            }
                        }

                        //langsung isi data
                        if (targetRow2 != -1)
                        {
                            sheet.Cells[targetRow2, 39].Value = amount;

                        }

                    }

                }
                #endregion
                else 
                {
                #region P&L / BSH

                    foreach (DataRow row in dt.Rows)
                    {
                        string accountCode = row["AccountCode"].ToString().Trim();                        
                        decimal amount = Convert.ToDecimal(row["Amount"]);
                        Boolean liability = Convert.ToBoolean(row["isliabilityBs"]);
                        int targetRow3 = -1;

                        // Cari row berdasarkan AccountCode di kolom C (index 3)
                        for (int i = 15; i <= sheet.Dimension.End.Row; i++)
                        {
                            string cellValue = sheet.Cells[i, 13].Text.Trim();
                            if (cellValue.Equals(accountCode, StringComparison.OrdinalIgnoreCase))
                            {
                                targetRow3 = i;
                                break;
                            }
                        }


                        if (targetRow3 != -1)
                        {
                            var currentValue = sheet.Cells[targetRow3, 15].Value;
                            decimal existingAmount = 0;



                            if (currentValue != null && decimal.TryParse(currentValue.ToString(), out var parsed))
                            {
                                existingAmount = parsed;
                            }

                            if (liability == true) 
                            {
                                sheet.Cells[targetRow3, 15].Value = existingAmount - amount;

                            }else
                            {
                                sheet.Cells[targetRow3, 15].Value = existingAmount + amount;

                            }

                           

                        }


                    }
               #endregion

                }                     
                using (MemoryStream ms = new MemoryStream())
                    {
                    string fileName = "";
                    package.SaveAs(ms);
                        ms.Position = 0;
                        
                       if(reportType ==  "PL") { fileName = $"{sheettype}_{branch}({periode} - {endperiode}).xlsx"; } else { fileName = $"{sheettype}_{branch}({periode}).xlsx"; }
                    Response.Cookies.Add(new HttpCookie("fileDownload", "success")
                    {
                        Path = "/",
                        Expires = DateTime.Now.AddMinutes(5)
                    });
                    Response.Clear();
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
                    Response.BinaryWrite(ms.ToArray());
                    Response.End();


                    //Response.Cookies.Add(new HttpCookie("fileDownload", "true")
                    //{
                    //    Path = "/",
                    //    Expires = DateTime.Now.AddMinutes(5)
                    //});
                    //Response.Clear();
                    //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    //Response.AddHeader("content-disposition", $"attachment;filename={fileName}");
                    //Response.BinaryWrite(ms.ToArray());
                    //HttpContext.Current.ApplicationInstance.CompleteRequest(); 

                }
            }
            #endregion



        }


        public (bool isSuccess, string identity, DataTable data) Sp_Branch(string Actionname)
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
                    using (SqlCommand cmd = new SqlCommand("sp_Master_Branch_BAP", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        //cmd.Parameters.AddWithValue("@Account_id", Account_id.Value.ToString());
                        cmd.Parameters.AddWithValue("@Branch_id", (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@NameBranch" ,(object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CodeBranch", (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@Address", (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@CreateBy", (object)DBNull.Value);
                        cmd.Parameters.AddWithValue("@ModifiedBy", (object)DBNull.Value);
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




        public ResultValue  Sp_bussinesmode(string Actionname)
        {
            var result = new ResultValue();


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

            return (result);
        }



        public ResultValue Sp_chekperiode(string Actionname, string periode, string endperiode)
        {
            var result = new ResultValue();


            try
            {
                string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
                using (SqlConnection con = new SqlConnection(path))
                {
                    con.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_chekperiode", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@StatementType", Actionname);
                        cmd.Parameters.AddWithValue("@start", periode);
                        cmd.Parameters.AddWithValue("@end", endperiode);
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

            return (result);
        }





    }
}
