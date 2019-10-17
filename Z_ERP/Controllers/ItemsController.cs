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
    public class ItemsController : Controller
    {
        private MainModel db = new MainModel();
         
        public async Task<ActionResult> Index(int? id)
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }


            if (id == null)
            {
                //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                return RedirectToAction("Index", "Inventory");
            }



            inv_Inventory inv_Inventory2 = await db.inv_Inventory.FindAsync(id);
            if (inv_Inventory2 == null)
            {
                //return HttpNotFound();
                return RedirectToAction("Index", "Inventory");
            }
            //return View(inv_Inventory);


            Session["ii"] = id;
            //ViewBag.EmployeeData =   inv_Inventory.ToList();
            return View(inv_Inventory2);
        }

        public     ActionResult GetItemsData()
        {
             
            int InvertoryID =  int.Parse (Session["ii"].ToString());

            List<inv_Items> Items = (from item in db.inv_Items where item.InventoryID == InvertoryID select item).ToList();

            return Json(new { data = Items }, JsonRequestBehavior.AllowGet);
 
        }
        
        public JsonResult  GetSubCategory(int? CategoryID, int? ItemID)  
        { 
            var x = ItemID;
            //inv_Items inv_Items = db.inv_Items.Where(item => (item.CategoryID == CategoryID && item.ItemID == ItemID) );

            var y = db.inv_Items.Where(item => (item.CategoryID == CategoryID && item.ItemID == ItemID) ).ToArray();
            return Json(new SelectList(db.inv_SubCategories.Where(SubCategory => (SubCategory.CategoryID == CategoryID)), "SubCategoryID", "SubCategoryNameAr"), JsonRequestBehavior.AllowGet);
        }
 
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Items inv_Items = await db.inv_Items.FindAsync(id);
            if (inv_Items == null)
            {
                return HttpNotFound();
            }

            return Json(inv_Items, JsonRequestBehavior.AllowGet);
            //return View(inv_Items);
        }

        
         
        [HttpGet]
        public ActionResult Save(int id)
        { 
            var v = db.inv_Items.Where(a => a.ItemID == id).FirstOrDefault();
 
            ViewBag.CategoryDropDownList = new SelectList(db.inv_Categories, "CategoryID", "CategoryNameAR");  
            ViewBag.SubCategoryDropDownList = new SelectList(db.inv_SubCategories.Where(SubCategory => (SubCategory.CategoryID == 0)), "SubCategoryID", "SubCategoryNameAr");

            return View(v);
        }

       
        [HttpPost]
        public ActionResult Save(inv_Items Item)
        {
            int status = 0;
 
        // Start Validation cheack
            if (ModelState.IsValid)
            {
                 

                //################### Edit Item ###################//
                if (Item.ItemID > 0)
                    {
                       
                        var v = db.inv_Items.Where(I => I.ItemID == Item.ItemID).FirstOrDefault();
                        if (v != null)
                        {
                            v.ItemID = Item.ItemID;
                            v.ItemNameAr = Item.ItemNameAr;
                            v.ItemQuantity = Item.ItemQuantity;
                            v.ItemSalePrice = Item.ItemSalePrice;
                        v.CategoryID = Item.CategoryID;
                        v.SubCategoryID = Item.SubCategoryID;
                            v.ItemExpiredDate = Item.ItemExpiredDate;
                            status = 1; // 1 for update 
                        db.SaveChanges();
                    }
                    }
                //################### Edit Item //###################

                // Start Save New Item
                else
                {
                    #region
                    
                    string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    

                        using (SqlConnection con2 = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd2 = new SqlCommand("Insert_PurshaseItems"))
                            {


                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@ItemID", Item.ItemID);
                                cmd2.Parameters.AddWithValue("@ItemNameAr", Item.ItemNameAr);
                                cmd2.Parameters.AddWithValue("@ItemQuantity", Item.ItemQuantity);
                                cmd2.Parameters.AddWithValue("@sellamount", Item.ItemSalePrice);
                                cmd2.Parameters.AddWithValue("@ItemPuchasePrice", Item.ItemPuchasePrice);
                                cmd2.Parameters.AddWithValue("@ItemHistoryDecription", "توريد بضاعة جديدة");
                                cmd2.Parameters.AddWithValue("@ItemHistoryProccessTypeID", 3);
                                cmd2.Parameters.AddWithValue("@InventoryID", int.Parse(Session["ii"].ToString()) );
                                cmd2.Parameters.AddWithValue("@CategoryID", Item.CategoryID);
                                cmd2.Parameters.AddWithValue("@ItemPurchaseCurrencyID", 1);
                                cmd2.Parameters.AddWithValue("@BillNo", "-1");
                                cmd2.Connection = con2;
                                con2.Open();
                                cmd2.ExecuteNonQuery();
                                con2.Close();
                            }
                        }

                    #endregion
                            status = 2;  // 2 fro 
                        }
                    //End Save New Item

                    return new JsonResult { Data = new { status = status } };

            }
         // End Validation cheack


        // Start Send validation error
                else
                {
                    var errorList = ModelState.Values.SelectMany(m => m.Errors)
                                     .Select(e => e.ErrorMessage)
                                     .ToList();

                    return new JsonResult { Data = new { status = errorList } };
                }
        // End  Send validation error

        }

        [HttpGet]
        public ActionResult UpdateQuantity(int id)
        {

            var v = db.inv_Items.Where(a => a.ItemID == id).FirstOrDefault();
            return View(v);
        }


        [HttpPost]
        public  async  Task <ActionResult> UpdateQuantity(inv_Items Item,inv_ItemHistory ItemHistory ,decimal? NewQuantity)
        {
            int status = 0;
 
            long ItemId;

                if (Item.ItemID > 0)
                {
                    //Edit 
                    var v = db.inv_Items.Where(a => a.ItemID == Item.ItemID).FirstOrDefault();
                    if (v != null)
                    {

                        Session["OldQuantityTemp"]=   v.ItemQuantity;
                        Session["NewQuantityTemp"] = NewQuantity;

                        v.ItemID = Item.ItemID;
                         ItemId = Item.ItemID;
                    
                       v.ItemQuantity = int.Parse( Session["OldQuantityTemp"].ToString())+ int.Parse(Session["NewQuantityTemp"].ToString());
                    
                  await  db.SaveChangesAsync();
                    status =  1 ;


                    // Log ItemHistroy 

                    string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        using (SqlCommand cmd = new SqlCommand("Insert_ItemHistory"))
                        {
  
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@ItemID", Item.ItemID);
                            cmd.Parameters.AddWithValue("@ItemHistoryQuantity", long.Parse(Session["NewQuantityTemp"].ToString()));
                            cmd.Parameters.AddWithValue("@ItemHistoryProccessTypeID", 1);
                            cmd.Parameters.AddWithValue("@ItemHistoryDebitOrCredit", true);
                            cmd.Parameters.AddWithValue("@ItemHistoryDecription", "توريدة جديدة");
                             
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                     
                    //Log ItemHistroy 
 
                }

                }
                
                 
            return new JsonResult { Data = new { status = status } };
        }
 

        [HttpGet]
        public ActionResult Remove(int id)
        {
            var v = db.inv_Items.Where(a => a.ItemID == id).FirstOrDefault();
            return View(v);
        }

        [HttpPost]
        public ActionResult Remove(inv_Items Item)
        {

            inv_Items inv_Items = db.inv_Items.Find(Item.ItemID);

            db.inv_Items.Remove(inv_Items);
            db.SaveChanges();
            return new JsonResult { Data = new { status = 1 } };
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
