using Newtonsoft.Json;

namespace DUOJU.FRAMEWORK
{
    public static class JsonHelper
    {
        /// <summary>
        /// 将Obj序列化成Json字符串
        /// </summary>
        /// <param name="obj">序列化数据</param>
        /// <param name="IgnoreNull">是否忽略Null值的字段</param>
        /// <returns></returns>
        private static string SerializeObject(object obj, bool IgnoreNull)
        {
            return JsonConvert.SerializeObject(
                obj,
                Formatting.Indented,
                new JsonSerializerSettings { NullValueHandling = IgnoreNull ? NullValueHandling.Ignore : NullValueHandling.Include }
            );
        }

        /// <summary>
        /// 将Json字符串反序列化成Obj
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">Json字符串</param>
        /// <returns></returns>
        private static T DeserializeObject<T>(string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }


        #region 序列化数据

        /// <summary>
        /// 获取Model的Json字符串
        /// </summary>
        /// <param name="model">序列化的Model</param>
        /// <param name="IgnoreNull">是否忽略Null值的字段</param>
        /// <returns></returns>
        public static string GetJsonWithModel(object model, bool IgnoreNull = true)
        {
            return SerializeObject(model, IgnoreNull);
        }

        #endregion


        #region 反序列化数据

        /// <summary>
        /// 获取Json字符串的Model
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json">Json字符串，不可为 null</param>
        /// <returns></returns>
        public static T GetModelWithJson<T>(string json)
        {
            if (json == null)
                return default(T);

            return DeserializeObject<T>(json);
        }

        #endregion
    }
}
