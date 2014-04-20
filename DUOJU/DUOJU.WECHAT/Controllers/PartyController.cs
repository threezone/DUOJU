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

namespace DUOJU.WECHAT.Controllers
{
    public class PartyController : Controller
    {
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
        public ActionResult PublishParty(int supplierId, string openId)
        {
            var model = new PublishPartyViewModel
            {
                SupplierInfo = SupplierService.GetSupplierInfoById(supplierId),
                PartyModel = new PublishPartyModel
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
        public ActionResult PublishParty(PublishPartyModel partyModel)
        {
            string json;
            if (ModelState.IsValid)
            {
                try
                {
                    var partyId = PartyService.AddParty(partyModel);

                    json = JsonHelper.GetJsonWithModel(new
                    {
                        Result = CommonSettings.OPERATE_SUCCESS,
                        Message = CommonSettings.TIPS_SUCCESS,
                        Url = "localhost:1002/Party/ViewParty/" + partyId
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

        public ActionResult ViewParty(int partyId)
        {
            var model = new ViewPartyViewModel
            {
            };

            return View(model);
        }
    }
}
