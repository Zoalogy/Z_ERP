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
    public class purshaseCostController : Controller
    {
        private MainModel db = new MainModel();
         
        

        // GET: purshaseBill
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            var result = db.pur_Bills.Where(s => db.pur_Purchase.Where(e => e.BillNo == s.BillNo && e.PurchaseStatusID == 0).Count() > 0).ToList();
            return View( result);
        }

        public JsonResult GetBillsData()
        {
//            List<pur_Bills> Supliers = db.pur_Bills.Where(pu=>pu.BillIsReturned == false).ToList();
            var result = db.pur_Bills.Where(s => s.BillStatusID == 0 &&  s.BillIsReturned == false && db.pur_Purchase.Where(e => e.BillNo == s.BillNo && e.PurchaseStatusID == 0).Count() > 0).ToList();
            return Json(new { data = result }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult toInventory(string id)
        {
            var billID = db.pur_Bills.Where(e => e.BillNo == id).FirstOrDefault().BillID;
            var bill = db.pur_Bills.Find(billID);
            
            if (bill.BillID > 0)
            {
                bill.BillStatusID = 1;
                db.Entry(bill).State = EntityState.Modified;
                
                db.SaveChanges();
            }
            else
            {
                return Json(0, JsonRequestBehavior.AllowGet);
            }
            return Json(1, JsonRequestBehavior.AllowGet);
        }
        
        public ActionResult Cost(string billNumber)
        { 

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            return View( db.pur_Purchase.Where(e=>e.BillNo == billNumber && e.PurchaseStatusID == 0).ToList());
        }
        public JsonResult InsertItemsToInv(string BillNo,int ItemID,string ItemNameAr,int ItemQuantity, double ItemCostPurchase, double ItemPuchasePrice, double sellamount, int InventoryID,int CategoryID)
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
                    cmd2.Parameters.AddWithValue("@ItemHistoryDecription", "توريد بضاعة جديدة");
                    cmd2.Parameters.AddWithValue("@ItemHistoryProccessTypeID", 3);
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



        // GET: purshaseBill/Details/5
       

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





     

        // POST: purshaseBill/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
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
