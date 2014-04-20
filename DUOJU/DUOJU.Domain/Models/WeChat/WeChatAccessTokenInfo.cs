
using System;
namespace DUOJU.Domain.Models.WeChat
{
    public class WeChatAccessTokenInfo : WeChatErrorInfo
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public DateTime ExpiresTime { get; set; }
    }
}
