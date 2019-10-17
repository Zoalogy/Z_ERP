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
    public class SalesCartController : Controller
    {
        //private MainModel db = new MainModel();
        private MainModel db = new MainModel();
         
         
        // GET: SalesCart
        public async Task<ActionResult> Index()
        {

            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }


            var PointOfSaleID = Session["PointOfSaleID"].ToString();

           // ViewBag.InvertoriesDropDownList = new SelectList(InvertoryModel.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            ViewBag.ItemsDropDownList = new SelectList(db.sal_SaleItems.Where(SaleItem => (SaleItem.PointOfSaleID == 50)), "ItemID", "SaleItemsNameAr");


            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");

            ViewBag.Customers = new SelectList(db.sal_Customer, "CustomerID", "CustomerName");
             
            return View(await db.sal_SalesCart.Where(SaleCart => (SaleCart.SalesCartType == 1)).ToListAsync());
        }

        // GET: SalesCart/Details/5
         
        public async Task <JsonResult> InsertSalesCart(sal_SalesCart SalesCart, int? SalesCartType)
        {


            int status = 0;

            try
            {


                // Get item details from items table
                var PointOfSale = Session["PointOfSaleID"].ToString();
                int PointOfSaleID = int.Parse(PointOfSale);

                var Item = db.sal_SaleItems.Where(I => I.ItemID == SalesCart.ItemID && I.PointOfSaleID == PointOfSaleID).FirstOrDefault();
                         var ItemInventoryOldQuantity = Item.SaleItemsQuantity;
                    // Get item details from items table

                    // Salse Cart Item 
                     var v = db.sal_SalesCart.Where(I => I.ItemID == SalesCart.ItemID && I.SalesCartType == SalesCartType ).FirstOrDefault();
                    // Salse Cart Item 

                    var newQnatity = SalesCart.ItemQuantity;

                    if ( (ItemInventoryOldQuantity - newQnatity) >=0 )
                    {
                   
                        // Update item in sales cart
                        if (v != null)
                        {
                                var cartOldQuantity = v.ItemQuantity;
                                if (cartOldQuantity + SalesCart.ItemQuantity <= Item.SaleItemsQuantity)
                                {
                           
                                    v.ItemQuantity = cartOldQuantity + SalesCart.ItemQuantity;
                                    var TotalItemsPrice = (cartOldQuantity + SalesCart.ItemQuantity) * Item.ItemSalePrice;
                                    v.TotalItemsPrice = TotalItemsPrice;
                                    db.SaveChanges();
                                    status = 1;  // success

                                }

                                else
                                {
                                    status = 3; //Quantity error 
                                }
                            
                        }

                        // insert new item in sales cart
                        else
                        {

                            if ( SalesCart.ItemQuantity <= Item.SaleItemsQuantity)
                            {

                                SalesCart.ItemPrice = Item.ItemSalePrice;
                                SalesCart.TotalItemsPrice = SalesCart.ItemQuantity * Item.ItemSalePrice;

                                SalesCart.SalesCartType = SalesCartType;

                                db.sal_SalesCart.Add(SalesCart);
                                await db.SaveChangesAsync();
                                status = 1; // success
                            }
                            else
                            {
                                status = 3; //Quantity error 
                            }
                    }
 
                
                    }


                    // New Quantity > inventory Quantity
                    else
                    {
                        status = 3;  // errror
               
                    }

            }
            catch (Exception)
            {

                status = 4;
                throw;
            }

            return new JsonResult { Data = new { status = status } };

            //return Json(SalesCart);
        }
        public async Task<JsonResult> UpdatetSalesCart(sal_SalesCart SalesCart, string TotalItemsPrice, int? SalesCartType)
        {

            int status = 0;

            try
            {

                    var v = db.sal_SalesCart.Where(I => I.ItemID == SalesCart.ItemID && I.SalesCartType == SalesCartType).FirstOrDefault();

                // Get item details from items table

                        var PointOfSale = Session["PointOfSaleID"].ToString();
                        int PointOfSaleID = int.Parse(PointOfSale);

                         var SaleItem = db.sal_SaleItems.Where(I => I.ItemID == SalesCart.ItemID && I.PointOfSaleID == PointOfSaleID).FirstOrDefault();

                         var ItemInventoryQuantity = SaleItem.SaleItemsQuantity;
                         var CartOldQuantity = v.ItemQuantity;
                    // Get item details from items table 
                     

                    // Depet (+) Update 

                    if (SalesCart.ItemQuantity > CartOldQuantity)
                        {
                             var  newQuantity = SalesCart.ItemQuantity + CartOldQuantity;

                                // Check Item Quantity in Point Of Sale 
                                if (ItemInventoryQuantity - newQuantity >= 0)
                                {

                                    v.ItemQuantity = SalesCart.ItemQuantity;
                                    v.TotalItemsPrice = SalesCart.TotalItemsPrice ;
                                    db.SaveChanges();

                                   // SaleItem.SaleItemsQuantity = ItemInventoryQuantity - newQuantity;
                                    //await db.SaveChangesAsync();

                                    status = 1;
                                }

                                // Cart Quantity > Item Quantity
                                else
                                {
                                    status = 3;
                                }

                        }

                        // Cart quantity > new quantity (-)
                        else if(CartOldQuantity > SalesCart.ItemQuantity)
                         {
                                var newQuantity = CartOldQuantity - SalesCart.ItemQuantity;
                                if (ItemInventoryQuantity - newQuantity >= 0)
                                    {
                                        v.ItemQuantity = SalesCart.ItemQuantity;
                                        v.TotalItemsPrice = SalesCart.TotalItemsPrice;
                                        await db.SaveChangesAsync();
                                        status = 1;

                                    }

                                    else
                                    {
                                        status = 3;
                                    }
                          
                       }

                    else  // not (+) and not (-)
                    {
                        status = 1;
                    }


            }
            catch (Exception e )
            {


                status = 4;

            }


            return new JsonResult { Data = new { status = status } };

            //return Json(SalesCart);
        }

        public JsonResult ComplatePurchaseOrder(int PaymentMethodID, int CustomerID, decimal  Discount, string ChequeNumber, string ChequeBanck, string ChequeDate, decimal OrderpaymentAmount, string ItemIDArray, string NamesArray, string ItemPriceArray,  string QuantitiesArray, int?  SalesCartType)
        {

            int status = 1;

            List<string> ErrorList = new List<string>();

          try
            {


            int LastReciept;
            List<sal_Reciept> LastList = db.sal_Reciept.ToList();
            if (LastList == null)
            {
                LastReciept = 1;
            }
            else
            {
                LastReciept =  db.sal_Reciept.Max(item => item.RecieptID); ;
            }
            
            var RecieptNo =  "SAL"+DateTime.Now.ToString("yyyyMMdd")+ (LastReciept+1); 
            int[] ItemIDs = Array.ConvertAll(ItemIDArray.Split(','), int.Parse);
            string[] Names = NamesArray.Split(',');
            int[] ItemsSellPrice = Array.ConvertAll(ItemPriceArray.Split(','), int.Parse);


            //int[] ItemspurchasePrice = Array.ConvertAll(ItempurchasePriceArray.Split(','), int.Parse);
            int[] Quantities = Array.ConvertAll(QuantitiesArray.Split(','), int.Parse);



            // ################################################ //
            //        Phase 1: Insert Sales Reciept (order)     //
            // ################################################ //
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {       
                    //ItemsSellPrice.Sum();

                    var TotatItmesAmount = ItemsSellPrice.Sum();
                    var NetAmount = TotatItmesAmount - Discount;
                    var Remaining = NetAmount -OrderpaymentAmount;

                if (Remaining >= 0)
                {
                    Functions.Functions.SalesRecieptPayment(TotatItmesAmount, NetAmount, Discount, OrderpaymentAmount, Remaining, RecieptNo, PaymentMethodID, ChequeNumber, ChequeBanck,ChequeDate, CustomerID);
                }
            }

            
            // ################################################ //
            //             Phase 2: Insert Sales Items     //
            // ################################################ //
            using (SqlConnection con2 = new SqlConnection(constr))
            {


                        var PointOfSaleID = Session["PointOfSaleID"];

                        for (int i = 0; i < Names.Length; i++)
                        {
                            using (SqlCommand cmd2 = new SqlCommand("sal_Insert_Sales"))
                            {

                                    var name = Names[i];
                                    var quantity = Quantities[i];
                                    var amount = ItemsSellPrice[i];
                                    var itemID = ItemIDs[i];
 
                                    var SaleItem = db.sal_SaleItems.Where(I => I.ItemID == itemID).FirstOrDefault();
                         
                                    var salePrice = SaleItem.ItemSalePrice;
                                    var purchasePrice = SaleItem.ItemSalePrice;

                                    cmd2.CommandType = CommandType.StoredProcedure;
                                    cmd2.Parameters.AddWithValue("@RecieptNo", RecieptNo);
                                    cmd2.Parameters.AddWithValue("@SalesItemsID", itemID);
                                    cmd2.Parameters.AddWithValue("@ItemName", name);
                                    cmd2.Parameters.AddWithValue("@SaleQuantity", quantity);
                                    cmd2.Parameters.AddWithValue("@ItemSalePrice", salePrice);
                                    cmd2.Parameters.AddWithValue("@ItemPurchasePrice", purchasePrice);
                                    cmd2.Parameters.AddWithValue("@ItemTotalSaleAmount", amount);
                                    cmd2.Parameters.AddWithValue("@PointOfSaleID", PointOfSaleID);

                                    var returnParameter = cmd2.Parameters.Add("@ErrorStatus", SqlDbType.Int);
                                    returnParameter.Direction = ParameterDirection.ReturnValue;

                                    cmd2.Connection = con2;
                                    con2.Open();
                                    cmd2.ExecuteNonQuery();
                                    var ErrorStatus = returnParameter.Value.ToString();
                                    con2.Close();

                                    if (ErrorStatus == "2")
                                    {
                                        status = 2;
                                        ErrorList.Add(SaleItem.SaleItemsNameAr);

                                    }

                                    else
                                    {
                                        Functions.Functions.InsertSalesHistory(itemID, quantity, 0, amount, "Sales");

                                    }


                            } // Using 

                        }

                    }

                    // ################################################ //
                    //             Phase 3: Empty Sales Cart
                    // ################################################ //

                  if(ErrorList.Count == 0) { 
                    using (SqlConnection con2 = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd2 = new SqlCommand("sal_DeleteSalesCart"))
                        {

                
                            cmd2.CommandType = CommandType.StoredProcedure;
                            cmd2.Parameters.AddWithValue("@SalesCartType", SalesCartType);
                            cmd2.Connection = con2;
                            con2.Open();
                            cmd2.ExecuteNonQuery();
                            con2.Close();
                        }
                    }
                }


            }

        catch (Exception e)
        {
            e.Message.ToString();

                status = 4;
        }


            return Json(new { status = status, ErrorList = ErrorList }, JsonRequestBehavior.AllowGet);

        }


        public async   Task  <ActionResult> DeleteCartItem(int id)
        {
            int status = 0;


            try
            {

          
                sal_SalesCart SaleCartitem = db.sal_SalesCart.Find(id);


                //  Old item 

           
                var SaleItem = db.sal_SaleItems.Where(I => I.ItemID == SaleCartitem.ItemID).FirstOrDefault();

                var OldQuantity = SaleItem.SaleItemsQuantity; 

                SaleItem.SaleItemsQuantity = OldQuantity+SaleCartitem.ItemQuantity;
               await  db.SaveChangesAsync();
            //  Old item 

                // remove from cart
                db.sal_SalesCart.Remove(SaleCartitem);
                  await  db.SaveChangesAsync();
                // remove from cart
                
                status = 1;

            }
            catch (Exception)
            {

                status = 4;
                throw;
            }

            return new JsonResult { Data = new { status = status } }; ;
        }

         
        // GET: SalesCart/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesCart sal_SalesCart = await db.sal_SalesCart.FindAsync(id);
            if (sal_SalesCart == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesCart);
        }

        // POST: SalesCart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SalesCartID,ItemID,ItemName,ItemQuantity,ItemPrice,UserName,SalesCartDate")] sal_SalesCart sal_SalesCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sal_SalesCart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(sal_SalesCart);
        }

        // GET: SalesCart/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            sal_SalesCart sal_SalesCart = await db.sal_SalesCart.FindAsync(id);
            if (sal_SalesCart == null)
            {
                return HttpNotFound();
            }
            return View(sal_SalesCart);
        }

        // POST: SalesCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            sal_SalesCart sal_SalesCart = await db.sal_SalesCart.FindAsync(id);
            db.sal_SalesCart.Remove(sal_SalesCart);
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
