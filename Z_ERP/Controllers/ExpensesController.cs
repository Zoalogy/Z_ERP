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
    public class ExpensesController : Controller
    {
        
        private MainModel db = new MainModel();
        // GET: Expenses
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.hr_Expenses.ToListAsync());
        }
        public  JsonResult getExpensesData()
        {
            var expenses = db.hr_Expenses.ToList();
            return Json(new { data = expenses }, JsonRequestBehavior.AllowGet);
        }
        // GET: Expenses/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Expenses hr_Expenses = await db.hr_Expenses.FindAsync(id);
            if (hr_Expenses == null)
            {
                return HttpNotFound();
            }
            return View(hr_Expenses);
        }

        // GET: Expenses/Create
        public ActionResult Create(int id)
        {

            var v = db.hr_Expenses.Where(a => a.ExpensesID == id).FirstOrDefault();
            if (id > 0)
            {
                ViewBag.ExpensesDescriptionToUpdate = db.hr_Expenses.Find(v.ExpensesID).ExpensesDescription;
                ViewBag.ExpensesAmountToUpdate = db.hr_Expenses.Find(v.ExpensesID).ExpensesDescription;
            }
           

            return View(v);

        }


        // POST: Expenses/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(hr_Expenses Item)
        {
            int status = 0;

            System.Diagnostics.Debug.Write(Item.ExpensesDate + "awad sa");
            // Start Validation cheack
            if (ModelState.IsValid)
            {

                //################### Edit Item ###################//
                //if (Item.ExpensesID > 0)
                //{

                //    var v = db.hr_Expenses.Where(I => I.ExpensesID == Item.ExpensesID).FirstOrDefault();
                //    if (v != null)
                //    {
                //        v.ExpensesDate = DateTime.Now;
                //        v.ExpensesDescription = Item.ExpensesDescription;
                //        db.SaveChanges();
                //        status = 1; // 1 for update 
                //    }
                //    status = 1; // 1 for update 
                //}
                ////################### Edit Item //###################

                //// Start Save New Item
                //else
                //{

                //Item.ExpensesDate = Item.ExpensesDate;

                Item.EmployeeName = Session["UserName"].ToString();
                Item.PointOfSaleID = Convert.ToInt32(Session["PointOfSaleID"].ToString());
                    db.hr_Expenses.Add(Item);
                    db.SaveChanges();
                Functions.Functions.InsertPaymentHistory(0,(decimal?)Item.ExpensesAmount??0,Item.ExpensesDescription +" منصرف  ",1);
                  status = 2;  // 2 fro 
                //}
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
        // GET: Expenses/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Expenses hr_Expenses = await db.hr_Expenses.FindAsync(id);
            if (hr_Expenses == null)
            {
                return HttpNotFound();
            }
            return View(hr_Expenses);
        }

        // POST: Expenses/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ExpensesID,ExpensesAmount,ExpensesDescription,EmployeeName,Uploaded,ExpensesDate")] hr_Expenses hr_Expenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(hr_Expenses).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(hr_Expenses);
        }

        // GET: Expenses/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            hr_Expenses hr_Expenses = await db.hr_Expenses.FindAsync(id);
            if (hr_Expenses == null)
            {
                return HttpNotFound();
            }
            return View(hr_Expenses);
        }

        // POST: Expenses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            hr_Expenses hr_Expenses = await db.hr_Expenses.FindAsync(id);
            db.hr_Expenses.Remove(hr_Expenses);
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
