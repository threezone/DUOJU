using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using YCF.CRM.Helper.Sys.Attribute;
using YCF.CRM.Models;
using YCF.CRM.Model.Entity;
using YCF.CRM.DAO.Abstract;
using YCF.CRM.DAO.Utilities;
using System.Web.Security;

namespace YCF.CRM.Controllers
{
    public class PlugInController : BaseController
    {
        private IHotSaleRepository SpHotSaleRepository { get; set; }
        private IPositionRepository SpPositionRepository { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowCrossSiteJsonAttribute]
        public ActionResult GetHotSale(string providerName, float tuanPrice, float price, int volume, string content,
            string start, string end, string address, string phone, string city, string district, string metro, string traffic,
            string desc, string evaluate, string url, string productId, string productType, string fromType, int followBy,
            string imgUrl, string lat, string lng, string productName, string rate, string channelCity)
        {
            try
            {
                var hotSaleList = SpHotSaleRepository.CheckHotSaleIsExist(productId, fromType);
                if (hotSaleList.Count==0)
                {
                    var hotSale = new HotSale();
                    hotSale.ProductName = productName;
                    hotSale.Provider = providerName;
                    hotSale.TuanPrice = tuanPrice;
                    hotSale.Price = price;
                    hotSale.SaleVolume = volume;
                    hotSale.ProductId = productId;
                    hotSale.ProductContent = content.TrimEnd('&');
                    hotSale.CreateDate = System.DateTime.Now;
                    hotSale.CreateBy = followBy;
                    hotSale.ChannelCity = channelCity;
                    if (!string.IsNullOrEmpty(start))
                    {
                        hotSale.StartDate = Convert.ToDateTime(start);
                    }
                    if (!string.IsNullOrEmpty(end))
                    {
                        hotSale.EndDate = Convert.ToDateTime(end);
                    }
                    hotSale.Address = address;
                    hotSale.Phone = phone;
                    if (!string.IsNullOrEmpty(city))
                    {
                        var cityObj = SpPositionRepository.GetCityByName(city);
                        if (cityObj != null)
                        {
                            hotSale.CityId = cityObj.CityId;
                            var provinceObj = SpPositionRepository.GetProvinceByProvinceCode(cityObj.ProvinceCode);
                            hotSale.ProvinceId = provinceObj.ProvinceId;
                        }
                    }
                    if (!string.IsNullOrEmpty(district))
                    {
                        var districtObj = SpPositionRepository.GetDistrictByName(district);
                        if (districtObj != null)
                        {
                            hotSale.DistrictId = districtObj.DistrictId;
                        }
                    }
                    if (!string.IsNullOrEmpty(rate)&&!rate.ToLower().Equals("null"))
                    {
                        hotSale.Rate = Convert.ToSingle(rate);
                    }
                    hotSale.Metro = metro;
                    hotSale.Traffic = traffic;
                    hotSale.Intro = desc;
                    hotSale.Evaluate = evaluate;
                    hotSale.Url = url;
                    hotSale.ProductType = productType;
                    hotSale.FromType = fromType;
                    hotSale.FollowById = followBy;

                    if (!string.IsNullOrEmpty(lat))
                    {
                        hotSale.Latitude = Convert.ToSingle(lat);
                    }

                    if (!string.IsNullOrEmpty(lng))
                    {
                        hotSale.Longitude = Convert.ToSingle(lng);
                    }

                    if (!string.IsNullOrEmpty(imgUrl)) 
                    {
                        var hotSaleImgUrlVirtual = "/upload/hotsale/" + fromType + ImageUtility.CreateRandomName() + System.IO.Path.GetExtension(imgUrl);
                        var hotSaleImgUrl = Server.MapPath("~" + hotSaleImgUrlVirtual);
                        ImageUtility.DownloadRemoteImageFile(imgUrl, hotSaleImgUrl);
                        hotSale.ImgUrl = hotSaleImgUrlVirtual;
                    }

                    SpHotSaleRepository.AddHotSale(hotSale);
                    return Content("success");
                }
                else
                {
                    return Content("该数据已被"+hotSaleList[0].FollowBy.Name+"采集，不可多次收录！");
                }
            }
            catch (Exception ex) {
                return Content("fail");
            }
        }
    }
}
