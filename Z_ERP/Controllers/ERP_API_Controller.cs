using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

using Z_ERP.Models;
using Z_ERP.Security;

namespace Z_ERP.Controllers
{
    [BasicAuthentication]
    public class ERP_API_Controller : ApiController
    {
        private   MainModel db = new MainModel();
        private bool LogIsActive = true;
        [Route("api/GetExecute")]
        [HttpPost]
        public   async Task<ResponseObject> GetExecuteAsync(RequsetObject obj)
        {
            var Temp = new ResponseObject();
            try
            {
                if (LogIsActive)
                InsertLog(JsonConvert.SerializeObject(obj),1);
                #region "HR"
                if (obj.ServiceID.Equals("1"))//Get Department And JobsName
                {

                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.hr_Department.ToList().Take(500) : db.hr_Department.ToList().FindAll(x => !x.Uploaded).Take(500);
                    Temp.ResponseSubData = obj.GetAllData ? db.hr_JobsName.ToList().Take(500) : db.hr_JobsName.ToList().FindAll(x => !x.Uploaded).Take(500);
                }
                else if (obj.ServiceID.Equals("2"))//Get Employees
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.hr_Employees.ToList().Take(500) : db.hr_Employees.ToList().FindAll(x => !x.Uploaded).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("3"))//Get Employees Salary
                {

                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.hr_EmployeeSalary.ToList().Take(500) : db.hr_EmployeeSalary.ToList().FindAll(x => !x.Uploaded).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("4"))//Get Employees Salary History
                {

                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.hr_SalaryHistory.ToList().Take(500) : db.hr_SalaryHistory.ToList().FindAll(x => !x.UpLoaded).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("5"))//Get Expenses
                {

                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.hr_Expenses.ToList().Take(500) : db.hr_Expenses.ToList().FindAll(x => !x.Uploaded).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("102"))//Set Employees
                {
                    var Employees = JsonConvert.DeserializeObject<List<hr_Employees>>(obj.RequsetData.ToString());
                    foreach (var item in Employees)
                    {
                        var checkRecord = db.hr_Employees.Where(x => x.EmployeeUserName == item.EmployeeUserName).FirstOrDefault();
                        if (checkRecord == null)
                        {
                            db.hr_Employees.Add(item);
                        }
                        else
                        {
                            db.Entry(checkRecord).State = EntityState.Modified;
                        }
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;//obj.GetAllData ? db.hr_Employees.ToList().ToList() : db.hr_Employees.ToList().FindAll(x => !x.Uploaded);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("105"))//Set Expenses
                {
                    var Expenses = JsonConvert.DeserializeObject<List<hr_Expenses>>(obj.RequsetData.ToString());
                    foreach (var item in Expenses)
                    {
                        db.hr_Expenses.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;//obj.GetAllData ? db.hr_Expenses.ToList().ToList() : db.hr_Expenses.ToList().FindAll(x => !x.Uploaded);
                    Temp.ResponseSubData = null;
                }
                #endregion
                #region "Inventory Section"
                else if (obj.ServiceID.Equals("15"))//Get inv_Categories
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.inv_Categories.ToList().Take(500) : db.inv_Categories.ToList().FindAll(x => x.UpLoaded==false ).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("16"))//Get inv_Inventory
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.inv_Inventory.ToList().Take(500) : db.inv_Inventory.ToList().FindAll(x => x.UpLoaded==false).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("17"))//Get inv_ItemHistory
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.inv_ItemHistory.ToList().Take(500) : db.inv_ItemHistory.ToList().FindAll(x => x.UpLoaded==false ).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("18"))//Get inv_Items
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.inv_Items.ToList().Take(500) : db.inv_Items.ToList().FindAll(x => x.UpLoaded==false ).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("19"))//Get inv_POSRequests
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.inv_Requests.ToList().Take(500) : db.inv_Requests.ToList().FindAll(x => x.UpLoaded==false).Take(500);
                    Temp.ResponseSubData = obj.GetAllData ? db.inv_RequestItems.ToList().Take(500) : db.inv_RequestItems.ToList().FindAll(x => x.UpLoaded == false).Take(500);
                }
                else if (obj.ServiceID.Equals("20"))//Get inv_POSRequests
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.inv_RequestItems.ToList().Take(500) : db.inv_RequestItems.ToList().FindAll(x => x.UpLoaded==false ).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("115"))//Set inv_Categories
                {
                    var ListObject = JsonConvert.DeserializeObject<List<inv_Categories>>(obj.RequsetData.ToString());
                   
                    foreach (var item in ListObject)
                    {
                        db.inv_Categories.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("116"))//Set inv_Inventory
                {
                    var ListObject = JsonConvert.DeserializeObject<List<inv_Inventory>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject)
                    {
                        db.inv_Inventory.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("117"))//Set inv_ItemHistory
                {
                    var ListObject = JsonConvert.DeserializeObject<List<inv_ItemHistory>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject)
                    {
                        db.inv_ItemHistory.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("118"))//Set inv_Items
                {
                    var ListObject = JsonConvert.DeserializeObject<List<inv_Items>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject)
                    {
                        db.inv_Items.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("119"))//Set inv_POSRequests
                {
                    var ListObject = JsonConvert.DeserializeObject<List<inv_Requests>>(obj.RequsetData.ToString());
                    var ListSubObject = JsonConvert.DeserializeObject<List<inv_RequestItems >>(obj.RequsetSubData1.ToString());

                    //Dictionary<int, int> keyValues = new Dictionary<int, int>();
                   
                    foreach (var item in ListObject)
                    {
                        //var oldId = item.RequestID;
                        db.inv_Requests.Add(item);
                        await db.SaveChangesAsync();
                       // var NewID = db.inv_Requests.OrderByDescending(x => x.RequestID ).ToList()[0].RequestID ;
                        //keyValues.Add(oldId, NewID);
                    }
                    foreach (var item in ListSubObject)
                    {
                       //item.re
                        db.inv_RequestItems.Add(item);
                        await db.SaveChangesAsync();
                        
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("120"))//Set inv_POSRequestItems
                {
                    var ListObject = JsonConvert.DeserializeObject<List<inv_RequestItems>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject)
                    {
                        db.inv_RequestItems .Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                #endregion
                #region "Sales"
                else if (obj.ServiceID.Equals("26"))//Get Get Customer
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.sal_Customer .ToList().ToList() : db.sal_Customer.ToList().FindAll(x => x.UpLoaded == false);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("27"))//Get sal_pointOfSale
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.sal_pointOfSale .ToList().ToList() : db.sal_pointOfSale.ToList().FindAll(x => x.UpLoaded == false);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("28"))//Get sal_Reciept and sal_RecieptDetails and sal_Sales
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.sal_Reciept .ToList().ToList() : db.sal_Reciept.ToList().FindAll(x => x.UpLoaded == false);
                    Temp.ResponseSubData = obj.GetAllData ? db.sal_RecieptDetails .ToList().ToList() : db.sal_RecieptDetails.ToList().FindAll(x => x.UpLoaded == false); 
                    Temp.ResponseSubData2 = obj.GetAllData ? db.sal_Sales  .ToList().ToList() : db.sal_Sales.ToList().FindAll(x => x.UpLoaded == false); 
                }
                else if (obj.ServiceID.Equals("29"))//Get sal_SaleItems
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.sal_SaleItems .ToList().ToList() : db.sal_SaleItems.ToList().FindAll(x => x.UpLoaded == false);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("30"))//Get sal_SalesItemHistory
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.sal_SalesItemHistory .ToList().ToList() : db.sal_SalesItemHistory.ToList().FindAll(x => x.UpLoaded  == false);
                }
                else if (obj.ServiceID.Equals("126"))//Set inv_Categories
                {
                    var ListObject = JsonConvert.DeserializeObject<List<sal_Customer>>(obj.RequsetData.ToString());

                    foreach (var item in ListObject)
                    {
                        db.sal_Customer.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("127"))//Set sal_pointOfSale
                {
                    var ListObject = JsonConvert.DeserializeObject<List<sal_pointOfSale>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject)
                    {
                        db.sal_pointOfSale.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("128"))//Set sal_Reciept and sal_RecieptDetails and sal_Sales
                {
                    var ListObject1 = JsonConvert.DeserializeObject<List<sal_Reciept>>(obj.RequsetData.ToString());
                    var ListObject2 = JsonConvert.DeserializeObject<List<sal_RecieptDetails>>(obj.RequsetSubData1 .ToString());
                    var ListObject3 = JsonConvert.DeserializeObject<List<sal_Sales>>(obj.RequsetSubData2 .ToString());
                    foreach (var item in ListObject1)
                    {
                        var CheckItemInSale = await db.sal_Reciept.Where(x => x.RecieptNo == item.RecieptNo).FirstOrDefaultAsync();
                        if (CheckItemInSale == null)
                        {
                            db.sal_Reciept.Add(item);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            db.Entry(CheckItemInSale).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    foreach (var item in ListObject2)
                    {
                        var CheckItemInSale = await db.sal_RecieptDetails.Where(x => x.RecieptNo == item.RecieptNo && x.RecieptDetailsID ==item .RecieptDetailsID ).FirstOrDefaultAsync();
                        if (CheckItemInSale == null)
                        {
                            db.sal_RecieptDetails.Add(item);
                            await db.SaveChangesAsync();
                        }
                        else
                        {
                            db.Entry(CheckItemInSale).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                       // db.sal_RecieptDetails .Add(item);
                       // await db.SaveChangesAsync();
                    }
                    foreach (var item in ListObject3)
                    {

                        var CheckItemInSale = await db.sal_Sales.Where(x=>x.RecieptNo == item.RecieptNo && x.SalesItemsID == item .SalesItemsID  ).FirstOrDefaultAsync  ();
                        if (CheckItemInSale == null)
                        {
                            if (item.PointOfSaleID == 0)
                            {
                                InCreasazeItem((long)item.SalesItemsID, 2, 0, -1, (int)item.SaleQuantity , item.RecieptNo);
                            }
                            else
                            {
                                InCreasazeItem((long)item.SalesItemsID, 3, (int)item.PointOfSaleID, -1, (int)item.SaleQuantity, item.RecieptNo);
                            }
                        }
                        else
                        {
                            db.Entry(CheckItemInSale).State = EntityState.Modified;
                            await db.SaveChangesAsync();
                        }
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("129"))//Set inv_Items
                {
                    var ListObject = JsonConvert.DeserializeObject<List<sal_SaleItems>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject)
                    {
                        db.sal_SaleItems.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("130"))//Set inv_POSRequests
                {
                    var ListObject = JsonConvert.DeserializeObject<List<sal_SalesItemHistory>>(obj.RequsetData.ToString());
                 
                    foreach (var item in ListObject)
                    {
                        //item.re
                        db.sal_SalesItemHistory.Add(item);
                        await db.SaveChangesAsync();

                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                #endregion
                #region "Purchase section"
                else if (obj.ServiceID.Equals("36"))//Get Get pur_Supliers
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.pur_Supliers .ToList().Take (500) : db.pur_Supliers.ToList().FindAll(x => x.UpLoaded == false).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("37"))//Get pur_Bills and sal_RecieptDetails and sal_Sales
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.pur_Bills .ToList().Take(500) : db.pur_Bills.ToList().FindAll(x => x.UpLoaded == false).Take(500);
                    Temp.ResponseSubData = obj.GetAllData ? db.pur_BillDetails.ToList().Take(500) : db.pur_BillDetails.ToList().FindAll(x => x.UpLoaded == false).Take(500); 
                    Temp.ResponseSubData2 = obj.GetAllData ? db.pur_Purchase.ToList().Take(500) : db.pur_Purchase.ToList().FindAll(x => x.UpLoaded == false).Take(500); 
                }
                else if (obj.ServiceID.Equals("136"))//Set inv_Categories
                {
                    var ListObject = JsonConvert.DeserializeObject<List<pur_Supliers>>(obj.RequsetData.ToString());

                    foreach (var item in ListObject)
                    {
                        db.pur_Supliers.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("137"))//Set pur_Bills and pur_BillDetails and pur_Purchase
                {
                    var ListObject1 = JsonConvert.DeserializeObject<List<pur_Bills>>(obj.RequsetData.ToString());
                    var ListObject2 = JsonConvert.DeserializeObject<List<pur_BillDetails>>(obj.RequsetSubData1 .ToString());
                    var ListObject3 = JsonConvert.DeserializeObject<List<pur_Purchase>>(obj.RequsetSubData2 .ToString());
                    foreach (var item in ListObject1)
                    {
                        var CheckIt = db.pur_Bills.Where(x => x.BillNo == item.BillNo).FirstOrDefault();
                        if (CheckIt == null)
                        {
                            item.UpLoaded = item.UpLoaded ?? false;
                            db.pur_Bills.Add(item);
                        }
                        else
                        {
                            db.Entry(CheckIt).State = EntityState.Modified;
                        }
                        await db.SaveChangesAsync();
                    }
                    foreach (var item in ListObject2)
                    {
                        item.UpLoaded = item.UpLoaded ?? false;
                        var CheckIt = db.pur_BillDetails .Where(x => x.BillNo == item.BillNo).FirstOrDefault();
                        if (CheckIt == null)
                            db.pur_BillDetails.Add(item);
                        else
                            db.Entry(CheckIt).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    foreach (var item in ListObject3)
                    {
                        item.UpLoaded = item.UpLoaded ?? false;
                        var CheckIt = db.pur_Purchase .Where(x => x.BillNo == item.BillNo && x.PurchaseItemNameAr .Equals (item.PurchaseItemNameAr)).FirstOrDefault();
                        if (CheckIt == null)
                            db.pur_Purchase.Add(item);
                        else
                            db.Entry(CheckIt).State = EntityState.Modified;
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                #endregion

                #region "Payment section"
                else if (obj.ServiceID.Equals("46"))//Get Get pur_Supliers
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.pay_CustomerAcounts.ToList().Take(500) : db.pay_CustomerAcounts.ToList().FindAll(x => x.UpLoaded == false).Take(500);
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("47"))//Get pay_SuplierAcounts
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.pay_SuplierAcounts .ToList().Take(500) : db.pay_SuplierAcounts.ToList().FindAll(x => x.UpLoaded == false).Take(500);
                }
                else if (obj.ServiceID.Equals("48"))//Get pay_Payment
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.pay_Payment .ToList().Take(500) : db.pay_Payment.ToList().FindAll(x => x.UpLoaded == false).Take(500);
                }
                else if (obj.ServiceID.Equals("146"))//Set inv_Categories
                {
                    var ListObject = JsonConvert.DeserializeObject<List<pay_CustomerAcounts>>(obj.RequsetData.ToString());

                    foreach (var item in ListObject)
                    {
                        db.pay_CustomerAcounts.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("147"))//Set pay_SuplierAcounts
                {
                    var ListObject1 = JsonConvert.DeserializeObject<List<pay_SuplierAcounts>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject1)
                    {
                        db.pay_SuplierAcounts.Add(item);
                        await db.SaveChangesAsync();
                    }
                    
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("148"))//Set pay_SuplierAcounts
                {
                    var ListObject1 = JsonConvert.DeserializeObject<List<pay_Payment>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject1)
                    {
                        db.pay_Payment.Add(item);
                        await db.SaveChangesAsync();
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                #endregion
                #region "Complate Request Sectiona"
                else if (obj.ServiceID.Equals("200"))//Accept  inv_Requests
                {
                    var Request = JsonConvert.DeserializeObject<inv_Requests>(obj.RequsetData.ToString());
                    var ListSubObject = JsonConvert.DeserializeObject<List<inv_RequestItems>>(obj.RequsetSubData1.ToString());
                    int RequestID = 0;
                   if (db.inv_Requests.Where(x=>x.RequestNo == Request.RequestNo) == null)
                    {
                        db.inv_Requests.Add(Request);
                        await db.SaveChangesAsync();
                        var MyReqest = db.inv_Requests.Where(x => x.RequestNo == Request.RequestNo).FirstOrDefault ();
                        RequestID = MyReqest.RequestID;

                        var RequestItems = db.inv_RequestItems.Where(x => x.RequestNo == Request.RequestNo).ToList ();
                        if (RequestItems == null)
                        {
                            foreach (var item in ListSubObject)
                            {
                                //item.re
                                db.inv_RequestItems.Add(item);
                                await db.SaveChangesAsync();
                            }
                        }
                    }
                   if ( Request.RequestSourceID ==1)//Point Of Sale
                   {
                        foreach (var item in ListSubObject)
                        {
                            InCreasazeItem(item.ItemID ,1,(int) Request.RequestSourceID , (int)Request.RequestDestinationID ,(int) item.ItemQuantity ,"");
                        }
                   }
                   else if (Request.RequestSourceID == 2)//Customer Requst
                   {
                        var RecieptNo = "MOB" + DateTime.Now.ToString("yyyyMMdd");
                        var oldReceipt = db.sal_Reciept.Where (w=>w.RecieptNo .Contains ("MOB")).OrderByDescending(x => x.RecieptID).FirstOrDefault();
                        if (oldReceipt == null)
                        {
                            RecieptNo = RecieptNo + "1";
                        }
                        else
                        {
                         var lastReceiptNo=   oldReceipt.RecieptNo.Remove(0, RecieptNo.Length );
                            RecieptNo = RecieptNo + (int.Parse(lastReceiptNo) + 1).ToString();
                        }
                        var NewRequest = new sal_Reciept
                        {
                            CustomerID = Request.RequestSourceID ,
                            CustomerName = Request.RequestSourceName ,
                            RecieptDate = DateTime .Now ,
                            RecieptNo = RecieptNo,
                            RecieptTotalAmount = Request.RequestTotalSaleAmount ,
                            RecieptPaidAmount = Request.RequestPaidAmount ,
                            RecieptRemaining = Request.RequestTotalSaleAmount- Request.RequestPaidAmount,
                            RecieptNetAmount = Request.RequestTotalSaleAmount,
                            PointOfSaleID = 0,
                            PaymentMethodID = 1,
                            RecieptDiscount = 0,
                        };
                        db.sal_Reciept.Add(NewRequest);
                        await  db.SaveChangesAsync();
                        foreach (var item in ListSubObject)
                        {
                            InCreasazeItem(item.ItemID, 2, (int)Request.RequestSourceID, (int)Request.RequestDestinationID, (int)item.ItemQuantity, RecieptNo);
                        }
                    }

                    var RequestToUpdate = db.inv_Requests.Where(x => x.RequestNo == Request.RequestNo).FirstOrDefault();
                    RequestToUpdate.RequestStatus = true;
                    db.Entry(RequestToUpdate).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    //var oldId = item.RequestID;

                    // var NewID = db.inv_Requests.OrderByDescending(x => x.RequestID ).ToList()[0].RequestID ;
                    //keyValues.Add(oldId, NewID);


                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("201"))//set Purchase to Store 
                {
                    var ListObject1 = JsonConvert.DeserializeObject<pur_Bills>(obj.RequsetData.ToString());
                    var ListObject3 = JsonConvert.DeserializeObject<List<pur_Purchase>>(obj.RequsetSubData1.ToString());
                    var ThisBillIsExist = db.pur_Bills.Where (x=>x.BillNo == ListObject1.BillNo ).ToList().FirstOrDefault() ;
                    if (ThisBillIsExist==null)
                    {
                        db.pur_Bills.Add(ListObject1);
                        await db.SaveChangesAsync();
                    }

                    foreach (var item in ListObject3)
                    {
                        var ItemBill = db.pur_Purchase.Where(x => x.BillNo == item.BillNo && x.PurchaseItemNameAr.Equals (item.PurchaseItemNameAr  )).ToList().FirstOrDefault();
                        if (ItemBill == null)
                        {
                            item.UpLoaded = item.UpLoaded ?? false;
                            db.pur_Purchase.Add(item);
                            await db.SaveChangesAsync();
                            ItemBill = db.pur_Purchase.Where(x => x.BillNo == item.BillNo && x.PurchaseItemNameAr.Equals(item.PurchaseItemNameAr)).ToList().FirstOrDefault(); 
                        }
                        var InverntoryID = int.Parse(obj.RequsetSubData2.ToString());
                        AddItemToStore(ItemBill, InverntoryID);
                    }
                    if (ThisBillIsExist == null)
                    {
                        ThisBillIsExist= db.pur_Bills.Where(x => x.BillNo == ListObject1.BillNo).ToList().FirstOrDefault();
                    }
                    ThisBillIsExist.BillStatusID  = 3;
                    ThisBillIsExist.UpLoaded = false;
                    db.Entry(ThisBillIsExist).State = EntityState.Modified;
                    await db.SaveChangesAsync();
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                #endregion

                else if (obj.ServiceID.Equals("1000"))// Update 
                {
                    if (obj.RequsetData.ToString().Equals("100"))
                    {
                        var sqlQueryArray = obj.RequsetSubData1.ToString().Split('#');
                        foreach (var item in sqlQueryArray )
                        {
                            sqlExcute(obj.RequsetData.ToString(), item);
                        }
                    }
                    else
                    {
                        sqlExcute(obj.RequsetData.ToString(), obj.RequsetSubData1.ToString());
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = "";
                    Temp.ResponseSubData = null;
                }
                #region "Trucks"
                else if (obj.ServiceID.Equals("1001"))//Set Trucks and Trips and Expanses
                {
                    var ListObject = JsonConvert.DeserializeObject<List<trc_Trucks>>(obj.RequsetData.ToString());
                    foreach (var item in ListObject)
                    {
                        var oldTruckID = item.TruckID;
                        db.trc_Trucks.Add(item);
                        await db.SaveChangesAsync();
                        var NewTruckID = db.trc_Trucks.OrderByDescending(x => x.TruckID).ToList ()[0].TruckID ;
                        var SubListObject = JsonConvert.DeserializeObject<List<trc_Trips>>(obj.RequsetSubData1.ToString());
                        SubListObject = SubListObject.FindAll(t => t.TruckID == oldTruckID);
                        foreach (var Trip in SubListObject)
                        {
                            var OldTripID = Trip.TripID;
                            Trip.TruckID = NewTruckID;
                            db.trc_Trips.Add(Trip);
                            await db.SaveChangesAsync();
                            var NewTripID = db.trc_Trips.OrderByDescending(x => x.TruckID).ToList()[0].TripID;
                            var Sub1ListObject = JsonConvert.DeserializeObject<List<trc_Expenses>>(obj.RequsetSubData2 .ToString());
                            Sub1ListObject = Sub1ListObject.FindAll(t => t.TripID == OldTripID);
                            foreach (var Expanses in Sub1ListObject)
                            {
                                Expanses.TripID = NewTripID;
                                Expanses.TruckID = NewTruckID;
                                db.trc_Expenses.Add(Expanses);
                                await db.SaveChangesAsync();
                            }
                        }
                        var expensesTruck = JsonConvert.DeserializeObject<List<trc_Expenses>>(obj.RequsetSubData2.ToString());
                        expensesTruck = expensesTruck.FindAll(t => t.TruckID == oldTruckID && t.TripID == 0);
                        foreach (var Expanses in expensesTruck)
                        {
                            Expanses.TruckID = NewTruckID;
                            db.trc_Expenses.Add(Expanses);
                            await db.SaveChangesAsync();
                        }
                    }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("1002"))//Get Truck And Tript And Expanses
                {
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = obj.GetAllData ? db.trc_Trucks .ToList().Take(500) : db.trc_Trucks.ToList().FindAll(x => x.UpLoaded==false  ).Take(500); 
                    Temp.ResponseSubData = obj.GetAllData ? db.trc_Trips .ToList().Take(500) : db.trc_Trips .ToList().FindAll(x => x.UpLoaded==false ).Take(500); 
                    Temp.ResponseSubData2 = obj.GetAllData ? db.trc_Expenses .ToList().Take(500) : db.trc_Expenses.ToList().FindAll(x => x.UpLoaded==false ).Take(500) ;
                }
                else if (obj.ServiceID.Equals("1003"))//Set Trips and Expanses
                {
                        var SubListObject = JsonConvert.DeserializeObject<List<trc_Trips>>(obj.RequsetData .ToString());
                        foreach (var Trip in SubListObject)
                        {
                            var OldTripID = Trip.TripID;
                            db.trc_Trips.Add(Trip);
                            await db.SaveChangesAsync();
                            var NewTripID = db.trc_Trips.OrderByDescending(x => x.TruckID).ToList()[0].TripID;
                            var Sub1ListObject = JsonConvert.DeserializeObject<List<trc_Expenses>>(obj.RequsetSubData1.ToString());
                            Sub1ListObject = Sub1ListObject.FindAll(t => t.TripID == OldTripID);
                            foreach (var Expanses in Sub1ListObject)
                            {
                                Expanses.TripID = NewTripID;
                                db.trc_Expenses.Add(Expanses);
                                await db.SaveChangesAsync();
                            }
                        }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                else if (obj.ServiceID.Equals("1004"))//Set Trips and Expanses
                {
                        var Sub1ListObject = JsonConvert.DeserializeObject<List<trc_Expenses>>(obj.RequsetData .ToString());
                        foreach (var Expanses in Sub1ListObject)
                        {
                            
                            db.trc_Expenses.Add(Expanses);
                            await db.SaveChangesAsync();
                        }
                    Temp.ErrorNo = 0;
                    Temp.ErrorMassege = "";
                    Temp.ResponseData = null;
                    Temp.ResponseSubData = null;
                }
                #endregion
            }
            catch (Exception ex)
            {
                Temp.ErrorNo = 50;
                Temp.ErrorMassege = ex.Message;
            }

            //Return Output
            if (LogIsActive)
                InsertLog(JsonConvert.SerializeObject(Temp) ,2);
            return  Temp;
        }

        public static void InsertLog(string Message ,int LogType)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory;
            var floder = "API_Call_Logs";
            var logPath = path + floder;
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
            string filename = logPath+"/API_Log_" + DateTime.Now.ToString("ddMMyyyy") + ".txt";
            var fs = new FileStream(filename, FileMode.Append, FileAccess.Write);
            var s = new StreamWriter(fs);
            s.BaseStream.Seek(0, SeekOrigin.End);
            s.WriteLine("Time : " + DateTime.Now.ToString());
            var logTypeString = LogType == 1 ? "Json Request " : "Json Response :";
            s.WriteLine(logTypeString);
            s.WriteLine(Message);
            s.WriteLine("------------------------------------------------------------------------------------------------------------------------------------");
            s.WriteLine("");
            s.Close();
        }
        void sqlExcute(string ServiceId ,string param)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            
                using (SqlConnection con = new SqlConnection(constr))
                {
                    SqlCommand MyCommnd = new SqlCommand("api_Update", con);
                    MyCommnd.CommandType = CommandType.StoredProcedure;
                    MyCommnd.Parameters.Add("@ServiceID", SqlDbType.NVarChar).Value = ServiceId;
                    if (ServiceId.Equals("100"))
                    {
                        MyCommnd.Parameters.AddWithValue("@SqlQuery" , param);
                    }
                    else
                    {
                        var paramArray = param.Split('#');
                        int Count = 1;
                        foreach (var item in paramArray)
                        {
                            MyCommnd.Parameters.AddWithValue("@MaxID" + Count, item);
                            Count++;
                        }
                    }


                    var dt = new DataTable();
                    SqlDataAdapter adp = new SqlDataAdapter(MyCommnd);
                    DataSet ds = new DataSet();
                    // string temp = "";
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    adp.Fill(dt);
                    //cmd.ExecuteNonQuery();
                    con.Close();
                }
            
        }


        void InCreasazeItem(long ItemID, int SourceTypeID,int SourceID, int DestinationID ,int Quntity ,string RecieptNo)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand MyCommnd = new SqlCommand("api_ComplateRequest", con);
                MyCommnd.CommandType = CommandType.StoredProcedure;
                MyCommnd.Parameters.Add("@ItemID", SqlDbType.BigInt).Value = ItemID;
                MyCommnd.Parameters.Add("@SourceTypeID", SqlDbType.Int ).Value = SourceTypeID;
                MyCommnd.Parameters.Add("@SourceID", SqlDbType.Int ).Value = SourceID;
                MyCommnd.Parameters.Add("@DestinationID", SqlDbType.Int ).Value = DestinationID;
                MyCommnd.Parameters.Add("@Quntity", SqlDbType.Int ).Value = Quntity;
                MyCommnd.Parameters.Add("@RecieptNo", SqlDbType.NVarChar).Value = RecieptNo;
                var dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(MyCommnd);
                DataSet ds = new DataSet();
                // string temp = "";
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                adp.Fill(dt);
                //cmd.ExecuteNonQuery();
                con.Close();
            }

        }
        void AddItemToStore(pur_Purchase pur, int InvertoryToID)
        {
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            using (SqlConnection con = new SqlConnection(constr))
            {
                SqlCommand MyCommnd = new SqlCommand("[api_MoveItemsToStore]", con);
                MyCommnd.CommandType = CommandType.StoredProcedure;
                MyCommnd.Parameters.AddWithValue("@ItemID", pur.ItemID);
                MyCommnd.Parameters.AddWithValue("@ItemNameAr", pur.PurchaseItemNameAr);
                MyCommnd.Parameters.AddWithValue("@ItemQuantity", pur.PurchaseItemQuantity);
                MyCommnd.Parameters.AddWithValue("@ItemSalePrice", pur.ItemSalePrice);
                MyCommnd.Parameters.AddWithValue("@ItemPuchasePrice", pur.ItemPurchasePrice);
                MyCommnd.Parameters.AddWithValue("@ItemHistoryDecription", "توريد بضاعة جديدة");
                MyCommnd.Parameters.AddWithValue("@ItemHistoryProccessTypeID", 3);
                MyCommnd.Parameters.AddWithValue("@InventoryID", InvertoryToID);
                MyCommnd.Parameters.AddWithValue("@CategoryID", pur.CategoryID);
                MyCommnd.Parameters.AddWithValue("@ItemPurchaseCurrencyID", 1);
                MyCommnd.Parameters.AddWithValue("@PurchaseItemID", pur.PurchaseItemID);
                var dt = new DataTable();
                SqlDataAdapter adp = new SqlDataAdapter(MyCommnd);
                DataSet ds = new DataSet();
                // string temp = "";
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                adp.Fill(dt);
                //cmd.ExecuteNonQuery();
                con.Close();
            }

        }
    }

   public class ResponseObject
    {
        public int    ErrorNo { get; set; }
        public string ErrorMassege { get; set; }
        public object ResponseData { get; set; }
		public object ResponseSubData { get; set; }
		public object ResponseSubData2 { get; set; }
    }
    public class RequsetObject
    {
        public string  ServiceID { get; set; }
		public bool GetAllData { get; set; }
        public object  RequsetData { get; set; }
        public object RequsetSubData1 { get; set; }
        public object RequsetSubData2 { get; set; }
    }
}