using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks; 
using MySql.Data.MySqlClient;
using System.Data;
using System.Reflection;

namespace DBHelper
{
    public class MysqlHelper
    {
//        String mysqlStr = "Data Source=192.168.1.1;Password=root;User ID=root;Database=test;port=3306";
//MySqlConnection connection = new MySqlConnection(mysqlStr);
//connection.Open();
        /// <summary>
        /// 数据库连接字符串
        /// </summary> 
        private static string _connectionString = ConnectionString.connectionString("MySqlHelper");

        public static string connectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// 返回数据库连接对象
        /// </summary>
        /// <returns></returns>
        private static MySqlConnection GetConnection(string connStr = null)
        {
            if (string.IsNullOrEmpty(connStr))
            {
                connStr = connectionString;
            }
            MySqlConnection conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        /// <summary>
        /// 为执行命令准备参数
        /// </summary>
        /// <param name="cmd">SqlCommand 命令</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="trans">数据库事物处理</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
        /// <param name="cmdParms">返回带参数的命令</param>
        private static void PrepareCommand(MySqlCommand cmd, string sql, string connectionStringName, CommandType cmdType, MySqlParameterCollection commandParameters, MySqlTransaction tran, int CommandTimeout)
        {
            string connstr;
            if (string.IsNullOrEmpty(connectionStringName))
            {
                connstr = connectionString;
            }
            else
            {
                connstr = ConnectionString.connectionString(connectionStringName);
            }

            cmd.Connection = GetConnection(connstr);
            cmd.CommandText = sql;
            //判断是否需要事物处理
            if (tran != null)
            {
                cmd.Transaction = tran;
            }

            cmd.CommandType = cmdType;
            cmd.CommandTimeout = CommandTimeout;
            if (commandParameters != null)
            {
                foreach (MySqlParameter parm in commandParameters)
                {
                    MySqlParameter pp = (MySqlParameter)((ICloneable)parm).Clone();
                    cmd.Parameters.Add(pp);

                }

            }
        }


        /// <summary>
        /// 执行SQL语句，返回是否成功
        /// </summary>
        /// <param name="sql">执行SQL语句</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <param name="tran">数据库事物处理</param>       
        /// <returns>1成功，0失败，-1异常</returns>
        public static int ExecteNonQuery(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text,MySqlParameterCollection commandParameters = null,MySqlTransaction tran = null, int CommandTimeout = 30)
        {
            int result = 0;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                    result = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                result = -1;
                throw ex;
            }
            return result;
        }


        public static int ExecteNonQuery(string sql, MysqlParameters param)
        {
            return ExecteNonQuery(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }


        /// <summary>
        /// 返回SqlDataReader对象
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <param name="tran"></param>
        /// <param name="CommandTimeout"></param>
        /// <returns></returns>
        public static IDataReader ExecuteReader(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, MySqlParameterCollection commandParameters = null, MySqlTransaction tran = null, int CommandTimeout = 30)
        {
            MySqlDataReader sdr;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                    sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sdr;
        }

        public static IDataReader ExecuteReader(string sql, MysqlParameters param)
        {
            return ExecuteReader(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

        public static IDataReader BeginExecuteReader(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, MySqlParameterCollection commandParameters = null, MySqlTransaction tran = null, int CommandTimeout = 30)
        {
            MySqlDataReader sdr = null;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                    IAsyncResult asyncResult = cmd.BeginExecuteReader(CommandBehavior.CloseConnection);
                    while (!asyncResult.IsCompleted)
                    {

                    }
                    sdr = cmd.EndExecuteReader(asyncResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return sdr;
        }

        /// <summary>
        /// 返回第一行的第一列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <param name="tran"></param>
        /// <param name="CommandTimeout"></param>
        /// <returns></returns>
        public static object ExecuteScalar(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, MySqlParameterCollection commandParameters = null, MySqlTransaction tran = null, int CommandTimeout = 30)
        {
            object obj;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                    obj = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        public static object ExecuteScalar(string sql, MysqlParameters param)
        {
            return ExecuteScalar(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

        /// <summary>
        /// 返回DataSet
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <param name="tran"></param>
        /// <param name="CommandTimeout"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, MySqlParameterCollection commandParameters = null, MySqlTransaction tran = null, int CommandTimeout = 30)
        {
            DataSet ds = new DataSet();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(ds);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(string sql, MysqlParameters param)
        {
            return ExecuteDataSet(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

        public static DataTable ExecuteDataTable(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, MySqlParameterCollection commandParameters = null, MySqlTransaction tran = null, int CommandTimeout = 30)
        {
            DataTable dt = new DataTable();
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                    MySqlDataAdapter da = new MySqlDataAdapter();
                    da.SelectCommand = cmd;
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static DataTable ExecuteDataTable(string sql, MysqlParameters param)
        {
            return ExecuteDataTable(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);

        }

        /// <summary>
        /// 返回IEnumerable泛型
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <param name="tran"></param>
        /// <param name="CommandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<T> ExecuteIEnumerable<T>(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, MySqlParameterCollection commandParameters = null, MySqlTransaction tran = null, int CommandTimeout = 30)
        {
            IEnumerable<T> Ienum;
            try
            {
                using (MySqlCommand cmd = new MySqlCommand())
                {
                    PrepareCommand(cmd, sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                    MySqlDataReader sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                    Ienum = ToIEnumerable<T>(sdr);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return Ienum;
        }

        public static IEnumerable<T> ExecuteIEnumerable<T>(string sql, MysqlParameters param)
        {
            return ExecuteIEnumerable<T>(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

        /// <summary>
        /// DataReader转成IEnumberable方法 
        /// </summary>
        /// <typeparam name="T">IEnumberable类型</typeparam>
        /// <param name="reader"></param>
        /// <returns></returns>
        private static IEnumerable<T> ToIEnumerable<T>(IDataReader reader)
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
         
    }
}
