using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z_ERP.Models;

namespace Z_ERP.Controllers
{
    public class EmployiesReportsController : Controller
    {
        private MainModel db = new MainModel();
        // GET: EmployiesReports

        public ActionResult Expenses()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        public ActionResult getExpenses(string keys, DateTime? maxDate, DateTime? minDate)
        {
            maxDate =  maxDate >= DateTime.Now ? DateTime.Now : maxDate;
            var expenses = db.hr_Expenses.Where(exp => (exp.ExpensesDescription.Contains(keys) || keys == "-1") && ((exp.ExpensesDate.Value < maxDate) && (exp.ExpensesDate.Value > minDate))).ToList();// && (minDate >= ((int)((DateTime)exp.ExpensesDate).Month)) ).ToList();
            return Json(new { data = expenses }, JsonRequestBehavior.AllowGet);

        }
        public ActionResult Debt()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.Employees = new SelectList(db.hr_Employees, "EmployeeID", "EmployeeFullName");
            return View();
        }
        public ActionResult getDept(int keys, DateTime? maxDate, DateTime? minDate)
        {
            maxDate = maxDate >= DateTime.Now ? DateTime.Now : maxDate;

            return Json(new { data = db.hr_EmployeeDebtRecords.Where(exp => (exp.DebtRecordsEmpoloyeeID == keys || keys == -1) && ((exp.EmployeeDebtRecordsDate.Value < maxDate) && (exp.EmployeeDebtRecordsDate.Value > minDate))).ToList() }, JsonRequestBehavior.AllowGet);
            

        }
    }
}