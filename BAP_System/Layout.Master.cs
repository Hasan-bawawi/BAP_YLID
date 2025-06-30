using DocumentFormat.OpenXml.Presentation;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.EnterpriseServices;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

using static System.Windows.Forms.LinkLabel;

namespace BAP_System
{
    public partial class Layout : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {


            GetDataUser();

            string roleid = Session["Role_id"].ToString();

            LoadDynamicMenus(roleid);

            lbFullname.InnerText = Session["Fullname"].ToString();
            lbNIK.Text = Session["nik"].ToString();
            lbluserid.Text = Session["User_id"].ToString();
            lblidEmployees.Text = Session["Employees"].ToString();
            lblSection.Text = Session["Section"].ToString();

        }

    


        protected void GetDataUser()
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_User_Management_BAP";
            sqlcomm.CommandType = CommandType.StoredProcedure;
            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@StatementType", "CheckUser");
            sqlcomm.Parameters.AddWithValue("@Employessid", Session["Employees"].ToString());
            SqlDataReader dr = null;
            dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {
                Session.Add("Fullname", (string)dr["fullname"]);
            }
            else
            {

            }
        }



        private void LoadDynamicMenus(string roleid)
        {
            string connStr = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            List<string> allowedMenuIds = GetAllowedMenusForRole(roleid);

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_Module_Management_BAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "View");

                SqlDataReader reader = cmd.ExecuteReader();

                var allMenus = new List<dynamic>();

                while (reader.Read())
                {
                  
                    var menuId = reader["Menu_id"].ToString();
                    if (allowedMenuIds.Contains(menuId))
                    {
                        allMenus.Add(new
                        {
                            MenuId = /*reader["Menu_id"].ToString()*/ menuId,
                            ParentId = reader["Parent_id"] == DBNull.Value ? null : reader["Parent_id"].ToString(),
                            Name = reader["ModuleName"].ToString(),
                            Href = reader["ModuleASPXid"].ToString(),
                            Submenu1 = (bool)reader["Submenu1"],
                            Submenu2 = (bool)reader["Submenu2"],
                            Submenu3 = (bool)reader["Submenu3"]

                        });

                    }
                }


                // Ambil menuId berdasarkan nama menu
                var adminMenuId = allMenus.FirstOrDefault(m => m.Name.Equals("ADMIN", StringComparison.OrdinalIgnoreCase))?.MenuId;
                var transaksiMenuId = allMenus.FirstOrDefault(m => m.Name.Equals("BAP MENU", StringComparison.OrdinalIgnoreCase))?.MenuId;
                var reportMenuId = allMenus.FirstOrDefault(m => m.Name.Equals("REPORT", StringComparison.OrdinalIgnoreCase))?.MenuId;

                Dictionary<string, PlaceHolder> rootMenuMapping = new Dictionary<string, PlaceHolder>();

                if (adminMenuId != null)
                    rootMenuMapping[adminMenuId] = adminMenuPlaceholder;

                if (transaksiMenuId != null)
                    rootMenuMapping[transaksiMenuId] = transaksiMenuPlaceholder;

                if (reportMenuId != null)
                    rootMenuMapping[reportMenuId] = reportMenuPlaceholder;

                
                var rootMenus = allMenus.Where(m => m.ParentId == null);

                foreach (var root in rootMenus)
                {
                   
                    if (rootMenuMapping.TryGetValue(root.MenuId, out PlaceHolder targetPlaceholder))
                    {
                        var menuControl = BuildRecursiveMenu(root, allMenus);
                        targetPlaceholder.Controls.Add(menuControl);
                    }
                }


            }
        }



        private List<string> GetAllowedMenusForRole(string roleId)
        {
            List<string> allowedMenuIds = new List<string>();

            string connStr = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;

            using (SqlConnection con = new SqlConnection(connStr))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("sp_Module_Management_BAP", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StatementType", "GetAllowMenus");
                cmd.Parameters.AddWithValue("@Role_id", roleId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    allowedMenuIds.Add(reader["Menu_id"].ToString());
                }
            }

            return allowedMenuIds;
        }



        private HtmlGenericControl BuildRecursiveMenu(dynamic item, List<dynamic> allMenus)
        {
            HtmlGenericControl li = new HtmlGenericControl("li");

            // Tentukan icon berdasarkan nama
            string iconClass = "fa fa-circle-o"; // default
            string nameUpper = item.Name.ToString().ToUpperInvariant();

            if (nameUpper == "ADMIN")
            {
                iconClass = "fa fa-users";
            }
            else if (nameUpper == "BAP MENU")
            {
                iconClass = "fa fa-list";
            }
            else if (nameUpper == "REPORT")
            {
                iconClass = "fa fa-bar-chart";
            }

            HtmlAnchor a = new HtmlAnchor
            {
                HRef = string.IsNullOrEmpty(item.Href) ? "javascript:void(0)" : item.Href,
                InnerHtml = $"<i class='{iconClass}' style='margin-right:8px;'></i><span class='nav-text'>{item.Name}</span>"
            };

            // Cari child
            var children = allMenus
                .Where(x => x.ParentId == item.MenuId)
                .ToList();

            if (children.Any())
            {
                a.Attributes["class"] = "has-arrow";
                li.Controls.Add(a);

                HtmlGenericControl ul = new HtmlGenericControl("ul");
                ul.Attributes["aria-expanded"] = "false";

                foreach (var child in children)
                {
                    ul.Controls.Add(BuildRecursiveMenu(child, allMenus));
                }

                li.Controls.Add(ul);
            }
            else
            {
                li.Controls.Add(a);
            }

            return li;
        }


        protected void Logout_Click(object sender, EventArgs e)
        {
            Session.Clear();           
            Session.Abandon();          
            Response.Cookies.Clear();  
            Response.Redirect("Login.aspx");
        }



    }
}