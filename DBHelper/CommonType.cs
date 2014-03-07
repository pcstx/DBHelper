using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    /// <summary>
    /// 安全的类型转换
    /// </summary>
   public class CommonType
    {
       /// <summary>
       /// 转成整形
       /// </summary>
       /// <param name="obj">要转换的对象</param>
       /// <param name="defaultValue">默认值</param>
       /// <returns></returns>
       public static int ToInt(object obj,int defaultValue=0)
       {
           int value = 0;
           try
           {
               if (!(obj != null && int.TryParse(obj.ToString(), out value)))
               {
                   value = defaultValue;
               }
           }
           catch
           {
               value = defaultValue;
           }
           return value;
       }

       public static string ToString(object obj, string defalutValue = "")
       {
           string value = "";

           try
           {
               if (obj != null)
               {
                   value = obj.ToString();
               }
               else
               {
                   value = defalutValue;
               }
           }
           catch
           {
               value = defalutValue;
           }

           return value;
       }

    }
}
