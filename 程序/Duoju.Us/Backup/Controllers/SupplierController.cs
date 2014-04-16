using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;

namespace YCF.CRM.Controllers
{
    [Authorize]
    public class SupplierController : BaseController
    {
        private ISupplierInfoRepository SpSupplierInfoRepository { get; set; }

        private IContactsInfoRepository SpContactsInfoRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult PublishSupplierList()
        {
            return View();
        }

        public ActionResult SupplierList()
        {
            return View();
        }

        public ActionResult SupplierDetial(string sid)
        {
            var supplierId = 0;
            if (int.TryParse(sid, out supplierId))
            {
                var supplier = SpSupplierInfoRepository.GetSupplierBySupplierId(supplierId);
                if (supplier.FollowById == CurrentUserId || supplier.FollowById == 0 || !supplier.FollowById.HasValue)
                {
                    return View(supplier);
                }
                else
                {
                    return Redirect("/Error");
                }
            }
            else
            {
                return View(new SupplierInfo());
            }
        }

        [HttpPost]
        public ActionResult GetSupplierList(int page, int rows)
        {
            int total = 0;
            var supplierList = SpSupplierInfoRepository.GetSupplierListByUserId(base.CurrentUserId, (page-1)*rows, rows, out total);
            GridModel<IList<SupplierInfo>> grid = new GridModel<IList<SupplierInfo>>();
            grid.rows = supplierList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetPublishSupplierList(int page, int rows)
        {
            int total = 0;
            int adminId = 0;
            var supplierList = SpSupplierInfoRepository.GetAllSupplierList((page - 1) * rows, rows, out total);
            foreach (var supplier in supplierList)
            {
                if (supplier.FollowById != adminId)
                {
                    supplier.Contacts = "--";
                    supplier.Position = "--";
                    supplier.TelePhone = "--";
                    supplier.Email = "--";
                    supplier.Address = "--";
                    supplier.Plane = "--";
                    supplier.QQ = "--";
                    supplier.WebSite = "--";
                    supplier.ZipCode = "--";
                }
            }
            GridModel<IList<SupplierInfo>> grid = new GridModel<IList<SupplierInfo>>();
            grid.rows = supplierList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetFollowLogListBySIdAndUId(string start, string end, int page, int rows)
        {
            int total = 0;
            var supplierList = SpSupplierInfoRepository.GetFollowSupplierByDateDiff(CurrentUserId, Convert.ToDateTime(start), Convert.ToDateTime(end), (page - 1) * rows, rows, out total);
            GridModel<IList<SupplierInfo>> grid = new GridModel<IList<SupplierInfo>>();
            grid.rows = supplierList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult CheckSupplierByName(string name)
        {
            var supplierList = SpSupplierInfoRepository.GetSupplierByName(name);
            MessageModel msg = new MessageModel();
            if (supplierList.Count <= 0)
            {
                msg.State = "NotExist";
                msg.Message = "not exist";
            }
            else
            {
                msg.State = "Exist";
                var i = 0;
                foreach (var s in supplierList)
                {
                    if (i <= 4)
                    {
                        msg.Message += s.SupplierName + ';';
                        i++;
                    }
                    else 
                    {
                        break;
                    }
                }
            }
            var jsonStr = JSONUtility.ToJson(msg, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult SaveOrUpdateSupplierInfo(SupplierInfo model)
        {
            MessageModel messageModel = new MessageModel();
            string jsonStr = string.Empty;
            if (model.ProvinceId.HasValue && model.ProvinceId.Value == 0)
            {
                model.ProvinceId = null;
            }
            if (model.CityId.HasValue && model.CityId.Value == 0)
            {
                model.CityId = null;
            }
            if (model.DistrictId.HasValue && model.DistrictId.Value == 0)
            {
                model.DistrictId = null;
            }
            if (base.CurrentUserId != 0)
            {
                if (model.SupplierId != 0)
                {
                    model.FollowById = base.CurrentUserId;
                    model.ModifyDate = System.DateTime.Now;
                    model.ModifyBy = base.CurrentUserId;
                    if (SpSupplierInfoRepository.UpdateSupplierInfo(model))
                    {
                        messageModel.State = "success";
                        messageModel.Message = "Update Success";
                    }
                    else
                    {
                        messageModel.State = "false";
                        messageModel.Message = "Update Faile";
                    }
                }
                else
                {
                    model.FollowById = base.CurrentUserId;
                    model.CreateDate = System.DateTime.Now;
                    model.CreateBy = base.CurrentUserId;
                    model.SupplierType = 1;

                    var newId = SpSupplierInfoRepository.AddSupplierInfo(model);

                    if (newId != 0)
                    {
                        messageModel.State = "success";
                        messageModel.Message = newId.ToString();
                    }
                    else
                    {
                        messageModel.State = "false";
                        messageModel.Message = "Save Faile";
                    }
                }
                jsonStr = JSONUtility.ToJson(messageModel, true);
            }
            else
            {
                messageModel.State = "false";
                messageModel.Message = "登陆已掉线";
            }
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult UpdateSupplierPosition(int supplierId, int? provinceId, int? cityId, int? disrtictId)
        {
            var supplier = SpSupplierInfoRepository.GetSupplierBySupplierId(supplierId);
            MessageModel messageModel = new MessageModel();
            if (provinceId.HasValue && provinceId != 0)
            {
                supplier.ProvinceId = provinceId.Value;
            }
            if (cityId.HasValue && cityId.Value != 0)
            {
                supplier.CityId = cityId.Value;
            }
            if (disrtictId.HasValue && disrtictId.Value != 0)
            {
                supplier.DistrictId = disrtictId.Value;
            }
            if (SpSupplierInfoRepository.UpdateSupplierInfo(supplier))
            {
                messageModel.State = "success";
                messageModel.Message = "区域信息更新成功！";
            }
            else
            {
                messageModel.State = "false";
                messageModel.Message = "区域信息更新失败！";
            }
            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult DeleteSupplierInfo(string sidList)
        {
            MessageModel messageModel = new MessageModel();
            if (!string.IsNullOrEmpty(sidList))
            {
                sidList = sidList.Trim(',');
                if (SpSupplierInfoRepository.DeleteSupplierByIdList(sidList))
                {
                    messageModel.State = "success";
                    messageModel.Message = "delete success";
                }
                else
                {
                    messageModel.State = "false";
                    messageModel.Message = "delete false";
                }
            }
            else
            {
                messageModel.State = "false";
                messageModel.Message = "not selections";
            }
            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult SearchSupplier(string keyWord, int page, int rows)
        {
            int total = 0;
            var supplierList = SpSupplierInfoRepository.SearchSupplier(base.CurrentUserId, keyWord, (page - 1) * rows, rows, out total);
            var grid = new GridModel<IList<SupplierInfo>> {rows = supplierList, total = total};
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult SearchPublishSupplier(string keyWord, int page, int rows)
        {
            int total = 0;
            int adminId = 0;
            var supplierList = SpSupplierInfoRepository.SearchSupplier(keyWord, (page - 1) * rows, rows, out total);
            foreach (var supplier in supplierList)
            {
                if (supplier.FollowById != adminId)
                {
                    supplier.Contacts = "--";
                    supplier.Position = "--";
                    supplier.TelePhone = "--";
                    supplier.Email = "--";
                    supplier.Address = "--";
                    supplier.Plane = "--";
                    supplier.QQ = "--";
                    supplier.WebSite = "--";
                    supplier.ZipCode = "--";
                }
            }
            GridModel<IList<SupplierInfo>> grid = new GridModel<IList<SupplierInfo>>();
            grid.rows = supplierList;
            grid.total = total;
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult FollowSupplier(int sid)
        {
            MessageModel messageModel = new MessageModel();
            var supplierInfo = SpSupplierInfoRepository.GetSupplierBySupplierId(sid);
            if (supplierInfo.FollowById != 0)
            {
                messageModel.State = "faile";
                messageModel.Message = "在你申请之前，该客户已经被其他人申请跟进";
            }
            else
            {
                supplierInfo.FollowById = CurrentUserId;
                supplierInfo.ModifyDate = System.DateTime.Now;
                supplierInfo.ModifyBy = base.CurrentUserId;
                if (SpSupplierInfoRepository.UpdateSupplierInfo(supplierInfo))
                {
                    messageModel.State = "success";
                    messageModel.Message = "申请成功";
                }
                else
                {
                    messageModel.State = "faile";
                    messageModel.Message = "申请失败";
                }
            }
            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult ReleaseSupplier(int sid)
        {
            MessageModel messageModel = new MessageModel();
            var supplierInfo = SpSupplierInfoRepository.GetSupplierBySupplierId(sid);
            
            supplierInfo.FollowById = 0;
            supplierInfo.ModifyDate = System.DateTime.Now;
            supplierInfo.ModifyBy = base.CurrentUserId;
            if (SpSupplierInfoRepository.UpdateSupplierInfo(supplierInfo))
            {
                messageModel.State = "success";
                messageModel.Message = "释放成功";
            }
            else
            {
                messageModel.State = "faile";
                messageModel.Message = "释放失败";
            }

            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult GetSupplierInWeek()
        {
            //int dayOfWeek = Convert.ToInt32(DateTime.Now.DayOfWeek);
            //int daydiff = (-1) * dayOfWeek;
            //int dayadd = 6 - dayOfWeek;

            DateTime weekStartDate = DateTime.Now.AddHours(-DateTime.Now.Hour).AddMinutes(-DateTime.Now.Minute).AddSeconds(-DateTime.Now.Second).AddDays(-7);
            DateTime weekEndDate = DateTime.Now;

            var supplierList = SpSupplierInfoRepository.GetSupplierByUserIdAndDateDiff(CurrentUserId, weekStartDate, weekEndDate);
            var dataModel = new DataModel<IList<SupplierInfo>> { 
                total = supplierList.Count,
                rows = supplierList,
                start = weekStartDate.ToShortDateString(),
                end = weekEndDate.ToShortDateString()
            };

            return Content(JSONUtility.ToJson(dataModel, true));
        }

        [HttpPost]
        public ActionResult GetContactsBySupplierId(int sid)
        {
            var contactsList = SpContactsInfoRepository.GetContactsBySupplierId(sid);
            GridModel<IList<ContactsInfo>> grid = new GridModel<IList<ContactsInfo>>();
            grid.rows = contactsList;
            grid.total = contactsList.Count();
            var jsonStr = JSONUtility.ToJson(grid, true);
            return Content(jsonStr);
        }

        [HttpPost]
        public ActionResult UpdateOrSaveContacts(ContactsInfo contactsInfo)
        {
            MessageModel messageModel = new MessageModel();
            if (contactsInfo.ContactsId != 0)
            {
                contactsInfo.ModifyDate = System.DateTime.Now;
                contactsInfo.ModifyBy = base.CurrentUserId;
                if (SpContactsInfoRepository.UpdateContacts(contactsInfo))
                {
                    messageModel.State = "success";
                    messageModel.Message = "Update Success";
                }
                else
                {
                    messageModel.State = "false";
                    messageModel.Message = "Update Faile";
                }
            }
            else
            {
                contactsInfo.CreateDate = System.DateTime.Now;
                contactsInfo.CreateBy = base.CurrentUserId;

                var newId = SpContactsInfoRepository.AddContacts(contactsInfo);

                if (newId != 0)
                {
                    messageModel.State = "success";
                    messageModel.Message = newId.ToString();
                }
                else
                {
                    messageModel.State = "false";
                    messageModel.Message = "Save Faile";
                }
            }
            var jsonStr = JSONUtility.ToJson(messageModel, true);
            return Content(jsonStr);
        }
    }
}
