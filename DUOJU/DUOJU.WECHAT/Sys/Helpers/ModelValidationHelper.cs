using System.Linq;
using System.Web.Mvc;

namespace DUOJU.WECHAT.Sys.Helpers
{
    public class ModelValidationHelper
    {
        /// <summary>
        /// 获取服务器端验证的错误信息
        /// </summary>
        /// <param name="modelState"></param>
        /// <returns></returns>
        public static string GetServSideValidErrorMsg(ModelStateDictionary modelState)
        {
            return string.Join(null, modelState.Where(i => i.Value.Errors.Count > 0)
                .SelectMany(i => i.Value.Errors)
                .Select(i => i.ErrorMessage.Trim()));
        }
    }
}