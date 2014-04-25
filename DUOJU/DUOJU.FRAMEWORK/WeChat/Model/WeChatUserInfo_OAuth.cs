using System.Collections.Generic;

namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChatUserInfo_OAuth : WeChatReturnInfo
    {
        public string openid { get; set; }

        public string nickname { get; set; }

        public int? sex { get; set; }

        public string city { get; set; }

        public string province { get; set; }

        public string country { get; set; }

        public string headimgurl { get; set; }

        public IList<string> privilege { get; set; }
    }
}
