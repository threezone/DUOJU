using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DUOJU.Domain.Models.Party
{
    public class PartyCommentInfo
    {
        public int? CommentId { get; set; }

        public int? SUPPLIER_ID { get; set; }

        public int? PARTY_ID { get; set; }

        public int? USER_ID { get; set; }

        public string CONTENT { get; set; }

        public int? STATUS { get; set; }

        public int? ESTATUS { get; set; }
    }
}
