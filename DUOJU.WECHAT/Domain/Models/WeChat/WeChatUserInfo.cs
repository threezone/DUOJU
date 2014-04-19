namespace Domain.Models.WeChat
{
    public class WeChatUserInfo : WeChatErrorInfo
    {
        public int subscribe { get; set; }

        public string openid { get; set; }

        public string nickname { get; set; }

        public int? sex { get; set; }

        public string language { get; set; }

        public string city { get; set; }

        public string province { get; set; }

        public string country { get; set; }

        public string headimgurl { get; set; }

        public int? subscribe_time { get; set; }
    }
}
