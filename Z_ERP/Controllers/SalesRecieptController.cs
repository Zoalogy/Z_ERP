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
using System.Data.SqlClient;
using System.Configuration;

namespace Z_ERP.Controllers
{
    public class SalesRecieptController : Controller
    {
        private MainModel db = new MainModel();
        //private PaymentMethodModel PaymentMethodModel = new PaymentMethodModel();

        //private SalesRecieptItems SalesHistoryModel = new SalesRecieptItems();
        //private SalesModel SalesModel = new SalesModel();


        
        //private ItemsModel ItemsModel = new ItemsModel();






        // GET: SalesReciept
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

          // ViewBag.SalesHistory = new SelectList(SalesHistoryModel.sal_Sales, "SaleQuantity", "SalesItemsID");
            return View(await db.sal_Reciept.ToListAsync());
        }

        

        public JsonResult GetRecieptsData()
        {
           
            List<sal_Reciept> Supliers = db.sal_Reciept.ToList();



            return Json(new { data = Supliers }, JsonRequestBehavior.AllowGet);
        }

        // GET: SalesReciept/Details/5
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
             
                      

           // ViewBag.RecieptItems = qury.ToList();
            ViewBag.RecieptItems = db.sal_Sales.Where(I => I.RecieptNo == sal_Reciept.RecieptNo).ToList();

            return View(sal_Reciept);
        }

        // GET: SalesReciept/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SalesReciept/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RecieptID,RecieptNo,PaymentMethodID,CustomerID,RecieptTotalAmount,RecieptDiscount,RecieptRemaining,RecieptTaxes,RecieptNetAmount,RecieptPaidAmount,RecieptChequeNo,RecieptChequeDate,RecieptDescription,RecieptDate,WithInstallments,InstallmentsNo")] sal_Reciept sal_Reciept)
        {
            if (ModelState.IsValid)
            {
                db.sal_Reciept.Add(sal_Reciept);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(sal_Reciept);
        }

        // GET: SalesReciept/Edit/5
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

            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");
            return View(sal_Reciept);
        }

        // POST: SalesReciept/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]


        
         
        public async Task<ActionResult> Edit([Bind(Include = "RecieptID,RecieptNo,PaymentMethodID,CustomerID,RecieptTotalAmount,RecieptDiscount,RecieptRemaining,RecieptTaxes,RecieptNetAmount,RecieptPaidAmount,RecieptChequeNo,RecieptChequeDate,RecieptDescription,RecieptDate,WithInstallments,InstallmentsNo")] sal_Reciept sal_Reciept, int NewAmount, string ChequeNumber, string ChequeDate, string ChequeBank)
        {


            int status=0;

            if (ModelState.IsValid)
            {
                
                var v = db.sal_Reciept.Where(I => I.RecieptID == sal_Reciept.RecieptID).FirstOrDefault();

                var OldAmount = v.RecieptPaidAmount;
                var Discount = v.RecieptDiscount;
                var RecieptNo = v.RecieptNo;
                var CustomerID = v.CustomerID;
                var PaymentMethodID = sal_Reciept.PaymentMethodID;


                var NewPaidAmount =  (OldAmount + NewAmount);

                //  v.RecieptPaidAmount = NewAmount;

               
                var RecieptRemaining = v.RecieptTotalAmount - (NewPaidAmount + Discount);

                if (RecieptRemaining >= 0)
                {

                    v.RecieptPaidAmount = NewPaidAmount;

                    var NewRemaining = RecieptRemaining;
                    v.RecieptRemaining = NewRemaining;
                
                    await db.SaveChangesAsync();


                    // Insert Reciept payment details

                    string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    using (SqlConnection connect = new SqlConnection(constr))
                    {

                        //  , string , string , string 

                        var disc = "تسديد فاتورة برقم " + RecieptNo;
                        string query = "Insert Into sal_RecieptDetails (RecieptNo, Amount, RecieptDetailsDescription, PaymentMethodID, ChequeNo, ChequeDate,ChequeBank)" +
                            "Values('" + RecieptNo + "','" + NewAmount + "','" + disc + "','" + PaymentMethodID + "','" + ChequeNumber + "','" + ChequeDate + "','" + ChequeBank + "')";

                        SqlCommand command = new SqlCommand(query, connect);
                        connect.Open();
                        command.ExecuteNonQuery();
                        connect.Close();
                        
                        var Reciept = db.sal_Reciept.Where(I => I.RecieptNo == RecieptNo).FirstOrDefault();
                        Functions.Functions.CustomerPaymentBookRecord(PaymentMethodID,1, NewAmount, disc, CustomerID);
                    }  // Insert Reciept payment details
 

                    status = 1;

                }

                // paid amount greter than total amount
                else
                {
                    status = 2;
                }


               Functions.Functions.InsertPaymentHistory(NewAmount,0,"Sales Reciept Payment",1);

              

            }


            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");

            return new JsonResult { Data = new { status= status } };
          //  return View(sal_Reciept);
        }

        // GET: SalesReciept/Delete/5
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

        // POST: SalesReciept/Delete/5
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
