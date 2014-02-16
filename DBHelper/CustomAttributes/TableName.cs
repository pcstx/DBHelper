using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBHelper.CustomAttributes
{
    /// <summary>
    /// 表名
    /// </summary>
    public class TableName:Attribute
    {
        private string tableName;
        public TableName(string tableName)
        {
            this.tableName = tableName;
        }

        public string GetTableName()
        {
            return tableName; 
        }

    }
}
