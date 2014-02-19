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
            Models.VistorEntity ve = new Models.VistorEntity();
            ve.ID = 104;
          int i=   SqlHelper.Delete(ve);

            PagingParam pp = new PagingParam();
            pp.TableName = "[website].[dbo].[Vistor]";
            int rowNum = 0;
            DataTable dt = SqlHelper.SelectPaging(pp,out rowNum);
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