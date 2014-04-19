using System.Collections.Generic;

namespace Domain.Models.WeChat
{
    public class WeChatUserListInfo : WeChatErrorInfo
    {
        public int total { get; set; }

        public int count { get; set; }

        public WeChatUserListData data { get; set; }

        public string next_openid { get; set; }
    }

    public class WeChatUserListData
    {
        public IList<string> openid { get; set; }
    }
}
