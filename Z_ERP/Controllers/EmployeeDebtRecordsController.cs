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

namespace Z_ERP.Controllers
{
    public class EmployeeDebtRecordsController : Controller
    {
        private MainModel db = new MainModel();

        // GET: EmployeeDebtRecords
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.hr_EmployeeDebtRecords.ToListAsync());
        }
        public JsonResult GetDebtsData()
        {
            
            return Json(new { data = db.hr_EmployeeDebtRecords.ToList() }, JsonRequestBehavior.AllowGet);
        }
        // GET: EmployeeDebtRecords/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmployeeDebtRecords hr_EmployeeDebtRecords = await db.hr_EmployeeDebtRecords.FindAsync(id);
            if (hr_EmployeeDebtRecords == null)
            {
                return HttpNotFound();
            }
            return View(hr_EmployeeDebtRecords);
        }

        // GET: EmployeeDebtRecords/Create
        public ActionResult Create(int id)
        {

            var v = db.hr_EmployeeDebtRecords.Where(a => a.DebtRecordsID == id).FirstOrDefault();
            if (id > 0)
            {
                ViewBag.ExpensesDescriptionToUpdate = db.hr_EmployeeDebtRecords.Find(v.DebtRecordsID).DebtRecordsDescription;
                ViewBag.ExpensesAmountToUpdate = db.hr_EmployeeDebtRecords.Find(v.DebtRecordsID).DebtRecordsEmployeeName;
            }
            ViewBag.Employees = new SelectList(db.hr_Employees, "EmployeeID", "EmployeeFullName");

            return View(v);
        }
        [HttpPost]
        public ActionResult Create(hr_EmployeeDebtRecords Item, FormCollection form)
        {
            int status = 0;


            // Start Validation cheack
            if (ModelState.IsValid)
            {

                //################### Edit Item ###################//
                if (Item.DebtRecordsID > 0)
                {

                    var v = db.hr_EmployeeDebtRecords.Where(I => I.DebtRecordsID == Item.DebtRecordsID).FirstOrDefault();
                    if (v != null)
                    {
                        //v.EmployeeDebtRecordsDate = DateTime.Now;
                        v.DebtRecordsDescription = Item.DebtRecordsDescription;
                        db.SaveChanges();
                        status = 1; // 1 for update 
                    }
                    status = 1; // 1 for update 
                }
                //################### Edit Item //###################

                // Start Save New Item
                else
                {
                    //Item.EmployeeJobID = .;
                    //Item.EmployeeDebtRecordsDate = DateTime.Now;
                    Item.AdminName = Session["UserName"].ToString();
                    Item.DebtRecordsEmployeeName = db.hr_Employees.Find(Item.DebtRecordsEmpoloyeeID).EmployeeFullName;
                    db.hr_EmployeeDebtRecords.Add(Item);
                    db.SaveChanges();
                    status = 2;  // 2 fro 
                    Functions.Functions.InsertPaymentHistory(0, (decimal?)Item.DebtRecordsAmount ?? 0, Item.DebtRecordsDescription + " سلفية  ", 0);
                    
                }
                //End Save New Item

                return new JsonResult { Data = new { status = status } };

            }
            // End Validation cheack


            // Start Send validation error
            else
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors)
                                 .Select(e => e.ErrorMessage)
                                 .ToList();

                return new JsonResult { Data = new { status = errorList } };
            }
            // End  Send validation error

        }

        // POST: EmployeeDebtRecords/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmployeeDebtRecords hr_Employees = db.hr_EmployeeDebtRecords.Find(id);
            if (hr_Employees == null)
            {
                return HttpNotFound();
            }
            return View(hr_Employees);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            hr_Employees hr_Employees =  db.hr_Employees.Find(id);
            db.hr_Employees.Remove(hr_Employees);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
