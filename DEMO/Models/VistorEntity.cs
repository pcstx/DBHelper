using DBHelper.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEMO.Models
{
    [TableName("[website].[dbo].[Vistor]")]
    public class VistorEntity : Attribute
    {

        /// <summary>
        /// 继承自Attribute,设为无效
        /// </summary>
        [Invalid]
        public override object TypeId
        {
            get { return null; }
        }

        [Key]
        [AutoIncre]
        public int ID
        {
            get;
            set;
        }
        public DateTime VisitDate { get; set; }
        public string VisitIP { get; set; }
        public string VisitBrowser { get; set; }
        public string VisitUrl { get; set; }
        public string BrowserLanguage { get; set; }
        public int? UserId { get; set; }
        public string Remarks { get; set; }

        public string UserAgent { get; set; }
        public string UrlReferrer { get; set; }

        public string HttpMethod { get; set; }
        public bool IsMobileDevice { get; set; }
        public string Cookies { get; set; }

    }
}