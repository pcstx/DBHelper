using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBHelper.CustomAttributes
{
    /// <summary>
    /// 最大长度
    /// </summary>
    public class MaxLength : Attribute
    {
          private int maxLength;
          public MaxLength(int n)
        {
            this.maxLength = n;
        }

        public int GetMaxLength()
        {
            return maxLength;
        }
    }
}
