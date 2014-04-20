namespace DUOJU.FRAMEWORK.WeChat
{
    public class WeChatReceiveMessageModel
    {
        //public string URL { get; set; }

        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public long CreateTime { get; set; }

        public MsgTypes MsgType { get; set; }


        /// <summary>
        /// TEXT / IMAGE / VOICE / VIDEO / LOCATION / LINK
        /// </summary>
        public long? MsgId { get; set; }

        /// <summary>
        /// TEXT
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// IMAGE
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// IMAGE / VOICE/ VIDEO
        /// </summary>
        public string MediaId { get; set; }

        /// <summary>
        /// VOICE
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// VIDEO
        /// </summary>
        public string ThumbMediaId { get; set; }

        /// <summary>
        /// LOCATION
        /// </summary>
        public double? Location_X { get; set; }

        /// <summary>
        /// LOCATION
        /// </summary>
        public double? Location_Y { get; set; }

        /// <summary>
        /// LOCATION
        /// </summary>
        public int? Scale { get; set; }

        /// <summary>
        /// LOCATION
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// LINK
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// LINK
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// LINK
        /// </summary>
        public string Url { get; set; }


        /// <summary>
        /// EVENT
        /// </summary>
        public Events? Event { get; set; }

        /// <summary>
        /// EVENT
        /// </summary>
        public string EventKey { get; set; }

        /// <summary>
        /// EVENT
        /// </summary>
        public string Ticket { get; set; }

        /// <summary>
        /// EVENT
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// EVENT
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// EVENT
        /// </summary>
        public double? Precision { get; set; }
    }
}
