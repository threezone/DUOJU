﻿using DUOJU.Domain;
using DUOJU.Domain.Exceptions;
using DUOJU.Domain.Extensions;
using DUOJU.Domain.Helpers;
using DUOJU.Domain.Models.Party;
using DUOJU.Service.Abstract;
using DUOJU.Service.Concrete;
using DUOJU.WECHAT.Models.Party;
using DUOJU.WECHAT.Sys.Helpers;
using System.Web.Mvc;
using DUOJU.FRAMEWORK.WeChat;
using System;
using log4net;

namespace DUOJU.WECHAT.Controllers
{
    public class PartyController : Controller
    {
        private ILog logger = LogManager.GetLogger(typeof(PartyController));

        private IPartyService PartyService { get; set; }

        private ISupplierService SupplierService { get; set; }


        public PartyController()
        {
            PartyService = new PartyService();
            SupplierService = new SupplierService();
        }


        /// <summary>
        /// 发布聚会
        /// </summary>
        public ActionResult PublishParty(int supplierId, string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return Content("无法获取当前用户信息。");
            else
            {
                var accessTokenInfo = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);

                var model = new PublishPartyViewModel
                {
                    SupplierInfo = SupplierService.GetSupplierInfoById(supplierId),
                    PartyInfo = new PublishPartyInfo
                    {
                        OpenId = accessTokenInfo.openid,
                        SupplierId = supplierId
                    }
                };

                return View(model);
            }
        }

        /// <summary>
        /// 发布聚会
        /// </summary>
        [HttpPost]
        public ActionResult PublishParty(PublishPartyInfo partyInfo)
        {
            string json;
            if (ModelState.IsValid)
            {
                try
                {
                    var partyId = PartyService.AddParty(partyInfo);

                    return RedirectToAction("ViewParty", new { partyId = partyId, isCreate = true });
                }
                catch (BasicSystemException ex)
                {
                    json = JsonHelper.GetJsonForFail(ex.ToLocalize());
                }
            }
            else
                json = JsonHelper.GetJsonForFail(ModelValidationHelper.GetServSideValidErrorMsg(ModelState));

            return Content(json);
        }

        /// <summary>
        /// 查看聚会
        /// </summary>
        public ActionResult ViewParty(int partyId, bool? isCreate, bool? isReturn)
        {
            var model = new ViewPartyViewModel
            {
                IsCreate = isCreate ?? false,
                IsReturn = isReturn ?? false,
                PartyInfo = PartyService.GetPartyInfo(partyId)
            };

            return View(model);
        }

        /// <summary>
        /// 报名聚会
        /// </summary>
        public ActionResult ParticipateParty(int partyId, string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return Content("无法获取当前用户信息。");
            else
            {
                var accessTokenInfo = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);

                try
                {
                    var participateCountInfo = PartyService.ParticipateParty(partyId, accessTokenInfo.openid);
                    if (participateCountInfo.ParticipateCount == participateCountInfo.MinIntoForce)
                        WeChatHelper.WeChat.SendCSTextMessage(participateCountInfo.InitiatorOpenId, string.Format(CommonSettings.DUOJUWECHATMESSAGE_PARTYFULLED_FORMAT, WeChatHelper.WeChat.ConvertOAuthUrl("http://wechat.duoju.us/Party/ConfirmParty/" + partyId, OauthScopes.SNSAPI_BASE, null)));

                    return RedirectToAction("ViewParty", new { partyId = partyId, isReturn = true });
                }
                catch (BasicSystemException ex)
                {
                    return Content(ex.ToLocalize());
                }
            }
        }

        /// <summary>
        /// 确定聚会
        /// </summary>
        public ActionResult ConfirmParty(int partyId, string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return Content("无法获取当前用户信息。");
            else
            {
                var accessTokenInfo = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);

                try
                {
                    var confirmPartyInfo = PartyService.ConfirmParty(partyId, accessTokenInfo.openid);

                    var model = new ConfirmPartyViewModel
                    {
                        ConfirmPartyInfo = confirmPartyInfo
                    };

                    return View(model);
                }
                catch (BasicSystemException ex)
                {
                    return Content(ex.ToLocalize());
                }
            }
        }


        /// <summary>
        /// 我发布的聚会
        /// </summary>
        public ActionResult MyParties(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return Content("无法获取当前用户信息。");
            else
            {
                var accessTokenInfo = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);

                var model = new ViewPartiesViewModel
                {
                    PartyInfos = PartyService.GetPartyInfosByCreateUser(accessTokenInfo.openid)
                };

                return View("ViewParties", model);
            }
        }

        /// <summary>
        /// 我参与的聚会
        /// </summary>
        public ActionResult MyParticipateParties(string code, string state)
        {
            if (string.IsNullOrEmpty(code))
                return Content("无法获取当前用户信息。");
            else
            {
                var accessTokenInfo = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);

                var model = new ViewPartiesViewModel
                {
                    PartyInfos = PartyService.GetPartyInfosByParticipantUser(accessTokenInfo.openid)
                };

                return View("ViewParties", model);
            }
        }
    }
}
