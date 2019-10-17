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
    public class EmployeeSalaryController : Controller
    {
        private MainModel db = new MainModel();

        // GET: EmployeeSalary
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.hr_EmployeeSalary.ToListAsync());
        }
        public ActionResult GetSalaryData(int month)
        {

            var query =
              from salary in db.hr_EmployeeSalary
              join employee in db.hr_Employees on salary.EmployeeID equals employee.EmployeeID
              join jobID in db.hr_JobsName on employee.EmployeeJobID equals jobID.JobNameID
              //join EDorA in db.hr_EmpDedicationAllowance on employee.EmployeeID equals EDorA.EmployeeID
              //join DorAL in db.hr_DedicationAllowanceList on EDorA.DedicationAllowanceListID equals DorAL.DeductionAllowanceListID
              //join DorA in db.hr_DedicationOrAllowance on DorAL.DeductionAllowanceID equals DorA.DedicationOrAllowanceID
              //join sh in db.hr_SalaryHistory on employee.EmployeeID equals sh.ImployeeID
              //where ()
              where (!db.hr_SalaryHistory.Any(h => (h.ImployeeID == salary.EmployeeID)) && salary.EmployeesSalarydate.Month == month)
              select new hr_EmployeesSalaryReturn
              {
                  EmployeeName = employee.EmployeeFullName,
                  //Allownaces = db.hr_EmpDedicationAllowance.Where(v =>  (v.DedicationAllowanceListID == db.hr_DedicationAllowanceList.FirstOrDefault(i => i.DeductionAllowanceID == 1).DeductionAllowanceListID)   && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0,//salary.EmployeesSalaryAllownace,,
                  //Deducations = db.hr_EmpDedicationAllowance.Where(v => (v.DedicationAllowanceListID == db.hr_DedicationAllowanceList.FirstOrDefault(i => i.DeductionAllowanceID == 2).DeductionAllowanceListID) && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0,//salary.EmployeesSalaryAllownace,,
                  //Allownaces = db.hr_EmpDedicationAllowance.Where(v => ((db.hr_DedicationAllowanceList.Find(v.DedicationAllowanceListID).DeductionAllowanceID)==1)&& v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0,//salary.EmployeesSalaryAllownace,
                  //Deducations = db.hr_EmpDedicationAllowance.Where(v => v.DedicationAllowanceListID == 2 && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0,//salary.EmployeesSalaryAllownace,,
                  EmployeesBasicSalary = (float)jobID.JobNameBasicSalary,
                  Allownaces = salary.EmployeesSalaryAllownace,
                  Deducations = salary.EmployeesSalarydeducate,
                  EmployeesExpenses = (double?) db.hr_EmployeeDebtRecords.Where(ex => ex.DebtRecordsEmpoloyeeID == employee.EmployeeID).Sum(e =>  e.DebtRecordsAmount) ?? 0,
                  EmployeesSalary =((float)jobID.JobNameBasicSalary + salary.EmployeesSalaryAllownace)- (salary.EmployeesSalarydeducate+((double?)db.hr_EmployeeDebtRecords.Where(ex => ex.DebtRecordsEmpoloyeeID == employee.EmployeeID).Sum(e => e.DebtRecordsAmount) ?? 0)),//( (db.hr_EmpDedicationAllowance.Where(v => v.DedicationAllowanceListID == 1 && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0)) - db.hr_EmpDedicationAllowance.Where(v => v.DedicationAllowanceListID == 2 && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0,
                  Id = employee.EmployeeID,
              };
            return Json(new { data = query.ToList() }, JsonRequestBehavior.AllowGet);
        }
        

        /// ////////////////////////////////////////////////////////
        public ActionResult Save(int id)
        {
            var v = db.hr_EmployeeSalary.Where(a => a.EmployeeID == id).FirstOrDefault();
            var em = db.hr_Employees.Where(a => a.EmployeeID == id).First();
            ViewBag.employeeName = em.EmployeeFullName;
            ViewBag.employeeAddress = em.EmployeeAddress;
            ViewBag.EmployeesExpenses = (double?)db.hr_EmployeeDebtRecords.Where(ex => ex.DebtRecordsEmpoloyeeID == v.EmployeeID).Sum(e => e.DebtRecordsAmount) ?? 0;
            ViewBag.PureSalary = ((float)v.EmployeesSalary + v.EmployeesSalaryAllownace) -( v.EmployeesSalarydeducate+((double?)db.hr_EmployeeDebtRecords.Where(ex => ex.DebtRecordsEmpoloyeeID == v.EmployeeID).Sum(e => e.DebtRecordsAmount) ?? 0));//( (db.hr_EmpDedicationAllowance.Where(v => v.DedicationAllowanceListID == 1 && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0)) - db.hr_EmpDedicationAllowance.Where(v => v.DedicationAllowanceListID == 2 && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0,;
            ViewBag.phone = em.EmployeePhone;
            return View(v);

        }
        [HttpPost]
        public ActionResult Save(hr_EmployeeSalary Item)
        {
            int status = 0;


            // Start Validation cheack
            if (ModelState.IsValid)
            {

                //################### Edit Item ###################//
                //Item.EmployeeJobID = .;
                var salary = new hr_SalaryHistory();
               Item= db.hr_EmployeeSalary.Where(a => a.EmployeeID == Item.EmployeeID).FirstOrDefault();
                salary.ImployeeID = Item.EmployeeID ?? 0;
                salary.SalaryHistoryAllownaceAmount = Item.EmployeesSalaryAllownace;
                salary.SalaryHistoryAmount = Item.EmployeesSalary ?? 0;
                salary.SalaryHistoryDeducationAmount = Item.EmployeesSalarydeducate;
                salary.SalaryHistoryProcduerDate = DateTime.Now;
                salary.SalaryHistoryDate = Item.EmployeesSalarydate;
                salary.ImployeeName = db.hr_Employees.Find(Item.EmployeeID).EmployeeFullName;
                db.hr_SalaryHistory.Add(salary);
                    db.SaveChanges();
                ////////////////////
                var employeeDebtRecords = db.hr_EmployeeDebtRecords.Where(h=> h.DebtRecordsEmpoloyeeID == Item.EmployeeID);
                
                foreach (var item in employeeDebtRecords.Where(x => employeeDebtRecords.Any(y => y.DebtRecordsEmpoloyeeID == Item.EmployeeID)).ToList())
                {
                    db.hr_EmployeeDebtRecords.Remove(item);
                }
                
                 db.SaveChanges();
                status = 2;  // 2 fro 
             
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

        ////////////////////////////////////////////////////////////////////
        // GET: EmployeeSalary/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmployeeSalary hr_EmployeeSalary = await db.hr_EmployeeSalary.FindAsync(id);
            if (hr_EmployeeSalary == null)
            {
                return HttpNotFound();
            }
            return View(hr_EmployeeSalary);
        }

        // GET: EmployeeSalary/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmployeeSalary/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,EmployeeID,EmployeesSalary,EmployeesSalarydate")] hr_EmployeeSalary hr_EmployeeSalary)
        {
            if (ModelState.IsValid)
            {
                db.hr_EmployeeSalary.Add(hr_EmployeeSalary);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hr_EmployeeSalary);
        }

        // GET: EmployeeSalary/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmployeeSalary hr_EmployeeSalary = await db.hr_EmployeeSalary.FindAsync(id);
            if (hr_EmployeeSalary == null)
            {
                return HttpNotFound();
            }
            return View(hr_EmployeeSalary);
        }

        // POST: EmployeeSalary/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,EmployeeID,EmployeesSalary,EmployeesSalarydate")] hr_EmployeeSalary hr_EmployeeSalary)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hr_EmployeeSalary).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hr_EmployeeSalary);
        }

        // GET: EmployeeSalary/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmployeeSalary hr_EmployeeSalary = await db.hr_EmployeeSalary.FindAsync(id);
            if (hr_EmployeeSalary == null)
            {
                return HttpNotFound();
            }
            return View(hr_EmployeeSalary);
        }

        // POST: EmployeeSalary/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            hr_EmployeeSalary hr_EmployeeSalary = await db.hr_EmployeeSalary.FindAsync(id);
            db.hr_EmployeeSalary.Remove(hr_EmployeeSalary);
            await db.SaveChangesAsync();
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
