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
    public class InventoryRequestCartController : Controller
    {
        //private InventoryRequestCart db = new InventoryRequestCart();
        //private InvertoryModel InvertoryModel = new InvertoryModel();
        //private ItemsModel ItemsModel = new ItemsModel();
        //private PaymentMethodModel PaymentMethodModel = new PaymentMethodModel();
        private MainModel db = new MainModel();

        // GET: InventoryRequestCart
        public async Task<ActionResult> Index()
        {


            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }

            var InvertoriesDropDownList = db.inv_Inventory    // your starting point - table in the "from" statement
           .Join(db.inv_Items, // the source table of the inner join
              inv_Inventory => inv_Inventory.InvertoryID,        // Select the primary key (the first part of the "on" clause in an sql "join" statement)
              inv_Items => inv_Items.InventoryID,   // Select the foreign key (the second part of the "on" clause)
              (inv_Inventory,inv_Items) => new { Items = inv_Items, Inventory  = inv_Inventory }) // selection
           .Where(InventoryAndItem => InventoryAndItem.Items.ItemQuantity >0).ToList();

            List<inv_Inventory> tempList = new List<inv_Inventory>();
            foreach (var item in InvertoriesDropDownList)
            {
                var temp = item.Inventory;
                tempList.Add(temp);
            }

            ViewBag.InvertoriesDropDownList =  new SelectList(tempList, "InvertoryID", "InvertoryNameAr");

            ViewBag.ItemsDropDownList = new SelectList(db.inv_Items.Where(SubCategory => (SubCategory.CategoryID == 0)), "ItemID", "ItemNameAr");
            ViewBag.CustomersDropDownList = db.sal_Customer.ToList();
             

            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");
            ViewBag.Customers = new SelectList(db.sal_Customer, "CustomerID", "CustomerName");
             
            return View(await db.inv_RequestCart.Where(r => (r.RequestType == 1)).ToListAsync());
        }

        public async Task<ActionResult> Customer()
        {


            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");

            }

            ViewBag.InvertoriesDropDownList = new SelectList(db.inv_Inventory, "InvertoryID", "InvertoryNameAr");

            ViewBag.ItemsDropDownList = new SelectList(db.inv_Items.Where(SubCategory => (SubCategory.CategoryID == 0)), "ItemID", "ItemNameAr");
            ViewBag.CustomersDropDownList = db.sal_Customer.ToList();


            ViewBag.PaymentMethod = new SelectList(db.pay_PaymentMethod, "PaymentMethodID", "PaymentMethod");
            ViewBag.Customers = new SelectList(db.sal_Customer, "CustomerID", "CustomerName");

              

            return View(await db.inv_RequestCart.Where(r => (r.RequestType == 2)).ToListAsync());
        }



        public async Task <JsonResult> InsertInvertoryCart(inv_RequestCart RequestCart, int? RequestType)
        {

            // Get item details from items table
            var Item = db.inv_Items.Where(I => I.ItemID == RequestCart.ItemID ).FirstOrDefault();
            var ItemInventoryOldQuantity = Item.ItemQuantity;
            // Get item details from items table

            // Salse Cart Item 
            var v = db.inv_RequestCart.Where(I => I.ItemID == RequestCart.ItemID && I.RequestType == RequestType).FirstOrDefault();
            // Salse Cart Item 

            int status = 0;
            var newQnatity = RequestCart.ItemQuantity;

            if ((ItemInventoryOldQuantity - newQnatity) >= 0)
            {


                // Update item in sales cart
                if (v != null)
                {

                    var cartOldQuantity = v.ItemQuantity;
                    v.ItemQuantity = cartOldQuantity + RequestCart.ItemQuantity;

                    var TotalItemsPrice = (cartOldQuantity + RequestCart.ItemQuantity) * Item.ItemSalePrice;
                    v.TotalItemsPrice = TotalItemsPrice;
                    await db.SaveChangesAsync();
                     
                    status = 1;  // success
                }

                // insert new item in sales cart
                else
                {

                    RequestCart.ItemPrice = Item.ItemSalePrice;
                    RequestCart.RequestType = RequestType;
                    RequestCart.TotalItemsPrice = RequestCart.ItemQuantity * Item.ItemSalePrice;

                    db.inv_RequestCart.Add(RequestCart);
                    await  db.SaveChangesAsync();
                    status = 1; // success
                }


            }


            // New Quantity > inventory Quantity
            else
            {
                status = 3;  // errror

            }



            return new JsonResult { Data = new { status = status } };

            //return Json(SalesCart);
        }

        public  async  Task <JsonResult> UpdatetRequestCart(inv_RequestCart RequestCart, decimal TotalItemsPrice, int RequestCartID)
        {

            var v = db.inv_RequestCart.Where(I => I.RequestCartID == RequestCartID).FirstOrDefault();


            // Get item details from items table
            var Item = db.inv_Items.Where(I => I.ItemID == RequestCart.ItemID).FirstOrDefault();
            var ItemInventoryQuantity = Item.ItemQuantity;
            var CartOldQuantity = v.ItemQuantity;
            // Get item details from items table 

            int status = 0;
            
            // Depet (+) Update 
            if (RequestCart.ItemQuantity > CartOldQuantity)
            {
                var newQuantity = RequestCart.ItemQuantity + CartOldQuantity;

                // Check Item Quantity in Inventory 
                if (ItemInventoryQuantity - newQuantity >= 0)
                {
                    v.ItemQuantity = RequestCart.ItemQuantity;
                    v.TotalItemsPrice = TotalItemsPrice;
                    await db.SaveChangesAsync();;
                    
                    status = 1;
                }

                // Cart Quantity > Item Quantity
                else
                {
                    status = 3;
                }

            }


            // Cart quantity > new quantity (-)
            else if (CartOldQuantity > RequestCart.ItemQuantity)
            {
                    var newQuantity = CartOldQuantity - RequestCart.ItemQuantity;
                if (ItemInventoryQuantity - newQuantity >= 0)
                {
                    v.ItemQuantity = RequestCart.ItemQuantity;
                    v.TotalItemsPrice = TotalItemsPrice;
                    await db.SaveChangesAsync();
                    status = 1;
                }
                else
                {
                    status = 3;
                }

            }

            else
            {
                status = 1;
            }

            
            return new JsonResult { Data = new { status = status } };

            //return Json(SalesCart);
        }

        public  async Task < ActionResult> DeleteCartItem(int id)
        {
            int status = 0;

            inv_RequestCart RequestCartItem = db.inv_RequestCart.Find(id);
            var CartQuantity = RequestCartItem.ItemQuantity;


            // Get Item Old Quantity from inventroy 
            var Item = db.inv_Items.Where(I => I.ItemID == RequestCartItem.ItemID).FirstOrDefault();
            var InventroyQuantity = Item.ItemQuantity;
            
            // Update Item  Quantity in  inventroy 
            Item.ItemQuantity = (int)InventroyQuantity + (int)CartQuantity;
            await db.SaveChangesAsync();;
           

            // Delete Item from request cart 
            db.inv_RequestCart.Remove(RequestCartItem);
           await db.SaveChangesAsync();


            status = 1;
            return new JsonResult { Data = new { status = status } }; ;
        }


        public JsonResult ComplateRequestOrder(string OrderpaymentAmount, string ItemIDArray, string NamesArray, string ItemPriceArray, string QuantitiesArray, int RequestSourceID, int RequestType)
        {

            int LastRequest;
            string OrderNumber =" ";
            List<inv_Requests> LastList = db.inv_Requests.ToList();
            if (LastList == null)
            {
                LastRequest = 1;
            }
            else
            {
                LastRequest = db.inv_Requests.Max(item => item.RequestID); ;
            }

            if(RequestType == 1)
            {
                OrderNumber = "POS" + DateTime.Now.ToString("yyyyMMdd") + (LastRequest + 1);
            }
            else if(RequestType ==2)
            {
                OrderNumber = "CUS" + DateTime.Now.ToString("yyyyMMdd") + (LastRequest + 1);
            }

             
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
                using (SqlCommand cmd = new SqlCommand("inv_Insert_RequestOrder"))
                {

                    var TotatItmesAmount = ItemsSellPrice.Sum();
                    var PointOfSaleID = Session["PointOfSaleID"];
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.AddWithValue("@RequestSourceTypeID", 1);  // Point Of Sale Request
                        cmd.Parameters.AddWithValue("@RequestSourceID", RequestSourceID);
                        cmd.Parameters.AddWithValue("@RequestSourceName", ' ');
                        cmd.Parameters.AddWithValue("@RequestDestinationID",0);
                        cmd.Parameters.AddWithValue("@RequestNo", OrderNumber);        
                        cmd.Parameters.AddWithValue("@RequestTotalSaleAmount", TotatItmesAmount);
                        cmd.Parameters.AddWithValue("@RequestPaidAmount", 0);
                        cmd.Parameters.AddWithValue("@RequestRemaining", 0);

 

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                     
                }
            }



            // ################################################ //
            //     Phase 2: Insert Inventory  Items  Request    //
            // ################################################ //
            using (SqlConnection con2 = new SqlConnection(constr))
            {

                for (int i = 0; i < Names.Length; i++)
                {
                    using (SqlCommand cmd2 = new SqlCommand("inv_Insert_RequestItems"))
                    {

                        var name = Names[i];
                        var quantity = Quantities[i];
                        var amount = ItemsSellPrice[i];
                        var itemID = ItemIDs[i];

                        inv_Items inv_Items = db.inv_Items.SingleOrDefault(I => I.ItemID == itemID);
                        // var name = inv_Items.ItemNameAr;
                       
                        var purchasePrice = inv_Items.ItemPuchasePrice;
                        var CategoryID = inv_Items.CategoryID;
                        var ItemSalePrice = inv_Items.ItemSalePrice;
                        var ItemPurshasePrice = inv_Items.ItemPuchasePrice;
 
           

                       cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@RequestNo", OrderNumber);
                        cmd2.Parameters.AddWithValue("@ItemID", itemID);
                        cmd2.Parameters.AddWithValue("@ItemNameAr", name);
                        cmd2.Parameters.AddWithValue("@ItemQuantity", quantity);
                        cmd2.Parameters.AddWithValue("@ItemSalePrice", ItemSalePrice);
                        cmd2.Parameters.AddWithValue("@ItemPurshasePrice", ItemPurshasePrice);
                        cmd2.Parameters.AddWithValue("@CategoryID", CategoryID);
                        cmd2.Parameters.AddWithValue("@ItemTotalAmount", amount);

                        

                        //cmd2.Parameters.AddWithValue("@ItemPurchasePrice", purchasePrice);

                        cmd2.Connection = con2;
                        con2.Open();

                        cmd2.ExecuteNonQuery();
                        con2.Close();

                        //   Functions.Functions.InsertItemHistory(itemID, quantity,"Sales Credit",0,0,"Sales Credit");

                     //   Functions.Functions.InsertSalesHistory(itemID, quantity, 0, amount, "Sales");

                    }
                    
                }

            }

        // ################################################ //
        //             Phase 3: Empty Sales Cart
        // ################################################ //

            using (SqlConnection con2 = new SqlConnection(constr))
            {
                using (SqlCommand cmd2 = new SqlCommand("inv_DeleteRequestCart"))
                { 

                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@RequestType", RequestType);
                    cmd2.Connection = con2;
                    con2.Open();
                    cmd2.ExecuteNonQuery();
                    con2.Close();


                }
            }




            return Json(1);

        }

         
        // GET: InventoryRequestCart/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            if (inv_RequestCart == null)
            {
                return HttpNotFound();
            }
            return View(inv_RequestCart);
        }

        // GET: InventoryRequestCart/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InventoryRequestCart/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "RequestCartID,ItemID,ItemName,ItemQuantity,ItemPrice,RequestCartDate")] inv_RequestCart inv_RequestCart)
        {
            if (ModelState.IsValid)
            {
                db.inv_RequestCart.Add(inv_RequestCart);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(inv_RequestCart);
        }

        // GET: InventoryRequestCart/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            if (inv_RequestCart == null)
            {
                return HttpNotFound();
            }
            return View(inv_RequestCart);
        }

        // POST: InventoryRequestCart/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestCartID,ItemID,ItemName,ItemQuantity,ItemPrice,RequestCartDate")] inv_RequestCart inv_RequestCart)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_RequestCart).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inv_RequestCart);
        }

        // GET: InventoryRequestCart/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            if (inv_RequestCart == null)
            {
                return HttpNotFound();
            }
            return View(inv_RequestCart);
        }

        // POST: InventoryRequestCart/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            inv_RequestCart inv_RequestCart = await db.inv_RequestCart.FindAsync(id);
            db.inv_RequestCart.Remove(inv_RequestCart);
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
