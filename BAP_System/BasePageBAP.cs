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
    public class BasePageBAP : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Context.Session != null && Session.IsNewSession)
            {
                string cookieHeader = Request.Headers["Cookie"];
                if (!string.IsNullOrEmpty(cookieHeader) && cookieHeader.Contains("ASP.NET_SessionId"))
                {
                    Response.Redirect("~/Login.aspx?expired=1&url=" + Server.UrlEncode(Request.RawUrl));
                }
            }
        }


        protected override void OnLoad(EventArgs e)
        {
            if (Session["username_AD"] == null)
            {
                Response.Redirect("~/Login.aspx?url=" + Server.UrlEncode(Request.RawUrl));
                return;
            }

            base.OnLoad(e);
        }


        public static RMA GetAccessForPage(/*string roleId, string asxid*/)
        {
            var access = new RMA();
            string roleId = HttpContext.Current.Session["Role_id"]?.ToString();
            string asxid = HttpContext.Current.Request.AppRelativeCurrentExecutionFilePath.Substring(2);

            string connStr = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_Module_Management_BAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "GetAccessForPage");
                cmd.Parameters.AddWithValue("@Role_id", roleId);
                cmd.Parameters.AddWithValue("@ModuleASPXid", asxid);
                
                    
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    access.CanCreate = (bool)reader["CanCreate"];
                    access.CanEdit = (bool)reader["CanEdit"];
                    access.CanView = (bool)reader["CanView"];
                    access.CanDelete = (bool)reader["CanDelete"];
                }
            }

            return access;
        }



    }
}