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
    public class AcceptGoodController : Controller
    {
        private MainModel db = new MainModel();
         
        

        // GET: purshaseBill
        public ActionResult Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            return View();
        }

        public JsonResult GetBillsData()
        {
//            List<pur_Bills> Supliers = db.pur_Bills.Where(pu=>pu.BillIsReturned == false).ToList();
            var result = db.pur_Bills.Where(s => s.BillStatusID == 1 &&  s.BillIsReturned == false).ToList();
            return Json(new { data = result.OrderBy(e => e.BillDate) }, JsonRequestBehavior.AllowGet);
        }
        public JsonResult toInventory(string id)
        {
            var billID = db.pur_Bills.Where(e => e.BillNo == id).FirstOrDefault().BillStatusID;
            var bill = db.pur_Bills.Find(id);
            
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
            ViewBag.billID = billNumber;
            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");
            return View(db.pur_Purchase.Where(e => e.BillNo == billNumber).ToList());
        }
        public JsonResult InsertItemsToInv(string BillNo,int InvertoryToID)
        {
            // ################################################ //
            //     Phase 1: Insert Purshases into Inventory     //
            // ################################################ //
            #region
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            var purchase = db.pur_Purchase.Where(pur => pur.BillNo == BillNo).ToList();
            foreach(var pur in purchase)
            {

                using (SqlConnection con2 = new SqlConnection(constr))
                {
                    using (SqlCommand cmd2 = new SqlCommand("Insert_PurshaseItems"))
                    {


                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@ItemID", pur.ItemID);
                        cmd2.Parameters.AddWithValue("@ItemNameAr", pur.PurchaseItemNameAr);
                        cmd2.Parameters.AddWithValue("@ItemQuantity", pur.PurchaseItemQuantity);
                        cmd2.Parameters.AddWithValue("@sellamount", pur.ItemSalePrice);
                        cmd2.Parameters.AddWithValue("@ItemPuchasePrice", pur.ItemPurchasePrice );
                        cmd2.Parameters.AddWithValue("@ItemHistoryDecription", "توريد بضاعة جديدة");
                        cmd2.Parameters.AddWithValue("@ItemHistoryProccessTypeID", 3);
                        cmd2.Parameters.AddWithValue("@InventoryID", InvertoryToID);
                        cmd2.Parameters.AddWithValue("@CategoryID", pur.CategoryID);
                        cmd2.Parameters.AddWithValue("@ItemPurchaseCurrencyID", 1);
                        cmd2.Parameters.AddWithValue("@BillNo", BillNo);
                        cmd2.Connection = con2;
                        con2.Open();
                        cmd2.ExecuteNonQuery();
                        con2.Close();
                    }
                }

            }


            #endregion

            var bills = db.pur_Bills.Where(p => p.BillNo == BillNo).FirstOrDefault().BillID;
            var update = db.pur_Bills.Find(bills);
            update.BillStatusID = 3;
            db.Entry(update).State = EntityState.Modified;
            db.SaveChanges();
            return Json( 1, JsonRequestBehavior.AllowGet);
        }






        // GET: purshaseBill/Edit/5
       

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
