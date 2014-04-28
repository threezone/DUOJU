using System.ComponentModel.DataAnnotations;

namespace DUOJU.Domain.Models.Party
{
    public class CommentPartyInfo
    {
        /// <summary>
        /// 评论Id
        /// </summary>
        public int? CommentId { get; set; }

        /// <summary>
        /// 聚会Id
        /// </summary>
        [Required]
        public int? PartyId { get; set; }

        /// <summary>
        /// 微信用户Open Id
        /// </summary>
        [Required]
        public string OpenId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [Required]
        [StringLength(100)]
        public string Content { get; set; }
    }
}
