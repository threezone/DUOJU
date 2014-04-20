using DUOJU.Domain;
using DUOJU.Domain.Helpers;
using DUOJU.Domain.Models.Party;
using DUOJU.FRAMEWORK;
using DUOJU.FRAMEWORK.WeChat;
using DUOJU.Service.Abstract;
using DUOJU.Service.Concrete;
using DUOJU.WECHAT.Models.WeChat;
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

        private ISupplierService SupplierService { get; set; }

        private WeChat WeChat { get; set; }


        public WeChatController()
        {
            UserService = new UserService();
            SupplierService = new SupplierService();
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
                case MsgTypes.EVENT:
                    switch (receiveModel.Event.Value)
                    {
                        case Events.SUBSCRIBE:
                            logger.WarnFormat("user ({0}) subscribe{1}.",
                                receiveModel.FromUserName,
                                string.IsNullOrEmpty(receiveModel.Ticket) ? null : string.Format(" with ticket ({0})", receiveModel.Ticket)
                            );

                            var subscribeUser = WeChat.GetWeChatUserInfo(receiveModel.FromUserName, WeChatSettings.WECHATLANGUAGE_EN);
                            UserService.AddWeChatUser(subscribeUser);

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


        public ActionResult PublishParty(string openId, int supplierId)
        {
            var model = new PublishPartyViewModel
            {
                SupplierInfo = SupplierService.GetSupplierInfoById(supplierId),
                PartyModel = new PublishPartyModel
                {
                    OpenId = openId
                }
            };

            return View(model);
        }





        public ActionResult TEST()
        {
            var user = new WeChatUserInfo
            {
                subscribe = (int)DUOJU.Domain.Enums.YesNo.Y,
                openid = "o2x6et8pNFA3QTAqkgCEjE2oslf8",
                nickname = "sugar.lin",
                sex = (int)DUOJU.Domain.Enums.UserSexes.M,
                language = "zh",
                city = "Guangzhou",
                province = "Guangdong",
                country = "China",
                headimgurl = "http://d.hiphotos.baidu.com/image/w%3D2048/sign=20aa6ccadfc451daf6f60beb82c55366/b219ebc4b74543a9b9590e581c178a82b80114e7.jpg",
                subscribe_time = 1397896767
            };

            UserService.AddWeChatUser(user);

            return Content(null);
        }
    }
}
