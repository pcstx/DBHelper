﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
   /// <summary>
   /// SQL Server操作类
   /// </summary>
    public partial class SqlHelper:BaseHelper
    { 
        private static string _connectionString = ConnectionString.connectionString("SqlServerHelper");

        public static string connectionString
        {
            get { return _connectionString; }
            set { _connectionString = value; }
        }

        /// <summary>
        /// 返回数据库连接对象
        /// </summary>
        /// <returns></returns>
        private static SqlConnection GetConnection(string connectionStringName = null)
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
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            return conn;        
        }

      /// <summary>
      ///  为执行命令准备参数
      /// </summary>
      /// <param name="cmd">SqlCommand 命令</param>
      /// <param name="sql">执行SQL语句</param>
        /// <param name="conn">已经存在的数据库连接</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="commandParameters">返回带参数的命令</param>
        /// <param name="tran">数据库事物处理</param>
      /// <param name="CommandTimeout">超时时间</param>
        private static void PrepareCommand(SqlCommand cmd, string sql, SqlConnection conn, CommandType cmdType, SqlParameterCollection commandParameters, SqlTransaction tran, int CommandTimeout)
        {
            cmd.Connection = conn;
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
                foreach (SqlParameter parm in commandParameters)
                {
                    SqlParameter pp = (SqlParameter)((ICloneable)parm).Clone();
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
        public static int ExecteNonQuery(string sql, string connectionStringName=null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            int result = 0; 
            try
            {
                using (SqlConnection conn = GetConnection(connectionStringName))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, sql, conn, cmdType, commandParameters, tran, CommandTimeout); 
                        result = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
                throw ex;
            }
            return result;
        }

        public static int ExecteNonQuery(string sql, SqlParameters param)
        {
            return ExecteNonQuery(sql,param.connectionStringName,param.cmdType,param.commandParameters,param.tran,param.CommandTimeout);
        }

        public static int BeginExecuteNonQuery(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            int result = 0;
            try
            {
                using (SqlConnection conn = GetConnection(connectionStringName))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, sql, conn, cmdType, commandParameters, tran, CommandTimeout);
                        IAsyncResult asyncResult = cmd.BeginExecuteNonQuery();
                        while (!asyncResult.IsCompleted) //异步待定完成
                        {

                        }
                        result = cmd.EndExecuteNonQuery(asyncResult);
                       // cmd.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
                throw ex;
            }
            return result;
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
        public static IDataReader ExecuteReader(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            SqlDataReader sdr;
            try
            {
                SqlConnection conn = GetConnection(connectionStringName);
                
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, sql, conn, cmdType, commandParameters, tran, CommandTimeout);
                        sdr = cmd.ExecuteReader(CommandBehavior.CloseConnection); 
                    } 
            }
            catch (Exception ex)
            { 
                throw ex;
            }

            return sdr;
        }

        public static IDataReader ExecuteReader(string sql, SqlParameters param)
        {
            return ExecuteReader(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

        public static IDataReader BeginExecuteReader(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            SqlDataReader sdr=null;
            try
            {
                SqlConnection conn = GetConnection(connectionStringName);
                
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, sql, conn, cmdType, commandParameters, tran, CommandTimeout);
                        IAsyncResult asyncResult = cmd.BeginExecuteReader(CommandBehavior.CloseConnection);
                        while (!asyncResult.IsCompleted)
                        {

                        }
                        sdr = cmd.EndExecuteReader(asyncResult);
                       // cmd.Connection.Close();
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
        public static object ExecuteScalar(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            object obj;
            try
            {
                using (SqlConnection conn = GetConnection(connectionStringName))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, sql, conn, cmdType, commandParameters, tran, CommandTimeout); 
                        obj = cmd.ExecuteScalar(); 
                    }
                } 
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return obj;
        }

        public static object ExecuteScalar(string sql, SqlParameters param)
        {
            return ExecuteScalar(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

        /// <summary>
        /// 返回单个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="connectionStringName"></param>
        /// <param name="cmdType"></param>
        /// <param name="commandParameters"></param>
        /// <param name="tran"></param>
        /// <param name="CommandTimeout"></param>
        /// <returns></returns>
        public static T ExecuteObject<T>(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            object first= ExecuteScalar(sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
            if (first is T)
            {
                return (T)first;
            }
            else
            {
                return default(T);
            }
        }

        public static T ExecuteObject<T>(string sql, SqlParameters param)
        {
            return ExecuteObject<T>(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
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
        public static DataSet ExecuteDataSet(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            DataSet ds = new DataSet();
            try
            {
                using (SqlConnection conn = GetConnection(connectionStringName))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, sql, conn, cmdType, commandParameters, tran, CommandTimeout);
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(ds);
                       // cmd.Connection.Close();
                    }
                }         
            }
            catch(Exception ex)
            {
                throw ex;
            }

            return ds;
        }

        public static DataSet ExecuteDataSet(string sql, SqlParameters param)
        {
            return ExecuteDataSet(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

        public static DataTable ExecuteDataTable(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            DataTable dt = new DataTable();
            try
            { 
                using (SqlConnection conn = GetConnection(connectionStringName))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        PrepareCommand(cmd, sql, conn, cmdType, commandParameters, tran, CommandTimeout);
                       // cmd.Connection = conn;
                        SqlDataAdapter da = new SqlDataAdapter();
                        da.SelectCommand = cmd;
                        da.Fill(dt);
                       // cmd.Connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return dt;
        }

        public static DataTable ExecuteDataTable(string sql, SqlParameters param)
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
        public static IEnumerable<T> ExecuteIEnumerable<T>(string sql, string connectionStringName = null, CommandType cmdType = CommandType.Text, SqlParameterCollection commandParameters = null, SqlTransaction tran = null, int CommandTimeout = 30)
        {
            IEnumerable<T> Ienum;
            try
            {
                SqlDataReader sdr = (SqlDataReader)ExecuteReader(sql, connectionStringName, cmdType, commandParameters, tran, CommandTimeout);
                Ienum = BaseHelper.ToIEnumerable<T>(sdr); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
             
            return Ienum;
        }

        public static IEnumerable<T> ExecuteIEnumerable<T>(string sql, SqlParameters param)
        {
            if (param == null)
            {
                param = new SqlParameters();
            }
            return ExecuteIEnumerable<T>(sql, param.connectionStringName, param.cmdType, param.commandParameters, param.tran, param.CommandTimeout);
        }

      
    }

}
