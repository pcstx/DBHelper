using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DBHelper.CustomAttributes
{
    /// <summary>
    /// 最小长度
    /// </summary>
    public class MinLength:Attribute
    {
        private int minLength;
        public MinLength(int n)
        {
            this.minLength = n;
        }

        public int GetMinLength()
        {
            return minLength;
        }

    }
}
