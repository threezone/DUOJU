using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using Spring.Web.Support;
using YCF.CRM.DAO.Utilities;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.Model.Admin.Entity;
using YCF.CRM.Model.Entity;
using YCF.CRM.Models;

namespace YCF.CRM.Controllers
{
    public class ImageController : BaseController
    {
        private IImageRepository SpImageRepository { get; set; }
        private IUser_AdminRepository SpUserAdminRepository { get; set; }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Upload(HttpPostedFileBase fileData, int resortId, int sort, int imageCategory, int imageSizeType, int adminUserId=0)
        {
            if (fileData != null)
            {
                var cdnPath = ConfigurationManager.AppSettings.Get("yaochufaCDNPath");
                var fileExt = Path.GetExtension(fileData.FileName);
                //var tmpTitle = Path.GetFileNameWithoutExtension(fileData.FileName);
                var fileName = "hl_" + resortId + "_od_" + sort + "_" + ImageUtility.CreateGuidName() + fileExt;
                var fileFullPath = Path.Combine("images", SetFolder(imageCategory.ToString()));
                if (!Directory.Exists(cdnPath + fileFullPath))
                {
                    Directory.CreateDirectory(cdnPath + fileFullPath);
                }
                var fileFullName = cdnPath + fileFullPath + "/" + fileName;
                fileData.SaveAs(fileFullName);
                //上传图片到七牛
                if (System.IO.File.Exists(fileFullName))
                {
                    string key = fileFullName.Replace(cdnPath, "").Replace("\\", "/").Replace("//", "/");
                    ImageUtility.PutFileToQiNiu(key, fileFullName, ImageFormat.Jpeg);
                }

                var currentUser = new User_Admin();
                try
                {
                    currentUser = SpUserAdminRepository.GetAdminUser(base.CurrentUser.AdminUserId);
                }
                catch (Exception ex)
                {
                    currentUser = SpUserAdminRepository.GetAdminUser(adminUserId);
                }

                var img = new YCF.CRM.Model.Admin.Entity.Image
                {
                    ObjectId = resortId,
                    Title = "",
                    ImageCategoryId = imageCategory,
                    SortOrder = sort,
                    CreatedBy = currentUser.LoginId + ":" + currentUser.UserName,
                    CreatedDate = System.DateTime.Now,
                    ImageSizeType = imageSizeType,
                    Url = (fileFullPath + "//" + fileName).Replace("//", "/").Replace("\\", "/")
                };
                var imgId = SpImageRepository.AddImage(img);

                //各种缩略图，不知毛线作用
                if (imageCategory == 13)
                {
                    string tmpstr = SetFolder(imageCategory.ToString());
                    // 600 *225 
                    ImageUtility.GetFlexibleThumb(fileFullName, 456, 171, fileFullName.Replace(tmpstr, "HolidayTheme"));
                    ImageUtility.GetFlexibleThumb(fileFullName, 600, 225, fileFullName.Replace(tmpstr, "iphonecontent"));
                    ImageUtility.GetFlexibleThumb(fileFullName, 960, 360, fileFullName.Replace(tmpstr, "iphonecontent960"));
                    ImageUtility.GetFlexibleThumb(fileFullName, 600, 225, fileFullName.Replace(tmpstr, "iphonefirst"));
                    ImageUtility.GetFlexibleThumb(fileFullName, 290, 108, fileFullName.Replace(tmpstr, "iphonelitle"));

                    ImageUtility.GetFlexibleThumb(fileFullName, 600, 360, fileFullName.Replace(tmpstr, "600x360"));
                    ImageUtility.GetFlexibleThumb(fileFullName, 772, 360, fileFullName.Replace(tmpstr, "600x280TMP"));
                    ImageUtility.GetFlexibleThumb(fileFullName, 600, 280, fileFullName.Replace(tmpstr, "600x280"));
                    //生成新接口
                    ImageUtility.GetFlexibleThumb(fileFullName, 384, 216, fileFullName.Replace(tmpstr, "384x216"));
                    ImageUtility.GetFlexibleThumb(fileFullName, 185, 102, fileFullName.Replace(tmpstr, "MobileHome"));

                }
                ImageUtility.GetFlexibleThumb(fileFullName, 325, 215, Server.MapPath("~/upload/thumb/") + fileName, false);
                return Content(imgId + "&/upload/thumb/" + fileName);
            }
            return Content("");
        }

        /// <summary>
        /// ckeditor 控件上传调用的方法， CKEditorFuncNum为回调函数队列中的值
        /// </summary>
        /// <param name="upload"></param>
        /// <param name="CKEditorFuncNum"></param>
        /// <returns></returns>
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult UploadImage(HttpPostedFileBase upload, string imageType, string CKEditorFuncNum)
        {
            if (upload != null)
            {
                var cdnPath = ConfigurationManager.AppSettings.Get("yaochufaCDNPath");
                var fileExt = Path.GetExtension(upload.FileName);
                var fileName = ImageUtility.CreateGuidName() + fileExt;
                var dateStr = DateTime.Now.Date.ToString("yyyyMMdd");
                var fileFullPath = Path.Combine("images", imageType, dateStr);
                if (!Directory.Exists(cdnPath + fileFullPath))
                {
                    Directory.CreateDirectory(cdnPath + fileFullPath);
                }
                var fileFullName = cdnPath + fileFullPath + "/" + fileName;
                upload.SaveAs(fileFullName);
                //上传图片到七牛
                if (System.IO.File.Exists(fileFullName))
                {
                    string key = fileFullName.Replace(cdnPath, "").Replace("\\", "/").Replace("//", "/");
                    ImageUtility.PutFileToQiNiu(key, fileFullName, ImageFormat.Jpeg);
                }
                string script = @"
                    <script type='text/javascript'>window.parent.CKEDITOR.tools.callFunction({0}, '{1}', '{2}');</script>
                ";
                return
                    Content(string.Format(script, CKEditorFuncNum,
                        ConfigurationManager.AppSettings["ImageBaseUrl"] + imageType + "/" + dateStr + "/" + fileName, ""));
            }
            return Content("");
        }

        [HttpPost]
        public ActionResult GetImageListByResortId(int resortId)
        {
            var imgCategory = new List<int> { 13, 23 };
            var imgList = SpImageRepository.GetImageListByResortId(resortId, imgCategory);
            var cdnPath = ConfigurationManager.AppSettings.Get("yaochufaCDNPath");
            foreach (var img in imgList)
            {
                var fileName = Path.GetFileName(img.Url);
                if (System.IO.File.Exists(Server.MapPath("~/upload/thumb/") + fileName))
                    continue;
                ImageUtility.GetFlexibleThumb(cdnPath + img.Url, 325, 215, Server.MapPath("~/upload/thumb/") + fileName);
            }
            var result = JSONUtility.ToJson(imgList, true);
            return Content(result);
        }

        [HttpPost]
        public ActionResult PostImageSort(int resortId, int imageCategoryId, string imgIdListStr)
        {
            var imgCategory = new List<int> { imageCategoryId };
            var imgList = SpImageRepository.GetImageListByResortId(resortId, imgCategory);
            var imgIdList = imgIdListStr.Trim(',').Split(',');
            for (var i = 0; i < imgIdList.Count(); i++)
            {
                var img = imgList.First(m => m.ImageId.ToString().Equals(imgIdList[i]));
                img.SortOrder = i + 1;
                SpImageRepository.UploadImage(img);
            }
            var msg = new MessageModel {State = "Success", Message = "更新成功！"};
            return Content(JSONUtility.ToJson(msg, true));
        }

        [HttpPost]
        public ActionResult UpdateImageTitle(int imgId, string title)
        {
            var img = SpImageRepository.GetImage(imgId);
            img.Title = title;
            SpImageRepository.UploadImage(img);
            var msg = new MessageModel { State = "Success", Message = "更新成功！" };
            return Content(JSONUtility.ToJson(msg, true));
        }

        public ActionResult DeleteImage(int imgId)
        {
            SpImageRepository.DeleteImage(imgId);
            var msg = new MessageModel { State = "Success", Message = "删除成功！" };
            return Content(JSONUtility.ToJson(msg, true));
        }

        private string SetFolder(string type)
        {
            var re = string.Empty;
            switch (type)
            {
                case "1":
                    re = "indexbig";
                    break;
                case "2":
                    re = "thumb";
                    break;
                case "3":
                    re = "content";
                    break;
                case "5":
                    re = "firstThumb";
                    break;
                case "10":
                    re = "FirstBig698";
                    break;
                case "11":
                    re = "FirstBar634";
                    break;

                case "12":
                    re = "FirstSquare308";
                    break;
                case "13":
                    re = "Content960";
                    break;
                case "14":
                    re = "ContentBig1200";
                    break;
                case  "23":
                    re = "ProductList466";
                    break;;
            }
            return re;
        }
    }
}
