using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public class BaseHelper
    { 
        /// <summary>
        /// 数据库连接字符串
        /// </summary> 
        public static string _connectionString = ConnectionString.connectionString();

        public static string connectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// DataReader转成IEnumberable方法 
        /// </summary>
        /// <typeparam name="T">IEnumberable类型</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToIEnumerable<T>(IDataReader reader)
        {
            Type type = typeof(T);
            while (reader.Read())
            {
                T t = System.Activator.CreateInstance<T>();
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    string temp = reader.GetName(i);
                    PropertyInfo p = type.GetProperty(temp);
                    try
                    {
                        p.SetValue(t, Convert.ChangeType(reader[temp], p.PropertyType), null);
                    }
                    catch
                    { }
                }
                yield return t;
            }
            reader.Close();
        }

        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str">要删除的字符串</param>
        /// <returns></returns>
        public static string DelLastChar(string str)
        {
            if (!string.IsNullOrEmpty(str) && str.Length > 0)
            {
                return str.Substring(0, str.Length - 1);
            }
            else
            {
                return str;
            }
        }
          
    }
}
