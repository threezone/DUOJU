using System;

namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChatAccessTokenInfo : WeChatReturnInfo
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public DateTime ExpiresTime { get; set; }
    }
}
