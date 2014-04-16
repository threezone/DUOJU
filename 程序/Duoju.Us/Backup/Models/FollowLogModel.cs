using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YCF.CRM.Model.Entity;
using System.Web.Mvc;

namespace YCF.CRM.Models
{
    public class FollowLogModel
    {
        public SupplierInfo SupInfo = new SupplierInfo();
        public FollowLog FLogInfo = new FollowLog();
        public IList<FollowState> FStateList = new List<FollowState>();
    }
}