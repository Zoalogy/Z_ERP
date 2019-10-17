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
    public class PurchaseCartController : Controller
    {
        private MainModel db = new MainModel();
        /*private PurchaseCartModel PurchaseCartModel = new PurchaseCartModel()*/


        // GET: PurchaseCart
        public async Task<ActionResult> Index()
        {
            return View(await db.pur_PurchaseCart.ToListAsync());
        }

        // GET: PurchaseCart/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_PurchaseCart pur_PurchaseCart = await db.pur_PurchaseCart.FindAsync(id);
            if (pur_PurchaseCart == null)
            {
                return HttpNotFound();
            }
            return View(pur_PurchaseCart);
        }

        // GET: PurchaseCart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PurchaseCart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "purchaseCartID,ItemID,ItemName,ItemQuantity,ItempurchasePrice,ItemSellPrice,UserName,PurchaseCartDate")] pur_PurchaseCart pur_PurchaseCart)
        {
            if (ModelState.IsValid)
            {
                db.pur_PurchaseCart.Add(pur_PurchaseCart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pur_PurchaseCart);
        }

        [HttpPost]
        public  async Task <JsonResult> InsertPurchaseCart(pur_PurchaseCart PurchaseCart)
        {

            var v = db.pur_PurchaseCart.Where(I => I.ItemName == PurchaseCart.ItemName).FirstOrDefault();
            

                if (v != null)
                {
                   
                    v.ItemQuantity = v.ItemQuantity+PurchaseCart.ItemQuantity;
                //status = 1; // 1 for update 
                     await db.SaveChangesAsync();
                }

                else{

                var x = PurchaseCart.ItemSellPrice;
                var y = PurchaseCart.ItempurchasePrice;
                    db.pur_PurchaseCart.Add(PurchaseCart);
                  await db.SaveChangesAsync();
                }

                return Json(PurchaseCart);
        }

        public  async  Task< JsonResult> UpdatetPurchaseCart(pur_PurchaseCart PurchaseCart)
        {

            var v = db.pur_PurchaseCart.Where(I => I.ItemName == PurchaseCart.ItemName).FirstOrDefault();


            if (v != null)
            {

                v.ItemQuantity =  PurchaseCart.ItemQuantity;
                v.ItemSellPrice = PurchaseCart.ItemSellPrice;
                v.ItempurchasePrice = PurchaseCart.ItempurchasePrice;
                //status = 1; // 1 for update 
               await db.SaveChangesAsync();
            }

            else
            {

                db.pur_PurchaseCart.Add(PurchaseCart);
               await db.SaveChangesAsync();
            }

            return Json(PurchaseCart);
        }

        // GET: PurchaseCart/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_PurchaseCart pur_PurchaseCart = await db.pur_PurchaseCart.FindAsync(id);
            if (pur_PurchaseCart == null)
            {
                return HttpNotFound();
            }
            return View(pur_PurchaseCart);
        }

        // POST: PurchaseCart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "purchaseCartID,ItemID,ItemName,ItemQuantity,ItempurchasePrice,ItemSellPrice,UserName,PurchaseCartDate")] pur_PurchaseCart pur_PurchaseCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pur_PurchaseCart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pur_PurchaseCart);
        }

        // GET: PurchaseCart/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_PurchaseCart pur_PurchaseCart = await db.pur_PurchaseCart.FindAsync(id);
            if (pur_PurchaseCart == null)
            {
                return HttpNotFound();
            }
            return View(pur_PurchaseCart);
        }

        // POST: PurchaseCart/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            pur_PurchaseCart pur_PurchaseCart = await db.pur_PurchaseCart.FindAsync(id);
            db.pur_PurchaseCart.Remove(pur_PurchaseCart);
            await db.SaveChangesAsync();

            return new EmptyResult();


            //return RedirectToAction("Index");

        }

        [HttpPost]

        public ActionResult DeleteCartItem(int id)
        {
            int status = 0;
            pur_PurchaseCart pur_PurchaseCart =   db.pur_PurchaseCart.Find(id);
            db.pur_PurchaseCart.Remove(pur_PurchaseCart);
              db.SaveChangesAsync();


            status = 1;
            return new JsonResult { Data = new { status = status } }; ;
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
