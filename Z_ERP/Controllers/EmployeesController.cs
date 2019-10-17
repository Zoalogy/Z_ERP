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
using System.Diagnostics;

namespace Z_ERP.Controllers
{
    public class EmployeesController : Controller
    {
        private MainModel db = new MainModel();
        string NameErrow = "";
        // GET: Employees
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.hr_Employees.ToListAsync());
        }
        public  JsonResult GetEmployeesData()
        {
            var employee = db.hr_Employees.ToList();
            var subCategoryToReturn = employee.Select(S => new hr_employeesToReturn
            {
                EmployeeID = S.EmployeeID,
                EmployeeFullName = S.EmployeeFullName,
                EmployeePhone = S.EmployeePhone,
                EmployeeAddress = S.EmployeeAddress,
                EmployeeDepartmentID = db.hr_Department.Find(S.EmployeeDepartmentID).DepartmentNameAr,
                EmployeeJobID = db.hr_JobsName.Find(S.EmployeeJobID).JobNameAr,
                EmployeeStatus = db.hr_EmployeeSatus.Find(S.EmployeeStatus).EmployeeSatusAr 
            });
            return Json(new { data = subCategoryToReturn.ToList() }, JsonRequestBehavior.AllowGet);
        }
        ////////////////////////////////////
        [HttpGet]
        public ActionResult Save(int id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            var v = db.hr_Employees.Where(a => a.EmployeeID == id).FirstOrDefault();
            if(id > 0 ) { 
                ViewBag.EmployeeDepartment_Toupdat = db.hr_Department.Find(v.EmployeeDepartmentID).DepartmentID;
                ViewBag.EmployeeJob_Toupdate = db.hr_JobsName.Find(v.EmployeeJobID).JobNameID;
                ViewBag.EmployeeStatus_TOupdate = db.hr_EmployeeSatus.Find(v.EmployeeStatus).EmployeeSatusId;
                ViewBag.Pos_TOupdate = db.sal_pointOfSale.Find(v.PointOfsaleID)==null?0: db.sal_pointOfSale.Find(v.PointOfsaleID).PointOfSaleID ;
            }
            ViewBag.NameErrow = NameErrow;
            //var selectedCategory = v.CategoryID;
            //inv_Categories.SelectListItem = new SelectList(db.inv_Items, "CategoryID", "CategoryNameAR", v.CategoryID);


            //ViewBag.BrandID = New SelectList(db.Brand, "Id", "BrandName", id.ToString())
            //ViewBag.selectedCategory = inv_Categories.SelectListItem = new SelectList(CategoriesModel.inv_Categories, "CategoryID", "CategoryNameAR");
            ViewBag.Pos = 1;
            ViewBag.EmployeePos = new SelectList(db.sal_pointOfSale, "PointOfSaleID", "PointOfSaleName");
            ViewBag.EmployeeStatus = new SelectList(db.hr_EmployeeSatus, "EmployeeSatusId", "EmployeeSatusAr");
            ViewBag.EmployeeJob = new SelectList(db.hr_JobsName, "JobNameID", "JobNameAr");
            ViewBag.EmployeeDeprtment = new SelectList(db.hr_Department, "DepartmentID", "DepartmentNameAr");
            ViewBag.EmployeeGender = new SelectList(db.hr_Gender, "GenderId", "GenderNmaeAr");
            //ViewBag.EmployeeDeprtment = new SelectList(db.hr_Department.Where(emp => (emp.DepartmentID == 0)),"DepartmentID", "DepartmentNameAr");


            return View(v);

        }


        [HttpPost]
        public ActionResult Save(hr_Employees Item,FormCollection form)
        {
            int status = 0;
            if (!EmployeeValidation(Item))
            {
                NameErrow  = "الرجاء إدخال إسم الموظف";
                ViewBag.NameErrow = NameErrow;
                return View();
            }

            // Start Validation cheack
            if (ModelState.IsValid)
            {

                //################### Edit Item ###################//
                if (Item.EmployeeID > 0)
                {

                    var v = db.hr_Employees.Where(I => I.EmployeeID == Item.EmployeeID).FirstOrDefault();
                    if (v != null)
                    {
                        v.EmployeeID = Item.EmployeeID;
                        v.EmployeeFullName = Item.EmployeeFullName;
                        v.EmployeeDepartmentID = Item.EmployeeDepartmentID;
                        v.EmployeeJobID = Item.EmployeeJobID;
                        v.EmployeePhone = Item.EmployeePhone;
                        v.EmployeeAddress = Item.EmployeeAddress;
                        v.PointOfsaleID = Item.PointOfsaleID;
                        v.EmployeeStatus = Item.EmployeeStatus ?? 1;
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
                    Item.EmployeeStatus = 1;
                    Item.EmployeeGender = 1;
                    db.hr_Employees.Add(Item);
                    db.SaveChanges();
                    var salary = new hr_EmployeeSalary();
                    salary.EmployeeID = Item.EmployeeID;
                    salary.EmployeesSalarydate = DateTime.Now;
                    salary.EmployeesSalary = Double.Parse(db.hr_JobsName.Find(Item.EmployeeJobID).JobNameBasicSalary.ToString());
                    db.hr_EmployeeSalary.Add(salary);
                    db.SaveChanges();
                    status = 2;  // 2 fro 
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

        private bool EmployeeValidation(hr_Employees item)
        {
            if(item.EmployeeFullName==null)
            {
                return false;
            }


            return true;
        }

        ///////////////////////////////////
        ///
        // GET: Employees/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Employees hr_Employees = await db.hr_Employees.FindAsync(id);
            if (hr_Employees == null)
            {
                return HttpNotFound();
            }
            return View(hr_Employees);
        }

        // GET: Employees/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "EmployeeID,EmployeeFullName,EmployeeUserName,EmployeePassword,EmployeeAddress,EmployeeAcountNo,EmployeeBankBranch,EmployeeGender,EmployeeMaritelStatus,EmployeeDepartmentID,EmployeeJobID,EmployeeStatus,EmployeeStatusDate,RegisterationDate,EmployeeLastLogin,EmployeeEmail,EmployeePhone")] hr_Employees hr_Employees)
        {
            if (ModelState.IsValid)
            {
                db.hr_Employees.Add(hr_Employees);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(hr_Employees);
        }

        // GET: Employees/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Employees hr_Employees = db.hr_Employees.Find(id);
            //////////////////////
            var v = db.hr_EmployeeSalary.Where(a => a.EmployeeID == id).FirstOrDefault();
            ViewBag.PureSalary = ((float)v.EmployeesSalary + v.EmployeesSalaryAllownace) - v.EmployeesSalarydeducate;//( (db.hr_EmpDedicationAllowance.Where(v => v.DedicationAllowanceListID == 1 && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0)) - db.hr_EmpDedicationAllowance.Where(v => v.DedicationAllowanceListID == 2 && v.EmployeeID == salary.EmployeeID).Sum(i => (double?)i.EmpBatchValue) ?? 0,;
            ViewBag.EmployeesSalaryAllownace = v.EmployeesSalaryAllownace;
            ViewBag.EmployeesSalarydeducate = v.EmployeesSalarydeducate;
            //////////////////////
            ViewBag.EmployeeDepartment = db.hr_Department.Find(hr_Employees.EmployeeDepartmentID).DepartmentNameAr;
                ViewBag.EmployeeJob = db.hr_JobsName.Find(hr_Employees.EmployeeJobID).JobNameAr;
                ViewBag.EmployeeStatus = db.hr_EmployeeSatus.Find(hr_Employees.EmployeeStatus).EmployeeSatusAr;
            ViewBag.BasicSalary = db.hr_JobsName.Find(hr_Employees.EmployeeJobID).JobNameBasicSalary; 
            if (hr_Employees == null)
            {
                return HttpNotFound();
            }
            return View(hr_Employees);
        }
       

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "EmployeeID,EmployeeFullName,EmployeeUserName,EmployeePassword,EmployeeAddress,EmployeeAcountNo,EmployeeBankBranch,EmployeeGender,EmployeeMaritelStatus,EmployeeDepartmentID,EmployeeJobID,EmployeeStatus,EmployeeStatusDate,RegisterationDate,EmployeeLastLogin,EmployeeEmail,EmployeePhone")] hr_Employees hr_Employees)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hr_Employees).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hr_Employees);
        }

        // GET: Employees/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Employees hr_Employees = await db.hr_Employees.FindAsync(id);
            if (hr_Employees == null)
            {
                return HttpNotFound();
            }
            return View(hr_Employees);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            hr_Employees hr_Employees = await db.hr_Employees.FindAsync(id);
            db.hr_Employees.Remove(hr_Employees);
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
