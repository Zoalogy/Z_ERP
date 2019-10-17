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
    public class CustomerController : Controller
    {
        private MainModel db = new MainModel();

        // GET: Customer
        public async Task<ActionResult> Index()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.sal_Customer.ToListAsync());
        }



        public JsonResult GetCustomersData()
        {
            List<sal_Customer> Customers = db.sal_Customer.ToList();

            return Json(new { data = Customers }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult Save(int id)
        {

            var v = db.sal_Customer.Where(a => a.CustomerID == id).FirstOrDefault();

            return View(v);

        }


        [HttpPost]
        public ActionResult Save(sal_Customer Customer)
        {
            int status = 0;
            string[] ErrorList = new string[400];


            // Start Validation cheack
            if (ModelState.IsValid)
            {

                //################### Edit Customer Page ###################//
                if (Customer.CustomerID > 0)
                {

                    var v = db.sal_Customer.Where(I => I.CustomerID == Customer.CustomerID).FirstOrDefault();
                    if (v != null)
                    {

                        var CustomerPhone = db.sal_Customer.Where(I => I.CustomerPhone == Customer.CustomerPhone && I.CustomerID != v.CustomerID).FirstOrDefault();
                        if (CustomerPhone != null)
                        {
                            status = 3; // error -> trucks exist 
                        }

                        else
                        {
                            v.CustomerID = Customer.CustomerID;
                            v.CustomerName = Customer.CustomerName;
                            v.CustomerPhone = Customer.CustomerPhone;
                            v.CustomerBankAcount = Customer.CustomerBankAcount;

                            status = 1; // 1 for update 
                            db.SaveChanges();

                        }
                       
                    }
                }   // Edit Customer Page 


                //###################   Save New Customer Pahe  ###################
                else
                {

                        // Check Customer Exist
                        var CustomerPhone = db.sal_Customer.Where(I => I.CustomerPhone == Customer.CustomerPhone).FirstOrDefault();
                        if (CustomerPhone != null)
                        {
                            status = 3; //  Error -> Customer phone exist  

                        }

                        else
                        {
                            db.sal_Customer.Add(Customer);
                            db.SaveChanges();
                            status = 2;  // 2 
                        }

                } // Save New Customer 
                

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


        // GET: Customer/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Customer sal_Customer = await db.sal_Customer.FindAsync(id);
            if (sal_Customer == null)
            {
                return HttpNotFound();
            }

            ViewBag.Reciept = db.sal_Reciept.Where(I => I.CustomerID == id).ToList();

           var vv = db.sal_Reciept.Where(I => I.CustomerID == id).ToList();

              ViewBag.Remaining = vv.Sum(x=> (decimal)x.RecieptRemaining);
            ViewBag.RecieptCount = vv. Count();
            ViewBag.NotPaidReciept = vv.Where(I => I.RecieptPaymentStatus == 2).ToList().Count();  // Not



            return View(sal_Customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CustomerID,CustomerName,CustomerPhone,CustomerEmail,CustomerBankAcount,CustomerRegisterationDate")] sal_Customer sal_Customer)
        {
            if (ModelState.IsValid)
            {
                db.sal_Customer.Add(sal_Customer);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_Customer);
        }

        // GET: Customer/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Customer sal_Customer = await db.sal_Customer.FindAsync(id);
            if (sal_Customer == null)
            {
                return HttpNotFound();
            }
            return View(sal_Customer);
        }

        // POST: Customer/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CustomerID,CustomerName,CustomerPhone,CustomerEmail,CustomerBankAcount,CustomerRegisterationDate")] sal_Customer sal_Customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_Customer).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_Customer);
        }

        // GET: Customer/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_Customer sal_Customer = await db.sal_Customer.FindAsync(id);
            if (sal_Customer == null)
            {
                return HttpNotFound();
            }
            return View(sal_Customer);
        }

        // POST: Customer/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            sal_Customer sal_Customer = await db.sal_Customer.FindAsync(id);
            db.sal_Customer.Remove(sal_Customer);
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
