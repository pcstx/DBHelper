using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public class SqlParameters:BaseParameters
    {  
        /// <summary>
        /// 参数集合
        /// </summary>
       public new SqlParameterCollection commandParameters{get;set;}

        /// <summary>
        /// 事务
        /// </summary>
       public new SqlTransaction tran {get;set;}
         
    }
}
