using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DBHelper;
using System.Data;

namespace DEMO.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            bussiness b = new bussiness();
            b.Exceute();

           string[] list= SqlHelper.GetDatabases();
           List<DbTable> s =SqlHelper.GetDbTables("website");
           var co= SqlHelper.GetDbColumns("website", "Menu");
           var index = SqlHelper.GetDbIndexs("website", "Menu");
            //string sql = @" insert into [website].[dbo].[User]([UserName],[Sex]) Values('myname',12); ";
            //for (int i = 0; i < 20;i++ )
            //{
            //    SqlHelper.ExecteNonQuery(sql);
            //}

         
            /*
            SqlHelper.connectionString = @"server=PC;uid=sa;pwd=sa;";
            Models.VistorEntity ve = new Models.VistorEntity();
            ve.ID = 104;
          int i=   SqlHelper.Delete(ve);

            PagingParam pp = new PagingParam();
            pp.TableName = "[website].[dbo].[Vistor]";
            int rowNum = 0;
            DataTable dt = SqlHelper.SelectPaging(pp,out rowNum);
             */
            return View(s);
        }

        public ActionResult About()
        {
            SqlHelper.connectionString = @"server=PC;uid=sa;pwd=sa;";
            string sql = @" INSERT INTO website.[dbo].[Other]([Name]) VALUES ('pcstx');select @@identity; ";

       object i= SqlHelper.ExecuteScalar(sql);
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            string strCon = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;HDR=Yes'";
            string path = @"D:\Data\文档\花名册.xls";
            strCon = string.Format(strCon, path);
           // string strCon = "Provider=Microsoft.Ace.OleDb.12.0;data source=" + path + ";Extended Properties='Excel 12.0; HDR=No; IMEX=0'"; //
             string sql = "select * from [Sheet1$] where Id=236";  
            
            OleDbHelper.connectionString = strCon;
            DataTable dt= OleDbHelper.ExecuteDataTable(sql);
            
            ViewBag.Message = "Your contact page.";

            return View(dt);
        }
    }
}