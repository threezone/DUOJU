using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.DAO.Abstract;

namespace YCF.CRM.Helper.Sys.Attribute
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private IActionInfoRepository SpActionInfoRepository { get; set; }
        protected IUserRepository SpUserRepository { get; set; }
        protected IAuthorityInfoRepository SpAuthorityInfoRepository { get; set; }

        public bool isAuthority { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }
            if (isAuthority)
            {
                return true;
            }
            return false;
        }

        public override void OnAuthorization(System.Web.Mvc.AuthorizationContext filterContext)
        {
            string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
            string actionName = filterContext.ActionDescriptor.ActionName;
            var actions = SpActionInfoRepository.GetActionByPath(controllerName + "/" + actionName);
            var user = SpUserRepository.GetUserById(Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name));
            //var authority = SpActionInfoRepository
            foreach(var a in actions)
            {
                
            }
            base.OnAuthorization(filterContext);
        }
    }
}