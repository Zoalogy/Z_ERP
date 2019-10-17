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
    public class SalesDetailReportController : Controller
    {
        private MainModel db = new MainModel();

        // GET: SalesDetailReport
        public async Task<ActionResult> Index()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.TrucksDropDownList = new SelectList(db.trc_Trucks, "TruckID", "TruckNameAr");
            ViewBag.TripsDropDownList = new SelectList(db.trc_Trips, "TruckID", "TruckNameAr");

            return View(await db.sal_Sales.ToListAsync());
        }


        public JsonResult SalesDetailReports(int ItemID, string RecieptNo, string DateFrom, string DateTo)
        {

            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                DataTable dt = new DataTable();
                List<SalesReportModel> SlaesReport = new List<SalesReportModel>();

                SqlCommand cmd = new SqlCommand("sal_Report_Detail", con);
                cmd.Parameters.AddWithValue("@SaleItemID", ItemID);
                cmd.Parameters.AddWithValue("@RecieptNo", RecieptNo);
                cmd.Parameters.AddWithValue("@DateFrom", Convert.ToDateTime(DateFrom));
                cmd.Parameters.AddWithValue("@DateTo", Convert.ToDateTime(DateTo));
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                foreach (DataRow dr in dt.Rows)
                {
                    SalesReportModel obj = new SalesReportModel();

                    obj.ItemName = dr["ItemName"].ToString();
                    obj.RecieptNo = dr["RecieptNo"].ToString();
                    obj.SaleQuantity = decimal.Parse( dr["SaleQuantity"].ToString());
                    obj.ItemSalePrice = decimal.Parse(dr["ItemSalePrice"].ToString());
                    obj.ItemTotalSaleAmount = decimal.Parse(dr["ItemTotalSaleAmount"].ToString());
                    obj.SaleDate =  DateTime.Parse (dr["SaleDate"].ToString());
                    
                    SlaesReport.Add(obj);
                }

                ViewBag.SlaesReportTemp = SlaesReport;

                return Json(new { data = SlaesReport }, JsonRequestBehavior.AllowGet);

            }

        }

        // GET: SalesDetailReport/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Sales sal_Sales = await db.sal_Sales.FindAsync(id);
            if (sal_Sales == null)
            {
                return HttpNotFound();
            }
            return View(sal_Sales);
        }

        // GET: SalesDetailReport/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesDetailReport/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SaleID,SalePrice,RecieptNo,SalesItemsID,ItemName,ItemSalePrice,ItemPurchasePrice,SaleQuantity,ItemTotalSaleAmount,SaleIsReturned,SaleReturnedDate,SaleDate,UpLoaded")] sal_Sales sal_Sales)
        {
            if (ModelState.IsValid)
            {
                db.sal_Sales.Add(sal_Sales);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_Sales);
        }

        // GET: SalesDetailReport/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Sales sal_Sales = await db.sal_Sales.FindAsync(id);
            if (sal_Sales == null)
            {
                return HttpNotFound();
            }
            return View(sal_Sales);
        }

        // POST: SalesDetailReport/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SaleID,SalePrice,RecieptNo,SalesItemsID,ItemName,ItemSalePrice,ItemPurchasePrice,SaleQuantity,ItemTotalSaleAmount,SaleIsReturned,SaleReturnedDate,SaleDate,UpLoaded")] sal_Sales sal_Sales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_Sales).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_Sales);
        }

        // GET: SalesDetailReport/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Sales sal_Sales = await db.sal_Sales.FindAsync(id);
            if (sal_Sales == null)
            {
                return HttpNotFound();
            }
            return View(sal_Sales);
        }

        // POST: SalesDetailReport/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            sal_Sales sal_Sales = await db.sal_Sales.FindAsync(id);
            db.sal_Sales.Remove(sal_Sales);
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
