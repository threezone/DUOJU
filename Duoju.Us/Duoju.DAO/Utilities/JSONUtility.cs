using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;

namespace Duoju.DAO.Utilities
{
    public static class JSONUtility
    {
        public static string ToJson(object obj, bool indent, bool bareTableConvertor = false)
        {
            Type type = obj.GetType();

            JsonSerializer json = new JsonSerializer();
            json.NullValueHandling = NullValueHandling.Ignore;
            json.ObjectCreationHandling = ObjectCreationHandling.Replace;
            json.MissingMemberHandling = MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            if (type == typeof(DataRow))
                json.Converters.Add(new DataRowConverter());
            else if (type == typeof(DataTable))
                json.Converters.Add(new DataTableConverter { OnlyRowArray = bareTableConvertor });
            else if (type == typeof(DataSet))
                json.Converters.Add(new DataSetConverter());

            StringWriter sw = new StringWriter();
            JsonTextWriter writer = new JsonTextWriter(sw);
            if (indent)
                writer.Formatting = Formatting.Indented;
            else
                writer.Formatting = Formatting.None;

            writer.QuoteChar = '"';
            json.Serialize(writer, obj);

            string jsonResult = sw.ToString();
            writer.Close();
            sw.Close();

            string p = @"\\/Date\((-?\d+)\+\d+\)\\/";
            string p2 = @"(\d{4}-\d{1,2}-\d{1,2})(\S)(\d{1,2}:\d{1,2}:\d{1,2})";
            MatchEvaluator matchEvaluator = new MatchEvaluator(ConvertJsonDateToDateString);
            Regex reg = new Regex(p);
            jsonResult = reg.Replace(jsonResult, matchEvaluator);

            MatchCollection mc = Regex.Matches(jsonResult, p2);
            foreach (Match m in mc)
            {
                string old = m.Groups[1].Value + m.Groups[2].Value + m.Groups[3].Value;
                string replacement = m.Groups[1].Value + ' ' + m.Groups[3].Value;
                jsonResult = Regex.Replace(jsonResult, old, replacement);
            }
            return jsonResult.Replace(@"\/", "/"); ;
        }

        public static T FromJsonTo<T>(string jsonString)
        {
            JsonSerializer json = new JsonSerializer();

            json.NullValueHandling = NullValueHandling.Ignore;
            json.ObjectCreationHandling = ObjectCreationHandling.Replace;
            json.MissingMemberHandling = MissingMemberHandling.Ignore;
            json.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

            StringReader sr = new StringReader(jsonString);
            JsonTextReader reader = new JsonTextReader(sr);
            T result = json.Deserialize<T>(reader);
            return result;
        }

        private static string ConvertJsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("yyyy-MM-dd HH:mm:ss");
            return result;
        }
    }

    /// <summary>
    /// 定义DataRowConverter,继承JsonConverter,重写WriteJson的方法
    /// 使用反射机制来读DataRow的键值
    /// </summary>
    public class DataRowConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object dataRow, JsonSerializer serializer)
        {
            DataRow row = dataRow as DataRow;
            writer.WriteStartObject();
            foreach (DataColumn column in row.Table.Columns)
            {
                writer.WritePropertyName(column.ColumnName);
                serializer.Serialize(writer, row[column]);
            }
            writer.WriteEndObject();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataRow).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 定义DataTableConverter,继承JsonConverter,重写WriteJson的方法
    /// 使用反射机制来读DataTable的键值
    /// </summary>
    public class DataTableConverter : JsonConverter
    {
        public bool OnlyRowArray { get; set; }

        public override void WriteJson(JsonWriter writer, object dataTable, JsonSerializer serializer)
        {
            DataTable table = dataTable as DataTable;
            DataRowConverter converter = new DataRowConverter();

            if (!this.OnlyRowArray)
            {
                writer.WriteStartObject();
                writer.WritePropertyName(table.TableName);
            }
            writer.WriteStartArray();
            foreach (DataRow row in table.Rows)
            {
                converter.WriteJson(writer, row, serializer);
            }
            writer.WriteEndArray();
            if (!this.OnlyRowArray)
                writer.WriteEndObject();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataTable).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 定义DataSetConverter,继承JsonConverter,重写WriteJson的方法
    /// 使用反射机制来读DataSet的键值
    /// </summary>
    public class DataSetConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object dataSet, JsonSerializer serializer)
        {
            DataSet ds = dataSet as DataSet;

            DataTableConverter converter = new DataTableConverter();
            writer.WriteStartObject();
            writer.WritePropertyName("Tables");
            writer.WriteStartArray();

            foreach (DataTable table in ds.Tables)
            {
                converter.WriteJson(writer, table, serializer);
            }
            writer.WriteEndArray();
            writer.WriteEndObject();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(DataSet).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
