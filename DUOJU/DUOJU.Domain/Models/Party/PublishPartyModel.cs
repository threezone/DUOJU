using DUOJU.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DUOJU.Domain.Models.Party
{
    public class PublishPartyModel
    {
        public string OpenId { get; set; }


        public int? PartyId { get; set; }

        [Required]
        public int? SupplierId { get; set; }

        [Required]
        public int? InitiatorId { get; set; }

        [Required]
        public DateTime? HoldDate { get; set; }

        [Required]
        public PartyHoldTimes? HoldTime { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [Required]
        [Range(2, int.MaxValue)]
        public int? MinIntoForce { get; set; }

        public int? MaxIntoForce { get; set; }

        public PartyStatuses? Status { get; set; }
    }
}
