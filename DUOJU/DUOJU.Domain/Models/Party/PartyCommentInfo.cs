using DUOJU.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DUOJU.Domain.Models.Party
{
    public class PartyCommentInfo
    {
        public int? CommentId { get; set; }

        [Required]
        public int? SupplierId { get; set; }

        [Required]
        public int? PartyId { get; set; }

        [Required]
        public int? UserId { get; set; }

        public string UserNickName { get; set; }

        [Required]
        public string Content { get; set; }

        public int? Status { get; set; }

        public PartyCommentStatuses? EStatus { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
