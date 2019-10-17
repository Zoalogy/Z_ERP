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
using System.Configuration;
using System.Data.SqlClient;

namespace Z_ERP.Controllers
{
    public class TruckGatherReportController : Controller
    {
        private MainModel db = new MainModel();

        // GET: TruckGatherReport
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.TrucksDropDownList = new SelectList(db.trc_Trucks, "TruckID", "TruckNameAr");
            ViewBag.TripsDropDownList = new SelectList(db.trc_Trips, "TruckID", "TruckNameAr");

            return View(await db.trc_Expenses.ToListAsync());
        }


        public JsonResult getTrucksTotalAmount(int truckId, string DateFrom, string DateTo)
        {

            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                DataTable dt = new DataTable();
                List<TrucksReportModel> ExpensesReport = new List<TrucksReportModel>();

                SqlCommand cmd = new SqlCommand("trc_TrucksExpensesGathering", con);
                cmd.Parameters.AddWithValue("@TruckId", truckId);
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    TrucksReportModel obj = new TrucksReportModel();

                    obj.ExpenseAmount = dr["ExpenseAmount"].ToString();
                    obj.TruckNameAr = dr["TruckNameAr"].ToString();



                    ExpensesReport.Add(obj);
                }

                ViewBag.ExpensesReportTemp = ExpensesReport;

                return Json(new { data = ExpensesReport }, JsonRequestBehavior.AllowGet);

            }

        }
        // GET: TruckGatherReport/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Expenses trc_Expenses = await db.trc_Expenses.FindAsync(id);
            if (trc_Expenses == null)
            {
                return HttpNotFound();
            }
            return View(trc_Expenses);
        }

        // GET: TruckGatherReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TruckGatherReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ExpenseID,ExpenseName,ExpenseDescription,ExpenseAmount,TripID,TruckID,UpLoaded,ExpenseDate")] trc_Expenses trc_Expenses)
        {
            if (ModelState.IsValid)
            {
                db.trc_Expenses.Add(trc_Expenses);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(trc_Expenses);
        }

        // GET: TruckGatherReport/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Expenses trc_Expenses = await db.trc_Expenses.FindAsync(id);
            if (trc_Expenses == null)
            {
                return HttpNotFound();
            }
            return View(trc_Expenses);
        }

        // POST: TruckGatherReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ExpenseID,ExpenseName,ExpenseDescription,ExpenseAmount,TripID,TruckID,UpLoaded,ExpenseDate")] trc_Expenses trc_Expenses)
        {
            if (ModelState.IsValid)
            {
                db.Entry(trc_Expenses).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(trc_Expenses);
        }

        // GET: TruckGatherReport/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            trc_Expenses trc_Expenses = await db.trc_Expenses.FindAsync(id);
            if (trc_Expenses == null)
            {
                return HttpNotFound();
            }
            return View(trc_Expenses);
        }

        // POST: TruckGatherReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            trc_Expenses trc_Expenses = await db.trc_Expenses.FindAsync(id);
            db.trc_Expenses.Remove(trc_Expenses);
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
