using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YCF.CRM.Model.Entity;

namespace YCF.CRM.Models
{
    public class MenuModel
    {
        public  int ActionId { get; set; }
        public string ActionName { get; set; }
        public int ParentId { get; set; }
        public string UrlPath { get; set; }
        public string Icon { get; set; }
        public List<MenuModel> SubMenu = new List<MenuModel>();
    }
}