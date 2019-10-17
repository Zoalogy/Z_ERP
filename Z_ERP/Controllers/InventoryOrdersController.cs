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
    public class InventoryOrdersController : Controller
    {
        private MainModel db = new MainModel();
        string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
        // GET: InventoryOrders
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");
            ViewBag.ItemsDropDownList = new SelectList(db.inv_Items.Where(SubCategory => SubCategory.SubCategoryID == 0), "ItemID", "ItemNameAr");
            ViewBag.ItemsDropDownLis = db.inv_Items.Where(SubCategory => SubCategory.SubCategoryID == 0).GroupBy(i=>i.ItemID).ToList();
            return View( db.inv_Stror_to_Store_order.ToList());
            
        }
        public JsonResult GetsTorsOrder()
        {
            
            var orders = db.inv_Stror_to_Store_order.Where(s=> s.orderStaus == 0);
            return Json(new { data = orders.ToList() }, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public  JsonResult InsertPurchaseCart(inv_Stror_to_Store_order Stror_to_Store_order)
        {
            var v = db.inv_Stror_to_Store_order.Where(I => I.ordernTOventoryID == Stror_to_Store_order.ordernTOventoryID && I.orderItemID == Stror_to_Store_order.orderItemID && I.orderStaus == 0).FirstOrDefault();
            if (v != null)
            {
                v.orderItemQuantity = Stror_to_Store_order.orderItemQuantity;
                v.toOrdeererDate = DateTime.Now;
                v.orderStaus = 0;
                db.SaveChanges();
            }
            else
            {
                Stror_to_Store_order.orderStaus = 0;
                Stror_to_Store_order.toOrdeererDate = DateTime.Now;
                db.inv_Stror_to_Store_order.Add(Stror_to_Store_order);
                 db.SaveChanges();
            }
            return Json(1);
        }
        // GET: InventoryOrders/Details/5
        public JsonResult AcceptOrder(int? id)
        {
            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            inv_Stror_to_Store_order inv_Stror_to_Store_order =  db.inv_Stror_to_Store_order.Find(id);
            if (inv_Stror_to_Store_order == null)
            {
                //return HttpNotFound();
            }
            ViewBag.Invertory = inv_Stror_to_Store_order;
            ViewBag.Items =  inv_Stror_to_Store_order.orderItemName == null ? "" : inv_Stror_to_Store_order.orderItemName ;
            var x = new
            {
                inv_Stror_to_Store_order = inv_Stror_to_Store_order,
                inv_from_order = new SelectList(
                    db.inv_Inventory.Where(
                    invent => (invent.InvertoryID != inv_Stror_to_Store_order.ordernTOventoryID)
                    && (
                            inv_Stror_to_Store_order.orderItemQuantity <= (
                                db.inv_Items.Where(item => item.InventoryID == invent.InvertoryID && item.ItemID == inv_Stror_to_Store_order.orderItemID && item.ItemQuantity - inv_Stror_to_Store_order.orderItemQuantity > item.ItemMinimumQuantity).Sum(s => s.ItemQuantity)
                                                                    )
                        )
                    ), "InvertoryID", "InvertoryNameAr")
            //new SelectList(db.inv_Inventory.Where(Itmes => (Itmes.InvertoryID != inv_Stror_to_Store_order.ordernTOventoryID)), "InvertoryID", "InvertoryNameAr")
        };
            
            return Json(x, JsonRequestBehavior.AllowGet); 
        }
        /// <summary>
        /// ///////////////////
        /// []
        public JsonResult AcceptOrderConfirme(string InvetoryfromListID,string InvetoryToID, string orderItemQuantity,int ID_todelete, int itemIdOconfirme)
        {
            int x = int.Parse(InvetoryfromListID);
            int? toInventory = int.Parse(InvetoryToID);
            int orderQuantity = int.Parse(orderItemQuantity);
            var results = db.inv_Items.Where(st => st.InventoryID == x && st.ItemID == itemIdOconfirme)
             .Select(st => new InvTransactionResult{ item_id =  st.ItemID,quanity = st.ItemQuantity, batch = st.ItemBatch })
             .OrderBy(r => r.item_id)
             .Take(orderQuantity).ToList();
           foreach(InvTransactionResult item in results)
            {
                if (item.quanity > 0)
                {
                    var updateItem = db.inv_Items.Where(i => i.ItemBatch == item.batch && i.ItemID == item.item_id).First();
                    if (item.quanity >= orderQuantity)
                    {
                        updateItem.ItemQuantity -= orderQuantity;
                        db.SaveChanges();
                        var updateToInventory = db.inv_Items.Where(i =>  i.ItemID == item.item_id).First();
                        updateToInventory.ItemQuantity += orderQuantity;
                        db.SaveChanges();
                        break;
                    }
                    if (item.quanity < orderQuantity)
                    {
                        updateItem.InventoryID = x;
                        orderQuantity = (int)orderQuantity - (int)item.quanity;
                        db.SaveChanges();
                    }
                }
            }
            #region
            inv_Stror_to_Store_order inv_Stror_Card_to_delete =  db.inv_Stror_to_Store_order.Find(ID_todelete);
            db.inv_Stror_to_Store_order.Remove(inv_Stror_Card_to_delete);
            db.SaveChanges();
            #endregion
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        /// </summary>
        /// <returns></returns>
            // GET: InventoryOrders/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryOrders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Stror_to_Store_orderID,orderItemID,ordernTOventoryID,orderFromnventoryID,orderItemName,orderItemQuantity,orderStaus,orderUserName,fromOrderDate,toOrdeererDate")] inv_Stror_to_Store_order inv_Stror_to_Store_order)
        {
            if (ModelState.IsValid)
            {
                db.inv_Stror_to_Store_order.Add(inv_Stror_to_Store_order);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(inv_Stror_to_Store_order);
        }

        // GET: InventoryOrders/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Stror_to_Store_order inv_Stror_to_Store_order = await db.inv_Stror_to_Store_order.FindAsync(id);
            if (inv_Stror_to_Store_order == null)
            {
                return HttpNotFound();
            }
            return View(inv_Stror_to_Store_order);
        }

        // POST: InventoryOrders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Stror_to_Store_orderID,orderItemID,ordernTOventoryID,orderFromnventoryID,orderItemName,orderItemQuantity,orderStaus,orderUserName,fromOrderDate,toOrdeererDate")] inv_Stror_to_Store_order inv_Stror_to_Store_order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_Stror_to_Store_order).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inv_Stror_to_Store_order);
        }

        // GET: InventoryOrders/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Stror_to_Store_order inv_Stror_to_Store_order = await db.inv_Stror_to_Store_order.FindAsync(id);
            if (inv_Stror_to_Store_order == null)
            {
                return HttpNotFound();
            }
            return View(inv_Stror_to_Store_order);
        }

        // POST: InventoryOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            inv_Stror_to_Store_order inv_Stror_to_Store_order = await db.inv_Stror_to_Store_order.FindAsync(id);
            db.inv_Stror_to_Store_order.Remove(inv_Stror_to_Store_order);
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

        private class InvTransactionResult
        {
            internal string batch;

            public int item_id { get; set; }
            public int? quanity { get; set; }
        }
    }
}
