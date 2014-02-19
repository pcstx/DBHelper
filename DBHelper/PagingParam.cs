using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper
{
    /// <summary>
    /// SQL分页条件
    /// </summary>
    public class PagingParam
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        private string _orderbyKey = "ID";
        /// <summary>
        /// 排序键名
        /// </summary>
        public string orderbyKey
        {
            get { return _orderbyKey; }
            set { _orderbyKey = value; }
        }
        private string _selectRows = "*";
        /// <summary>
        /// 查询字段
        /// </summary>
        public string selectRows
        {
            get { return _selectRows; }
            set { _selectRows = value; }
        }

        private string _whereCondition = "";
        /// <summary>
        /// where条件
        /// </summary>
        public string whereCondition
        {
            get { return _whereCondition; }
            set { _whereCondition = value; }
        }

        private int _PageIndex = 1;
        /// <summary>
        /// 当前页
        /// </summary>
        public int PageIndex
        {
            get { return _PageIndex; }
            set { _PageIndex = value; }
        }

        private int _pageSize = 10;
        /// <summary>
        /// 每页大小
        /// </summary>
        public int pageSize
        {
            get { return _pageSize; }
            set { _pageSize = value; }
        }
    }
}
