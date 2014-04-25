using DUOJU.Domain.Enums;
using DUOJU.Domain.Models.Supplier;
using System;
using System.Collections.Generic;

namespace DUOJU.Domain.Models.Party
{
    public class PartyInfo
    {
        /// <summary>
        /// 聚会Id
        /// </summary>
        public int? PartyId { get; set; }

        /// <summary>
        /// 发起者姓名
        /// </summary>
        public string InitiatorName { get; set; }

        /// <summary>
        /// 聚会日期
        /// </summary>
        public DateTime HoldDate { get; set; }

        /// <summary>
        /// 聚会时间段
        /// </summary>
        public int HoldTime { get; set; }

        /// <summary>
        /// 聚会时间段
        /// </summary>
        public PartyHoldTimes EHoldTime { get; set; }

        /// <summary>
        /// 说明
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 最少人数
        /// </summary>
        public int MinIntoForce { get; set; }

        /// <summary>
        /// 最多人数
        /// </summary>
        public int? MaxIntoForce { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public PartyStatuses EStatus { get; set; }


        /// <summary>
        /// 客户信息
        /// </summary>
        public SupplierInfo SupplierInfo { get; set; }

        /// <summary>
        /// 参与者信息
        /// </summary>
        public IList<PartyParticipantInfo> PartyParticipantInfos { get; set; }
    }
}
