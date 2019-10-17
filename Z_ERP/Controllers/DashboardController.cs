using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Z_ERP.Models;

namespace Z_ERP.Controllers
{
    public class DashboardController : Controller
    {
        private MainModel db = new MainModel();
       

        // GET: Dashboard
        public ActionResult Index()
        {
            return View();

            
        }



        // Start Company Dashboard
        [HttpGet]
        public  JsonResult  ChartsDashboard()
        {

            pay_Payment obj = new pay_Payment();
            var tempDaashbord = new List<DashboardModel>();
            string constr = ConfigurationManager.ConnectionStrings["DBConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Dash_charts"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@FromDate", "");
                    cmd.Parameters.AddWithValue("@ToDate", "");

                    DataSet ds = new DataSet();
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    cmd.Connection = con;
                    con.Open();
                    adp.Fill(ds);
                    var dt1 = ds.Tables[0];
                    var dt2 = ds.Tables[1];

                    if (dt1 .Rows .Count > 0)
                    {
                        List<decimal > dataList = new List<decimal>();
                        List<string > labelList = new List<string >();
                        foreach (DataRow  item in dt1.Rows)
                        {
                            labelList.Add(item ["Label"].ToString ());
                            dataList.Add(decimal.Parse (item ["Data"].ToString ()));
                        }
                        tempDaashbord.Add(new DashboardModel()
                        {
                            ChartData = new DashboardClass() { ChartLabel = labelList, ChartData = dataList },
                            chartID = 2
                        });
                    }

                    if (dt2.Rows.Count > 0)
                    {
                        List<decimal> dataList = new List<decimal>();
                        List<string> labelList = new List<string>();
                        foreach (DataRow item in dt2.Rows)
                        {
                            labelList.Add(item["Label"].ToString());
                            dataList.Add(decimal.Parse(item["Data"].ToString()));
                        }
                        tempDaashbord.Add(new DashboardModel()
                        {
                            ChartData = new DashboardClass() { ChartLabel = labelList, ChartData = dataList },
                            chartID = 1
                        });
                    }

                    
                    con.Close();
                }
            }


            


            return Json( tempDaashbord , JsonRequestBehavior.AllowGet);
        }


        // GET: Dashboard/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Dashboard/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Dashboard/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Dashboard/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Dashboard/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Dashboard/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
