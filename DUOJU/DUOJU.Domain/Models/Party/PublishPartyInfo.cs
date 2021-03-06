﻿using System;
using System.ComponentModel.DataAnnotations;
using DUOJU.Domain.Enums;

namespace DUOJU.Domain.Models.Party
{
    public class PublishPartyInfo
    {
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
        /// 微信用户Open Id
        /// </summary>
        [Required]
        public string OpenId { get; set; }

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
        /// 聚会说明
        /// </summary>
        [StringLength(100)]
        public string Description { get; set; }

        /// <summary>
        /// 最少参加人数
        /// </summary>
        [Required]
        [Range(2, int.MaxValue)]
        public int? MinIntoForce { get; set; }

        /// <summary>
        /// 最多参加人数
        /// </summary>
        [Range(2, int.MaxValue)]
        public int? MaxIntoForce { get; set; }
    }
}
