using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    public sealed partial class SqlHelper
    {

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] parms)
        {
            if (connection.State != ConnectionState.Open) connection.Open();

            command.Connection = connection;
            command.CommandTimeout = CommandTimeOut;
            // 设置命令文本(存储过程名或SQL语句)
            command.CommandText = commandText;
            // 分配事务
            if (transaction != null)
            {
                command.Transaction = transaction;
            }
            // 设置命令类型.
            command.CommandType = commandType;
            if (parms != null && parms.Length > 0)
            {
                //预处理SqlParameter参数数组，将为NULL的参数赋值为DBNull.Value;
                foreach (SqlParameter parameter in parms)
                {
                    if ((parameter.Direction == ParameterDirection.InputOutput || parameter.Direction == ParameterDirection.Input) && (parameter.Value == null))
                    {
                        parameter.Value = DBNull.Value;
                    }
                }
                command.Parameters.AddRange(parms);
            }
        }


    }
}
