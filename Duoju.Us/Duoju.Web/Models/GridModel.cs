using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Duoju.Models
{
    public class GridModel<T>
    {
        public T rows { get; set; }
        public int total { get; set; }
    }
}