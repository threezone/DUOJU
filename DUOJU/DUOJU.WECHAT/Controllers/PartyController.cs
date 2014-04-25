using DUOJU.Domain;
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

        private IUserService UserService { get; set; }


        public PartyController()
        {
            PartyService = new PartyService();
            SupplierService = new SupplierService();
            UserService = new UserService();
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

                    return RedirectToAction("ViewParty", new { partyId = partyId });
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
        public ActionResult ViewParty(int partyId, bool? isReturn, int? participantId)
        {
            var model = new ViewPartyViewModel
            {
                IsReturn = isReturn ?? false,
                ParticipantId = participantId,
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
                return Content("请先允许获取当前用户信息。");
            else
            {
                var accessTokenInfo = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);
                var userInfo = WeChatHelper.WeChat.GetWeChatUserInfo_OAuth(accessTokenInfo.access_token, accessTokenInfo.openid);

                try
                {
                    var userId = UserService.AddWeChatUser(userInfo);
                    var participateCountInfo = PartyService.ParticipateParty(partyId, userId);
                    if (participateCountInfo.ParticipateCount == participateCountInfo.MinIntoForce)
                        WeChatHelper.WeChat.SendCSTextMessage(userInfo.openid, string.Format(CommonSettings.DUOJUWECHATMESSAGE_PARTYFULLED_FORMAT, WeChatHelper.WeChat.ConvertOAuthUrl("http://wechat.duoju.us/Party/ConfirmParty/" + partyId, OauthScopes.SNSAPI_BASE, null)));

                    return RedirectToAction("ViewParty", new { partyId = partyId, isReturn = true, participantId = userId });
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
                    var identifierInfo = PartyService.ConfirmParty(partyId, accessTokenInfo.openid);

                    return Content(identifierInfo.Item1 + "-" + identifierInfo.Item2);
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

                var model = new MyPartiesViewModel
                {
                    PartyInfos = PartyService.GetPartyInfosByCreateUser(accessTokenInfo.openid)
                };

                return View(model);
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

                var model = new MyParticipatePartiesViewModel
                {
                    PartyInfos = PartyService.GetPartyInfosByParticipantUser(accessTokenInfo.openid)
                };

                return View(model);
            }
        }
    }
}
