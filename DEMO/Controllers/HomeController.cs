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
            //string sql = @" insert into test.user(Name,Age) Values('myname',12); ";
            //MysqlHelper.ExecteNonQuery(sql);
            SqlHelper.connectionString = @"server=PC;uid=sa;pwd=sa;";
            PagingParam pp = new PagingParam();
            pp.TableName = "[website].[dbo].[Vistor]";
            DataTable dt= SqlHelper.SelectPaging(pp);
            return View();
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
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}