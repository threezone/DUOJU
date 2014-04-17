using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;
using System.Xml.Linq;
using Domain;
using Domain.Enums;
using Domain.Helpers;
using Domain.Models;
using log4net;

namespace DUOJU.WECHAT.Controllers
{
    public class WeChatController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(WeChatController));


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
                CreateTime = DateTime.Now.Ticks / 1000
            };
            switch (receiveModel.MsgType)
            {
                case MsgTypes.TEXT:
                    switch (receiveModel.Content)
                    {
                        case "help":
                        case "HELP":
                        case "Help":
                        case "?":
                        case "？":
                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = "1、重设菜单\n2、获取用户信息\n3、获取access_token";
                            break;

                        case "1":
                            break;

                        case "2":
                            break;

                        case "3":
                            break;
                    }
                    break;

                case MsgTypes.EVENT:
                    switch (receiveModel.Event.Value)
                    {
                        case Events.SUBSCRIBE:
                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = "欢迎关注！";
                            break;

                        case Events.SCAN:
                            sendModel.MsgType = MsgTypes.TEXT;
                            sendModel.Content = "有什么我可以帮到您？";
                            break;
                    }
                    break;
            }

            logger.Info(ConvertSendXML(sendModel));
            return Content(ConvertSendXML(sendModel));
        }

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
                            receiveModel.EventKey = rootElement.Descendants(CommonSettings.WECHATXML_EVENTKEY_NAME).FirstOrDefault().Value;
                            receiveModel.Ticket = rootElement.Descendants(CommonSettings.WECHATXML_TICKET_NAME).FirstOrDefault().Value;
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
        /// GET调用接口
        /// </summary>
        private string Get(string url, int time = 60000)
        {
            var address = new Uri(url);
            var request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json;charset=utf-8"; // "application/x-www-form-urlencoded";
            request.Timeout = time;
            string result = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return (result);
        }

        /// <summary>
        /// POST调用接口
        /// </summary>
        private string Post(string url, string param, int time = 60000)
        {
            Uri address = new Uri(url);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8"; // "application/x-www-form-urlencoded";
            request.Timeout = time;
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(param == null ? "" : param);
            request.ContentLength = byteData.Length;
            using (Stream postStream = request.GetRequestStream())
            {
                postStream.Write(byteData, 0, byteData.Length);
            }
            string result = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                StreamReader reader = new StreamReader(response.GetResponseStream());
                result = reader.ReadToEnd();
            }
            return (result);
        }


        private JsonResult GetWxCredential()
        {
            // http://www.cnblogs.com/qidian10/p/3492751.html
            var url = string.Format(CommonSettings.WECHATURL_GETACCESSTOKEN_FORMAT, CommonSettings.DUOJU_APPID, CommonSettings.DUOJU_APPSECRET);
            string rst = Get(url);
            if (rst.Contains("access_token"))
            {
                string tokenId=rst.Replace("{\"access_token\":\"", "").Replace("\",\"expires_in\":7200}", "");
                //CacheHelper.CacheInsertAddMinutes("access_token",tokenId,120);
                return Json(tokenId, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(rst, JsonRequestBehavior.AllowGet);
        }
    }
}
