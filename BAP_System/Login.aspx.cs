using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static System.Windows.Forms.LinkLabel;

namespace BAP_System
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "InfoBox();", true);
        }


        protected void btnlogin_Click(object sender, EventArgs e)
        {


            if (IsPostBack)
            {

                bool isexistsAD = IsAuthenticated(txtusername.Value, txtpassword.Value);
                if (isexistsAD)
                {

                    bool isexists = CheckUser(txtusername.Value, txtpassword.Value, 1);
                    if (isexists)
                    {
                        string ReturnUrl = Convert.ToString(Request.QueryString["url"]);
                        if (!string.IsNullOrEmpty(ReturnUrl))
                        {
                            txtFullname.InnerText = Session["username_AD"].ToString();
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SuccessLogin();", true);
                            string _vFullname = txtFullname.InnerText;
                            string script = $@"
                                $(document).ready(function() {{
                                    // Show Toastr notification
                                    toastr.success('Welcome' + ' ' + '{_vFullname}', 'Login Success');

                                    // Redirect after 2 seconds (2000 milliseconds)
                                    setTimeout(function() {{
                                        window.location.href = 'Home.aspx'; // replace with your target URL
                                    }}, 2000);
                                }});
                            ";

                            // Register the script for partial postbacks
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                            Response.Redirect(ReturnUrl);
                        }
                        else
                        {
                            txtFullname.InnerText = Session["username_AD"].ToString();
                            //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "SuccessLogin();", true);
                            string _vFullname = txtFullname.InnerText;
                            string script = $@"
                                $(document).ready(function() {{
                                    // Show Toastr notification
                                    toastr.success('Welcome' + ' ' + '{_vFullname}', 'Login Success');

                                    // Redirect after 2 seconds (2000 milliseconds)
                                    setTimeout(function() {{
                                        window.location.href = 'Home.aspx'; // replace with your target URL
                                    }}, 2000);
                                }});
                            ";

                            // Register the script for partial postbacks
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "ToastrRedirect", script, true);
                        }
                    }
                    else
                    {
                        //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "FailedLogin();", true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Login failed, Username or Password incorrect, please try again!');", true);
                    }

                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "toastrMessage", "toastr.error('Login failed, Please Register Your Active Directory!');", true);


                }


            }



        }



        public bool IsAuthenticated(string username, string password)
       
        {
            string domain = "ylid.local";          
            string domainAndUsername = domain + @"\" + username;
            DirectoryEntry entry = new DirectoryEntry("LDAP://DC=ylid,DC=local", domainAndUsername, password);



            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(sAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {

                    return false;
                }
                else { return true; }
               
            }
            catch (Exception ex)
            {

                
                return false;
            }

        }


        public bool CheckUser(string username, string password, int _vFlag)
        {
            string path = ConfigurationManager.ConnectionStrings["dbpath"].ConnectionString;
            SqlConnection Con = new SqlConnection(path);
            Con.Open();
            SqlCommand sqlcomm = new SqlCommand();
            sqlcomm.CommandText = "sp_Dashboard_Login_BAP";
            sqlcomm.CommandType = CommandType.StoredProcedure;

            sqlcomm.Connection = Con;
            sqlcomm.Parameters.AddWithValue("@Username", username);
            //sqlcomm.Parameters.AddWithValue("@Password", /*password*/Decrypt(password));
            //sqlcomm.Parameters.AddWithValue("@Flag", _vFlag);
            SqlDataReader dr = sqlcomm.ExecuteReader(); ;
            //dr = sqlcomm.ExecuteReader();

            if (dr.Read())
            {

                Session.Add("User_id", (Guid)dr["User_id"]);
                Session.Add("nik", (string)dr["nik"]);
                Session.Add("fullname", (string)dr["Fullname"]);
                Session.Add("Section", (string)dr["Section"].ToString());
                Session.Add("Employees", (Guid)dr["Employees"]);
                Session.Add("Role_id", (Guid)dr["Role_id"]);
                Session.Add("username_AD", (string)dr["username_AD"]);


                return true;
            }
            else
            {
                return false;
            }
        }



    }
}