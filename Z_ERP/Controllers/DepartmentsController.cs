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
    public class DepartmentsController : Controller
    {
        private MainModel db = new MainModel();

        // GET: Departments
        public async Task<ActionResult> Index()
        {
            return View(await db.hr_Department.ToListAsync());
        }
        public JsonResult getDepartmentsData()
        {
            
            return Json(new { data = db.hr_Department.ToList() }, JsonRequestBehavior.AllowGet);
        }
        // GET: Departments/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Department hr_Department = await db.hr_Department.FindAsync(id);
            if (hr_Department == null)
            {
                return HttpNotFound();
            }
            return View(hr_Department);
        }

        // GET: Departments/Create
        [HttpGet]
        public ActionResult Create(int id)
        {
            var v = db.hr_Department.Where(a => a.DepartmentID == id).FirstOrDefault();
            if (id > 0)
            {
                ViewBag.departmentToUpdate = db.hr_Department.Find(v.DepartmentID).DepartmentNameAr;
                
            }
            return View(v);
        }

        // POST: Departments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(hr_Department Item)
        {
            int status = 0;
            
            if (ModelState.IsValid)
            { 
                if (Item.DepartmentID > 0)
                {

                    var v = db.hr_Department.Where(I => I.DepartmentID == Item.DepartmentID).FirstOrDefault();
                    if (v != null)
                    {
                        v.DepartmentDate = DateTime.Now;
                        v.DepartmentNameAr = Item.DepartmentNameAr;
                        db.SaveChanges();
                        status = 1; // 1 for update 
                    }
                    status = 1; // 1 for update 
                }
                //################### Edit Item //###################

                // Start Save New Item
                else
                {


                    Item.DepartmentDate = DateTime.Now;
                    db.hr_Department.Add(Item);
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
        // POST: Departments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "DepartmentID,DepartmentNameEn,DepartmentNameAr,Uploaded,DepartmentDate")] hr_Department hr_Department)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hr_Department).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hr_Department);
        }

        // GET: Departments/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Department hr_Department = await db.hr_Department.FindAsync(id);
            if (hr_Department == null)
            {
                return HttpNotFound();
            }
            return View(hr_Department);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            hr_Department hr_Department = await db.hr_Department.FindAsync(id);
            db.hr_Department.Remove(hr_Department);
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
