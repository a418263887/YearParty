
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Util.Ext
{
    public static partial class Ext
    {


        public static string GetMD5(this string input) {

            string md5 = "";
            MD5 creater = MD5.Create();
            byte[] bytes = creater.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (var item in bytes)
            {
                md5 += item.ToString("x2");
            }
            return md5;
        }


        /// <summary>
        /// 分割字符串转换为in查询条件 分隔符包含 , / | ，四种
        /// </summary>
        /// <param name="input">字符串</param>
        /// <param name="filed">条件字段</param>
        /// <returns></returns>
        public static string ToInCondition(this string input, string filed)
        {

            string result = "";
            if (input.isNull() || input == "全部")
            {
                return result;
            }
            //订单类型 出 退 改
            var conArray = input.Split(',', '，', '/').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (conArray.Length > 0)
            {

                result += $" and  {filed} in ('" + string.Join("','", conArray) + "')";
            }
            return result;
        }
        /// <summary>
        /// 分割字符串转换为not in查询条件 分隔符包含 , / | ，四种
        /// </summary>
        /// <param name="input"></param>
        /// <param name="filed"></param>
        /// <returns></returns>
        public static string ToNotInCondition(this string input, string filed)
        {

            string result = "";
            if (input.isNull() || input == "全部")
            {
                return result;
            }
            //订单类型 出 退 改
            var conArray = input.Split(',', '，').Where(x => !string.IsNullOrEmpty(x)).ToArray();
            if (conArray.Length > 0)
            {

                result += $" and  {filed} not  in ('" + string.Join("','", conArray) + "')";
            }
            return result;
        }

        /// <summary>
        /// GZip解压
        /// </summary>
        /// <param name=“str”></param>
        /// <returns></returns>
        public static string DeCompress(this string str)
        {
            var array = Convert.FromBase64String(str);
            MemoryStream srcMs = new MemoryStream(array);
            GZipStream zipStream = new GZipStream(srcMs, CompressionMode.Decompress);
            MemoryStream ms = new MemoryStream();
            byte[] bytes = new byte[40960];
            int n;
            while ((n = zipStream.Read(bytes, 0, bytes.Length)) > 0)
            {
                ms.Write(bytes, 0, n);
            }
            zipStream.Close();
            return Encoding.UTF8.GetString(ms.ToArray());
        }


        /// <summary>
        /// 拼接为字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list">集合</param>
        /// <param name="division">分隔符</param>
        /// <returns></returns>
        public static string ToStr<T>(this IEnumerable<T> list, string division=",") {


            if (list == null || list.Count() == 0)
                return "";

            return string.Join(division,list);
        }


        public static string SubLen(this string str, int len)
        {



            return (str != null && str.Length > len) ? str.Substring(0, len) : str;
        }


        public static string Replace2(this string str,string strMatch,string strRep="")
        {



            return str == null ? null:str.Replace(strMatch, strRep).Trim();
        }

        /// <summary>
        /// 拼接为字符串
        /// </summary>
        /// <param name="list">集合</param>
        /// <param name="division">分隔符</param>
        /// <returns></returns>
        public static string ToStr(this IList<string> list, string division = ",")
        {


            if (list == null || list.Count() == 0)
                return "";
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var item in list)
            {
                if (item.isNull())
                    continue;
                stringBuilder.Append(division);
                stringBuilder.Append(item);
            }

            return stringBuilder.ToString().Substring(1);
        }



        /// <summary>
        /// 对象转url参数形式 不支持复杂类型
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string ModelToUriParam(this object obj, string url = "")
        {
            PropertyInfo[] propertis = obj.GetType().GetProperties();
            StringBuilder sb = new StringBuilder();
            if (url.isNotNull()) {
                sb.Append(url);
                sb.Append("?");
            }
            
            foreach (var p in propertis)
            {
                var v = p.GetValue(obj, null);
                if (v == null)
                    continue;
                sb.Append(p.Name);
                sb.Append("=");
                if (v is string)
                {
                    sb.Append(HttpUtility.UrlEncode(v.ToString()));
                }
                else {
                    sb.Append(HttpUtility.UrlEncode(v.ToJson()));
                }
               
                sb.Append("&");
            }
            sb.Remove(sb.Length - 1, 1);

            return sb.ToString();
        }
            public static List<string> SplitByLen(this string str,int len) {

            List<string> result = new List<string>();
            if (str.isNotNull()&&str.Length % len == 0) {

                int v = str.Length / len;
                for (int i = 0; i < v; i++)
                {
                    result.Add(str.Substring(i*len,len));
                }
            }
            return result;
        }
       
        public static bool DateStringLtEq(this string str,string str2) {

           return str.ToDate() <= str2.ToDate();
        }
        public static bool DateStringLt(this string str, DateTime str2)
        {

            return str.ToDate() <= str2;
        }
        public static bool DateStringGt(this string str, string str2)
        {

            return str.ToDate() >str2.ToDate();
        }
        public static bool DateStringGt(this string str, DateTime str2)
        {

            return str.ToDate() > str2;
        }

        public static bool DateStringGtEq(this string str, string str2)
        {

            return str.ToDate() >= str2.ToDate();
        }
        public static bool DateStringGtEq(this string str, DateTime str2)
        {

            return str.ToDate() >=str2;
        }

        public static List<T> GetPage<T>(this List<T> data, int pageIndex, int pageSize)
        {           
          
            var pageData=  data.Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            return pageData;
        }
        public static int GetPageCount(this int total,int pageSize) {
            var allPage = (int)Math.Ceiling(total * 1.0 / pageSize);
            return allPage;
        }
        public static T ClearNull<T>(this T data) where T : class
        {

            if (data == null)
                return null;

            Type type = typeof(T);
            foreach (var item in type.GetProperties())
            {
                var typeinfo = type.GetProperty(item.Name);
                var value = typeinfo.GetValue(data);
                if (typeinfo.PropertyType == typeof(string))
                {

                    if (value == null)
                    {
                        typeinfo.SetValue(data, "", null);
                    }
                    else
                    {

                        var tempValue = value as string;
                        tempValue = tempValue.Replace('\t', ' ').Replace('\r', ' ').Replace('\n', ' ');
                        tempValue = tempValue.Trim();

                        typeinfo.SetValue(data, tempValue, null);
                    }
                }
            }
            return data;

        }
        public static bool IsNum(this object data)
        {
            if (data == null)
                return false;
            double result;
            return double.TryParse(data.ToString(), out result);

        }

        public static bool isNull(this object data) {

            if (data is string)
            {
                return string.IsNullOrEmpty(data as string) || string.IsNullOrWhiteSpace(data as string);
            }
            else {

                return data == null;
            }

        }
        public static bool isNotNull(this object data)
        {

           return !data.isNull();

        }
        public static bool NotNull(this object data)
        {
            return !data.isNull();
        }
        public static bool IsNull(this object data)
        {
            return data.isNull();
        }
        public static bool Match(this string data, string regstr)
        {

            if (data != null && data is string)
            {
               Regex reg = new Regex(regstr);
                var result = reg.IsMatch(data);
                return result;
            }
            else
            {
                return false;
            }
        }

        public static bool IsDate(this string data) {

            return data.Match(@"^\d{4}-\d{2}-\d{2}(\s\d{2}:\d{2}:\d{2})?$");
        }
        public static bool IsEmail(this string data)
        {

            return data.Match(@"^[a-zA-Z0-9_.-]+@[a-zA-Z0-9-]+(\.[a-zA-Z0-9-]+)*\.(com|cn|net)$");
        }
        public static bool IsPhone(this string data)
        {

            return data.Match(@"^1[3-9][0-9]{9}$");
        }
        /// <summary>
        /// 10/25/72 - 1972-10-25
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string FormationDate(this string data)
        {
            var str = "";
            if (data.isNotNull())
            {
                var arr = data.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (arr.Length == 3)
                {
                    str = "19" + arr[2] + "-" + arr[1].PadLeft(2, '0') + "-" + arr[0].PadLeft(2, '0');
                }
            }
            return str;
        }
        public static bool Match(this string data, string regstr,bool multiline=false)
        {

            if (data != null && data is string)
            {
                Regex reg = new Regex(regstr);
                if (multiline) {
                    reg = new Regex(regstr, RegexOptions.Multiline);
                }
                var result = reg.IsMatch(data);
                return result;
            }
            else
            {
                return false;
            }
        }

        public static string MatchValue(this string data, string regstr,string groupName)
        {

            if (data != null && data is string)
            {
                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(regstr);
                var match = reg.Match(data);
                if (match.Success)
                {
                    return match.Groups[groupName].Value;

                }
                else {
                    return null;
                }
               
                
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 字符串Contains 解决null值错误
        /// </summary>
        /// <param name="data"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Contains2(this string data, string input)
        {

            if (data.NotNull())
            {
                var result = data.Contains(input);
                return result;
            }
            else
            {
                return false;
            }
        }
        public static bool Contains2(this string[] data, string input)
        {

            if (data != null && input != null)
            {
                var result = data.Contains(input);
                return result;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 按常见分割符分割字符串  检查是否含义子字符串
        /// </summary>
        /// <param name="data"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool ItemCheck(this object data, string input)
        {

            if (data != null && data is string)
            {
                var items = (data as string).Split(',', ' ', '，', '\t', '|').ToList();
                return items.Contains(input);

            }
            else
            {
                return false;
            }
        }
        public static bool ItemContains(this object data, string input)
        {

            if (data != null && data is string)
            {
                var items = (data as string).Split( '/').ToList();
                return items.Contains(input);

            }
            else
            {
                return false;
            }
        }

    
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="data">数据</param>
        public static string toLenStr(this int data, int len = 4)
        {

            string result = data.ToString();
            if (data > 0)
            {
                while (result.Length < len)
                {
                    result = "0" + result;
                }

            }
            return result;

        }
    
        public static string StringCut(this object data, int len = 50)
        {


            if (data is string)
            {
                if (data == null)
                {

                    return null;
                }
                else
                {
                    var temp = data as string;
                    if (temp.Length > len && len > 0)
                    {
                        return temp.Substring(0, len) + "……";
                    }
                    else
                    {

                        return temp;
                    }

                }
            }
            return null;



        }

        public static string ReplaceReg(this string str,string reg,string rep) {

            if (str.isNull())
                return str;
            Regex regex = new Regex(reg);
           return regex.Replace(str, rep);
        }
        public static string TrimToEmpty(this string str) {
            if (str == null)
                return "";
            return str.Trim();
        }


    }
}