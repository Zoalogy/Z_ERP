using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z_ERP.Models;
namespace Z_ERP.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(UserLogin user)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow Row;

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand MyCommnd = new SqlCommand("sys_EmployeeLogin", con);
                MyCommnd.CommandType = CommandType.StoredProcedure;
                MyCommnd.Parameters.Add("@UserNameOrUserEmail", SqlDbType.VarChar).Value = user.Username;
                MyCommnd.Parameters.Add("@UserPassword", SqlDbType.VarChar).Value = user.Password ; //new Function().Encrypt(txtPassword.Text);
                SqlDataAdapter adp = new SqlDataAdapter(MyCommnd);
                DataSet ds = new DataSet();
                // string temp = "";
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                adp.Fill(ds);
                //string MenuString = "";
                Session["MenuString"] = "";
                DataTable tb1 = ds.Tables[0];
                DataTable tb2 = ds.Tables[1];

                if ((tb1.Rows.Count > 0))
                {
                    Row = tb1.Rows[0];
                    Session["UserID"] = Row[0].ToString();
                    Session["UserName"] = Row[1].ToString();
                    Session["UserEmail"] = Row[2].ToString();
                    Session["LastLogin"] = Row[3].ToString();
                    Session["EmployeeDepartmentID"] = Row["EmployeeDepartmentID"].ToString();


                    // PointOfSaleID 

                    Session["PointOfSaleID"] =  "50";

                    string active = "active";

                    Session["FullName"] = Row["FullName"].ToString();

                    if ((tb2.Rows.Count > 0))
                    {
                        for (int index = 0; index <= tb2.Rows.Count - 1; index++)
                        {
                            Row = tb2.Rows[index];
                            if ((Row[4].ToString().Equals("1")))
                            {
                                string title = "";
                                title = "<li class='" + active + "treeview'>" +
                                        "<a href='#'>" +
                                        "<i class='" + Row[3].ToString() + "'></i> <span>" + Row[1].ToString() + "</span> <i class='fa fa-angle-left pull-left'></i>" +
                                        "</a>" +
                                        "<ul class='treeview-menu'>";
                                Session["MenuString"] += title;
                                Session["MenuString"] += getSubMenu(Row[0].ToString());
                                Session["MenuString"] += "</ul></li>";
                                active = "";
                            }
                            else if (Row[4].ToString().Equals("0"))
                            {
                                string url = Row[0].ToString() + "#" + Row[1].ToString();
                               // string EncryptedURL = new Function().Encrypt_ASCII(url);
                                if (!string.IsNullOrEmpty(active))
                                {
                                    active = "class='active'";
                                }
                                string child = "<li " + active + "><a  href='" + Row[2].ToString() +  "'><i class='" + Row[3].ToString() + "'></i><span>" + Row[1].ToString() + "</span></a></li>";
                                // string child = "<li><a href='" + Row[2].ToString() + "?id=" + EncryptedURL + "'><i class='" + Row[3].ToString() + "'></i>" + Row[1].ToString() + "</a></li>";
                                //string Child = "<li><a href='" + Row[2].ToString() + "' data-i18n='nav.menu_levels.second_level' class='menu-item'>" + Row[1].ToString() + "</a></li>";
                                Session["MenuString"] += child;
                                // Session["MobileMenuString"] += child;
                                active = "";
                            }

                        }

                    }




                    // Session["MenuString"] = MenuString;
                    // Response.Redirect("Dashboard.aspx");
                 //   return View("Home");
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    // AlertDIV.Visible = true;
                   // Label1.Text = "UserName Or Password  is Wrong";
                }

                con.Close();
            }


            
            return View();
        }
        private string getSubMenu(string menuId)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            System.Data.DataTable dt = new System.Data.DataTable();
            DataRow Row;
            string temp = "";
            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand MyCommnd = new SqlCommand("wp_GetSubMenu", con);
                MyCommnd.CommandType = CommandType.StoredProcedure;
                MyCommnd.Parameters.Add("@MenuID", SqlDbType.VarChar).Value = menuId;
                MyCommnd.Parameters.Add("@UserName", SqlDbType.VarChar).Value = Session["UserName"];

                SqlDataAdapter adp = new SqlDataAdapter(MyCommnd);
                adp.Fill(dt);
                if ((dt.Rows.Count > 0))
                {

                    for (int index = 0; index <= dt.Rows.Count - 1; index++)
                    {
                        Row = dt.Rows[index];
                        string url = Row[0].ToString() + "#" + Row[1].ToString();
                        //string EncryptedURL = new Function().Encrypt_ASCII(url);

                        //temp += " <li><a  href='" + Row[2].ToString() + "?id=" + EncryptedURL + "'>" + Row[1].ToString() + "</a> </li>";
                        temp += "<li><a href='" + Row[2].ToString() +  "'><i class='" + Row[3].ToString() + "'></i>" + Row[1].ToString() + "</a></li>";

                    }
                }


            }


            return temp;
        }
        public ActionResult LogOut()
        {
            Session["UserID"] = "";
            Session["UserName"] = "";
            Session["UserEmail"] = "";
            Session["LastLogin"] = "";
            Session.Clear();
            // return RedirectToAction("Index", "Login");
            return RedirectToAction("Index");
        }
    }
   
}