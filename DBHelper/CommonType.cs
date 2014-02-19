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

           if (!(obj != null && int.TryParse(obj.ToString(), out value)))
           {
               value = defaultValue;
           }  
           return value;
       }

    }
}
