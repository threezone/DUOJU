using DUOJU.Domain;
using DUOJU.Domain.Helpers;
using DUOJU.FRAMEWORK;
using DUOJU.FRAMEWORK.WeChat;
using DUOJU.Service.Abstract;
using DUOJU.Service.Concrete;
using log4net;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DUOJU.WECHAT.Controllers
{
    public class WeChatController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(WeChatController));

        private IUserService UserService { get; set; }

        private WeChat WeChat { get; set; }


        public WeChatController()
        {
            UserService = new UserService();
            WeChat = new WeChat(CommonSettings.DUOJUWECHAT_TOKEN, CommonSettings.DUOJUWECHAT_APPID, CommonSettings.DUOJUWECHAT_APPSECRET);
        }


        /// <summary>
        /// 微信验证
        /// </summary>
        public ActionResult Index(string signature, string timestamp, string nonce, string echostr)
        {
            if (!string.IsNullOrEmpty(signature) && !string.IsNullOrEmpty(timestamp) && !string.IsNullOrEmpty(nonce) && !string.IsNullOrEmpty(echostr))
            {
                // 微信验证
                // http://mp.weixin.qq.com/wiki/index.php?title=%E6%8E%A5%E5%85%A5%E6%8C%87%E5%8D%97
                var key = SecurityHelper.SHA1Encrypt(string.Join("", new string[] { CommonSettings.DUOJUWECHAT_TOKEN, timestamp, nonce }.OrderBy(i => i)));
                logger.InfoFormat("wechat - signature:{0}; timestamp:{1}; nonce:{2}; echostr:{3}; token:{4}; key:{5};", signature, timestamp, nonce, echostr, CommonSettings.DUOJUWECHAT_TOKEN, key);

                if (key == signature)
                    return Content(echostr);
                else
                    return Content(null);
            }

            return Content(null);
        }

        /// <summary>
        /// 接受消息
        /// </summary>
        [HttpPost]
        public ActionResult Index()
        {
            var receiveModel = WeChat.ConvertReceiveXML(Request.InputStream);
            logger.Info(JsonHelper.GetJsonWithModel(receiveModel));

            var sendModel = new WeChatSendMessageModel
            {
                ToUserName = receiveModel.FromUserName,
                FromUserName = receiveModel.ToUserName,
                CreateTime = DateTimeHelper.ConvertTimeStamp(DateTime.Now)
            };
            switch (receiveModel.MsgType)
            {
                case MsgTypes.TEXT:
                    if (receiveModel.Content == "test1")
                    {
                        var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2b8e67341ef40666&redirect_uri=http%3A%2F%2Fwechat.duoju.us%2Fparty%2Ftest&response_type=code&scope=snsapi_base&state=test1#wechat_redirect";
                        sendModel.MsgType = MsgTypes.TEXT;
                        sendModel.Content = "<a href=\"" + url + "\">go</a>";
                    }
                    break;

                case MsgTypes.EVENT:
                    switch (receiveModel.Event.Value)
                    {
                        case Events.SUBSCRIBE:
                            logger.WarnFormat("user ({0}) subscribe{1}.",
                                receiveModel.FromUserName,
                                string.IsNullOrEmpty(receiveModel.Ticket) ? null : string.Format(" with ticket ({0})", receiveModel.Ticket)
                            );

                            var subscribeUser = WeChat.GetWeChatUserInfo(receiveModel.FromUserName, WeChatSettings.WECHATLANGUAGE_EN);
                            //UserService.AddWeChatUser(subscribeUser);

                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = "欢迎关注！";
                            break;

                        case Events.SCAN:
                            logger.WarnFormat("user ({0}) scan, ticket: {1}.",
                                receiveModel.FromUserName,
                                receiveModel.Ticket
                            );
                            break;

                        case Events.UNSUBSCRIBE:
                            logger.WarnFormat("user ({0}) unsubscribe.", receiveModel.FromUserName);

                            UserService.WeChatUserUnsubscribe(receiveModel.FromUserName);
                            break;

                        case Events.CLICK:
                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = "用户点击:" + receiveModel.EventKey;
                            break;

                        case Events.VIEW:
                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = "用户点击:" + receiveModel.EventKey;
                            break;
                    }
                    break;
            }

            return Content(WeChat.ConvertSendXML(sendModel));
        }
    }
}
