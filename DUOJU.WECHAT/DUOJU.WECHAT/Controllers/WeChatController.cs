using Domain;
using Domain.Helper;
using log4net;
using System.Linq;
using System.Web.Mvc;

namespace DUOJU.WECHAT.Controllers
{
    public class WeChatController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(WeChatController));


        public ActionResult Index(string signature, string timestamp, string nonce, string echostr)
        {
            if (!string.IsNullOrEmpty(signature) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(nonce) && !string.IsNullOrEmpty(echostr))
            {
                // 微信验证
                // http://mp.weixin.qq.com/wiki/index.php?title=%E6%8E%A5%E5%85%A5%E6%8C%87%E5%8D%97
                var key = SecurityHelper.SHA1Encrypt(string.Join("", new string[] { CommonSettings.DUO_JU_TOKEN, timestamp, nonce }.OrderBy(i => i)));

                logger.InfoFormat("微信验证 - signature:{0}; timestamp:{1}; nonce:{2}; echostr:{3}; token:{4}; key:{5};", signature, timestamp, nonce, echostr, CommonSettings.DUO_JU_TOKEN, key);
                if (key == signature)
                    return Content(echostr);
                else
                    return Content(null);
            }

            return Content(null);
        }
    }
}
