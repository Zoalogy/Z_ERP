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
    public class PointOfSalesStocktakingController : Controller
    {
        private MainModel db = new MainModel();

        // GET: PointOfSalesStocktaking
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.sal_Sales.ToListAsync());
        }

        public JsonResult GetStocktaking()
        {



            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            //List<string> ItemsList = new List<string>();
            IEnumerable<string> ItemsList;

            List<DataRow> list = new List<DataRow>();

            using (SqlConnection con = new SqlConnection(constr))
            {

                DataSet ds = new DataSet();
                using (SqlCommand cmd = new SqlCommand("sal_Stocktaking"))
                {

                    var PointOfSaleID = Session["PointOfSaleID"].ToString();

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PointOfSaleID", PointOfSaleID);


                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.Connection = con;
                    con.Open();
                    adp.Fill(ds);
                    var dt1 = ds.Tables[0];
                     

                    List<SalesReportModel> SlaesReport = new List<SalesReportModel>();


                    decimal ItemTotalSaleAmount = 0;
                    decimal TotalSaleQuantity = 0;

                    if (dt1.Rows.Count > 0)
                    {

                        foreach (DataRow item in dt1.Rows)
                        {
                            SalesReportModel obj = new SalesReportModel();
                            obj.ItemName = item["SaleItemsNameAr"].ToString();
                            obj.PointOfSaleName = item["PointOfSaleName"].ToString();
                            obj.ItemSalePrice = decimal.Parse(item["ItemSalePrice"].ToString());
                            obj.SaleQuantity = decimal.Parse(item["SaleItemsQuantity"].ToString());
                            obj.ItemTotalSaleAmount = decimal.Parse(item["TotalItemPrice"].ToString());
                            SlaesReport.Add(obj);

                            ItemTotalSaleAmount += decimal.Parse(item["TotalItemPrice"].ToString());
                            TotalSaleQuantity += decimal.Parse(item["SaleItemsQuantity"].ToString());
                        }

                    }

                   

                    //                    ViewBag.TotalSaleQuantity = TotalSaleQuantity;


                    return Json(new { data = SlaesReport, TotalSaleQuantity = TotalSaleQuantity, ItemTotalSaleAmount = ItemTotalSaleAmount }, JsonRequestBehavior.AllowGet);

                }
            }

        }

        // GET: PointOfSalesStocktaking/Details/5
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

        // GET: PointOfSalesStocktaking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PointOfSalesStocktaking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SaleID,SalePrice,RecieptNo,SalesItemsID,ItemName,ItemSalePrice,ItemPurchasePrice,SaleQuantity,ItemTotalSaleAmount,SaleIsReturned,SaleReturnedDate,SaleDate,PointOfSaleID,UpLoaded")] sal_Sales sal_Sales)
        {
            if (ModelState.IsValid)
            {
                db.sal_Sales.Add(sal_Sales);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_Sales);
        }

        // GET: PointOfSalesStocktaking/Edit/5
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

        // POST: PointOfSalesStocktaking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SaleID,SalePrice,RecieptNo,SalesItemsID,ItemName,ItemSalePrice,ItemPurchasePrice,SaleQuantity,ItemTotalSaleAmount,SaleIsReturned,SaleReturnedDate,SaleDate,PointOfSaleID,UpLoaded")] sal_Sales sal_Sales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_Sales).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_Sales);
        }

        // GET: PointOfSalesStocktaking/Delete/5
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

        // POST: PointOfSalesStocktaking/Delete/5
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
