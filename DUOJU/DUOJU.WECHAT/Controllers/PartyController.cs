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

namespace DUOJU.WECHAT.Controllers
{
    public class PartyController : Controller
    {
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
        public ActionResult PublishParty(int supplierId, string openId)
        {
            var model = new PublishPartyViewModel
            {
                SupplierInfo = SupplierService.GetSupplierInfoById(supplierId),
                PartyInfo = new PublishPartyInfo
                {
                    OpenId = openId,
                    SupplierId = supplierId
                }
            };

            return View(model);
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
                    json = JsonHelper.GetJsonWithModel(new
                    {
                        Result = CommonSettings.OPERATE_SUCCESS,
                        Message = CommonSettings.TIPS_SUCCESS,
                        Url = "/Party/ViewParty/" + partyId + "/" + partyInfo.OpenId
                    });
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
                var accessToken = WeChatHelper.WeChat.GetWeChatAccessTokenInfo_OAuth(code);
                var userInfo = WeChatHelper.WeChat.GetWeChatUserInfo_OAuth(accessToken.access_token, accessToken.openid);

                var userId = UserService.AddWeChatUser(userInfo);
                try
                {
                    var participateCountInfo = PartyService.ParticipateParty(partyId, userId);
                    if (participateCountInfo.ParticipateCount == participateCountInfo.MinIntoForce)
                    {
                        // 通知创建者
                    }

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
        public ActionResult ConfirmParty(int partyId)
        {
            return null;
        }

        /// <summary>
        /// 消费聚会
        /// </summary>
        public ActionResult ConsumParty(int partyId)
        {
            return null;
        }





        public ActionResult TEST(string code, string state)
        {
            return Content(code + " & " + state);
        }
    }
}
