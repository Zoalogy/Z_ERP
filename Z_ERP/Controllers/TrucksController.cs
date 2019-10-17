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
    public class TrucksController : Controller
    {
        private MainModel db = new MainModel();

        // GET: Trucks
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.trc_Trucks.ToListAsync());
        }


        public ActionResult GetTrucksData()
        {


            List<trc_Trucks> Trucks = db.trc_Trucks.ToList();

              

            return Json(new { data = Trucks }, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public ActionResult Save(int id)
        {
            var v = db.trc_Trucks.Where(a => a.TruckID == id).FirstOrDefault();

            ViewBag.DriverDropDownList = new SelectList(db.hr_Employees, "EmployeeFullName", "EmployeeFullName");
            ViewBag.DriverAssistantDropDownList = new SelectList(db.hr_Employees, "EmployeeFullName", "EmployeeFullName");
            
            return View(v);
        }


        [HttpPost]
        public ActionResult Save(trc_Trucks Truck)
        {
            int status = 0;

            // Start Validation cheack
            if (ModelState.IsValid)
            {


                //################### Edit Truck ###################//
                if (Truck.TruckID > 0)
                {

                    var v = db.trc_Trucks.Where(I => I.TruckID == Truck.TruckID).FirstOrDefault();
                    if (v != null)
                    {
                        var TruckNumber = db.trc_Trucks.Where(I => I.TruckNumber == Truck.TruckNumber && I.TruckID != v.TruckID).FirstOrDefault();
                        if (TruckNumber != null)
                        {
                            status = 3; // error -> trucks exist 
                        }

                        else
                        {
                            v.TruckID = Truck.TruckID;
                            v.TruckNameAr = Truck.TruckNameAr;
                            v.TruckNumber = Truck.TruckNumber;
                            v.DriverName = Truck.DriverName;
                            v.DriverAssistant = Truck.DriverAssistant;

                            status = 1; // 1 for update 
                            db.SaveChanges();
                        }

                        
                    }
                } //  Edit Truck


                //###################  Save New Truck ###################
                else
                {

                    var TruckNumber = db.trc_Trucks.Where(I => I.TruckNumber == Truck.TruckNumber).FirstOrDefault();
                    if (TruckNumber != null)
                    {
                        status = 3; // error -> trucks exist 
                    }

                    else
                    {
                        db.trc_Trucks.Add(Truck);
                        db.SaveChanges();
                        status = 2;  // 2 save new trucks 
                    }
                   
                }
                // Save New Truck

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

        // GET: Trucks/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Trucks trc_Trucks = await db.trc_Trucks.FindAsync(id);
            if (trc_Trucks == null)
            {
                return HttpNotFound();
            }
            return View(trc_Trucks);
        }


        public ActionResult SaveGeneralExpenses(int? id)
        {
            var v = db.trc_Trucks.Where(a => a.TruckID == id).FirstOrDefault();

            return View(v);
        }

        [HttpPost]
        public ActionResult SaveGeneralExpenses(int TruckID, string[] name, string[] Amount, string[] expenseDate)
        {

            int status = 0;
            int TripID = 0;

            var TempTruckID = TruckID;
            for (int x = 0; x < name.Length; x++)
            {
                var TempName = name[x];

                var TempAmount = Amount[x];
                var TempexpenseDate = DateTime.Now;

                if (String.IsNullOrEmpty(expenseDate[x]))
                {
                    TempexpenseDate = DateTime.Now;

                }
                else
                {
                    TempexpenseDate = DateTime.Parse(expenseDate[x]);
                }

                // Expencese not Empty
                if (!String.IsNullOrEmpty(TempName) && !String.IsNullOrEmpty(TempAmount))
                {
                    Functions.Functions.InsertTruckExpense(TempName, int.Parse(TempAmount), TempexpenseDate, TempTruckID, TripID);

                    // Payent History record
                    var discreption = "Truck " + TempTruckID + " Expense";
                    Functions.Functions.InsertPaymentHistory(0, int.Parse(TempAmount), discreption, 2);

                }

                if (String.IsNullOrEmpty(TempName) || String.IsNullOrEmpty(TempAmount))
                {
                    if (String.IsNullOrEmpty(name[x]))
                    {
                        TempName = " ";
                    }

                    if (String.IsNullOrEmpty(Amount[x]))
                    {
                        TempAmount = "0";
                    }
                    Functions.Functions.InsertTruckExpense(TempName, int.Parse(TempAmount), TempexpenseDate, TempTruckID, TripID);

                    // Payent History record
                    var discreption = "Truck " + TempTruckID + " Expense";
                    Functions.Functions.InsertPaymentHistory(0, int.Parse(TempAmount), discreption, 2);
                }

            }


            return new JsonResult { Data = new { status = status } };



        }
        // GET: Trucks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trucks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TruckID,TruckNameAr,TruckNameEn,TruckNumber,DriverName,DriverAssistant,Status,UpLoaded,TruckAddedDate")] trc_Trucks trc_Trucks)
        {
            if (ModelState.IsValid)
            {
                db.trc_Trucks.Add(trc_Trucks);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(trc_Trucks);
        }

        // GET: Trucks/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Trucks trc_Trucks = await db.trc_Trucks.FindAsync(id);
            if (trc_Trucks == null)
            {
                return HttpNotFound();
            }
            return View(trc_Trucks);
        }

        // POST: Trucks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TruckID,TruckNameAr,TruckNameEn,TruckNumber,DriverName,DriverAssistant,Status,UpLoaded,TruckAddedDate")] trc_Trucks trc_Trucks)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trc_Trucks).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(trc_Trucks);
        }

        // GET: Trucks/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Trucks trc_Trucks = await db.trc_Trucks.FindAsync(id);
            if (trc_Trucks == null)
            {
                return HttpNotFound();
            }
            return View(trc_Trucks);
        }

        // POST: Trucks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            trc_Trucks trc_Trucks = await db.trc_Trucks.FindAsync(id);
            db.trc_Trucks.Remove(trc_Trucks);
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
