using System.Collections.Generic;

namespace Domain.Models.WeChat
{
    public class WeChatMenuInfo
    {
        public IList<WeChatMenuItemInfo> button { get; set; }
    }

    public class WeChatMenuItemInfo
    {
        public string type { get; set; }

        public string name { get; set; }

        public string key { get; set; }

        public string url { get; set; }

        public IList<WeChatMenuItemInfo> sub_button { get; set; }
    }
}
