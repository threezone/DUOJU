using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YCF.CRM.Models
{
    [Serializable]
    public class ErrorModel
    {
        public string ErrorCode { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
    }
}