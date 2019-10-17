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
    public class purshaseBillController : Controller
    {
        private MainModel db = new MainModel();
         
        

        // GET: purshaseBill
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }

            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            return View(await db.pur_Bills.Where(pu=>pu.BillIsReturned == false).ToListAsync());
        }

        public JsonResult GetBillsData()
        {
            List<pur_Bills> Supliers = db.pur_Bills.Where(pu => pu.BillIsReturned == false && pu.BillPaymentStatus == 1 && pu.BillRemainingAmount != 0).ToList();
            return Json(new { data = Supliers }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Cost(string billNumber)
        { 

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            return View(await db.pur_Purchase.Where(e=>e.BillNo == billNumber && e.IsReturn == true && e.PurchaseStatusID == 0).ToListAsync());
        }
        public JsonResult InsertItemsToInv(string BillNo,int ItemID,string ItemNameAr,int ItemQuantity, decimal ItemCostPurchase, double ItemPuchasePrice, double sellamount, int InventoryID,int CategoryID)
        {
            // ################################################ //
            //     Phase 1: Insert Purshases into Inventory     //
            // ################################################ //
            #region
            
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection con2 = new SqlConnection(constr))
            {
             
    //@ItemPurchaseCurrencyID int = 1,
                    
                    
     
                    using (SqlCommand cmd2 = new SqlCommand("Insert_PurshaseItems"))
                    {

                        
                        cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@ItemID", ItemID);
                    cmd2.Parameters.AddWithValue("@ItemNameAr",ItemNameAr);
                    cmd2.Parameters.AddWithValue("@ItemQuantity", ItemQuantity);
                    cmd2.Parameters.AddWithValue("@sellamount", sellamount);
                    cmd2.Parameters.AddWithValue("@ItemPuchasePrice", ItemPuchasePrice / ItemQuantity);

                    cmd2.Parameters.AddWithValue("@InventoryID", InventoryID);
                    cmd2.Parameters.AddWithValue("@CategoryID", CategoryID);
                    cmd2.Parameters.AddWithValue("@ItemPurchaseCurrencyID", 0);
                    cmd2.Parameters.AddWithValue("@BillNo", BillNo);
                    
                    ////cmd2.Parameters.AddWithValue("@PurchaseStatusID", 1);// if purchase operation done or not
                    //////    cmd2.Parameters.AddWithValue("@ItemTotalPurchaseAmount", ItemPuchasePrice );
                    //////cmd2.Parameters.AddWithValue("@ItemCostPurchase", ItemCostPurchase);
                    ////////////////



                    cmd2.Connection = con2;
                        con2.Open();
                        cmd2.ExecuteNonQuery();
                        con2.Close();


                    }
                }
            #endregion
            #region
            // ################################################ //
            //     Phase 1: update Purshases card    //
            // ################################################ //
            var pur = db.pur_Purchase.Where(e => e.BillNo == BillNo && e.ItemID == ItemID).ToList();
            //foreach(item in pur)
            //{

            //}
            #endregion


            return Json( 1, JsonRequestBehavior.AllowGet);
        }

        
        // GET: purshaseBill/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: purshaseBill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        public JsonResult UpdatetPurchaseCostAndSellPrice(pur_Purchase PurchaseCart)
        {

            var v = db.pur_Purchase.Where(I => I.BillNo == PurchaseCart.BillNo && I.PurchaseItemNameAr == PurchaseCart.PurchaseItemNameAr).FirstOrDefault();
            if (v != null)
            {
                v.ItemSalePrice = PurchaseCart.ItemSalePrice;
                v.ItemCostPurchase = PurchaseCart.ItemCostPurchase;
                
                v.UpLoaded = false;
                db.Entry(v).State = EntityState.Modified;
                db.SaveChanges();
            }
            var totalCost= db.pur_Purchase.Where(I => I.BillNo == PurchaseCart.BillNo).Sum(s=>s.ItemCostPurchase);
            var xx= db.pur_Bills.Where(b => b.BillNo == PurchaseCart.BillNo).ToList();
            var billID = db.pur_Bills.Find(xx.FirstOrDefault().BillID).BillID;
            if (billID > 0)
            {
                var bill = db.pur_Bills.Find(billID);
                if (ModelState.IsValid)
                {

                    bill.TotalCostPurchase = totalCost;
                    bill.UpLoaded = false;
                    db.Entry(bill).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
            return Json(1);
        }





        // GET: purshaseBill/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_Bills pur_Bills =  db.pur_Bills.Find(id);
            if (pur_Bills == null)
            {
                return HttpNotFound();
            }

            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod.Where(e => e.PaymentMethodID!=3), "PaymentMethodID", "PaymentMethod");
            return View(pur_Bills);
        }

        // POST: purshaseBill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        
        public JsonResult Edit( int BillID,string BillNo, int NewAmount,string chequeBransh, string chequeNumber, DateTime chequeNumberDate, int methodIDToBill)
        {
            if (ModelState.IsValid)
            {
              //  db.Entry(pur_purshaseOrders).State = EntityState.Modified;

                var v = db.pur_Bills.Where(I => I.BillID == BillID).FirstOrDefault();
                var OldAmount = v.BillPaidAmount;
                if (v.BillTotalAmount - (OldAmount + NewAmount) > 0)
                {
                    
                    v.BillPaidAmount = OldAmount + NewAmount;
                    v.BillRemainingAmount = v.BillTotalAmount - (OldAmount + NewAmount);
                    v.PaymentMethodID = v.PaymentMethodID == methodIDToBill ? v.PaymentMethodID : 3;
                    v.BillPaymentStatus = v.BillRemainingAmount == 0 ? 2 : 1;
                    db.SaveChanges();
                }
                else
                {
                    return Json(0);
                }
                //var lastbill = ( from items in db.pur_BillDetails where (items.BillNo == BillNo) select items ).ToList();
                //var mid = methodIDToBill;
                //foreach(var x in lastbill)
                //{
                //    if (x.PaymentMethodID != methodIDToBill)
                //    {
                //        mid = 3;
                //        break;
                //    }
                //}
                string decription = " دفع مبلغ بقيمة : ";
                decription += " " + NewAmount + " ";
                decription += "الى المورد : ";
                decription += v.SuplierName + "";
                decription += " والمتبقى : ";
                decription += v.BillTotalAmount - (OldAmount + NewAmount) + "  ";
                
                var billhestory = new pur_BillDetails
                {
                    Amount = NewAmount,
                    PaymentMethodID= methodIDToBill,
                    BillDetailsDate=DateTime.Now,
                    BillDetailsDescription = decription,
                    BillNo = BillNo,
                    ChequeBank = chequeBransh,
                    ChequeDate=chequeNumberDate,
                    ChequeNo=chequeNumber,
                    UpLoaded=false,
                    ChequeBankBranch=chequeBransh,

                };
                db.pur_BillDetails.Add(billhestory);
                db.SaveChanges();
                #region
                var suplier = db.pay_SuplierAcounts.Where(s => s.SuplierID == v.SuplierID).Count();
                decimal? LastSuplierAcountAmount = 0;
                if (suplier > 0)
                {
                    LastSuplierAcountAmount = db.pay_SuplierAcounts.Where(s => s.SuplierID == v.SuplierID).OrderBy(c => 1 == 1).Skip(suplier - 1).FirstOrDefault().SuplierAcountAmount;
                }
                else
                {
                    LastSuplierAcountAmount = 0;
                }
                var supAcount = new pay_SuplierAcounts
                {
                    PaymentMethodID = methodIDToBill,
                    SuplierAcountDepit = NewAmount,
                    PaymentDescription = decription,
                    SuplierAcountAmount = 0,//LastSuplierAcountAmount - NewAmount,
                    SuplierAcountCredit = 0,
                    SuplierID = v.SuplierID,
                    SuplierName = v.SuplierName,
                    SuplierAcountTransactionDate = DateTime.Now,
                    UpLoaded = false
                };
                db.pay_SuplierAcounts.Add(supAcount);
                db.SaveChanges();
                #endregion
            }


            return Json(1);
        }

        // GET: purshaseBill/Delete/5
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
