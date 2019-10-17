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
    public class SalesController : Controller
    {
        private MainModel db = new MainModel();

        // GET: Sales
        public async Task<ActionResult> Index()
        {
            return View(await db.sal_Sales.ToListAsync());
        }

        // GET: Sales/Details/5
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

        // GET: Sales/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Sales/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SaleID,SalesItemsID,SaleQuantity,SalePrice,RecieptNo,SaleIsReturned,SaleReturnedDate,SaleDate")] sal_Sales sal_Sales)
        {
            if (ModelState.IsValid)
            {
                db.sal_Sales.Add(sal_Sales);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_Sales);
        }

        // GET: Sales/Edit/5
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

        // POST: Sales/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SaleID,SalesItemsID,SaleQuantity,SalePrice,RecieptNo,SaleIsReturned,SaleReturnedDate,SaleDate")] sal_Sales sal_Sales)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_Sales).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_Sales);
        }

        // GET: Sales/Delete/5
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

        // POST: Sales/Delete/5
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
