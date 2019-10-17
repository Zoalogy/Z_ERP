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
    public class ItemHistoryController : Controller
    {
        private MainModel db = new MainModel();

        // GET: ItemHistory
        public async Task<ActionResult> Index()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.inv_ItemHistory.ToListAsync());
        }

        public JsonResult GetItemHistory()
        {
            List<inv_ItemHistory> Customers = db.inv_ItemHistory.ToList();

            return Json(new { data = Customers }, JsonRequestBehavior.AllowGet);
        }

        // GET: ItemHistory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_ItemHistory inv_ItemHistory = await db.inv_ItemHistory.FindAsync(id);
            if (inv_ItemHistory == null)
            {
                return HttpNotFound();
            }
            return View(inv_ItemHistory);
        }

        // GET: ItemHistory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ItemHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemHistoryID,ItemID,ItemHistoryQuantity,QuantityMeasureUnit,ItemHistoryDebitOrCredit,ItemHistoryCuurentQuantity,ItemHistoryProccessTypeID,ItemHistoryProccessType,ItemHistoryDecription,ItemHistoryDate,UpLoaded,ItemHistoyPrice")] inv_ItemHistory inv_ItemHistory)
        {
            if (ModelState.IsValid)
            {
                db.inv_ItemHistory.Add(inv_ItemHistory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(inv_ItemHistory);
        }

        // GET: ItemHistory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_ItemHistory inv_ItemHistory = await db.inv_ItemHistory.FindAsync(id);
            if (inv_ItemHistory == null)
            {
                return HttpNotFound();
            }
            return View(inv_ItemHistory);
        }

        // POST: ItemHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemHistoryID,ItemID,ItemHistoryQuantity,QuantityMeasureUnit,ItemHistoryDebitOrCredit,ItemHistoryCuurentQuantity,ItemHistoryProccessTypeID,ItemHistoryProccessType,ItemHistoryDecription,ItemHistoryDate,UpLoaded,ItemHistoyPrice")] inv_ItemHistory inv_ItemHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_ItemHistory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inv_ItemHistory);
        }

        // GET: ItemHistory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_ItemHistory inv_ItemHistory = await db.inv_ItemHistory.FindAsync(id);
            if (inv_ItemHistory == null)
            {
                return HttpNotFound();
            }
            return View(inv_ItemHistory);
        }

        // POST: ItemHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            inv_ItemHistory inv_ItemHistory = await db.inv_ItemHistory.FindAsync(id);
            db.inv_ItemHistory.Remove(inv_ItemHistory);
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
