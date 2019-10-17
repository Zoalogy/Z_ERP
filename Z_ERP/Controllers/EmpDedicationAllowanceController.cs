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
    public class EmpDedicationAllowanceController : Controller
    {
        private MainModel db = new MainModel();

        // GET: EmpDedicationAllowance
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var dedicationAllowanceList = db.hr_EmpDedicationAllowance.ToList();
            ViewBag.AlownaceOrDeducation = new SelectList(db.hr_DedicationOrAllowance, "DedicationOrAllowanceID", "DedicationOrAllowanceNameAr");
            ViewBag.Employees = new SelectList(db.hr_Employees, "EmployeeID", "EmployeeFullName");
            ViewBag.DorA = new SelectList(db.hr_DedicationAllowanceList.Where(n => n.DeductionAllowanceID == 1), "DeductionAllowanceListID", "DeductionAllowanceNmaeAr");
            return View();
        }
        public ActionResult GetDeducationAndAllownaceData(int? empId=-1,int? CategoryID=-1)
        {
            var dedicationAllowanceList = db.hr_EmpDedicationAllowance.Where(e => (e.EmployeeID == empId || empId == -1) && (CategoryID == -1 || db.hr_DedicationAllowanceList.Any( l=>l.DeductionAllowanceListID == e.DedicationAllowanceListID && l.DeductionAllowanceID == CategoryID ))).ToList();
            var subCategoryToReturn = dedicationAllowanceList.Select(S => new hr_EmpDedicationAllowanceReturn
            {
                EmpDedicationAllowanceID = S.EmpDedicationAllowanceID,
                DedicationAllowanceListName = db.hr_DedicationAllowanceList.Find(S.DedicationAllowanceListID).DeductionAllowanceNmaeAr,
                EmployeeName = db.hr_Employees.Find(S.EmployeeID).EmployeeFullName,
                EmpBatchValue = S.EmpBatchValue + "",
                EmpBatchDate = S.EmpBatchDate + ""
            });
            return Json(new { data = subCategoryToReturn.ToList() }, JsonRequestBehavior.AllowGet);
        }
        ////////////////////////////////////

        public JsonResult GetSubCategory(int? CategoryID, int? EmpID)
        {
            
            //inv_Items inv_Items = db.inv_Items.Where(item => (item.CategoryID == CategoryID && item.ItemID == ItemID) );

            //var y = db.hr_DedicationAllowanceList.Where(item => (item.DeductionAllowanceID == CategoryID)).ToArray();

            return Json(new SelectList(db.hr_DedicationAllowanceList.Where(n => n.DeductionAllowanceID == CategoryID &&  !db.hr_EmpDedicationAllowance.Any(h => (h.DedicationAllowanceListID == n.DeductionAllowanceListID && h.EmployeeID == EmpID))), "DeductionAllowanceListID", "DeductionAllowanceNmaeAr"), JsonRequestBehavior.AllowGet);
        }


        /// <summary>
        /// /////////////////
        /// </summary>
        /// <param name="id"></param>
        /// 
        /// <returns></returns>
        
        







    /////////////////////////////
    [HttpGet]
        public ActionResult Save(int id)
        {

            var v = db.hr_EmpDedicationAllowance.Where(a => a.EmpDedicationAllowanceID == id).FirstOrDefault();
            if (id > 0)
            {
                ViewBag.EmployeesToUpdate = db.hr_Employees.Find(v.EmployeeID).EmployeeFullName;
                ViewBag.DorA_Toupdate = db.hr_DedicationAllowanceList.Find(v.DedicationAllowanceListID).DeductionAllowanceNmaeAr;
                ViewBag.AlownaceOrDeducation_Toupdate = db.hr_DedicationOrAllowance.Find(db.hr_DedicationAllowanceList.Find(v.DedicationAllowanceListID).DeductionAllowanceID).DedicationOrAllowanceNameAr;
            }
            ViewBag.AlownaceOrDeducation = new SelectList(db.hr_DedicationOrAllowance, "DedicationOrAllowanceID", "DedicationOrAllowanceNameAr");
            ViewBag.Employees = new SelectList(db.hr_Employees, "EmployeeID", "EmployeeFullName");
            ViewBag.DorA = new SelectList(db.hr_DedicationAllowanceList.Where(n => n.DeductionAllowanceID == 1), "DeductionAllowanceListID", "DeductionAllowanceNmaeAr");
            return View(v);

        }

        public void updateSalary(int? allownaceID,int VBatchvalue,int EBatchvalue,int? employeeID=0,int? type=0)
        {
            
           
            var salary = db.hr_EmployeeSalary.Where(I => I.EmployeeID == employeeID).FirstOrDefault();
            if (type == 1)
            {
                if (VBatchvalue > EBatchvalue)
                {
                    salary.EmployeesSalaryAllownace = salary.EmployeesSalaryAllownace - (Double)(VBatchvalue - EBatchvalue);
                }
                else if (VBatchvalue < EBatchvalue)
                {
                    salary.EmployeesSalaryAllownace = salary.EmployeesSalaryAllownace + (Double)(VBatchvalue - EBatchvalue);
                }

            }
            else
            {
                if (VBatchvalue > EBatchvalue)
                {
                    salary.EmployeesSalarydeducate = salary.EmployeesSalarydeducate + (Double)(VBatchvalue - EBatchvalue);
                }
                else if (VBatchvalue < EBatchvalue)
                {
                    salary.EmployeesSalarydeducate = salary.EmployeesSalarydeducate + (Double)(VBatchvalue - EBatchvalue);
                }
            }
            
            /////////////////////////////////////////////////
            db.SaveChanges();
        }
        [HttpPost]
        public ActionResult Save(hr_EmpDedicationAllowance Item)
        {
            int status = 0;


            // Start Validation cheack
            if (ModelState.IsValid)
            {
                var type = db.hr_DedicationAllowanceList.Find(Item.DedicationAllowanceListID).DeductionAllowanceID;
                //################### Edit Item ###################//
                if (Item.EmpDedicationAllowanceID > 0)
                {

                    var v = db.hr_EmpDedicationAllowance.Where(I => I.EmpDedicationAllowanceID == Item.EmpDedicationAllowanceID).FirstOrDefault();
                    if (v != null)
                    {
                        updateSalary(Item.DedicationAllowanceListID,(int)v.EmpBatchValue, (int)Item.EmpBatchValue,Item.EmployeeID,type);
                        v.EmpBatchValue = Item.EmpBatchValue;
                        db.SaveChanges();
                        status = 1; // 1 for update 
                        

                        ///////////////////////////////

                    }
                    status = 1; // 1 for update 
                }
                //################### Edit Item //###################

                // Start Save New Item
                else
                {
                    Item.EmpBatchDate = DateTime.Now;
                    db.hr_EmpDedicationAllowance.Add(Item);
                    db.SaveChanges();
                    /////////////////////////////////////////////
                    var salary = new hr_EmployeeSalary();
                    
                    salary = db.hr_EmployeeSalary.Where(I => I.EmployeeID == Item.EmployeeID).FirstOrDefault();
                    if (type == 1)
                    {
                        salary.EmployeesSalaryAllownace = salary.EmployeesSalaryAllownace + (Double)Item.EmpBatchValue;
                    }
                    else
                    {
                        salary.EmployeesSalarydeducate = salary.EmployeesSalarydeducate + (Double)Item.EmpBatchValue;
                    }

                    db.SaveChanges();
                    status = 2; 
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
        ///////////////////////////////////
        // GET: EmpDedicationAllowance/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmpDedicationAllowance hr_EmpDedicationAllowance = await db.hr_EmpDedicationAllowance.FindAsync(id);
            if (hr_EmpDedicationAllowance == null)
            {
                return HttpNotFound();
            }
            return View(hr_EmpDedicationAllowance);
        }

        // GET: EmpDedicationAllowance/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: EmpDedicationAllowance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmpDedicationAllowanceID,DedicationAllowanceListID,EmployeeID,EmpBatchValue,EmpBatchDate")] hr_EmpDedicationAllowance hr_EmpDedicationAllowance)
        {
            if (ModelState.IsValid)
            {
                db.hr_EmpDedicationAllowance.Add(hr_EmpDedicationAllowance);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hr_EmpDedicationAllowance);
        }

        // GET: EmpDedicationAllowance/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmpDedicationAllowance hr_EmpDedicationAllowance = await db.hr_EmpDedicationAllowance.FindAsync(id);
            if (hr_EmpDedicationAllowance == null)
            {
                return HttpNotFound();
            }
            return View(hr_EmpDedicationAllowance);
        }

        // POST: EmpDedicationAllowance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmpDedicationAllowanceID,DedicationAllowanceListID,EmployeeID,EmpBatchValue,EmpBatchDate")] hr_EmpDedicationAllowance hr_EmpDedicationAllowance)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hr_EmpDedicationAllowance).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hr_EmpDedicationAllowance);
        }

        // GET: EmpDedicationAllowance/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_EmpDedicationAllowance hr_EmpDedicationAllowance = await db.hr_EmpDedicationAllowance.FindAsync(id);
            if (hr_EmpDedicationAllowance == null)
            {
                return HttpNotFound();
            }
            return View(hr_EmpDedicationAllowance);
        }

        // POST: EmpDedicationAllowance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            hr_EmpDedicationAllowance hr_EmpDedicationAllowance = await db.hr_EmpDedicationAllowance.FindAsync(id);
            db.hr_EmpDedicationAllowance.Remove(hr_EmpDedicationAllowance);
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
