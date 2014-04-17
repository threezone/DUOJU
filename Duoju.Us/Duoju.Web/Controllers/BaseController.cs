using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Duoju.Model.Entity;
using Duoju.DAO.Abstract;

namespace Duoju.Controllers
{
    public class BaseController : Controller
    {
        protected IUserRepository SpUserRepository { get; set; }
        protected IAuthorityInfoRepository SpAuthorityInfoRepository { get; set; }

        private int _currentUserId;
        public int CurrentUserId
        {
            get
            {
                return Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name);
            }
            protected set { _currentUserId = value; }
        }

        private UserInfo _currentUser;
        public UserInfo CurrentUser
        {
            get {
                return _currentUser ??
                       (_currentUser =
                           SpUserRepository.GetUserById(
                               Convert.ToInt32(System.Web.HttpContext.Current.User.Identity.Name)));
            }
            protected set { _currentUser = value; }
        }
    }
}
