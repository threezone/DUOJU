namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChatSettings
    {
        public const int REQUEST_TIMEOUT = 6000;

        public const string REQUEST_CONTENTTYPE_FORM = "application/x-www-form-urlencoded; charset=utf-8";

        public const string REQUEST_CONTENTTYPE_JSON = "application/json; charset=utf-8";

        public const string REQUEST_CONTENTTYPE_XML = "text/xml; charset=utf-8";


        public const string WECHATLANGUAGE_ZHCN = "zh_CN";

        public const string WECHATLANGUAGE_ZHTW = "zh_TW";

        public const string WECHATLANGUAGE_EN = "en";


        public const string WECHATURL_GETACCESSTOKEN_FORMAT = "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";

        public const string WECHATURL_GETACCESSTOKEN_OAUTH_FORMAT = "https://api.weixin.qq.com/sns/oauth2/access_token?appid={0}&secret={0}&code={0}&grant_type=authorization_code";

        public const string WECHATURL_REFRESHACCESSTOKEN_OAUTH_FORMAT = "https://api.weixin.qq.com/sns/oauth2/refresh_token?appid={0}&grant_type=refresh_token&refresh_token={1}";

        public const string WECHATURL_GETMENU_FORMAT = "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}";

        public const string WECHATURL_CREATEMENU_FORMAT = "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}";

        public const string WECHATURL_DELETEMENU_FORMAT = "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}";

        public const string WECHATURL_GETUSERLISTINFO_FORMAT = "https://api.weixin.qq.com/cgi-bin/user/get?access_token={0}&next_openid={1}";

        public const string WECHATURL_GETUSERINFO_FORMAT = "https://api.weixin.qq.com/cgi-bin/user/info?access_token={0}&openid={1}&lang={2}";

        public const string WECHATURL_GETUSERINFO_OAUTH_FORMAT = "https://api.weixin.qq.com/sns/userinfo?access_token={0}&openid={1}&lang={2}";


        public const string WECHATXML_ROOT_NAME = "xml";

        //public const string WECHATXML_URL_NAME = "URL";

        public const string WECHATXML_TOUSERNAME_NAME = "ToUserName";

        public const string WECHATXML_FROMUSERNAME_NAME = "FromUserName";

        public const string WECHATXML_CREATETIME_NAME = "CreateTime";

        public const string WECHATXML_MSGTYPE_NAME = "MsgType";

        public const string WECHATXML_MSGID_NAME = "MsgId";

        public const string WECHATXML_CONTENT_NAME = "Content";

        public const string WECHATXML_PICURL_NAME = "PicUrl";

        public const string WECHATXML_MEDIAID_NAME = "MediaId";

        public const string WECHATXML_FORMAT_NAME = "Format";

        public const string WECHATXML_THUMBMEDIAID_NAME = "ThumbMediaId";

        public const string WECHATXML_LOCATIONX_NAME = "Location_X";

        public const string WECHATXML_LOCATIONY_NAME = "Location_Y";

        public const string WECHATXML_SCALE_NAME = "Scale";

        public const string WECHATXML_LABEL_NAME = "Label";

        public const string WECHATXML_TITLE_NAME = "Title";

        public const string WECHATXML_DESCRIPTION_NAME = "Description";

        public const string WECHATXML_URL_NAME = "Url";

        public const string WECHATXML_EVENT_NAME = "Event";

        public const string WECHATXML_EVENTKEY_NAME = "EventKey";

        public const string WECHATXML_TICKET_NAME = "Ticket";

        public const string WECHATXML_LATITUDE_NAME = "Latitude";

        public const string WECHATXML_LONGITUDE_NAME = "Longitude";

        public const string WECHATXML_PRECISION_NAME = "Precision";

        public const string WECHATXML_IMAGE_NAME = "Image";

        public const string WECHATXML_VOICE_NAME = "Voice";

        public const string WECHATXML_VIDEO_NAME = "Video";

        public const string WECHATXML_MUSIC_NAME = "Music";

        public const string WECHATXML_MUSICURL_NAME = "MusicUrl";

        public const string WECHATXML_HQMUSICURL_NAME = "HQMusicUrl";

        public const string WECHATXML_ARTICLECOUNT_NAME = "ArticleCount";

        public const string WECHATXML_ARTICLES_NAME = "Articles";

        public const string WECHATXML_ITEM_NAME = "item";
    }
}
