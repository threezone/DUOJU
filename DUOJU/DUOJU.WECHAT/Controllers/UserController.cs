using DUOJU.Service.Abstract;
using DUOJU.Service.Concrete;
using DUOJU.WECHAT.Models.User;
using DUOJU.WECHAT.Sys.Helpers;
using log4net;
using System.Web.Mvc;

namespace DUOJU.WECHAT.Controllers
{
    public class UserController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(UserController));

        private IUserService UserService { get; set; }


        public UserController()
        {
            UserService = new UserService();
        }


        /// <summary>
        /// 我的个人财产
        /// </summary>
        public ActionResult MyFinances(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return Content("无法获取当前用户信息。");
            else
            {
                var accessTokenInfo = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);

                var model = new MyFinancesViewModel
                {
                    UserFinanceInfo = UserService.GetUserFinanceInfoByOpenId(accessTokenInfo.openid)
                };

                return View(model);
            }
        }
    }
}
