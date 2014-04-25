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
using DUOJU.WECHAT.Sys.Helpers;
using System.Collections.Generic;

namespace DUOJU.WECHAT.Controllers
{
    public class WeChatController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(WeChatController));

        private IUserService UserService { get; set; }


        public WeChatController()
        {
            UserService = new UserService();
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
                CreateTime = WeChat.ConvertTimeStamp(DateTime.Now)
            };
            switch (receiveModel.MsgType)
            {
                case MsgTypes.TEXT:
                    switch (receiveModel.Content)
                    {
                        case "manager.getmenu":
                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = WeChatHelper.WeChat.GetMenu();
                            break;

                        case "manager.createmenu":
                            var menuInfo = new WeChatMenuInfo
                            {
                                button = new List<WeChatMenuItemInfo>
                                {
                                    new WeChatMenuItemInfo
                                    {
                                        name = "开始",
                                        sub_button = new List<WeChatMenuItemInfo>
                                        {
                                            new WeChatMenuItemInfo
                                            {
                                                name = "发布聚会",
                                                type = MenuItemTypes.VIEW.ToString().ToLower(),
                                                url = "http://www.baidu.com",
                                            },
                                            new WeChatMenuItemInfo
                                            {
                                                name = "使用说明",
                                                type = MenuItemTypes.CLICK.ToString().ToLower(),
                                                key = "key_instruction"
                                            }
                                        }
                                    },
                                    new WeChatMenuItemInfo
                                    {
                                        name = "个人",
                                        sub_button = new List<WeChatMenuItemInfo>
                                        {
                                            new WeChatMenuItemInfo
                                            {
                                                name = "发布的聚会",
                                                type = MenuItemTypes.VIEW.ToString().ToLower(),
                                                url = "https://wechat.duoju.us/Party/MyParties",
                                            },
                                            new WeChatMenuItemInfo
                                            {
                                                name = "参与的聚会",
                                                type = MenuItemTypes.VIEW.ToString().ToLower(),
                                                url = "https://wechat.duoju.us/Party/MyParticipateParties",
                                            },
                                            new WeChatMenuItemInfo
                                            {
                                                name = "点个赞",
                                                type = MenuItemTypes.CLICK.ToString().ToLower(),
                                                key = "key_praise",
                                            }
                                        }
                                    }
                                }
                            };

                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = JsonHelper.GetJsonWithModel(WeChatHelper.WeChat.CreateMenu(menuInfo));
                            break;

                        case "manager.deletemenu":
                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = JsonHelper.GetJsonWithModel(WeChatHelper.WeChat.DeleteMenu());
                            break;

                        case "manager.userurl":
                            var url = "https://open.weixin.qq.com/connect/oauth2/authorize?appid=wx2b8e67341ef40666&redirect_uri=http%3A%2F%2Fwechat.duoju.us%2Fparty%2Ftest&response_type=code&scope=snsapi_base&state=test1#wechat_redirect";

                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = "<a href=\"" + url + "\">go</a>";
                            break;
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

                            try
                            {
                                var subscribeUser = WeChatHelper.WeChat.GetWeChatUserInfo(receiveModel.FromUserName);
                                var userid = UserService.AddWeChatUser(subscribeUser);
                            }
                            catch (Exception ex)
                            {
                                logger.Warn(ex);
                            }

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
                            logger.InfoFormat("user click {0}.", receiveModel.EventKey);

                            switch (receiveModel.EventKey)
                            {
                                case "key_instruction":
                                    sendModel.MsgType = MsgTypes.TEXT;
                                    sendModel.Content = "图片版说明weibo：<a href=\"http://www.weibo.com/\">Go</a>\r\n文字版说明qqweibo：<a href=\"http://t.qq.com/\">Go</a>";
                                    break;

                                case "key_praise":
                                    logger.WarnFormat("user ({0}) praise.", receiveModel.FromUserName);

                                    sendModel.MsgType = MsgTypes.TEXT;
                                    sendModel.Content = "感谢您的支持！";
                                    break;
                            }
                            break;

                        case Events.VIEW:
                            logger.InfoFormat("user view {0}.", receiveModel.EventKey);
                            break;
                    }
                    break;
            }

            return Content(WeChat.ConvertSendXML(sendModel));
        }
    }
}
