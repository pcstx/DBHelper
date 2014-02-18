using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
   public class MysqlParameters:BaseParameters
    {  
           /// <summary>
           /// 参数集合
           /// </summary>
           public new MySqlParameterCollection commandParameters { get; set; }

           /// <summary>
           /// 事务
           /// </summary>
           public new MySqlTransaction tran { get; set; }
        
    }
}
