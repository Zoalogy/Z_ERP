using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Z_ERP.Controllers
{
    public class LogOutController : Controller
    {
        // GET: LogOut
        public ActionResult Index()
        { 

            Session["UserID"] = "";
            Session["UserName"] = "";
            Session["UserEmail"] = "";
            Session["LastLogin"] = "";
            Session.Clear();
            // return RedirectToAction("Index", "Login");
            return RedirectToAction("Index", "Home");
        }
    }
}