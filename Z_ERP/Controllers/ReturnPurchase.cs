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
    public class ReturnPurchaseController : Controller
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

            return View(await db.pur_Bills.ToListAsync());
        }

        public JsonResult GetBillsData()
        {
            List<pur_Bills> Supliers = db.pur_Bills.Where(pu=> pu.BillIsReturned == false).ToList();

            return Json(new { data = Supliers }, JsonRequestBehavior.AllowGet);
        }
        public async Task<ActionResult> Cost(string billNumber)
        { 

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");
            ViewBag.pur_Bills = db.pur_Supliers.Find(db.pur_Bills.Where(o=>o.BillNo==billNumber).Select(o=>o.SuplierID).First()).SuplierName;
            ViewBag.billNo = billNumber;
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");
            return View(await db.pur_Purchase.Where(e=>e.BillNo == billNumber && e.IsReturn == false ).ToListAsync());
        }
        public JsonResult getPaymentData(string billNumber)
        {
            
           return Json(new { data = db.pur_Bills.Where(o => o.BillNo == billNumber).First() }, JsonRequestBehavior.AllowGet);
            
        }
        public JsonResult ReturnGood(string BillNo,int ItemID,string ItemNameAr,int ItemQuantity, double ItemCostPurchase, double ItemPuchasePrice, double sellamount, int InventoryID,int CategoryID)
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
                    ///////////////
                    //                    @ItemID int= 1,
                    //@ItemNameAr nvarchar(100) = 'test',
                    //	@ItemQuantity   int = 5.00,
                    //    @ItemPuchasePrice   int = 50.00,
                    //    @InventoryID    int = 1,
                    //    @CategoryID int = 0,
                    //    @ItemPurchaseCurrencyID int = 1,

                    //    -------------------------------
                    //    --@SuplierName nvarchar(100) = 'test',
                    //	--@SuplierID    int = 5.00,

                    //    --@ItemCostPurchase int = 50.00,
                    //    @sellamount int = 50.00,
                    //    @PurchaseStatusID   int = 1
                    ////////////
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
        public JsonResult ReturnAllGood(string BillNo)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            var billID = db.pur_Bills.Where(b => b.BillNo == BillNo).FirstOrDefault().BillID;

            var bill = db.pur_Bills.Find(billID);
            #region
            //
           // var suplier = db.pay_SuplierAcounts.Where(s => s.SuplierID == v.SuplierID).Count();
         // decimal? LastSuplierAcountAmount = 0;
        //    if (suplier > 0)
        //    {
        //        LastSuplierAcountAmount = db.pay_SuplierAcounts.Where(s => s.SuplierID == bill.SuplierID).OrderBy(c => 1 == 1).Skip(suplier - 1).FirstOrDefault().SuplierAcountAmount;
        //    }
        //    else
        //    {
        //        LastSuplierAcountAmount = 0;
        //    }
        //    var suplierPayment = new pay_SuplierAcounts
        //    {

        //        SuplierID = bill.SuplierID,
        //        SuplierName = bill.SuplierName,
        //        SuplierAcountCredit = su.SuplierAcountAmount,
        //        CustomeAcountDepit = 0,
        //        UpLoaded = false,
        //        PaymentDescription = "تم ارجاع بضاعة بقية" + bill.BillPaidAmount,
        //        PaymentMethodID = 1,
        //        SuplierAcountTransactionDate = DateTime.Now,
        //        SuplierAcountAmount = su.SuplierAcountAmount,
        //    };
        //db.pay_SuplierAcounts.Add(suplierPayment);
        //    db.SaveChanges();
            #endregion

            using (SqlConnection con2 = new SqlConnection(constr))
            {

                //@ItemPurchaseCurrencyID int = 1,

                

                using (SqlCommand cmd2 = new SqlCommand("Pur_Return_AllPurshaseItems"))
                {
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@BillNo", BillNo);
                    
                    cmd2.Connection = con2;
                    con2.Open();
                    cmd2.ExecuteNonQuery();
                    con2.Close();


                }
            }
            //            //    var Remaining = OrderAmount - OrderpaymentAmount;

            //            //int[] ItemIDs = Array.ConvertAll(ItemIDArray.Split(','), int.Parse);
            //            //string[] Names = NamesArray.Split(',');
            //            //int[] CategoryIDs = Array.ConvertAll(CategoryIDArray.Split(','), int.Parse);

            //            //int[] ItemspurchasePrice = Array.ConvertAll(ItempurchasePriceArray.Split(','), int.Parse);
            //            //int[] Quantities = Array.ConvertAll(QuantitiesArray.Split(','), int.Parse);

            //            // ################################################ //
            //            //        Phase 1: Insert PurshaseOrder         //
            //            // ################################################ //
            //            #region 
            //            var parchaceOrder = db.pur_purshaseOrders.Where(r => r.purshaseOrderNumber == BillNo).First();
            //            //{
            //            //    purshaseOrderAmount = OrderAmount,
            //            //    purshaseOrderDate = DateTime.Now,
            //            //    purshaseOrderNumber = OrderNumber,
            //            //    purshaseOrderPaidAmount = OrderpaymentAmount,
            //            //    purshaseOrderRemainAmount = Remaining,
            //            //    SuplierID = SuplierID
            //            //};
            //        db.pur_purshaseOrders.Add(parchaceOrder);
            //            db.SaveChanges();

            //            #endregion
            //            // ################################################ //
            //            //             Phase 2: Insert in   pur_BillDetails         //
            //            // ################################################ //
            //            #region
            //            var billDetailes = new pur_BillDetails
            //            {
            //                Amount = OrderAmount,
            //                BillDetailsDate = DateTime.Now,
            //                BillDetailsDescription = decription,
            //                BillNo = OrderNumber,
            //                PaymentMethodID = paymentMethodeID,
            //                ChequeNo = chequeNumber,
            //                ChequeBank = chequeBransh,
            //                UpLoaded = false,
            //                ChequeDate = DateTime.Now,
            //                ChequeBankBranch = "",
            //            };
            //        db.pur_BillDetails.Add(billDetailes);
            //               db.SaveChanges();
            //            #endregion
            //            // ################################################ //
            //            //        Phase 2: Insert Purshase Bill (order)     //
            //            // ################################################ //
            //            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            //            // ################################################ //
            //            //             Phase 2: Insert Purshase History     //
            //            // ################################################ //

            //            // ################################################ //
            //            //             Phase 3: Empty Parshase Cart
            //            // ################################################ //
            //            #region
            //            using (SqlConnection con2 = new SqlConnection(constr))
            //            {                                                                              
            //                using (SqlCommand cmd2 = new SqlCommand("Delete_PurchaseCart"))            
            //                {                                                                          
            //                    cmd2.Connection = con2;                                                
            //                    con2.Open();                                                           
            //                    cmd2.ExecuteNonQuery();                                                
            //                    con2.Close();                                                          
            //                }                                                                          
            //            }
            //            #endregion

            //            // ################################################ //
            //            //        Phase 2: Insert Purshase      //
            //            // ################################################ //
            //            #region
            //            for (int i = 0; i<Names.Length; i++)
            //            {


            //                var name = Names[i];
            //var quantity = Quantities[i];
            ////   var sellamount = ItemsSellPrice[i];
            //var itemID = ItemIDs[i];
            //// var ItemInventoryID = ItemInventoryIDs[i];

            ////var ItemCostPurchase = ItemsCostPurchase[i];
            //var ItempurchasePrice = ItemspurchasePrice[i];
            ////        cmd2.CommandType = CommandType.StoredProcedure;
            ////        ///////////////

            ////        //////////////
            ////        cmd2.Parameters.AddWithValue("@ItemID", itemID);
            ////        cmd2.Parameters.AddWithValue("@ItemNameAr", name);
            ////        cmd2.Parameters.AddWithValue("@ItemQuantity", quantity);
            ////        cmd2.Parameters.AddWithValue("@sellamount", 0);
            ////        cmd2.Parameters.AddWithValue("@ItemPuchasePrice", ItempurchasePrice);
            ////        //cmd2.Parameters.AddWithValue("@ItemCostPurchase", 0);
            ////        cmd2.Parameters.AddWithValue("@InventoryID", 0);
            ////        cmd2.Parameters.AddWithValue("@CategoryID", 0);
            ////        cmd2.Parameters.AddWithValue("@ItemPurchaseCurrencyID", 0);
            ////        /////////////////////////////
            ////        cmd2.Parameters.AddWithValue("@PosID",  Convert.ToInt32(Session["PointOfSaleID"].ToString()));

            ////cmd2.Parameters.AddWithValue("@ItemTotalPurchaseAmount", ItempurchasePrice * quantity);
            //var purchase = new pur_Purchase
            //{
            //    BillNo = OrderNumber,
            //    CategoryID = CategoryIDs[i],
            //    ItemCostPurchase = 0,
            //    ItemID = itemID,
            //    ItemPurchaseCurrencyID = 0,
            //    ItemPurchasePrice = ItempurchasePrice,
            //    ItemSalePrice = 0,
            //    ItemSaleCurrencyID = 0,
            //    PurchaseItemID = itemID,
            //    ItemTotalPurchaseAmount = ItempurchasePrice,
            //    PurchaseItemNameAr = name,
            //    PurchaseItemAddedDate = DateTime.Now,
            //    PurchaseItemQuantity = quantity,
            //    PurchaseStatusID = 0,

            //    SubCategoryID = 1,
            //    UpLoaded = false
            //};
            //db.pur_Purchase.Add(purchase);
            //                db.SaveChanges();


            //        }
            //            #endregion

            //            // ################################################ //
            //            //     (OPtional)  Phase 5: Insert Purshases as Requseted  Items    //
            //            // ################################################ //
            //            #region
            //            //using (SqlConnection con2 = new SqlConnection(constr))
            //            //{


            //            //    for (int i = 0; i < Names.Length; i++)
            //            //    {
            //            //        using (SqlCommand cmd2 = new SqlCommand("Insert_RequestItems"))
            //            //        {

            //            //            var name = Names[i];
            //            //            var quantity = Quantities[i];
            //            //            var amount = ItemsSellPrice[i];
            //            //            var itemID = ItemIDs[i];
            //            //            var ItemInventoryID = ItemInventoryIDs[i];
            //            //            cmd2.CommandType = CommandType.StoredProcedure;

            //            //            cmd2.Parameters.AddWithValue("@ItemID", itemID);
            //            //            cmd2.Parameters.AddWithValue("@ItemNameAr", name);
            //            //            cmd2.Parameters.AddWithValue("@ItemQuantity", quantity);
            //            //            cmd2.Parameters.AddWithValue("@ItemPriceSDG", amount);
            //            //            cmd2.Parameters.AddWithValue("@InventoryID", ItemInventoryID);
            //            //            cmd2.Parameters.AddWithValue("@CategoryID", 0);
            //            //            cmd2.Connection = con2;
            //            //            con2.Open();
            //            //            cmd2.ExecuteNonQuery();
            //            //            con2.Close();


            //            //        }
            //            //    }

            //            //}

            //            #endregion
            //            // ############################################## //
            //            //   Phase 6: Insert Suplier payment transaction  //
            //            // ############################################## //
            //            #region
            //            var suplier = db.pay_SuplierAcounts.Where(s => s.SuplierID == SuplierID).ToList();
          
            //            #endregion
            //            // ############################################## //
            //            //   Phase 6: Insert bill payment transaction  //
            //            // ############################################## //
            //            #region
            //            string Billdecription = " إضافة بضاعة بقيمة : ";
            //Billdecription += " " + OrderAmount + " ";
            //            Billdecription += "من المورد : ";
            //            Billdecription += SuplierName + "";
            //            var bill = new pur_Bills
            //            {

            //                TotalCostPurchase = 0,
            //                BillNo = OrderNumber,
            //                SuplierID = SuplierID,
            //                SuplierName = SuplierName,
            //                TotalAmount = OrderAmount,
            //                BillPaidAmount = OrderpaymentAmount,
            //                BillRemainingAmount = Remaining,
            //                BillDescription = decription,
            //                BillPaymentStatus = 1,
            //                BillDate = DateTime.Now,
            //                UpLoaded = false,
            //                PaymentMethodID = paymentMethodeID,
            //            };
            //db.pur_Bills.Add(bill);
            //            db.SaveChanges();
            //            #endregion
            //            if (Remaining == 0)
            //            {

            //                Functions.Functions.InsertPaymentHistory(0, OrderpaymentAmount, decription, paymentMethodeID);
            //            }
            //            else
            //            {
            //                Functions.Functions.InsertPaymentHistory(0, OrderpaymentAmount, decription, paymentMethodeID);
            //                Functions.Functions.InsertPaymentHistory(Remaining, 0, decription, paymentMethodeID);
            //            }


            return Json(1, JsonRequestBehavior.AllowGet);

        }


        // GET: purshaseBill/Details/5
      

        // GET: purshaseBill/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: purshaseBill/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
     


        public JsonResult UpdatetPurchaseCostAndSellPrice(pur_Purchase PurchaseCart)
        {

            var v = db.pur_Purchase.Where(I => I.BillNo == PurchaseCart.BillNo && I.ItemID == PurchaseCart.ItemID).FirstOrDefault();
            if (v != null)
            {
                v.ItemSalePrice = PurchaseCart.ItemSalePrice;
                v.ItemCostPurchase = PurchaseCart.ItemCostPurchase;
                
                v.UpLoaded = false;
                db.SaveChanges();
            }
            return Json(1);
        }





        // GET: purshaseBill/Edit/5
       
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
