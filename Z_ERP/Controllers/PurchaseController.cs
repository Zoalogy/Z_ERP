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
    public class PurchaseController : Controller
    {
        private MainModel db = new MainModel();
        


        // GET: Purchase
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }

            ViewBag.InvertoriesDropDownList =   new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            ViewBag.ItemsDropDownList = new SelectList(db.inv_Items.Where(SubCategory => (SubCategory.CategoryID == 0)), "ItemID", "ItemNameAr");

            return View(await db.pur_PurchaseCart.ToListAsync());
        }


        public JsonResult GetInvenrotyItems(int? InventoryID)
        {

            return Json(new SelectList(db.inv_Items.Where(Itmes => (Itmes.InventoryID == InventoryID)), "ItemID", "ItemNameAr"), JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
         
        public JsonResult ComplatePurchaseOrder(string OrderNumber,string OrderpaymentAmount, string ItemIDArray, string NamesArray, string ItemSellPriceArray, string ItempurchasePriceArray, string QuantitiesArray)
        {
            

            int[] ItemIDs = Array.ConvertAll(ItemIDArray.Split(','), int.Parse);
            string[] Names = NamesArray.Split(',');
            int[] ItemsSellPrice = Array.ConvertAll(ItemSellPriceArray.Split(','), int.Parse);


            int[] ItemspurchasePrice = Array.ConvertAll(ItempurchasePriceArray.Split(','), int.Parse);
            int[] Quantities = Array.ConvertAll(QuantitiesArray.Split(','), int.Parse);

             
            
            // ################################################ //
            //        Phase 2: Insert Purshase Bill (order)     //
            // ################################################ //
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Insert_PurshaseOrder"))
                {

                    var TotatItmesAmount = ItemsSellPrice.Sum();
                    var Remaining = TotatItmesAmount - int.Parse(OrderpaymentAmount) ;
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@purshaseOrderNumber", OrderNumber);
                    cmd.Parameters.AddWithValue("@purshaseOrderAmount", ItemsSellPrice.Sum());
                    cmd.Parameters.AddWithValue("@purshaseOrderPaidAmount", OrderpaymentAmount);
                    cmd.Parameters.AddWithValue("@purshaseOrderRemainAmount", Remaining);

                    
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
 


            // ################################################ //
            //             Phase 2: Insert Purshase History     //
            // ################################################ //
            using (SqlConnection con2 = new SqlConnection(constr))
            {

               
                for (int i=0; i< Names.Length; i++)
                { 
                    using (SqlCommand cmd2 = new SqlCommand("Insert_PurshasHistory"))
                    {

                        var name = Names[i];
                        var quantity = Quantities[i];
                        var amount = ItemsSellPrice[i];
                        var itemID = ItemIDs[i];


                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@PurchaseItemNameAr", name);
                        cmd2.Parameters.AddWithValue("@PurchaseItemQuantity", quantity);
                        cmd2.Parameters.AddWithValue("@PurchaseItemPriceSDG", amount);
                        cmd2.Parameters.AddWithValue("@SuplierID", 0);
                        cmd2.Parameters.AddWithValue("@ItemID", itemID);
                        cmd2.Parameters.AddWithValue("@BillNo", OrderNumber);
                        cmd2.Parameters.AddWithValue("@PurchaseStatusID", 0);

                        cmd2.Connection = con2;
                        con2.Open();
                        cmd2.ExecuteNonQuery();
                        con2.Close();
                         

                    }
                }
                
            }

            // ################################################ //
            //             Phase 3: Empty Parshase Cart
            // ################################################ //

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

            // ################################################ //
            //     Phase 4: Insert Purshases into Inventory     //
            // ################################################ //
            using (SqlConnection con2 = new SqlConnection(constr))
            {


                for (int i = 0; i < Names.Length; i++)
                {
                    using (SqlCommand cmd2 = new SqlCommand("Insert_PurshaseItems"))
                    {

                        var name = Names[i];
                        var quantity = Quantities[i];
                        var amount = ItemsSellPrice[i];
                        var itemID = ItemIDs[i];

                     cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@ItemID", itemID);
                        cmd2.Parameters.AddWithValue("@ItemNameAr", name);
                        cmd2.Parameters.AddWithValue("@ItemQuantity", quantity);
                        cmd2.Parameters.AddWithValue("@ItemPriceSDG", amount);
                        cmd2.Parameters.AddWithValue("@InventoryID", 3);
                        cmd2.Parameters.AddWithValue("@CategoryID", 0);
                        cmd2.Connection = con2;
                        con2.Open();
                        cmd2.ExecuteNonQuery();
                        con2.Close();


                    }
                }

            }


            // ################################################ //
            //     (OPtional)  Phase 5: Insert Purshases as Requseted  Items    //
            // ################################################ //
            using (SqlConnection con2 = new SqlConnection(constr))
            {


                for (int i = 0; i < Names.Length; i++)
                {
                    using (SqlCommand cmd2 = new SqlCommand("Insert_RequestItems"))
                    {

                        var name = Names[i];
                        var quantity = Quantities[i];
                        var amount = ItemsSellPrice[i];
                        var itemID = ItemIDs[i];

                        cmd2.CommandType = CommandType.StoredProcedure;

                        cmd2.Parameters.AddWithValue("@ItemID", itemID);
                        cmd2.Parameters.AddWithValue("@ItemNameAr", name);
                        cmd2.Parameters.AddWithValue("@ItemQuantity", quantity);
                        cmd2.Parameters.AddWithValue("@ItemPriceSDG", amount);
                        cmd2.Parameters.AddWithValue("@InventoryID", 3);
                        cmd2.Parameters.AddWithValue("@CategoryID", 0);
                        cmd2.Connection = con2;
                        con2.Open();
                        cmd2.ExecuteNonQuery();
                        con2.Close();


                    }
                }

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
