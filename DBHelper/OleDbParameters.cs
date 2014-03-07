using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;

namespace DBHelper
{
    public class OleDbParameters : BaseParameters
    {
        /// <summary>
        /// 参数集合
        /// </summary>
        public new OleDbParameterCollection commandParameters { get; set; }

        /// <summary>
        /// 事务
        /// </summary>
        public new OleDbTransaction tran { get; set; }
    }
}
