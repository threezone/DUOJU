
using System;
namespace Domain.Models
{
    public class WeChatAccessTokenInfo : WeChatErrorInfo
    {
        public string access_token { get; set; }

        public int expires_in { get; set; }

        public DateTime ExpiresTime { get; set; }
    }
}
