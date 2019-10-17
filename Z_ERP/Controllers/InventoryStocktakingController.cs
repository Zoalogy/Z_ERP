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
    public class InventoryStocktakingController : Controller
    {
        private MainModel db = new MainModel();

        // GET: InventoryStocktaking
        public async Task<ActionResult> Index()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.inv_Items.ToListAsync());
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
                using (SqlCommand cmd = new SqlCommand("inv_Stocktaking"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.Connection = con;
                    con.Open();
                    adp.Fill(ds);
                    var dt1 = ds.Tables[0];


                    // ItemsList = dt1.ToList();

                    //   list = dt1ToList();

                    //var dt2 = ds.Tables[1];

                    List<SalesReportModel> SlaesReport = new List<SalesReportModel>();


                    decimal ItemTotalSaleAmount =0;
                    decimal TotalSaleQuantity = 0;

                    if (dt1.Rows.Count > 0)
                    {
                        
                        foreach (DataRow item in dt1.Rows)
                        {
                            SalesReportModel obj = new SalesReportModel();
                            obj.ItemName = item["ItemNameAr"].ToString();
                            obj.ItemSalePrice = decimal.Parse(item["ItemSalePrice"].ToString());
                            obj.SaleQuantity = decimal.Parse (item["ItemQuantity"].ToString());
                            obj.ItemTotalSaleAmount = decimal.Parse (item["TotslItemPrice"].ToString());
                            SlaesReport.Add(obj);

                           ItemTotalSaleAmount +=  decimal.Parse(item["TotalItemPrice"].ToString());
                            TotalSaleQuantity += decimal.Parse(item["ItemQuantity"].ToString());
                        }

                    }

                    Session["TotalSaleQuantity"] = TotalSaleQuantity;
                    Session["ItemTotalSaleAmount"] = ItemTotalSaleAmount;

//                    ViewBag.TotalSaleQuantity = TotalSaleQuantity;


                    return Json(new { data = SlaesReport, TotalSaleQuantity= TotalSaleQuantity, ItemTotalSaleAmount= ItemTotalSaleAmount }, JsonRequestBehavior.AllowGet);

                }
            }

        }

        // GET: InventoryStocktaking/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Items inv_Items = await db.inv_Items.FindAsync(id);
            if (inv_Items == null)
            {
                return HttpNotFound();
            }
            return View(inv_Items);
        }

        // GET: InventoryStocktaking/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryStocktaking/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemID,ItemBatch,ItemNameAr,ItemNameEn,ItemQuantity,ItemMinimumQuantity,ItemSalePrice,ItemSaleCurrencyID,ItemPuchasePrice,ItemPurchaseCurrencyID,InventoryID,CategoryID,SubCategoryID,ItemExpiredDate,ItemAddedDate,ItemUpdatedAt,UpLoaded,BarCode")] inv_Items inv_Items)
        {
            if (ModelState.IsValid)
            {
                db.inv_Items.Add(inv_Items);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(inv_Items);
        }

        // GET: InventoryStocktaking/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Items inv_Items = await db.inv_Items.FindAsync(id);
            if (inv_Items == null)
            {
                return HttpNotFound();
            }
            return View(inv_Items);
        }

        // POST: InventoryStocktaking/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemID,ItemBatch,ItemNameAr,ItemNameEn,ItemQuantity,ItemMinimumQuantity,ItemSalePrice,ItemSaleCurrencyID,ItemPuchasePrice,ItemPurchaseCurrencyID,InventoryID,CategoryID,SubCategoryID,ItemExpiredDate,ItemAddedDate,ItemUpdatedAt,UpLoaded,BarCode")] inv_Items inv_Items)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_Items).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inv_Items);
        }

        // GET: InventoryStocktaking/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Items inv_Items = await db.inv_Items.FindAsync(id);
            if (inv_Items == null)
            {
                return HttpNotFound();
            }
            return View(inv_Items);
        }

        // POST: InventoryStocktaking/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            inv_Items inv_Items = await db.inv_Items.FindAsync(id);
            db.inv_Items.Remove(inv_Items);
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
