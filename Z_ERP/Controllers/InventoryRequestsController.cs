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
    public class InventoryRequestsController : Controller
    {
        private MainModel db = new MainModel();
         
         
        // GET: InventoryRequests
        public async Task<ActionResult> Index()
        {
            if (Session["UserName"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View(await db.inv_Requests.ToListAsync());
        }

        public JsonResult GetRequestsData()
        {

            List<inv_Requests> Requests = db.inv_Requests.Where(inv_Requests => (inv_Requests.RequestStatus == false)).ToList();



            return Json(new { data = Requests }, JsonRequestBehavior.AllowGet);
        }

        // GET: InventoryRequests/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Requests inv_POSRequests = await db.inv_Requests.FindAsync(id);
            if (inv_POSRequests == null)
            {
                return HttpNotFound();
            }

            var RequestNo = inv_POSRequests.RequestNo; //24598
            ViewBag.RaquestItems = db.inv_RequestItems.Where(I => I.RequestNo == RequestNo).ToList();

            var RaquestItems = db.inv_RequestItems.Where(I => I.RequestNo == RequestNo).ToList();


            return View(inv_POSRequests);
        }


        [HttpGet]
        public JsonResult ShiftRequestItems(string RequestNo)

        {
            int status =1;
         
            List<string> ErrorList = new List<string>();
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            var v = db.inv_Requests.Where(I => I.RequestNo == RequestNo).FirstOrDefault();


            if (v != null)
            {

                var RaquestItems = db.inv_RequestItems.Where(I => I.RequestNo == RequestNo).ToList();
                if (v.RequestSourceTypeID == 1) // Point Of Sale Request  (Shift Request into Sales Items)
                { 
                   
                    foreach (var Item in RaquestItems)
                    {
                        
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd2 = new SqlCommand("inv_ItemDecrease"))
                            {
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@ItemID", Item.ItemID);
                                cmd2.Parameters.AddWithValue("@RecieptNo", ' ');
                                cmd2.Parameters.AddWithValue("@Quntity", (int)Item.ItemQuantity);
                                cmd2.Parameters.AddWithValue("@SourceID", v.RequestDestinationID);
                                cmd2.Parameters.AddWithValue("@DestinationID", v.RequestSourceID);
                                cmd2.Parameters.AddWithValue("@SourceTypeID", 2);

                                var returnParameter = cmd2.Parameters.Add("@ErrorStatus", SqlDbType.Int);
                                returnParameter.Direction = ParameterDirection.ReturnValue;

                                cmd2.Connection = con;
                                con.Open();
                                cmd2.ExecuteNonQuery();

                                var ErrorStatus = returnParameter.Value.ToString();
                                
                                con.Close();

                                if(ErrorStatus == "2")
                                {
                                    status = 2;
                                    ErrorList.Add(Item.ItemNameAr);
                                  
                                }

                              
                            }
                        }

                    }

                    if(ErrorList.Count == 0) { 
                        v.RequestStatus = true;
                        status = 1;
                    }
                    else
                    {
                        status = 2; // Quantity error 
                    }
                    //  status = 1;
                    db.SaveChanges();
                }

                if (v.RequestSourceTypeID == 2) // Customer Request   (Shift Request into Sales Reciept)
                {

                    int LastReciept;

                    List<sal_Reciept> LastList = db.sal_Reciept.ToList();
                    if (LastList == null)
                    {
                        LastReciept = 1;
                    }
                    else
                    {
                        LastReciept = db.sal_Reciept.Max(item => item.RecieptID); ;
                    }

                    var RecieptNo = "SAL" + DateTime.Now.ToString("yyyyMMdd") + (LastReciept + 1);
                 
                    // ################################################ //
                    //        Phase 1: Insert Sales Reciept (order)     //
                    // ################################################ //
                
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        
                        string TotatItmesAmount =  v.RequestTotalSaleAmount.ToString();
                        string NetAmount = v.RequestTotalSaleAmount.ToString();
                        string  Remaining = v.RequestTotalSaleAmount.ToString();
                        string RequestSourceID = v.RequestSourceID.ToString();
                        string RequestDestinationID = v.RequestDestinationID.ToString();

                        Functions.Functions.SalesRecieptPayment( decimal.Parse(TotatItmesAmount), decimal.Parse(NetAmount), 0, 0, decimal.Parse(Remaining), RecieptNo, 1, " ", " ", " ", int.Parse(RequestSourceID));

                        SqlConnection con2 = new SqlConnection(constr);
                        foreach (var Item in RaquestItems)
                        {
                            using (SqlCommand cmd2 = new SqlCommand("inv_ItemDecrease"))
                            { 
                                cmd2.CommandType = CommandType.StoredProcedure;
                                cmd2.Parameters.AddWithValue("@ItemID", Item.ItemID);
                                cmd2.Parameters.AddWithValue("@RecieptNo", RecieptNo);
                                cmd2.Parameters.AddWithValue("@Quntity", (int)Item.ItemQuantity);
                                cmd2.Parameters.AddWithValue("@SourceID", RequestDestinationID);
                                cmd2.Parameters.AddWithValue("@DestinationID", RequestSourceID);
                                cmd2.Parameters.AddWithValue("@SourceTypeID", 2);

                                var returnParameter = cmd2.Parameters.Add("@ErrorStatus", SqlDbType.Int);
                                returnParameter.Direction = ParameterDirection.ReturnValue;

                                cmd2.Connection = con;
                                con.Open();
                                cmd2.ExecuteNonQuery();

                                var ErrorStatus = returnParameter.Value.ToString();

                                con.Close();

                                if (ErrorStatus == "2")
                                {
                                    status = 2;
                                    ErrorList.Add(Item.ItemNameAr);

                                }

                            }
                        }

                        if (ErrorList.Count == 0)
                        {
                            v.RequestStatus = true;
                            status = 1;
                        }
                        else
                        {
                            status = 2; // Quantity error 
                        }

                        db.SaveChanges();

                    }
 

                }
            }

            else  // Request not fount
            {
                status = 3;
            }



            return Json(new { status = status, ErrorList = ErrorList }, JsonRequestBehavior.AllowGet);
         //  return new Json { Data = new { status = status, ErrorList = ErrorList } , JsonRequestBehavior.AllowGet };
        }

       
        // GET: InventoryRequests/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inv_Requests inv_POSRequests = await db.inv_Requests.FindAsync(id);
            if (inv_POSRequests == null)
            {
                return HttpNotFound();
            }
            return View(inv_POSRequests);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "RequestID,PointOfSaleID,PointOfSaleName,RequestNo,RequestTotalAmount,RequestPaidAmount,RequestRemaining,RequestStatus,RequestDate")] inv_POSRequests inv_POSRequests)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inv_POSRequests).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(inv_POSRequests);
        }

        // GET: InventoryRequests/Delete/5
       
       

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
