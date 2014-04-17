using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class WeChatSendMessageModel
    {
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public long CreateTime { get; set; }

        public MsgTypes MsgType { get; set; }


        /// <summary>
        /// TEXT
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// IMAGE
        /// </summary>
        public WeChatSendImageModel Image { get; set; }

        /// <summary>
        /// VOICE
        /// </summary>
        public WeChatSendVoiceModel Voice { get; set; }

        /// <summary>
        /// VIDEO
        /// </summary>
        public WeChatSendVideoModel Video { get; set; }

        /// <summary>
        /// MUSIC
        /// </summary>
        public WeChatSendMusicModel Music { get; set; }

        /// <summary>
        /// NEWS
        /// </summary>
        public IList<WeChatSendArticleModel> Articles { get; set; }
    }

    /// <summary>
    /// IMAGE
    /// </summary>
    public class WeChatSendImageModel
    {
        public string MediaId { get; set; }
    }

    /// <summary>
    /// VOICE
    /// </summary>
    public class WeChatSendVoiceModel
    {
        public string MediaId { get; set; }
    }

    /// <summary>
    /// VIDEO
    /// </summary>
    public class WeChatSendVideoModel
    {
        public string MediaId { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }
    }

    /// <summary>
    /// MUSIC
    /// </summary>
    public class WeChatSendMusicModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string MusicUrl { get; set; }

        public string HQMusicUrl { get; set; }

        public string ThumbMediaId { get; set; }
    }

    /// <summary>
    /// NEWS
    /// </summary>
    public class WeChatSendArticleModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public string PicUrl { get; set; }

        public string Url { get; set; }
    }
}
