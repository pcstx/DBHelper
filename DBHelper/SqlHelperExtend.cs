using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public partial class SqlHelper
    {
        /// <summary>
        /// 删除最后一个字符
        /// </summary>
        /// <param name="str">要删除的字符串</param>
        /// <returns></returns>
        private static string DelLastChar(string str)
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

        /// <summary>
        /// 单表插入操作
        /// </summary>
        /// <typeparam name="T">插入实体类型</typeparam>
        /// <param name="t">插入实体</param>
        /// <param name="TableName">插入的表名</param>
        /// <returns>是否插入成功</returns>
        public static bool Insert<T>(T t, string TableName = null)
        {
            string sql = @" INSERT INTO {0}
                                    ({1})
                                VALUES
                                    ({2})
                            ";
            string attributes = "";
            string attributesValue = "";
            SqlParameterCollection sqlParamCollection = new SqlCommand().Parameters;
            Type type = typeof(T);
            PropertyInfo[] pi = type.GetProperties();

            foreach (PropertyInfo p in pi)
            {
                bool hasAutoIncre = Attribute.IsDefined(p, typeof(CustomAttributes.AutoIncre)); //判断是否是[自增长]特性
                bool hasInvalid = Attribute.IsDefined(p, typeof(CustomAttributes.Invalid)); //判断是否是[无效]特性
                object value = p.GetValue(t, null);
                string name = p.Name;
                if (value != null && !hasAutoIncre && !hasInvalid)
                {
                    attributes += name + ",";
                    attributesValue += "@" + name + ",";
                    sqlParamCollection.Add(new SqlParameter("@" + name, value));
                }
            }

            attributes = DelLastChar(attributes);
            attributesValue = DelLastChar(attributesValue);

            if (string.IsNullOrEmpty(TableName))
            {
                Attribute attr = Attribute.GetCustomAttribute(type, typeof(CustomAttributes.TableName));
                CustomAttributes.TableName a = (CustomAttributes.TableName)attr;
                TableName = a.GetTableName();
            }
            sql = string.Format(sql, TableName, attributes, attributesValue);

            int result = SqlHelper.ExecteNonQuery(sql, null, System.Data.CommandType.Text, sqlParamCollection);

            if (result > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 根据主键更新单表
        /// </summary>
        /// <typeparam name="T">更新实体类型</typeparam>
        /// <param name="t">更新实体</param>
        /// <param name="whereT">更新条件字符串</param>
        /// <param name="TableName">更新的表名</param>
        /// <returns>受影响行数</returns>
        public static int Update<T>(T t, string whereT = null, string TableName = null)
        {
            string sql = @" UPDATE {0}
                               SET {1}
                               WHERE 1=1 {2} ;";

            string attributes = "";
            string wherecondition = "";
            SqlParameterCollection sqlParamCollection = new SqlCommand().Parameters;
            Type type = typeof(T);
            PropertyInfo[] pi = type.GetProperties();

            //T where;
            if (whereT != null)
            {
                wherecondition += (string)whereT;  
            } 

            foreach (PropertyInfo p in pi)
            {
                bool hasAutoIncre = Attribute.IsDefined(p, typeof(CustomAttributes.AutoIncre)); //判断是否是[自增长]特性
                bool hasInvalid = Attribute.IsDefined(p, typeof(CustomAttributes.Invalid)); //判断是否是[无效]特性
                object value = p.GetValue(t, null);
                //object whereValue = p.GetValue(where, null); //where条件的值
                string name = p.Name;
                if (value != null && !hasAutoIncre && !hasInvalid)
                {
                    attributes += name + "=" + "@" + name + ",";
                    sqlParamCollection.Add(new SqlParameter("@" + name, value));
                }
                if (whereT==null)
                {
                    bool hasKey = Attribute.IsDefined(p, typeof(CustomAttributes.Key));
                    if (hasKey)
                    {
                        wherecondition += " and " + name + "=" + value;
                    }
                }
                ////////////////////////////
                //if (whereValue != null)
                //{
                //    wherecondition += " and "+name+"="+whereValue;
                //}

            }
            attributes = DelLastChar(attributes);


            if (string.IsNullOrEmpty(TableName))
            {
                Attribute attr = Attribute.GetCustomAttribute(type, typeof(CustomAttributes.TableName));
                CustomAttributes.TableName a = (CustomAttributes.TableName)attr;
                TableName = a.GetTableName();
            }
            sql = string.Format(sql, TableName, attributes, wherecondition);

            int result = SqlHelper.ExecteNonQuery(sql, null, System.Data.CommandType.Text, sqlParamCollection);

            return result;
        }

        public static int Update<T>(T t, T where, string TableName = null)
        {
            string sql = @" UPDATE {0}
                               SET {1}
                               WHERE 1=1 {2} ;";

            string attributes = "";
            string wherecondition = "";
            SqlParameterCollection sqlParamCollection = new SqlCommand().Parameters;
            SqlParameterCollection sqlwhereParamCollection = new SqlCommand().Parameters;
            Type type = typeof(T);
            PropertyInfo[] pi = type.GetProperties();
             
            foreach (PropertyInfo p in pi)
            {
                bool hasAutoIncre = Attribute.IsDefined(p, typeof(CustomAttributes.AutoIncre)); //判断是否是[自增长]特性
                bool hasInvalid = Attribute.IsDefined(p, typeof(CustomAttributes.Invalid)); //判断是否是[无效]特性
                object value = p.GetValue(t, null);
                object whereValue = p.GetValue(where, null); //where条件的值
                string name = p.Name;
                if (value != null && !hasAutoIncre && !hasInvalid)
                {
                    attributes += name + "=" + "@" + name + ",";
                    sqlParamCollection.Add(new SqlParameter("@" + name, value));
                }
                if (where ==null)
                {
                    bool hasKey = Attribute.IsDefined(p, typeof(CustomAttributes.Key));
                    if (hasKey)
                    {
                        wherecondition += " and " + name + "=@" + name + "2";
                        sqlParamCollection.Add(new SqlParameter("@" + name + "2", value));
                    }
                }
                ////////////////////////////
                if (where != null)
                {
                    if(whereValue!=null)
                    { 
                        wherecondition += " and " + name + "=@" + name+"2";
                        sqlParamCollection.Add(new SqlParameter("@" + name + "2", whereValue));
                    } 
                }

            }
            attributes = DelLastChar(attributes);


            if (string.IsNullOrEmpty(TableName))
            {
                Attribute attr = Attribute.GetCustomAttribute(type, typeof(CustomAttributes.TableName));
                CustomAttributes.TableName a = (CustomAttributes.TableName)attr;
                TableName = a.GetTableName();
            }
            sql = string.Format(sql, TableName, attributes, wherecondition);

            int result = SqlHelper.ExecteNonQuery(sql, null, System.Data.CommandType.Text, sqlParamCollection);

            return result;
        }

    }
}
