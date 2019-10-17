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
    public class SalesReverseRecieptController : Controller
    {
        private MainModel db = new MainModel();

        // GET: SalesReverseReciept
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.sal_Reciept.ToListAsync());
        }

        public JsonResult GetRecieptsData()
        {

            List<sal_Reciept> Receipts = db.sal_Reciept.Where(I => I.RecieptIsReturned == false).ToList();


            return Json(new { data = Receipts }, JsonRequestBehavior.AllowGet);
        }
        // GET: SalesReverseReciept/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Reciept sal_Reciept = await db.sal_Reciept.FindAsync(id);
            if (sal_Reciept == null)
            {
                return HttpNotFound();
            }
            ViewBag.RecieptItems = db.sal_Sales.Where(I => I.RecieptNo == sal_Reciept.RecieptNo).ToList();
            return View(sal_Reciept);
        }

        // GET: SalesReverseReciept/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesReverseReciept/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RecieptID,RecieptNo,PaymentMethodID,CustomerID,CustomerName,RecieptTotalAmount,RecieptDiscount,RecieptRemaining,RecieptTaxes,RecieptNetAmount,RecieptPaidAmount,RecieptDescription,WithInstallments,InstallmentsNo,RecieptStatus,UpLoaded,RecieptDate,PointOfSaleID")] sal_Reciept sal_Reciept)
        {
            if (ModelState.IsValid)
            {
                db.sal_Reciept.Add(sal_Reciept);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_Reciept);
        }

        // GET: SalesReverseReciept/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Reciept sal_Reciept = await db.sal_Reciept.FindAsync(id);
            if (sal_Reciept == null)
            {
                return HttpNotFound();
            }
            return View(sal_Reciept);
        }

        // POST: SalesReverseReciept/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RecieptID,RecieptNo,PaymentMethodID,CustomerID,CustomerName,RecieptTotalAmount,RecieptDiscount,RecieptRemaining,RecieptTaxes,RecieptNetAmount,RecieptPaidAmount,RecieptDescription,WithInstallments,InstallmentsNo,RecieptStatus,UpLoaded,RecieptDate,PointOfSaleID")] sal_Reciept sal_Reciept)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_Reciept).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_Reciept);
        }

        // GET: SalesReverseReciept/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Reciept sal_Reciept = await db.sal_Reciept.FindAsync(id);
            if (sal_Reciept == null)
            {
                return HttpNotFound();
            }
            return View(sal_Reciept);
        }


        public async Task<ActionResult> ReverseOne(int id)
        {
            int status = 1;

            sal_Sales  Sales = await db.sal_Sales.FindAsync(id);
            db.sal_Sales.Remove(Sales);
            await db.SaveChangesAsync();
            return new JsonResult { Data = new { status = status } };
        }

        public async Task<ActionResult> ReverseAll(int id)
        {
            int status = 1;

            sal_Reciept Reciept = await db.sal_Reciept.FindAsync(id);
            List<sal_Sales> Sales = db.sal_Sales.Where(I => I.RecieptNo == Reciept.RecieptNo).ToList();

            // 1: Return Reciept Items Quantities into Inventory 

            var PointOfSaleId = Reciept.PointOfSaleID;
            foreach (var item in Sales)
            {
               
                Functions.Functions.ReturnSalesItem(item.SalesItemsID, PointOfSaleId, item.SaleQuantity);


                sal_Sales sales = await db.sal_Sales.FindAsync(item.SaleID);
                sales.SaleIsReturned = true;
                sales.SaleReturnedDate = DateTime.Now;

            }


            Reciept.RecieptIsReturned = true;
            Reciept.RecieptReturnDate = DateTime.Now;
            await db.SaveChangesAsync();
            return new JsonResult { Data = new { status = status } };
        }

        // POST: SalesReverseReciept/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            sal_Reciept sal_Reciept = await db.sal_Reciept.FindAsync(id);
            db.sal_Reciept.Remove(sal_Reciept);
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
