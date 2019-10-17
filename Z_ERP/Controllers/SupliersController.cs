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
    public class SupliersController : Controller
    {
        private MainModel db = new MainModel();

        // GET: Supliers
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.pur_Supliers.ToListAsync());
        }

        public JsonResult GetSupliersData()
        {
            List<pur_Supliers> Supliers =    db.pur_Supliers.ToList();

            return Json(new { data = Supliers }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Save(int id)
        {

            var v = db.pur_Supliers.Where(a => a.SuplierID == id).FirstOrDefault();
 
            return View(v);

        }


        [HttpPost]
        public ActionResult Save(pur_Supliers Suplire)
        {
            int status = 0;


            // Start Validation cheack
            if (ModelState.IsValid)
            {

                //################### Edit Item ###################//
                if (Suplire.SuplierID > 0)
                {

                    var v = db.pur_Supliers.Where(I => I.SuplierID == Suplire.SuplierID).FirstOrDefault();
                    if (v != null)
                    {
                        v.SuplierID = Suplire.SuplierID;
                        v.SuplierName = Suplire.SuplierName;
                        v.SuplierPhone = Suplire.SuplierPhone;
                        v.SuplierBankAcount = Suplire.SuplierBankAcount;
                        v.UpLoaded = false;
                        status = 1; // 1 for update 
                        db.SaveChanges();
                    }
                }
                //################### Edit Item //###################

                // Start Save New Item
                else
                {
                    //Suplire.SuplierID = int.Parse(Session["ii"].ToString());
                    try
                    {
                        Suplire.UpLoaded = false;
                        Suplire.SuplierRegisterationDate = DateTime.Now;
                        db.pur_Supliers.Add(Suplire);
                        db.SaveChanges();
                        status = 2;  // 2 fro 
                    }
                    catch
                    {
                        status = 0;  // 2 fro 
                    }
                    
                }
                //End Save New Item

                return new JsonResult { Data = new { status = status } };

            }
            // End Validation cheack


            // Start Send validation error
            else
            {
                var errorList = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage).ToList();

                return new JsonResult { Data = new { status = errorList } };
            }
            // End  Send validation error

        }

        // GET: Supliers/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_Supliers pur_Supliers = await db.pur_Supliers.FindAsync(id);
            if (pur_Supliers == null)
            {
                return HttpNotFound();
            }
            return View(pur_Supliers);
        }

        // GET: Supliers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Supliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SuplierID,SuplierName,SuplierPhone,SuplierAddress,SuplierAddedDate")] pur_Supliers pur_Supliers)
        {
            if (ModelState.IsValid)
            {
                db.pur_Supliers.Add(pur_Supliers);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pur_Supliers);
        }

        // GET: Supliers/Edit/5
        public  ActionResult Edit(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_Supliers pur_Supliers =  db.pur_Supliers.Find(id);
            if (pur_Supliers == null)
            {
                return HttpNotFound();
            }
            var bilsNo = db.pur_Bills.Where(b => b.SuplierID == id && b.BillIsReturned == false).Count();
            var bilsAmont = db.pur_Bills.Where(b => b.SuplierID == id && b.BillIsReturned == false).Sum(s=>s.BillTotalAmount) ?? 0;
            var bilsPayed = db.pur_Bills.Where(b => b.SuplierID == id && b.BillIsReturned == false).Sum(s => s.BillPaidAmount) ?? 0;
            var billrimainig = bilsAmont - bilsPayed ;
            ViewBag.bilsNo= bilsNo;
            ViewBag.bilsAmont = bilsAmont;
            ViewBag.bilsPayed = bilsPayed;
            ViewBag.billrimainig = billrimainig;
            return View(pur_Supliers);
        }
        public JsonResult GetSuplierBillsData(int? id)
        {
            List<pur_Bills> Supliers = db.pur_Bills.Where(pu => pu.SuplierID == id && pu.BillIsReturned == false).ToList();
            return Json(new { data = Supliers }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult PaymentData(string BillNo)
        {
            return View(db.pur_BillDetails.Where(d=>d.BillNo == BillNo).ToList());
        }
[HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SuplierID,SuplierName,SuplierPhone,SuplierAddress,SuplierAddedDate")] pur_Supliers pur_Supliers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pur_Supliers).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pur_Supliers);
        }

        // GET: Supliers/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_Supliers pur_Supliers = await db.pur_Supliers.FindAsync(id);
            if (pur_Supliers == null)
            {
                return HttpNotFound();
            }
            return View(pur_Supliers);
        }
        // POST: Supliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            pur_Supliers pur_Supliers = await db.pur_Supliers.FindAsync(id);
            db.pur_Supliers.Remove(pur_Supliers);
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
