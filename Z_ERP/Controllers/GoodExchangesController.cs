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
    public class GoodExchangesController : Controller
    {
        private MainModel db = new MainModel();
        string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");
            return View(db.inv_Stror_to_Store_order.ToList());
        }
        public JsonResult getGoodSExchangesData()
        {
            var expenses = db.pur_GoodSExchange_Store_StoreHisory;
            return Json(new { data = expenses }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetInvenrotyItems(int? InventoryID)
        {
            List<inv_Items> Items = (from item in db.inv_Items where item.InventoryID == InventoryID && item.ItemQuantity >0 select item).Distinct().ToList();
            var Ites = (from Item in Items group Item by Item.ItemNameAr);
            return Json(Ites, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetPosipoleInvenroty()
        {
            List<inv_Inventory> inv_Inventory = (from item in db.inv_Inventory select item).Distinct().ToList();
            
            return Json(inv_Inventory, JsonRequestBehavior.AllowGet);
        }
        
        public JsonResult GetInvenrotyInvintory(int? InventoryID)
        {

            return Json(new SelectList(db.inv_Inventory.Where(Itmes => (Itmes.InvertoryID != InventoryID)), "InvertoryID", "InvertoryNameAr"), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public  JsonResult InsertGoodExchanges(inv_Stror_to_Store_order GoodSExchange)
        {

            if (ModelState.IsValid)
            {

                //################### Edit Item ###################//
                var v = db.inv_Stror_to_Store_order.Where(t => t.Stror_to_Store_orderID == GoodSExchange.Stror_to_Store_orderID || (t.orderItemName == GoodSExchange.orderItemName && t.orderFromnventoryID == GoodSExchange.orderFromnventoryID)).FirstOrDefault(); 
               
                    if (v != null)
                    {
                        v.orderItemQuantity = GoodSExchange.orderItemQuantity;
                        db.SaveChanges();
                    }
               
                else
                {
                    GoodSExchange.UpLoaded = false;
                    db.inv_Stror_to_Store_order.Add(GoodSExchange);
                    db.SaveChanges();
                }
            }
           
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// in this fuction => InsertGoodExchangesRecourds
        /// send items to transformed from inventory to another
        /// by checking the quantity
        /// remove from card 
        /// exchange the item
        /// </summary>
        /// <param name="StoreIDArray"></param>
        /// <param name="ItemIDArray"></param>
        /// <param name="NamesArray"></param>
        /// <param name="InventoriesArray"></param>
        /// <param name="QuantitiesArray"></param>
        /// <param name="InventoryToID"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult InsertGoodExchangesRecourds(string StoreIDArray,string ItemIDArray, string NamesArray, string InventoriesArray, string QuantitiesArray, int InventoryToID)
        {


            int[] ItemIDs = Array.ConvertAll(ItemIDArray.Split(','), int.Parse);
            string[] Names = NamesArray.Split(',');
            int[] StoreID = Array.ConvertAll(StoreIDArray.Split(','), int.Parse);
            
            int[] Inventories = Array.ConvertAll(InventoriesArray.Split(','), int.Parse);
            int[] Quantities = Array.ConvertAll(QuantitiesArray.Split(','), int.Parse);
            for (int c = 0; c < ItemIDs.Count(); c++)
            {

                
                int quantity = Quantities[c];
                int fromInventory = Inventories[c];
                int ItemID = ItemIDs[c];
                string Name = Names[c];
                JsonResult j = checkQuantity(fromInventory, Name, quantity);
               if (bool.Parse(j.Data.ToString())) {

                    DeleteCartItem(StoreID[c]);
                    var newExchange = new List<inv_Items>();
                var restQuantity = (long) quantity;

                var queryProducts = (from p in db.inv_Items
                                     where (p.ItemNameAr == Name && p.InventoryID == fromInventory)
                                     select p);
                int count = 0;
                foreach (var _p in queryProducts)
                {
                    if (_p.ItemQuantity >= restQuantity)
                    {
                        #region
                        using (SqlConnection con2 = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd2 = new SqlCommand("pur_update_inventory"))
                            {
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@ItemID", _p.ItemID);
                                cmd2.Parameters.AddWithValue("@Batch", _p.ItemBatch);
                                cmd2.Parameters.AddWithValue("@quantity", _p.ItemQuantity - restQuantity);
                                cmd2.Connection = con2;
                                con2.Open();
                                cmd2.ExecuteNonQuery();
                                con2.Close();
                            }
                        }
                        #endregion
                        AddExchangedItem(_p, restQuantity, InventoryToID);
                        break;
                    }
                    else
                    {
                        //;
                        #region
                        using (SqlConnection con2 = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd2 = new SqlCommand("pur_update_inventory"))
                            {
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@ItemID", _p.ItemID);
                                cmd2.Parameters.AddWithValue("@Batch", _p.ItemBatch);
                                cmd2.Parameters.AddWithValue("@quantity", 0);
                                cmd2.Connection = con2;
                                con2.Open();
                                cmd2.ExecuteNonQuery();
                                con2.Close();
                            }
                        }
                        #endregion
                        AddExchangedItem(_p, (long)_p.ItemQuantity, InventoryToID);

                        restQuantity = (long)quantity - (long)_p.ItemQuantity;
                    }
                    count++;
                }
            }
            }
            

            //////////////////////////////////////////////////////////////////////////////////////



            return Json(1);
        }

        private void AddExchangedItem(inv_Items _p, long quantity, int? ordernTOventoryID)
        {
            using (SqlConnection con2 = new SqlConnection(constr))
            {
                using (SqlCommand cmd2 = new SqlCommand("Insert_PurshaseItems"))
                {


                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@ItemID", _p.ItemID);
                    cmd2.Parameters.AddWithValue("@ItemNameAr", _p.ItemNameAr);
                    cmd2.Parameters.AddWithValue("@ItemQuantity", quantity);
                    cmd2.Parameters.AddWithValue("@sellamount", _p.ItemSalePrice);
                    cmd2.Parameters.AddWithValue("@ItemPuchasePrice", _p.ItemPuchasePrice);

                    cmd2.Parameters.AddWithValue("@ItemHistoryDecription", " تحويل بضاعة من المخزن الى المخزن");
                    
                        cmd2.Parameters.AddWithValue("@ItemHistoryProccessTypeID", 2);
                    cmd2.Parameters.AddWithValue("@InventoryID", ordernTOventoryID);
                    cmd2.Parameters.AddWithValue("@CategoryID", _p.CategoryID);
                    cmd2.Parameters.AddWithValue("@ItemPurchaseCurrencyID", _p.CategoryID);
                    cmd2.Parameters.AddWithValue("@BillNo", "-1");

                    ////cmd2.Parameters.AddWithValue("@PurchaseStatusID", 1);// if purchase operation done or not
                    //////    cmd2.Parameters.AddWithValue("@ItemTotalPurchaseAmount", ItemPuchasePrice );
                    //////cmd2.Parameters.AddWithValue("@ItemCostPurchase", ItemCostPurchase);
                    ////////////////



                    cmd2.Connection = con2;
                    con2.Open();
                    cmd2.ExecuteNonQuery();
                    con2.Close();


                }
            }
        }
        
        public  JsonResult checkQuantity(int fromID, string ItemNameAr, int? Quantity=0)
        {
            if(db.inv_Items.Where(e=> e.InventoryID == fromID && e.ItemNameAr == ItemNameAr).Sum(e=>e.ItemQuantity) >= Quantity)
            {
                return  Json(true);
            }

            return Json(false);
        }
        [HttpPost]
        public JsonResult DeleteCartItem(int id)
        {
           
            int status = 0;
            inv_Stror_to_Store_order pur_PurchaseCart = db.inv_Stror_to_Store_order.Find(id);
            db.inv_Stror_to_Store_order.Remove(pur_PurchaseCart);
            db.SaveChanges();


            status = 1;
            return new JsonResult { Data = new { status = status } }; ;
        }

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

        // GET: GoodExchanges/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: GoodExchanges/Create
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

        // GET: GoodExchanges/Edit/5
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

        public JsonResult UpdateGoodExchangesCart(inv_Stror_to_Store_order store_ex)
        {

            var v = db.inv_Stror_to_Store_order.Where(I => I.orderItemName == store_ex.orderItemName && I.orderFromnventoryID == store_ex.orderFromnventoryID).FirstOrDefault();


            if (v != null)
            {

                v.orderItemQuantity = store_ex.orderItemQuantity;
                
                //status = 1; // 1 for update 
                db.SaveChanges();

                return Json(1);
            }

            else
            {

                return Json(0);
            }

            return Json(0);
        }
        // POST: GoodExchanges/Edit/5
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
                //return RedirectToAction("Index");
            }
            return View(inv_Items);
        }

        // GET: GoodExchanges/Delete/5
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

        // POST: GoodExchanges/Delete/5
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
