
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Util.Ext;

namespace System
{
    public static class JsonHelper
    {
        //public static object ToJson(this string Json)
        //{
        //    return Json == null ? null : JsonConvert.DeserializeObject(Json);
        //}
        public static string ToJson(this object obj, bool time = true, bool serNull = true)
        {
        
            if (obj == null)
                return null;
            if (obj is string)
                return obj as string;

            JsonSerializerSettings set = new JsonSerializerSettings();

            string format = time? "yyyy-MM-dd HH:mm:ss": "yyyy-MM-dd";   
            set.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = format });
            if (!serNull) {
                set.NullValueHandling = NullValueHandling.Ignore;
            }
            return JsonConvert.SerializeObject(obj, set);

        }

        public static string ToJsonNoChild(this object obj, bool time = true, bool serNull = true)
        {
            if (obj == null)
                return null;
            if (obj is string)
                return obj as string;

            JsonSerializerSettings set = new JsonSerializerSettings();
            set.ReferenceLoopHandling= ReferenceLoopHandling.Ignore;
            string format = time ? "yyyy-MM-dd HH:mm:ss" : "yyyy-MM-dd";
            set.Converters.Add(new IsoDateTimeConverter { DateTimeFormat = format });
            if (!serNull)
            {
                set.NullValueHandling = NullValueHandling.Ignore;
            }
            return JsonConvert.SerializeObject(obj, set);
        }
    
        public static string ToJsonNoSec(this object obj)
        {
            string format = "yyyy-MM-dd HH:mm";
            if (obj == null)
                return null;
            if (obj is string)
                return obj as string;
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = format };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }


        /// <summary>
        /// 转json后进行gzip压缩 返回的是base64的字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonGzip(this object obj)
        {
            if (obj == null)
                return null;
            if (obj is string)
                return obj as string;
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" };
            return WebUtil.GZipCompress(JsonConvert.SerializeObject(obj, timeConverter)); ;
        }

        public static T ConvertByJson<T>(this object obj) where T : class
        {

            return obj.ToJson().JsonToObject<T>();
        }

        public static string ToJsonBase64(this object obj, bool time = true, bool serNull = false)
        {
            
            byte[] bytes = Encoding.UTF8.GetBytes(obj.ToJson(time, serNull));
            return Convert.ToBase64String(bytes); ;
        }

        public static T JsonBase64ToObject<T>(this string Json) where T : class
        {
            if (typeof(T) == typeof(string))
                return Json as T;
            if (Json == null || Json == "")
            {
                return default(T);
            }

            byte[] outputb = Convert.FromBase64String(Json);
            string orgStr = Encoding.UTF8.GetString(outputb);

            return JsonConvert.DeserializeObject<T>(orgStr);

        }
       

        public static string ToJson(this object obj, string datetimeformats)
        {
            var timeConverter = new IsoDateTimeConverter { DateTimeFormat = datetimeformats };
            return JsonConvert.SerializeObject(obj, timeConverter);
        }
        public static T JsonToObject<T>(this string Json) where T : class
        {
            if (typeof(T) == typeof(string))
                return Json as T;
            if (Json == null||Json=="")
            {
                return default(T);
            }
            if (!Json.Contains("{") && !Json.Contains("["))
                throw new Exception("错误的json:"+Json);
            return JsonConvert.DeserializeObject<T>(Json);

        }
        public static T ToObject<T>(this string Json) where T : class
        {
            if (typeof(T) == typeof(string))
                return Json as T;
            if (Json == null) {
                return default(T);
            }
            if (Json.Contains("{")|| Json.Contains("["))
            {
                //之前做了gzip压缩 做个兼容（压缩性能太差 取消了）  压缩是转了base64的所有不会以{开头   临时设置后期删除
                return JsonConvert.DeserializeObject<T>(Json);
            }
            else {
                var temp = WebUtil.GZipDecompress(Json);
                return JsonConvert.DeserializeObject<T>(temp);
            }

           
        }
        /// <summary>
        /// gzip压缩的json字符串转对象  只能对base64压缩的json字符串操作
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Json"></param>
        /// <returns></returns>
        public static T ToObjectGzip<T>(this string Json) where T : class
        {
            if (Json.isNull())
                return default(T);
            if (typeof(T) == typeof(string))
                return Json as T;
            else
            {
                var temp = WebUtil.GZipDecompress(Json);
                return JsonConvert.DeserializeObject<T>(temp);
            }

        }

        public static string JsonDeCompress(this string Json)
        {
            if (Json.isNull())
                return null;          
            else
            {
                var temp = WebUtil.GZipDecompress(Json);
                return temp;
            }

        }

        public static List<T> ToList<T>(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<List<T>>(Json);
        }
        public static DataTable ToTable(this string Json)
        {
            return Json == null ? null : JsonConvert.DeserializeObject<DataTable>(Json);
        }
        public static JObject ToJObject(this string Json)
        {
            return Json == null ? JObject.Parse("{}") : JObject.Parse(Json.Replace("&nbsp;", ""));
        }
    }
}
