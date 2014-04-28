using DUOJU.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DUOJU.Domain.Models.Party
{
    public class PartyCommentInfo
    {
        public int? CommentId { get; set; }

        public string UserNickName { get; set; }

        public string Content { get; set; }

        public int? Status { get; set; }

        public PartyCommentStatuses? EStatus { get; set; }

        public DateTime CreateTime { get; set; }
    }
}
