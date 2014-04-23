using System;

namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChatAccessTokenInfo_OAuth : WeChatErrorInfo
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public string refresh_token { get; set; }

        public string openid { get; set; }

        public string scope { get; set; }
    }
}
