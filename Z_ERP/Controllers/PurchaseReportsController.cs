using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Z_ERP.Models;
using System.Data.SqlClient;
using System.Configuration;

namespace Z_ERP.Controllers
{
    public class PurchaseReportsController : Controller
    {
        // GET: PurchaseReports

        private MainModel db = new MainModel();

        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Suppliers = new SelectList(db.pur_Supliers, "SuplierID", "SuplierName");
            return View();
        }
        public JsonResult GetBillsReports(bool? BillIsReturnedKay, DateTime? maxDate, DateTime? minDate,int? SuplierID=-1)
        {
            maxDate =  maxDate >= DateTime.Now? DateTime.Now : maxDate;
              List<pur_Bills> bill = db.pur_Bills.Where
                (pu => pu.BillIsReturned == BillIsReturnedKay && ((pu.BillDate.Value <= maxDate) && (pu.BillDate.Value >= minDate))
                && (pu.SuplierID == SuplierID || SuplierID == -1)
                )
                .ToList();
            return Json(new { data = bill }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> BillDetails(string billNumber)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            return View(await db.pur_BillDetails.Where(e => e.BillNo == billNumber).ToListAsync());
        }
    }
}