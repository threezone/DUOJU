using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YCF.CRM.Models
{
    public class DataModel<T>
    {
        public T rows { get; set; }
        public int total { get; set; }
        public string start { get; set; }
        public string end { get; set; }
    }
}