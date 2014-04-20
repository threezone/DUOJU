using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;

namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChat
    {
        public WeChat(string token, string appid, string appsecret)
        {
            _token = token;
            _appid = appid;
            _appsecret = appsecret;
            _accessTokenInfo = null;
            _lockObj = new object();
        }


        private string _token { get; set; }

        private string _appid { get; set; }

        private string _appsecret { get; set; }

        private WeChatAccessTokenInfo _accessTokenInfo { get; set; }

        private object _lockObj { get; set; }


        /// <summary>
        /// 转换接收到的XML STREAM
        /// </summary>
        public WeChatReceiveMessageModel ConvertReceiveXML(Stream xmlStream)
        {
            var xmlDoc = XDocument.Load(xmlStream);
            var rootElement = xmlDoc.Descendants(WeChatSettings.WECHATXML_ROOT_NAME);

            var receiveModel = new WeChatReceiveMessageModel
            {
                ToUserName = rootElement.Descendants(WeChatSettings.WECHATXML_TOUSERNAME_NAME).FirstOrDefault().Value,
                FromUserName = rootElement.Descendants(WeChatSettings.WECHATXML_FROMUSERNAME_NAME).FirstOrDefault().Value,
                CreateTime = long.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_CREATETIME_NAME).FirstOrDefault().Value)
            };

            var MsgType = rootElement.Descendants(WeChatSettings.WECHATXML_MSGTYPE_NAME).FirstOrDefault().Value.ToUpper();
            if (Enum.IsDefined(typeof(MsgTypes), MsgType))
                receiveModel.MsgType = (MsgTypes)Enum.Parse(typeof(MsgTypes), MsgType);
            else
                receiveModel.MsgType = MsgTypes.UNDEFINE;

            switch (receiveModel.MsgType)
            {
                case MsgTypes.TEXT:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.Content = rootElement.Descendants(WeChatSettings.WECHATXML_CONTENT_NAME).FirstOrDefault().Value.Trim();
                    break;

                case MsgTypes.IMAGE:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.PicUrl = rootElement.Descendants(WeChatSettings.WECHATXML_PICURL_NAME).FirstOrDefault().Value;
                    receiveModel.MediaId = rootElement.Descendants(WeChatSettings.WECHATXML_MEDIAID_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.VOICE:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.MediaId = rootElement.Descendants(WeChatSettings.WECHATXML_MEDIAID_NAME).FirstOrDefault().Value;
                    receiveModel.Format = rootElement.Descendants(WeChatSettings.WECHATXML_FORMAT_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.VIDEO:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.MediaId = rootElement.Descendants(WeChatSettings.WECHATXML_MEDIAID_NAME).FirstOrDefault().Value;
                    receiveModel.ThumbMediaId = rootElement.Descendants(WeChatSettings.WECHATXML_THUMBMEDIAID_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.LOCATION:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.Location_X = double.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_LOCATIONX_NAME).FirstOrDefault().Value);
                    receiveModel.Location_Y = double.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_LOCATIONY_NAME).FirstOrDefault().Value);
                    receiveModel.Scale = int.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_SCALE_NAME).FirstOrDefault().Value);
                    receiveModel.Label = rootElement.Descendants(WeChatSettings.WECHATXML_LABEL_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.LINK:
                    receiveModel.MsgId = long.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_MSGID_NAME).FirstOrDefault().Value);
                    receiveModel.Title = rootElement.Descendants(WeChatSettings.WECHATXML_TITLE_NAME).FirstOrDefault().Value;
                    receiveModel.Description = rootElement.Descendants(WeChatSettings.WECHATXML_DESCRIPTION_NAME).FirstOrDefault().Value;
                    receiveModel.Url = rootElement.Descendants(WeChatSettings.WECHATXML_URL_NAME).FirstOrDefault().Value;
                    break;

                case MsgTypes.EVENT:
                    var Event = rootElement.Descendants(WeChatSettings.WECHATXML_EVENT_NAME).FirstOrDefault().Value.ToUpper();
                    if (Enum.IsDefined(typeof(Events), Event))
                        receiveModel.Event = (Events)Enum.Parse(typeof(Events), Event);
                    else
                        receiveModel.Event = Events.UNDEFINE;

                    switch (receiveModel.Event.Value)
                    {
                        case Events.SUBSCRIBE:
                            var eventKey = rootElement.Descendants(WeChatSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault();
                            if (eventKey != null)
                                receiveModel.EventKey = eventKey.Value;
                            var ticket = rootElement.Descendants(WeChatSettings.WECHATXML_TICKET_NAME).FirstOrDefault();
                            if (ticket != null)
                                receiveModel.Ticket = ticket.Value;
                            break;

                        case Events.UNSUBSCRIBE:
                            break;

                        case Events.SCAN:
                            receiveModel.EventKey = rootElement.Descendants(WeChatSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault().Value;
                            receiveModel.Ticket = rootElement.Descendants(WeChatSettings.WECHATXML_TICKET_NAME).FirstOrDefault().Value;
                            break;

                        case Events.LOCATION:
                            receiveModel.Latitude = double.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_LATITUDE_NAME).FirstOrDefault().Value);
                            receiveModel.Longitude = double.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_LONGITUDE_NAME).FirstOrDefault().Value);
                            receiveModel.Precision = double.Parse(rootElement.Descendants(WeChatSettings.WECHATXML_PRECISION_NAME).FirstOrDefault().Value);
                            break;

                        case Events.CLICK:
                            receiveModel.EventKey = rootElement.Descendants(WeChatSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault().Value;
                            break;

                        case Events.VIEW:
                            receiveModel.EventKey = rootElement.Descendants(WeChatSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault().Value;
                            break;
                    }
                    break;
            }

            return receiveModel;
        }

        /// <summary>
        /// 转换发送的XML数据
        /// </summary>
        public string ConvertSendXML(WeChatSendMessageModel model)
        {
            var xmlDoc = new XDocument();
            var rootElement = new XElement(
                WeChatSettings.WECHATXML_ROOT_NAME,
                new XElement(WeChatSettings.WECHATXML_TOUSERNAME_NAME, new XCData(model.ToUserName)),
                new XElement(WeChatSettings.WECHATXML_FROMUSERNAME_NAME, new XCData(model.FromUserName)),
                new XElement(WeChatSettings.WECHATXML_CREATETIME_NAME, model.CreateTime.ToString()),
                new XElement(WeChatSettings.WECHATXML_MSGTYPE_NAME, new XCData(model.MsgType.ToString().ToLower()))
            );

            switch (model.MsgType)
            {
                case MsgTypes.TEXT:
                    rootElement.Add(new XElement(WeChatSettings.WECHATXML_CONTENT_NAME, new XCData(model.Content)));
                    break;

                case MsgTypes.IMAGE:
                    rootElement.Add(new XElement(
                        WeChatSettings.WECHATXML_IMAGE_NAME,
                        new XElement(WeChatSettings.WECHATXML_MEDIAID_NAME, new XCData(model.Image.MediaId))
                    ));
                    break;

                case MsgTypes.VOICE:
                    rootElement.Add(new XElement(
                        WeChatSettings.WECHATXML_VOICE_NAME,
                        new XElement(WeChatSettings.WECHATXML_MEDIAID_NAME, new XCData(model.Voice.MediaId))
                    ));
                    break;

                case MsgTypes.VIDEO:
                    rootElement.Add(new XElement(
                        WeChatSettings.WECHATXML_VIDEO_NAME,
                        new XElement(WeChatSettings.WECHATXML_MEDIAID_NAME, new XCData(model.Video.MediaId)),
                        new XElement(WeChatSettings.WECHATXML_TITLE_NAME, new XCData(model.Video.Title)),
                        new XElement(WeChatSettings.WECHATXML_DESCRIPTION_NAME, new XCData(model.Video.Description))
                    ));
                    break;

                case MsgTypes.MUSIC:
                    rootElement.Add(new XElement(
                        WeChatSettings.WECHATXML_MUSIC_NAME,
                        new XElement(WeChatSettings.WECHATXML_TITLE_NAME, new XCData(model.Music.Title)),
                        new XElement(WeChatSettings.WECHATXML_DESCRIPTION_NAME, new XCData(model.Music.Description)),
                        new XElement(WeChatSettings.WECHATXML_MUSICURL_NAME, new XCData(model.Music.MusicUrl)),
                        new XElement(WeChatSettings.WECHATXML_HQMUSICURL_NAME, new XCData(model.Music.HQMusicUrl)),
                        new XElement(WeChatSettings.WECHATXML_THUMBMEDIAID_NAME, new XCData(model.Music.ThumbMediaId))
                    ));
                    break;

                case MsgTypes.NEWS:
                    if (model.Articles == null || model.Articles.Count == 0)
                    {
                        rootElement.Add(
                            new XElement(WeChatSettings.WECHATXML_ARTICLECOUNT_NAME, 0),
                            new XElement(WeChatSettings.WECHATXML_ARTICLES_NAME)
                        );
                    }
                    else
                    {
                        rootElement.Add(new XElement(WeChatSettings.WECHATXML_ARTICLECOUNT_NAME, model.Articles.Count));
                        var articlesElement = new XElement(WeChatSettings.WECHATXML_ARTICLES_NAME);
                        foreach (var article in model.Articles)
                        {
                            articlesElement.Add(new XElement(
                                WeChatSettings.WECHATXML_ITEM_NAME,
                                new XElement(WeChatSettings.WECHATXML_TITLE_NAME, new XCData(article.Title)),
                                new XElement(WeChatSettings.WECHATXML_DESCRIPTION_NAME, new XCData(article.Description)),
                                new XElement(WeChatSettings.WECHATXML_PICURL_NAME, new XCData(article.PicUrl)),
                                new XElement(WeChatSettings.WECHATXML_URL_NAME, new XCData(article.Url))
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
        public T CallRemoteInterface<T>(RequestTypes requestType, RequestContentTypes requestContentType, string url, string parameters = null) where T : class
        {
            var uri = new Uri(url);
            var request = WebRequest.Create(uri);

            request.Method = requestType.ToString();
            request.ContentType = requestContentType == RequestContentTypes.FORM ? WeChatSettings.REQUEST_CONTENTTYPE_FORM : WeChatSettings.REQUEST_CONTENTTYPE_JSON;
            request.Timeout = WeChatSettings.REQUEST_TIMEOUT;

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
                var content = streamReader.ReadToEnd();
                if (typeof(T) == typeof(string))
                    return content as T;
                else
                    return JsonHelper.GetModelWithJson<T>(content);
            }
        }


        #region Remote

        public WeChatAccessTokenInfo GetWeChatAccessTokenInfo()
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
                        var url = string.Format(WeChatSettings.WECHATURL_GETACCESSTOKEN_FORMAT, _appid, _appsecret);

                        var accessTokenInfo = CallRemoteInterface<WeChatAccessTokenInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
                        accessTokenInfo.ExpiresTime = DateTime.Now.AddSeconds(accessTokenInfo.expires_in);

                        _accessTokenInfo = accessTokenInfo;
                    }
                }
            }

            return _accessTokenInfo;
        }

        public string GetMenu()
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(WeChatSettings.WECHATURL_GETMENU_FORMAT, accessTokenInfo.access_token);

            return CallRemoteInterface<string>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        public WeChatErrorInfo CreateMenu()
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(WeChatSettings.WECHATURL_CREATEMENU_FORMAT, accessTokenInfo.access_token);

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

        public WeChatErrorInfo DeleteMenu()
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(WeChatSettings.WECHATURL_DELETEMENU_FORMAT, accessTokenInfo.access_token);

            return CallRemoteInterface<WeChatErrorInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        public WeChatUserListInfo GetWeChatUserListInfo(string nextOpenid = "")
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(WeChatSettings.WECHATURL_GETUSERLISTINFO_FORMAT, accessTokenInfo.access_token, nextOpenid);

            return CallRemoteInterface<WeChatUserListInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        public WeChatUserInfo GetWeChatUserInfo(string openid, string language = WeChatSettings.WECHATLANGUAGE_ZHCN)
        {
            var accessTokenInfo = GetWeChatAccessTokenInfo();
            var url = string.Format(WeChatSettings.WECHATURL_GETUSERINFO_FORMAT, accessTokenInfo.access_token, openid, language);

            return CallRemoteInterface<WeChatUserInfo>(RequestTypes.GET, RequestContentTypes.JSON, url);
        }

        #endregion
    }
}
