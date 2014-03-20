using DBHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEMO
{
    public class bussiness
    {

        public string Exceute()
        {
            string sql = @" select top 1 UserName from [website].[dbo].[User] ";
            string abc = OleDbHelper.ExecuteObject<string>(sql);// SqlHelper.ExecuteObject<long>(sql);
            return abc;
        }


    }
}