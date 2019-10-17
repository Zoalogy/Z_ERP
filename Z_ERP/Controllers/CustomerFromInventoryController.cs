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
    public class CustomerFromInventoryController : Controller
    {
        private MainModel db = new MainModel();

        // GET: CustomerFromInventory
        public async Task<ActionResult> Index()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }


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
        //    ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            ViewBag.ItemsDropDownList = new SelectList(db.inv_Items.Where(SubCategory => (SubCategory.CategoryID == 0)), "ItemID", "ItemNameAr");
            ViewBag.CustomersDropDownList = db.sal_Customer.ToList();


            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");
            ViewBag.Customers = new SelectList(db.sal_Customer, "CustomerID", "CustomerName");

             
            return View(await db.inv_RequestCart.Where(r => (r.RequestType == 2)).ToListAsync());
        }

        // GET: CustomerFromInventory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            if (inv_RequestCart == null)
            {
                return HttpNotFound();
            }
            return View(inv_RequestCart);
        }

        // GET: CustomerFromInventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CustomerFromInventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RequestCartID,ItemID,ItemName,ItemQuantity,ItemPrice,TotalItemsPrice,RequestType,RequestCartDate")] inv_RequestCart inv_RequestCart)
        {
            if (ModelState.IsValid)
            {
                db.inv_RequestCart.Add(inv_RequestCart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(inv_RequestCart);
        }

        // GET: CustomerFromInventory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            if (inv_RequestCart == null)
            {
                return HttpNotFound();
            }
            return View(inv_RequestCart);
        }

        // POST: CustomerFromInventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestCartID,ItemID,ItemName,ItemQuantity,ItemPrice,TotalItemsPrice,RequestType,RequestCartDate")] inv_RequestCart inv_RequestCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_RequestCart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inv_RequestCart);
        }

        // GET: CustomerFromInventory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            if (inv_RequestCart == null)
            {
                return HttpNotFound();
            }
            return View(inv_RequestCart);
        }

        // POST: CustomerFromInventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            db.inv_RequestCart.Remove(inv_RequestCart);
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
