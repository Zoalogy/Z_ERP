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
    public class InventoryController : Controller
    {
        private MainModel db = new MainModel();

        // GET: Inventory
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.inv_Inventory.ToListAsync());
        }


        // Return Inventory data to datatabels function 

        public async Task<ActionResult> GetInventoryData()
        {
            return Json(new { data = await db.inv_Inventory.ToListAsync() }, JsonRequestBehavior.AllowGet);
        }


        // GET: Inventory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Inventory inv_Inventory = await db.inv_Inventory.FindAsync(id);
            if (inv_Inventory == null)
            {
                return HttpNotFound();
            }
            return View(inv_Inventory);
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Save(int id)
        {

            var v = db.inv_Inventory.Where(a => a.InvertoryID == id).FirstOrDefault();
            return View(v);

        }

        // POST: Inventory/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "InvertoryID,InvertoryNameAr,InvertoryNameEn,InvertoryAddressAr,InvertoryAddressEn,InvertoryAddDate,InvertoryRent,InvertoryRentDate")] inv_Inventory inv_Inventory)
        {
            if (ModelState.IsValid)
            {
                db.inv_Inventory.Add(inv_Inventory);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(inv_Inventory);
        }

        [HttpPost]
        public async Task< ActionResult> Save(inv_Inventory Inventory)
        {
            int status = 0;
            if (ModelState.IsValid)
            {

                if (Inventory.InvertoryID > 0)
                {
                    //Edit 
                    var v = db.inv_Inventory.Where(a => a.InvertoryID == Inventory.InvertoryID).FirstOrDefault();
                    if (v != null)
                    {
                        v.InvertoryNameAr = Inventory.InvertoryNameAr;
                        v.InvertoryAddressAr = Inventory.InvertoryAddressAr;
                        v.InvertoryRent = Inventory.InvertoryRent;
                        v.InvertoryRentDate = Inventory.InvertoryRentDate;

                        status = 1;

                    }
                }
                else
                {
                    //Save
                    db.inv_Inventory.Add(Inventory);
                    status = 2;
                }
                await db.SaveChangesAsync();


            }
            return new JsonResult { Data = new { status = status } };
        }

        // GET: Inventory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Inventory inv_Inventory = await db.inv_Inventory.FindAsync(id);
            if (inv_Inventory == null)
            {
                return HttpNotFound();
            }
            return View(inv_Inventory);
        }

        // POST: Inventory/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "InvertoryID,InvertoryNameAr,InvertoryNameEn,InvertoryAddressAr,InvertoryAddressEn,InvertoryAddDate,InvertoryRent,InvertoryRentDate")] inv_Inventory inv_Inventory)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_Inventory).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inv_Inventory);
        }

        // GET: Inventory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Inventory inv_Inventory = await db.inv_Inventory.FindAsync(id);
            if (inv_Inventory == null)
            {
                return HttpNotFound();
            }
            return View(inv_Inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            inv_Inventory inv_Inventory = await db.inv_Inventory.FindAsync(id);
            db.inv_Inventory.Remove(inv_Inventory);
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
