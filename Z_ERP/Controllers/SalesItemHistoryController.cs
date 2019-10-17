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
    public class SalesItemHistoryController : Controller
    {
        private MainModel db = new MainModel();

        // GET: SalesItemHistory
        public async Task<ActionResult> Index()
        {
            return View(await db.sal_SalesItemHistory.ToListAsync());
        }

        // GET: SalesItemHistory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesItemHistory sal_SalesItemHistory = await db.sal_SalesItemHistory.FindAsync(id);
            if (sal_SalesItemHistory == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesItemHistory);
        }

        // GET: SalesItemHistory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesItemHistory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ItemHistroyHistroyID,SaleItemID,SaleItemQuantity,SaleItemDebitOrCredit,ItemAmount,ItemCurrentQuantity,ItemHistoryDescription,EmployeeName,ItemHistoryDate,UpLoaded")] sal_SalesItemHistory sal_SalesItemHistory)
        {
            if (ModelState.IsValid)
            {
                db.sal_SalesItemHistory.Add(sal_SalesItemHistory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_SalesItemHistory);
        }

        // GET: SalesItemHistory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesItemHistory sal_SalesItemHistory = await db.sal_SalesItemHistory.FindAsync(id);
            if (sal_SalesItemHistory == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesItemHistory);
        }

        // POST: SalesItemHistory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ItemHistroyHistroyID,SaleItemID,SaleItemQuantity,SaleItemDebitOrCredit,ItemAmount,ItemCurrentQuantity,ItemHistoryDescription,EmployeeName,ItemHistoryDate,UpLoaded")] sal_SalesItemHistory sal_SalesItemHistory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_SalesItemHistory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_SalesItemHistory);
        }

        // GET: SalesItemHistory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesItemHistory sal_SalesItemHistory = await db.sal_SalesItemHistory.FindAsync(id);
            if (sal_SalesItemHistory == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesItemHistory);
        }

        // POST: SalesItemHistory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            sal_SalesItemHistory sal_SalesItemHistory = await db.sal_SalesItemHistory.FindAsync(id);
            db.sal_SalesItemHistory.Remove(sal_SalesItemHistory);
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
