using System.Collections.Generic;

namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChatUserListInfo : WeChatReturnInfo
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
