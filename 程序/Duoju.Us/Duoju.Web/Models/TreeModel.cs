using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Duoju.Models
{
    public class TreeModel
    {
        public int id { get; set; }
        public int pId { get; set; }
        public string name { get; set; }
        public bool? isParent { get; set; }
        public bool? isGroupAuthority { get; set; }
    }
}