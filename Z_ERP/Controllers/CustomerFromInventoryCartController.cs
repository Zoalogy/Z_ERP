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
using System.Data.SqlClient;
using System.Configuration;

namespace Z_ERP.Controllers
{
    public class CustomerFromInventoryCartController : Controller
    {
        private MainModel db = new MainModel();

        // GET: CustomerFromInventoryCart
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }


            var PointOfSaleID = Session["PointOfSaleID"].ToString();
            var InvertoriesDropDownList = db.inv_Inventory    // your starting point - table in the "from" statement
                                          .Join(db.inv_Items, // the source table of the inner join
                                             inv_Inventory => inv_Inventory.InvertoryID,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
                                             inv_Items => inv_Items.InventoryID,   // Select the foreign key (the second part of the "on" clause)
                                             (inv_Inventory, inv_Items) => new { Items = inv_Items, Inventory = inv_Inventory }) // selection
                                          .Where(InventoryAndItem => InventoryAndItem.Items.ItemQuantity > 0).ToList();

            List<inv_Inventory> tempList = new List<inv_Inventory>();
            foreach (var item in InvertoriesDropDownList)
            {
                var temp = item.Inventory;
                tempList.Add(temp);
            }

            ViewBag.InvertoriesDropDownList = new SelectList(tempList, "InvertoryID", "InvertoryNameAr");

          //  ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            ViewBag.ItemsDropDownList = new SelectList(db.inv_Items.Where(SubCategory => (SubCategory.CategoryID == 0)), "ItemID", "ItemNameAr");
            ViewBag.CustomersDropDownList = db.sal_Customer.ToList();


            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");
            ViewBag.Customers = new SelectList(db.sal_Customer, "CustomerID", "CustomerName");



            //return View(await db.inv_RequestCart.Where(r => (r.RequestType == 2)).ToListAsync());

            return View(await db.sal_SalesCart.Where(SaleCart => (SaleCart.SalesCartType == 2)).ToListAsync());
        }



        public JsonResult ComplateInventoryRequestOrder(string OrderpaymentAmount, string ItemIDArray, string NamesArray, string ItemPriceArray, string QuantitiesArray)
        {


            int LastRequest;

            List<inv_Requests> LastList = db.inv_Requests.ToList();
            if (LastList == null)
            {
                LastRequest = 1;
            }
            else
            {
                LastRequest = db.inv_Requests.Max(item => item.RequestID); ;
            }

            var OrderNumber = "SAL" + DateTime.Now.ToString("yyyyMMdd") + (LastRequest + 1);

            int[] ItemIDs = Array.ConvertAll(ItemIDArray.Split(','), int.Parse);
            string[] Names = NamesArray.Split(',');
            int[] ItemsSellPrice = Array.ConvertAll(ItemPriceArray.Split(','), int.Parse);


            //int[] ItemspurchasePrice = Array.ConvertAll(ItempurchasePriceArray.Split(','), int.Parse);
            int[] Quantities = Array.ConvertAll(QuantitiesArray.Split(','), int.Parse);


            // ################################################ //
            //        Phase 1: Insert Inventory Request Items Order //
            // ################################################ //
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("inv_Insert_RequestOrder"))
                {

                    //ItemsSellPrice.Sum();

                    var TotatItmesAmount = ItemsSellPrice.Sum();
  
                    var PointOfSaleID = Session["PointOfSaleID"];
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@RequestSourceTypeID", 2);
                    cmd.Parameters.AddWithValue("@RequestSourceID", PointOfSaleID);
                    cmd.Parameters.AddWithValue("@RequestSourceName", ' ');
                    cmd.Parameters.AddWithValue("@RequestDestinationID", 0);
                    cmd.Parameters.AddWithValue("@RequestNo", OrderNumber);
                    cmd.Parameters.AddWithValue("@RequestTotalSaleAmount", TotatItmesAmount);

                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();


                }
            }



            // ################################################ //
            //     Phase 2: Insert Inventory  Items  Request    //
            // ################################################ //
            using (SqlConnection con2 = new SqlConnection(constr))
            {

                for (int i = 0; i < Names.Length; i++)
                {
                    using (SqlCommand cmd2 = new SqlCommand("inv_Insert_RequestItems"))
                    {

                        var name = Names[i];
                        var quantity = Quantities[i];
                        var amount = ItemsSellPrice[i];
                        var itemID = ItemIDs[i];

                        inv_Items inv_Items = db.inv_Items.Find(itemID);
                        // var name = inv_Items.ItemNameAr;
                        var purchasePrice = inv_Items.ItemPuchasePrice;
                        var CategoryID = inv_Items.CategoryID;
                        var ItemSalePrice = inv_Items.ItemSalePrice;
                        var ItemPurshasePrice = inv_Items.ItemPuchasePrice;
                         
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@RequestNo", OrderNumber);
                        cmd2.Parameters.AddWithValue("@ItemID", itemID);
                        cmd2.Parameters.AddWithValue("@ItemNameAr", name);
                        cmd2.Parameters.AddWithValue("@ItemQuantity", quantity);
                        cmd2.Parameters.AddWithValue("@ItemSalePrice", ItemSalePrice);
                        cmd2.Parameters.AddWithValue("@ItemPurshasePrice", ItemPurshasePrice);
                        cmd2.Parameters.AddWithValue("@CategoryID", CategoryID);                    
                        cmd2.Parameters.AddWithValue("@ItemTotalAmount", amount);
                        
                        cmd2.Connection = con2;
                        con2.Open();
                        cmd2.ExecuteNonQuery();
                        con2.Close();

                        //   Functions.Functions.InsertItemHistory(itemID, quantity,"Sales Credit",0,0,"Sales Credit");

                        //   Functions.Functions.InsertSalesHistory(itemID, quantity, 0, amount, "Sales");

                    }

                }

            }

            // ################################################ //
            //             Phase 3: Empty Sales Cart
            // ################################################ //

            using (SqlConnection con2 = new SqlConnection(constr))
            {
                using (SqlCommand cmd2 = new SqlCommand("inv_DeleteRequestCart"))
                {
                    cmd2.Connection = con2;
                    con2.Open();
                    cmd2.ExecuteNonQuery();
                    con2.Close();
                }
            }
  
            return Json(1);

        }


        // GET: CustomerFromInventoryCart/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesCart sal_SalesCart = await db.sal_SalesCart.FindAsync(id);
            if (sal_SalesCart == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesCart);
        }

        // GET: CustomerFromInventoryCart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerFromInventoryCart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SalesCartID,ItemID,ItemName,ItemQuantity,ItemPrice,TotalItemsPrice,SourceID,SourceName,SalesCartType,SalesCartDate")] sal_SalesCart sal_SalesCart)
        {
            if (ModelState.IsValid)
            {
                db.sal_SalesCart.Add(sal_SalesCart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_SalesCart);
        }

        // GET: CustomerFromInventoryCart/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesCart sal_SalesCart = await db.sal_SalesCart.FindAsync(id);
            if (sal_SalesCart == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesCart);
        }

        // POST: CustomerFromInventoryCart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SalesCartID,ItemID,ItemName,ItemQuantity,ItemPrice,TotalItemsPrice,SourceID,SourceName,SalesCartType,SalesCartDate")] sal_SalesCart sal_SalesCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_SalesCart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_SalesCart);
        }

        // GET: CustomerFromInventoryCart/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesCart sal_SalesCart = await db.sal_SalesCart.FindAsync(id);
            if (sal_SalesCart == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesCart);
        }

        // POST: CustomerFromInventoryCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            sal_SalesCart sal_SalesCart = await db.sal_SalesCart.FindAsync(id);
            db.sal_SalesCart.Remove(sal_SalesCart);
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
