using Domain;
using Domain.Enums.WeChat;
using Domain.Helpers;
using Domain.Models.WeChat;
using log4net;
using Service.Abstract;
using Service.Concrete;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;

namespace DUOJU.WECHAT.Controllers
{
    public class WeChatController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(WeChatController));

        private IUserService UserService { get; set; }

        private static WeChatAccessTokenInfo _accessTokenInfo = null;

        private static object _lockObj = new object();


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
                var key = SecurityHelper.SHA1Encrypt(string.Join("", new string[] { CommonSettings.DUOJU_TOKEN, timestamp, nonce }.OrderBy(i => i)));

                logger.InfoFormat("wechat - signature:{0}; timestamp:{1}; nonce:{2}; echostr:{3}; token:{4}; key:{5};", signature, timestamp, nonce, echostr, CommonSettings.DUOJU_TOKEN, key);
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
            var receiveModel = ConvertReceiveXML(Request.InputStream);
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
                    break;

                case MsgTypes.EVENT:
                    switch (receiveModel.Event.Value)
                    {
                        case Events.SUBSCRIBE:
                            logger.WarnFormat("user ({0}) subscribe{1}.",
                                receiveModel.FromUserName,
                                string.IsNullOrEmpty(receiveModel.Ticket) ? null : string.Format(" with ticket ({0})", receiveModel.Ticket)
                            );

                            var subscribeUser = GetWeChatUserInfo(receiveModel.FromUserName, CommonSettings.WECHATLANGUAGE_EN);
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

            return Content(ConvertSendXML(sendModel));
        }


        public ActionResult TEST(int suplierId, string openId)
        {
            var user = new WeChatUserInfo
            {
                subscribe = (int)Domain.Enums.Common.YesNo.Y,
                openid = "o2x6et8pNFA3QTAqkgCEjE2oslf8",
                nickname = "sugar.lin",
                sex = (int)Domain.Enums.Common.UserSexes.M,
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


        #region Helper

        /// <summary>
        /// 转换接收到的XML STREAM
        /// </summary>
        private WeChatReceiveMessageModel ConvertReceiveXML(Stream xmlStream)
        {
            var xmlDoc = XDocument.Load(xmlStream);
            var rootElement = xmlDoc.Descendants(CommonSettings.WECHATXML_ROOT_NAME);

            var receiveModel = new WeChatReceiveMessageModel
            {
                ToUserName = rootElement.Descendants(CommonSettings.WECHATXML_TOUSERNAME_NAME).FirstOrDefault().Value,
                FromUserName = rootElement.Descendants(CommonSettings.WECHATXML_FROMUSERNAME_NAME).FirstOrDefault().Value,
                CreateTime = long.Parse(rootElement.Descendants(CommonSettings.WECHATXML_CREATETIME_NAME).FirstOrDefault().Value)
            };

            var MsgType = rootElement.Descendants(CommonSettings.WECHATXML_MSGTYPE_NAME).FirstOrDefault().Value.ToUpper();
            if (Enum.IsDefined(typeof(MsgTypes), MsgType))
                receiveModel.MsgType = (MsgTypes)Enum.Parse(typeof(MsgTypes), MsgType);
            else
                receiveModel.MsgType = MsgTypes.UNDEFINE;

            switch (receiveModel.MsgType)
            {
                case MsgTypes.TEXT:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(CommonSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.Content = rootElement.Descendants(CommonSettings.WECHATXML_CONTENT_NAME).FirstOrDefault().Value.Trim();
                    break;

                case MsgTypes.IMAGE:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(CommonSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.PicUrl = rootElement.Descendants(CommonSettings.WECHATXML_PICURL_NAME).FirstOrDefault().Value;
                    receiveModel.MediaId = rootElement.Descendants(CommonSettings.WECHATXML_MEDIAID_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.VOICE:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(CommonSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.MediaId = rootElement.Descendants(CommonSettings.WECHATXML_MEDIAID_NAME).FirstOrDefault().Value;
                    receiveModel.Format = rootElement.Descendants(CommonSettings.WECHATXML_FORMAT_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.VIDEO:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(CommonSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.MediaId = rootElement.Descendants(CommonSettings.WECHATXML_MEDIAID_NAME).FirstOrDefault().Value;
                    receiveModel.ThumbMediaId = rootElement.Descendants(CommonSettings.WECHATXML_THUMBMEDIAID_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.LOCATION:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(CommonSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.Location_X = double.Parse(rootElement.Descendants(CommonSettings.WECHATXML_LOCATIONX_NAME).FirstOrDefault().Value);
                    receiveModel.Location_Y = double.Parse(rootElement.Descendants(CommonSettings.WECHATXML_LOCATIONY_NAME).FirstOrDefault().Value);
                    receiveModel.Scale = int.Parse(rootElement.Descendants(CommonSettings.WECHATXML_SCALE_NAME).FirstOrDefault().Value);
                    receiveModel.Label = rootElement.Descendants(CommonSettings.WECHATXML_LABEL_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.LINK:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(CommonSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.Title = rootElement.Descendants(CommonSettings.WECHATXML_TITLE_NAME).FirstOrDefault().Value;
                    receiveModel.Description = rootElement.Descendants(CommonSettings.WECHATXML_DESCRIPTION_NAME).FirstOrDefault().Value;
                    receiveModel.Url = rootElement.Descendants(CommonSettings.WECHATXML_URL_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.EVENT:
                    var Event = rootElement.Descendants(CommonSettings.WECHATXML_EVENT_NAME).FirstOrDefault().Value.ToUpper();
                    if (Enum.IsDefined(typeof(Events), Event))
                        receiveModel.Event = (Events)Enum.Parse(typeof(Events), Event);
                    else
                        receiveModel.Event = Events.UNDEFINE;

                    switch (receiveModel.Event.Value)
                    {
                        case Events.SUBSCRIBE:
                            var eventKey = rootElement.Descendants(CommonSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault();
                            if (eventKey != null)
                                receiveModel.EventKey = eventKey.Value;
                            var ticket = rootElement.Descendants(CommonSettings.WECHATXML_TICKET_NAME).FirstOrDefault();
                            if (ticket != null)
                                receiveModel.Ticket = ticket.Value;
                            break;

                        case Events.UNSUBSCRIBE:
                            break;

                        case Events.SCAN:
                            receiveModel.EventKey = rootElement.Descendants(CommonSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault().Value;
                            receiveModel.Ticket = rootElement.Descendants(CommonSettings.WECHATXML_TICKET_NAME).FirstOrDefault().Value;
                            break;

                        case Events.LOCATION:
                            receiveModel.Latitude = double.Parse(rootElement.Descendants(CommonSettings.WECHATXML_LATITUDE_NAME).FirstOrDefault().Value);
                            receiveModel.Longitude = double.Parse(rootElement.Descendants(CommonSettings.WECHATXML_LONGITUDE_NAME).FirstOrDefault().Value);
                            receiveModel.Precision = double.Parse(rootElement.Descendants(CommonSettings.WECHATXML_PRECISION_NAME).FirstOrDefault().Value);
                            break;

                        case Events.CLICK:
                            receiveModel.EventKey = rootElement.Descendants(CommonSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault().Value;
                            break;

                        case Events.VIEW:
                            receiveModel.EventKey = rootElement.Descendants(CommonSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault().Value;
                            break;
                    }
                    break;
            }

            return receiveModel;
        }

        /// <summary>
        /// 转换发送的XML数据
        /// </summary>
        private string ConvertSendXML(WeChatSendMessageModel model)
        {
            var xmlDoc = new XDocument();
            var rootElement = new XElement(
                CommonSettings.WECHATXML_ROOT_NAME,
                new XElement(CommonSettings.WECHATXML_TOUSERNAME_NAME, new XCData(model.ToUserName)),
                new XElement(CommonSettings.WECHATXML_FROMUSERNAME_NAME, new XCData(model.FromUserName)),
                new XElement(CommonSettings.WECHATXML_CREATETIME_NAME, model.CreateTime.ToString()),
                new XElement(CommonSettings.WECHATXML_MSGTYPE_NAME, new XCData(model.MsgType.ToString().ToLower()))
            );

            switch (model.MsgType)
            {
                case MsgTypes.TEXT:
                    rootElement.Add(new XElement(CommonSettings.WECHATXML_CONTENT_NAME, new XCData(model.Content)));
                    break;

                case MsgTypes.IMAGE:
                    rootElement.Add(new XElement(
                        CommonSettings.WECHATXML_IMAGE_NAME,
                        new XElement(CommonSettings.WECHATXML_MEDIAID_NAME, new XCData(model.Image.MediaId))
                    ));
                    break;

                case MsgTypes.VOICE:
                    rootElement.Add(new XElement(
                        CommonSettings.WECHATXML_VOICE_NAME,
                        new XElement(CommonSettings.WECHATXML_MEDIAID_NAME, new XCData(model.Voice.MediaId))
                    ));
                    break;

                case MsgTypes.VIDEO:
                    rootElement.Add(new XElement(
                        CommonSettings.WECHATXML_VIDEO_NAME,
                        new XElement(CommonSettings.WECHATXML_MEDIAID_NAME, new XCData(model.Video.MediaId)),
                        new XElement(CommonSettings.WECHATXML_TITLE_NAME, new XCData(model.Video.Title)),
                        new XElement(CommonSettings.WECHATXML_DESCRIPTION_NAME, new XCData(model.Video.Description))
                    ));
                    break;

                case MsgTypes.MUSIC:
                    rootElement.Add(new XElement(
                        CommonSettings.WECHATXML_MUSIC_NAME,
                        new XElement(CommonSettings.WECHATXML_TITLE_NAME, new XCData(model.Music.Title)),
                        new XElement(CommonSettings.WECHATXML_DESCRIPTION_NAME, new XCData(model.Music.Description)),
                        new XElement(CommonSettings.WECHATXML_MUSICURL_NAME, new XCData(model.Music.MusicUrl)),
                        new XElement(CommonSettings.WECHATXML_HQMUSICURL_NAME, new XCData(model.Music.HQMusicUrl)),
                        new XElement(CommonSettings.WECHATXML_THUMBMEDIAID_NAME, new XCData(model.Music.ThumbMediaId))
                    ));
                    break;

                case MsgTypes.NEWS:
                    if (model.Articles == null || model.Articles.Count == 0)
                    {
                        rootElement.Add(
                            new XElement(CommonSettings.WECHATXML_ARTICLECOUNT_NAME, 0),
                            new XElement(CommonSettings.WECHATXML_ARTICLES_NAME)
                        );
                    }
                    else
                    {
                        rootElement.Add(new XElement(CommonSettings.WECHATXML_ARTICLECOUNT_NAME, model.Articles.Count));
                        var articlesElement = new XElement(CommonSettings.WECHATXML_ARTICLES_NAME);
                        foreach (var article in model.Articles)
                        {
                            articlesElement.Add(new XElement(
                                CommonSettings.WECHATXML_ITEM_NAME,
                                new XElement(CommonSettings.WECHATXML_TITLE_NAME, new XCData(article.Title)),
                                new XElement(CommonSettings.WECHATXML_DESCRIPTION_NAME, new XCData(article.Description)),
                                new XElement(CommonSettings.WECHATXML_PICURL_NAME, new XCData(article.PicUrl)),
                                new XElement(CommonSettings.WECHATXML_URL_NAME, new XCData(article.Url))
                            ));
                        }
                        rootElement.Add(articlesElement);
                    }
                    break;
            }

            xmlDoc.Add(rootElement);
            return xmlDoc.ToString();
        }

        /// <summary>
        /// 调用远程接口
        /// </summary>
        private T CallRemoteInterface<T>(RequestTypes requestType, RequestContentTypes requestContentType, string url, string parameters = null) where T : class
        {
            var uri = new Uri(url);
            var request = WebRequest.Create(uri);

            request.Method = requestType.ToString();
            request.ContentType = requestContentType == RequestContentTypes.FORM ? CommonSettings.REQUEST_CONTENTTYPE_FORM : CommonSettings.REQUEST_CONTENTTYPE_JSON;
            request.Timeout = CommonSettings.REQUEST_TIMEOUT;

            if (requestType == RequestTypes.POST && !string.IsNullOrEmpty(parameters))
            {
                var bytes = UTF8Encoding.UTF8.GetBytes(parameters);
                request.ContentLength = bytes.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            using (var response = request.GetResponse())
            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                if (typeof(T) == typeof(string))
                    return streamReader.ReadToEnd() as T;
                else
                    return JsonHelper.GetModelWithJson<T>(streamReader.ReadToEnd());
            }
        }

        #endregion


        #region Remote

        private WeChatAccessTokenInfo GetWeChatAccessTokenInfo()
        {
            if (_accessTokenInfo != null)
                if (DateTime.Now.AddSeconds(10) >= _accessTokenInfo.ExpiresTime)
                    _accessTokenInfo = null;

            if (_accessTokenInfo == null)
            {
                lock (_lockObj)
                {
                    if (_accessTokenInfo == null)
                    {
                        var url = string.Format(CommonSettings.WECHATURL_GETACCESSTOKEN_FORMAT, CommonSettings.DUOJU_APPID, CommonSettings.DUOJU_APPSECRET);

                        var accessTokenInfo = CallRemoteInterface<WeChatAccessTokenInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
                        accessTokenInfo.ExpiresTime = DateTime.Now.AddSeconds(accessTokenInfo.expires_in);

                        _accessTokenInfo = accessTokenInfo;
                    }
                }
            }

            return _accessTokenInfo;
        }

        private string GetMenu()
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(CommonSettings.WECHATURL_GETMENU_FORMAT, accessTokenInfo.access_token);

            return CallRemoteInterface<string>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        private WeChatErrorInfo CreateMenu()
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(CommonSettings.WECHATURL_CREATEMENU_FORMAT, accessTokenInfo.access_token);

            var menuInfo = new WeChatMenuInfo
            {
                button = new List<WeChatMenuItemInfo>
                {
                    new WeChatMenuItemInfo
                    {
                        name = "菜单",
                        sub_button = new List<WeChatMenuItemInfo>
                        {
                            new WeChatMenuItemInfo
                            {
                                name = "发布聚会",
                                type = MenuItemTypes.CLICK.ToString().ToLower(),
                                key = "key1"
                            },
                            new WeChatMenuItemInfo
                            {
                                name = "周围的聚会",
                                type = MenuItemTypes.CLICK.ToString().ToLower(),
                                key = "key2"
                            },
                            new WeChatMenuItemInfo
                            {
                                name = "百度一下",
                                type = MenuItemTypes.VIEW.ToString().ToLower(),
                                url = "http://www.baidu.com"
                            }
                        }
                    },
                    new WeChatMenuItemInfo
                    {
                        name = "点个赞",
                        type = MenuItemTypes.CLICK.ToString().ToLower(),
                        key = "key3"
                    }
                }
            };

            return CallRemoteInterface<WeChatErrorInfo>(RequestTypes.POST, RequestContentTypes.JSON, url, JsonHelper.GetJsonWithModel(menuInfo));
        }

        private WeChatErrorInfo DeleteMenu()
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(CommonSettings.WECHATURL_DELETEMENU_FORMAT, accessTokenInfo.access_token);

            return CallRemoteInterface<WeChatErrorInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        private WeChatUserListInfo GetWeChatUserListInfo(string nextOpenid = "")
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(CommonSettings.WECHATURL_GETUSERLISTINFO_FORMAT, accessTokenInfo.access_token, nextOpenid);

            return CallRemoteInterface<WeChatUserListInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        private WeChatUserInfo GetWeChatUserInfo(string openid, string language = CommonSettings.WECHATLANGUAGE_ZHCN)
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(CommonSettings.WECHATURL_GETUSERINFO_FORMAT, accessTokenInfo.access_token, openid, language);

            return CallRemoteInterface<WeChatUserInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        #endregion
    }
}
