using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Collections.Generic; 
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Z_ERP.Models;


namespace Z_ERP.Functions
{
    public class Functions
    {



        // ################################################ //
        //              Payment Functions     //
        // ################################################ //

        //   Sales Payement Reciept //

        public static void SalesRecieptPayment(decimal TotatItmesAmount, decimal NetAmount, decimal Discount,decimal OrderpaymentAmount, decimal Remaining, string OrderNumber, int PaymentMethodID, string ChequeNumber, string ChequeBanck, string ChequeDate, int CustomerID)
             {
            // ################################################ //
            //        Phase 1: Insert Sales Reciept (order)     //
            // ################################################ //
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            var PointOfSaleID = HttpContext.Current.Session["PointOfSaleID"];
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("sal_Insert_SalesOrder"))
                { 
                    if (Remaining >= 0)
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@RecieptNo", OrderNumber);
                        cmd.Parameters.AddWithValue("@PaymentMethodID", PaymentMethodID);
                        cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                        cmd.Parameters.AddWithValue("@CustomerName", ' ');
                        cmd.Parameters.AddWithValue("@RecieptTotalAmount", TotatItmesAmount);
                        cmd.Parameters.AddWithValue("@RecieptDiscount", Discount);
                        cmd.Parameters.AddWithValue("@RecieptRemaining", Remaining);
                        cmd.Parameters.AddWithValue("@RecieptNetAmount", NetAmount);
                        cmd.Parameters.AddWithValue("@RecieptPaidAmount", OrderpaymentAmount);
                        cmd.Parameters.AddWithValue("@RecieptChequeNo", ChequeNumber);
                        cmd.Parameters.AddWithValue("@RecieptChequeDate", ChequeDate);
                        cmd.Parameters.AddWithValue("@RecieptChequeBank", ChequeBanck);
                        cmd.Parameters.AddWithValue("@RecieptChequeBranch", "");
                        cmd.Parameters.AddWithValue("@RecieptDescription", "");
                        cmd.Parameters.AddWithValue("@PointOfSaleID", PointOfSaleID);

                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();

                        // Payment History
                        InsertPaymentHistory(OrderpaymentAmount, 0, "Sales Reciept Payment", 1);


                        // ######################################  //
                        // #   Record Customer Payment Book     #  //
                        // ######################################  //

                            // 1: Credit  Coustomer  By Reciept Amount  
                            CustomerPaymentBookRecord(PaymentMethodID,2, TotatItmesAmount,"First Time Reciept Credit",CustomerID);

                            // 1: Depit   -> Reciept Second payment
                                if (OrderpaymentAmount > 0)
                                {
                                  CustomerPaymentBookRecord(PaymentMethodID,1, OrderpaymentAmount, "Customer payment", CustomerID);
                            }

                    }

                    else
                    {
                        
                    }

                }
            }

        }
        public static void  InsertPaymentHistory(decimal Depit, decimal Credit, string Description, int PaymentType)
        {
             
             string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("pay_Insert_PaymentHistory"))
                { 

                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.AddWithValue("@PaymentDepitAmount", Depit);
                    cmd.Parameters.AddWithValue("@PaymentCreditAmount", Credit);
                    cmd.Parameters.AddWithValue("@PaymentDescription", Description);
                    cmd.Parameters.AddWithValue("@PaymentType", PaymentType);
                 
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }



        }




    // ################################################ //
    //              Sales Functions     //
    // ################################################ //


            // Item Histroy Function //
        public static void InsertItemHistory(int ItemId, int Quantity, string ProccessType,int ProccessTypeId,int DebitOrCredit, string Decription)
        {

            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("Insert_ItemHistory"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ItemID", ItemId);
                    cmd.Parameters.AddWithValue("@ItemHistoryQuantity", Quantity);
                    cmd.Parameters.AddWithValue("@ItemHistoryProccessTypeID", ProccessTypeId);
                    cmd.Parameters.AddWithValue("@ItemHistoryProccessType", ProccessType);
                    cmd.Parameters.AddWithValue("@ItemHistoryDebitOrCredit", DebitOrCredit);
                    cmd.Parameters.AddWithValue("@ItemHistoryDecription", Decription);


                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }



        }


        public static void InsertSalesHistory(int SaleItemID, decimal SaleItemQuantity, int SaleItemDebitOrCredit, decimal ItemAmount, string ItemHistoryDescription)

        {

            var PointOfSaleID = HttpContext.Current.Session["PointOfSaleID"];

            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("sal_Insert_SalesHistory"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaleItemID", SaleItemID);
                    cmd.Parameters.AddWithValue("@SaleItemQuantity", SaleItemQuantity);
                    cmd.Parameters.AddWithValue("@SaleItemDebitOrCredit", SaleItemDebitOrCredit);
                    cmd.Parameters.AddWithValue("@ItemAmount", ItemAmount);
                    cmd.Parameters.AddWithValue("@ItemHistoryDescription", ItemHistoryDescription);
                    cmd.Parameters.AddWithValue("@PointOfSaleID", PointOfSaleID);



                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }



        }


        // Return Sales Item 
        public static void ReturnSalesItem(int? SaleItemID, int? PointOfSaleID, long? ReturnedQuantity)

        {

            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("sal_Return_SalesItem"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@SaleItemID", SaleItemID);
                    cmd.Parameters.AddWithValue("@PointOfSaleID", PointOfSaleID);
                    cmd.Parameters.AddWithValue("@RetrunedQuantity", ReturnedQuantity);
                     
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }



        }



        // ################################################ //
        //              Shift invenrty requent Functions     //
        // ################################################ //

        public static void ShiftInvenrtyRequent(int ItemID, string ItemName, decimal ItemQuantity, decimal SaleAmount, decimal PurchaseAmount, int? PointOfSaleID)

        {

            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("inv_Shift_RequestOrderToPontOfSale"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
 
                    cmd.Parameters.AddWithValue("@ItemID", ItemID);
                    cmd.Parameters.AddWithValue("@ItemName", ItemName);
                    cmd.Parameters.AddWithValue("@ItemQuantity", ItemQuantity);
                    cmd.Parameters.AddWithValue("@SaleAmount", SaleAmount);
                    cmd.Parameters.AddWithValue("@PurchaseAmount", PurchaseAmount);
                    cmd.Parameters.AddWithValue("@PointOfSaleID", PointOfSaleID);


                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }



        }


        public static void  CustomerPaymentBookRecord(int? PaymentMethod , int TransactionType, decimal PaymentAmount, string PaymentDescription, int? CustomerID)
        {


            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;


         //   Record Customer Cash Acount  

            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("pay_Insert_CustomersBook"))
                {


                    var PointOfSaleID= HttpContext.Current.Session["PointOfSaleID"];

                    cmd.CommandType = CommandType.StoredProcedure;
                      
                    cmd.Parameters.AddWithValue("@PaymentMethod", PaymentMethod);
                    cmd.Parameters.AddWithValue("@TransactionType", TransactionType);
                    cmd.Parameters.AddWithValue("@PaymentAmount", PaymentAmount);
                    cmd.Parameters.AddWithValue("@PaymentDescription", PaymentDescription);
                    cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
                    cmd.Parameters.AddWithValue("@PointOfSaleID", PointOfSaleID);


                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }


        //  Record Customer Cash Acount  



        }


        // ################################################ //
        //            Truck Expenses Functions              //
        // ################################################ //

        public static void InsertTruckExpense(string ExpenseName, decimal ExpenseAmount, DateTime TempexpenseDate, int? TruckID, int? TripID)
        {


            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;

            
            using (SqlConnection con = new SqlConnection(constr))
            {

                
                string query = "Insert Into trc_Expenses (ExpenseName, ExpenseAmount, TruckID,TripID,ExpenseDate)" +
                    "Values('" + ExpenseName + "','" + ExpenseAmount + "','" + TruckID + "','" + TripID + "', '" + TempexpenseDate + "')";

                SqlCommand command = new SqlCommand(query, con);
                con.Open();
                command.ExecuteNonQuery();
                con.Close();
            }


            //  Record Customer Cash Acount  



        }

    }
}