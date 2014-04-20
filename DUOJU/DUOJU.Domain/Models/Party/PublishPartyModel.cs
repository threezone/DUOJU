using DUOJU.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace DUOJU.Domain.Models.Party
{
    public class PublishPartyModel
    {
        /// <summary>
        /// 微信用户Open Id
        /// </summary>
        public string OpenId { get; set; }

        /// <summary>
        /// 聚会Id
        /// </summary>
        public int? PartyId { get; set; }

        /// <summary>
        /// 客户Id
        /// </summary>
        [Required]
        public int? SupplierId { get; set; }

        /// <summary>
        /// 发起者Id
        /// </summary>
        //[Required]
        //public int? InitiatorId { get; set; }

        /// <summary>
        /// 聚会日期
        /// </summary>
        [Required]
        public DateTime? HoldDate { get; set; }

        /// <summary>
        /// 聚会时间段
        /// </summary>
        [Required]
        public PartyHoldTimes? HoldTime { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 最少人数
        /// </summary>
        [Required]
        [Range(2, int.MaxValue)]
        public int? MinIntoForce { get; set; }

        /// <summary>
        /// 最多人数
        /// </summary>
        public int? MaxIntoForce { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        //public PartyStatuses? Status { get; set; }
    }
}
