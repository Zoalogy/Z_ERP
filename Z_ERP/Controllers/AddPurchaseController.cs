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
using System.Configuration;
using System.Data.SqlClient;


namespace Z_ERP.Controllers
{
    public class AddPurchaseController : Controller
    {
        private MainModel db = new MainModel();



        // GET: Purchase
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");
            ViewBag.ItemsDropDownList = new SelectList(db.inv_Items.Where(SubCategory => SubCategory.CategoryID == 0),"إختر منتج", "ItemID", "ItemNameAr");
            ViewBag.ItemsCostPurchase = db.pur_PurchaseCart.Sum(e => e.ItemCostPurchase * e.ItemQuantity);
            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");
            ViewBag.ItemMainCategory = new SelectList(db.inv_Categories, "CategoryID", "CategoryNameAR", " ----- إختر تصنيف ----- ");
            ViewBag.Suppliers = new SelectList(db.pur_Supliers, "SuplierID", "SuplierName");
            return View(db.pur_PurchaseCart.ToList());
        }


        public JsonResult GetInvenrotyItems(int? InventoryID)
        {
            List<inv_Items> Items = (from  item in db.inv_Items select item ).Distinct().ToList();
            var Ites = (from Item in Items  group Item by Item.ItemNameAr );

            return Json(Ites, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetCegory(int? id=0)
        {
            
            return Json(new SelectList(db.inv_SubCategories.Where(sub=>sub.CategoryID==id).ToList(), "CategoryID", "SubCategoryNameAr"), JsonRequestBehavior.AllowGet);
        }


        [HttpPost]

        public JsonResult ComplatePurchaseOrder
            (DateTime chequeNumberDate, string OrderNumber, decimal OrderpaymentAmount, string CategoryIDArray, string ItemIDArray, string NamesArray, string ItempurchasePriceArray, string QuantitiesArray, decimal OrderAmount, int SuplierID, string SuplierName, int paymentMethodeID, string chequeNumber, string chequeBransh)
        {
            var Remaining = OrderAmount - OrderpaymentAmount;

            int[] ItemIDs = Array.ConvertAll(ItemIDArray.Split(','), int.Parse);
            string[] Names = NamesArray.Split(',');
            int[] CategoryIDs = Array.ConvertAll(CategoryIDArray.Split(','), int.Parse);

            int[] ItemspurchasePrice = Array.ConvertAll(ItempurchasePriceArray.Split(','), int.Parse);
            int[] Quantities = Array.ConvertAll(QuantitiesArray.Split(','), int.Parse);

            string decription = " إضافة بضاعة بقيمة : ";
            decription += " " + OrderAmount + " ";
            decription += "من المورد : ";
            decription += SuplierName + "";
            // ################################################ //
            //        Phase 0: select purchaseNo         //
            try
            {
                #region
                var NewbillSNo = db.pur_Bills.Count();
                var NewbillNo = "";
                string DateFormateToInventory = "PUR" + DateTime.Now.ToString("yyyyMMdd");
                if (NewbillSNo > 0)
                {
                    NewbillNo = db.pur_Bills.OrderByDescending(o => o.BillID).FirstOrDefault().BillNo;
                    var LastNo = NewbillNo.Remove(0, DateFormateToInventory.Length);
                    NewbillNo = DateFormateToInventory + (int.Parse(LastNo) + 1).ToString(); //NewbillNo.Substring(0, DateFormateToInventory.) +Int32.Parse(NewbillNo.Substring(9, -1)+1);
                }
                else
                {
                    NewbillNo = DateFormateToInventory + "1";
                }
                Console.Write("dsdsa");
                #endregion

                // ################################################ //
                //             Phase 2: Insert in   pur_BillDetails         //
                // ################################################ //
                #region
                var billDetailes = new pur_BillDetails
                {
                    Amount = OrderAmount,
                    BillDetailsDate = DateTime.Now,
                    BillDetailsDescription = decription,
                    BillNo = NewbillNo,
                    PaymentMethodID = paymentMethodeID,
                    ChequeNo = chequeNumber,
                    ChequeBank = chequeBransh,
                    UpLoaded = false,
                    ChequeDate = chequeNumberDate,
                    ChequeBankBranch = chequeBransh,
                };
                db.pur_BillDetails.Add(billDetailes);
                db.SaveChanges();
                #endregion
                // ################################################ //
                //        Phase 2: Insert Purshase Bill (order)     //
                // ################################################ //
                
                string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

                // ################################################ //
                //             Phase 2: Insert Purshase History     //
                // ################################################ //

                // ################################################ //
                //             Phase 3: Empty Parshase Cart
                // ################################################ //
                #region
                using (SqlConnection con2 = new SqlConnection(constr))
                {
                    using (SqlCommand cmd2 = new SqlCommand("Delete_PurchaseCart"))
                    {
                        cmd2.Connection = con2;
                        con2.Open();
                        cmd2.ExecuteNonQuery();
                        con2.Close();
                    }
                }
                #endregion

                // ################################################ //
                //        Phase 2: Insert Purshase      //
                // ################################################ //
                #region
                for (int i = 0; i < Names.Length; i++)
                {


                    var name = Names[i];
                    var quantity = Quantities[i];
                    //   var sellamount = ItemsSellPrice[i];
                    var itemID = ItemIDs[i];
                    // var ItemInventoryID = ItemInventoryIDs[i];

                    //var ItemCostPurchase = ItemsCostPurchase[i];
                    var ItempurchasePrice = ItemspurchasePrice[i];

                    var purchase = new pur_Purchase
                    {
                        BillNo = NewbillNo,
                        CategoryID = CategoryIDs[i],
                        ItemCostPurchase = 0,
                        ItemID = itemID,
                        ItemPurchaseCurrencyID = 1,
                        ItemPurchasePrice = ItempurchasePrice,
                        ItemSalePrice = 0,
                        ItemSaleCurrencyID = 1,
                        PurchaseItemID = itemID,
                        ItemTotalPurchaseAmount = ItempurchasePrice * quantity,
                        PurchaseItemNameAr = name,
                        PurchaseItemNameEn = name,
                        PurchaseItemAddedDate = DateTime.Now,
                        PurchaseItemQuantity = quantity,
                        PurchaseStatusID = 0,
                        IsReturn = false,
                        SubCategoryID = 1,
                        UpLoaded = false,
                        BarCode = "",
                        SuplierID = SuplierID,
                        SuplierName = SuplierName

                    };
                    db.pur_Purchase.Add(purchase);
                    db.SaveChanges();


                }
                #endregion

                // ################################################ //
                //     (OPtional)  Phase 5: Insert Purshases as Requseted  Items    //
                // ################################################ //

                // ############################################## //
                //   Phase 6: Insert Suplier payment transaction  //
                // ############################################## //
                #region
                var suplier = db.pay_SuplierAcounts.Where(s => s.SuplierID == SuplierID).Count();
                decimal? LastSuplierAcountAmount = 0;
                if (suplier > 0)
                {
                    LastSuplierAcountAmount = db.pay_SuplierAcounts.OrderBy(c => 1 == 1).Skip(suplier - 1).FirstOrDefault().SuplierAcountAmount;
                }
                else
                {
                    LastSuplierAcountAmount = 0;
                }
                var suplierPayment = new pay_SuplierAcounts();
                //suplierPayment.SuplierID = SuplierID;
                //suplierPayment.SuplierName = SuplierName;
                //suplierPayment.SuplierAcountCredit = OrderAmount;
                //suplierPayment.CustomeAcountDepit = 0;
                //suplierPayment.UpLoaded = false;
                //suplierPayment.PaymentDescription = decription;
                //suplierPayment.PaymentMethodID = paymentMethodeID;
                //suplierPayment.SuplierAcountTransactionDate = DateTime.Now;
                //suplierPayment.SuplierAcountAmount = LastSuplierAcountAmount + OrderAmount;
                //db.pay_SuplierAcounts.Add(suplierPayment);
                //db.SaveChanges();
                decription = " دفع مبلغ بقيمة : ";
                decription += " " + OrderpaymentAmount + " ";
                decription += " والمتبقى : ";
                decription += Remaining + "  ";
                decription += "الى المورد : ";
                decription += SuplierName + "";
                suplierPayment.SuplierID = SuplierID;
                suplierPayment.SuplierName = SuplierName;
                suplierPayment.SuplierAcountCredit = 0;
                suplierPayment.SuplierAcountDepit = OrderpaymentAmount;
                suplierPayment.UpLoaded = false;
                suplierPayment.PaymentDescription = decription;
                suplierPayment.PaymentMethodID = paymentMethodeID;
                suplierPayment.SuplierAcountAmount = 0;///(LastSuplierAcountAmount + OrderAmount) - OrderpaymentAmount;
                suplierPayment.SuplierAcountTransactionDate = DateTime.Now;
                db.pay_SuplierAcounts.Add(suplierPayment);
                db.SaveChanges();


                #endregion
                // ############################################## //
                //   Phase 6: Insert bill payment transaction  //
                // ############################################## //
                #region
                string Billdecription = " إضافة بضاعة بقيمة : ";
                Billdecription += " " + OrderAmount + " ";
                Billdecription += "من المورد : ";
                Billdecription += SuplierName + "";
                var lastPayMent = db.pur_Bills.Count();
                decimal? lastPayMentAmount = 0;
                if (lastPayMent > 0)
                {
                    lastPayMentAmount = db.pur_Bills.OrderBy(c => 1 == 1).Skip(lastPayMent - 1).FirstOrDefault().BillNetAmount;
                }
                else
                {
                    lastPayMentAmount = 0;
                }
                var bill = new pur_Bills
                {

                    TotalCostPurchase = 0,
                    BillNo = NewbillNo,
                    SuplierID = SuplierID,
                    SuplierName = SuplierName,
                    TotalAmount = OrderAmount,
                    BillPaidAmount = OrderpaymentAmount,
                    BillRemainingAmount = Remaining,
                    BillStatusID = 0,
                    BillDescription = decription,
                    BillPaymentStatus = 1,
                    BillDate = DateTime.Now,
                    BillTotalAmount = OrderAmount,
                    BillIsReturned = false,
                    BillNetAmount = OrderAmount,
                    UpLoaded = false,
                    PaymentMethodID = paymentMethodeID,
                };
                db.pur_Bills.Add(bill);
                db.SaveChanges();
                #endregion
                if (Remaining == 0)
                {

                    Functions.Functions.InsertPaymentHistory(0, OrderpaymentAmount, decription, paymentMethodeID);
                }
                else
                {
                    Functions.Functions.InsertPaymentHistory(0, OrderpaymentAmount, decription, paymentMethodeID);
                    Functions.Functions.InsertPaymentHistory(Remaining, 0, decription, paymentMethodeID);
                }
            }
            catch(Exception ex)
            {
                return Json(2);
            }

            return Json(1);

        }

        // GET: Purchase/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_Purchase pur_Purchase = await db.pur_Purchase.FindAsync(id);
            if (pur_Purchase == null)
            {
                return HttpNotFound();
            }
            return View(pur_Purchase);
        }

        // GET: Purchase/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Purchase/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "PurchaseItemID,PurchaseItemNameAr,PurchaseItemNameEn,PurchaseItemQuantity,PurchaseItemPriceSDG,PurchaseItemPriceUSD,SuplierID,CategoryID,SubCategoryID,PurchaseItemAddedDate,ItemID,BillNo,PurchaseStatusID")] pur_Purchase pur_Purchase)
        {
            if (ModelState.IsValid)
            {
                db.pur_Purchase.Add(pur_Purchase);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(pur_Purchase);
        }

        // GET: Purchase/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_Purchase pur_Purchase = await db.pur_Purchase.FindAsync(id);
            if (pur_Purchase == null)
            {
                return HttpNotFound();
            }
            return View(pur_Purchase);
        }

        // POST: Purchase/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "PurchaseItemID,PurchaseItemNameAr,PurchaseItemNameEn,PurchaseItemQuantity,PurchaseItemPriceSDG,PurchaseItemPriceUSD,SuplierID,CategoryID,SubCategoryID,PurchaseItemAddedDate,ItemID,BillNo,PurchaseStatusID")] pur_Purchase pur_Purchase)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pur_Purchase).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(pur_Purchase);
        }

        // GET: Purchase/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            pur_Purchase pur_Purchase = await db.pur_Purchase.FindAsync(id);
            if (pur_Purchase == null)
            {
                return HttpNotFound();
            }
            return View(pur_Purchase);
        }

        // POST: Purchase/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            pur_Purchase pur_Purchase = await db.pur_Purchase.FindAsync(id);
            db.pur_Purchase.Remove(pur_Purchase);
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
