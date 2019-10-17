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
    public class TripsController : Controller
    {
        private MainModel db = new MainModel();

        // GET: Trips
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.TrucksDropDownList = new SelectList(db.trc_Trucks, "TruckID", "TruckNameAr");
            return View(await db.trc_Trips.ToListAsync());
        }

        public JsonResult GetTripsData(int TruckID)
        {
            List<trc_Trips> Trips = db.trc_Trips.Where(T => T.TruckID == TruckID).ToList();


            Session["TruckID"] = TruckID;
            return Json(new { data = Trips }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save(int id)
        {
            var v = db.trc_Trips.Where(a => a.TripID == id).FirstOrDefault();

            ViewBag.StatusDropDownList = new SelectList(db.trc_TripStatus, "TripStatusID", "TripStatusName");


            return View(v);

        }


        [HttpPost]
        public ActionResult Save(trc_Trips Trips, string[] name, string[] Amount, string[] expenseDate)
        {
            int status = 0;


            // Start Validation cheack
            if (ModelState.IsValid)
            {

                //################### Edit Item ###################//
                if (Trips.TripID > 0)
                {

                    var v = db.trc_Trips.Where(I => I.TripID == Trips.TripID).FirstOrDefault();
                    if (v != null)
                    {
                        v.TripID = Trips.TripID;
                        v.TripName = Trips.TripName;
                        v.TripDescription = Trips.TripDescription;
                        v.TripStartDate = Trips.TripStartDate;
                        v.TripStatusID = Trips.TripStatusID;

                        status = 1; // 1 for update 
                        db.SaveChanges();
                    }
                }
                //################### Edit Item //###################

                // Start Save New Item
                else
                {
                    //Suplire.SuplierID = int.Parse(Session["ii"].ToString());

                    int TempTruckID = int.Parse(Session["TruckID"].ToString());
                    Trips.TruckID = TempTruckID;
                    db.trc_Trips.Add(Trips);
                    db.SaveChanges();

                    int TempTripId = Trips.TripID;


                    // Insert Trip Expenses
                    for (int x = 0; x < name.Length; x++)
                    {

                        var TempName = name[x];
                        var TempAmount = Amount[x];
                        var TempexpenseDate = DateTime.Now;

                        if ( String.IsNullOrEmpty(expenseDate[x]))
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
                            Functions.Functions.InsertTruckExpense(TempName, int.Parse(TempAmount), TempexpenseDate, TempTruckID, TempTripId);

                            // Payent History record
                            var discreption = "Truck " + TempTruckID + " Expense";
                            Functions.Functions.InsertPaymentHistory(0, int.Parse(TempAmount), discreption, 2);

                        }

                        if (String.IsNullOrEmpty(TempName) || String.IsNullOrEmpty(TempAmount))
                        {
                             if(String.IsNullOrEmpty(name[x]))
                            {
                                TempName = " ";
                            }

                            if (String.IsNullOrEmpty(Amount[x]))
                            {
                                TempAmount = "0";
                            }
                            Functions.Functions.InsertTruckExpense(TempName, int.Parse(TempAmount), TempexpenseDate, TempTruckID, TempTripId);

                            // Payent History record
                            var discreption = "Truck " + TempTruckID + " Expense";
                            Functions.Functions.InsertPaymentHistory(0, int.Parse(TempAmount), discreption, 2);
                        }
                        

                       

                    }



                    status = 2;  // 2 fro 
                }
                //End Save New Item


                return new JsonResult { Data = new { status = status } };

            }
            // End Validation cheack



            // Start Send validation error
            else
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();

                return new JsonResult { Data = new { status = errorList } };
            }
            // End  Send validation error



        }

       
        public ActionResult SaveNewExpenses(int id)
        {
            var v = db.trc_Trips.Where(a => a.TripID == id).FirstOrDefault();

            return View(v);
        }

        [HttpPost]
        public ActionResult SaveNewExpenses(int? TripID,string[] name, string[] Amount, string[] expenseDate)
        {

            int status =0;

            if(TripID == null)
            {
                TripID = 0;
            }

            var v = db.trc_Trips.Where(a => a.TripID == TripID).FirstOrDefault();
           var  TempTruckID = v.TruckID;
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
                    Functions.Functions.InsertTruckExpense(TempName, int.Parse(TempAmount), TempexpenseDate,TempTruckID, TripID);

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



        // GET: Trips/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Trips trc_Trips = await db.trc_Trips.FindAsync(id);

            var Expenses = db.trc_Expenses.Where(T => T.TripID == id).ToList();
            ViewBag.Expenses = db.trc_Expenses.Where(T => T.TripID == id).ToList();

            int ExpensesTotalAmount = 0;
            foreach (var Expense in Expenses)
            {
                ExpensesTotalAmount = ExpensesTotalAmount+ (int) Expense.ExpenseAmount;

            }

            ViewBag.ExpensesTotalAmount = String.Format("{0:n}", ExpensesTotalAmount);

            if (trc_Trips == null)
            {
                return HttpNotFound();
            }

            return View(trc_Trips);
        }

        // GET: Trips/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Trips/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "TripID,TripName,TruckID,TripDescription,UpLoaded,TripStartDate,TripEndDate,TripStatusID")] trc_Trips trc_Trips)
        {
            if (ModelState.IsValid)
            {
                db.trc_Trips.Add(trc_Trips);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(trc_Trips);
        }

        // GET: Trips/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Trips trc_Trips = await db.trc_Trips.FindAsync(id);
            if (trc_Trips == null)
            {
                return HttpNotFound();
            }
            return View(trc_Trips);
        }

        // POST: Trips/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "TripID,TripName,TruckID,TripDescription,UpLoaded,TripStartDate,TripEndDate,TripStatusID")] trc_Trips trc_Trips)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trc_Trips).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(trc_Trips);
        }

        // GET: Trips/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Trips trc_Trips = await db.trc_Trips.FindAsync(id);
            if (trc_Trips == null)
            {
                return HttpNotFound();
            }
            return View(trc_Trips);
        }

        // POST: Trips/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            trc_Trips trc_Trips = await db.trc_Trips.FindAsync(id);
            db.trc_Trips.Remove(trc_Trips);
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
