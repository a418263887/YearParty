using Cqwy.DatabaseAccessor.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Util.Ext;

namespace Util.Ext
{
    public static class ToolHelper
    {
        static long _currentTime;
        static long _current;
        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns></returns>
        public static string CreateOrderNumber()
        {
            var rq = DateTime.Now.ToString("yyyyMMdd");
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            var ct = Interlocked.Read(ref _currentTime);
            if (now > ct)
            {
                if (Interlocked.CompareExchange(ref _currentTime, now, ct) == ct)
                {
                    Interlocked.Exchange(ref _current, 0);
                }
            }
            var Increment = Convert.ToString(Interlocked.Increment(ref _current));
            string orderno = rq + now.ToString() + Increment.PadLeft(1, '0');
            return orderno;
        }
        /// <summary>
        /// 生成雪花编号 int64
        /// </summary>
        /// <returns></returns>
        public static long SnowflakeNumber(int Complement = 4)
        {
            //var rq = DateTime.Now.ToString("yyMMdd");
            var now = DateTimeOffset.Now.ToUnixTimeSeconds();
            var ct = Interlocked.Read(ref _currentTime);
            if (now > ct)
            {
                if (Interlocked.CompareExchange(ref _currentTime, now, ct) == ct)
                {
                    Interlocked.Exchange(ref _current, 0);
                }
            }
            var Increment = Convert.ToString(Interlocked.Increment(ref _current));
            string orderno = now.ToString() + Increment.PadLeft(Complement, '0');
            return Convert.ToInt64(orderno);
        }


        /// <summary>
        /// 处理实体中的null的string类型转为字符串Empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T ProssaEntityNullToEmpty<T>(T input)
        {
            Type type = input.GetType();

            // 获取 request 对象中的所有属性信息
            PropertyInfo[] properties = type.GetProperties();

            // 遍历属性列表，查找所有字符串类型的属性，并将其值转换为空白字符串
            foreach (PropertyInfo property in properties)
            {
                // 如果该属性不是字符串类型，则跳过
                if (property.PropertyType == typeof(string))
                {
                    // 获取属性值
                    string value = (string)property.GetValue(input);

                    // 将 null 值转换为空白字符串
                    if (value == null)
                    {
                        property.SetValue(input, "");
                    }
                }

            
            }
            return input;
        }
        /// <summary>
        /// 处理实体中的decimal的值  设置为0
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T ProssaEntityDecimalToZero<T>(T input)
        {
            Type type = input.GetType();

            // 获取 request 对象中的所有属性信息
            PropertyInfo[] properties = type.GetProperties();

            // 遍历属性列表，查找所有字符串类型的属性，并将其值转换为空白字符串
            foreach (PropertyInfo property in properties)
            {
                // 如果该属性不是字符串类型，则跳过
                if (property.PropertyType == typeof(System.Decimal) || property.PropertyType == typeof(System.Decimal?) || property.PropertyType == typeof(decimal) || property.PropertyType == typeof(decimal?))
                {
                    property.SetValue(input, 0M);
      
                }




            }
            return input;
        }


        /// <summary>
        /// 创建文件路径  没有就新增
        /// </summary>
        /// <param name="dir"></param>
        public static void CreateDir(string dir)
        {
            if (dir.Length == 0) return;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }
        /// <summary>
        /// 13位带-的票号处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static List<string> Achilles_LianPiaoHao(this string input)
        {
            List<string> numbers = new List<string>();

            if (input.Contains("-"))
            {
                string[] rangeParts = input.Split('-');
                if (rangeParts.Length == 2)
                {
                    string start = rangeParts[0];
                    string end = rangeParts[1];

                    long startIndex = long.Parse(start);
                    long endIndex = long.Parse(start.Achilles_CutLastOneChar(end.Length) + end);

                    for (long i = startIndex; i <= endIndex; i++)
                    {
                        var ph = i.ToString();
                        if (ph.Length < 13)
                        {
                            int a = ph.Length;
                            for (int j = a; j < 13; j++)
                            {
                                ph = "0" + ph;
                            }
                        }
                        numbers.Add(ph);
                    }
                }
                else
                {
                    numbers.Add(rangeParts[0]);
                }
            }
            else
            {
                numbers.Add(input);
            }

            return numbers;
        }

        public static List<string> Split2(this string data)
        {

            if (data != null && data is string)
            {
                var items = (data as string).Split(',', ' ', '，', '\t', '|', '/', '\\', '·', '、').ToList();
                return items;

            }
            else
            {
                return new List<string>();
            }
        }
        /// <summary>
        /// 票号解析 支持的格式
        /// 1234567890
        /// 1234567890-1
        /// 1234567890-91
        /// 1234567890-891
        /// 9991234567890
        /// 9991234567890-1
        /// 9991234567890-91
        /// 9991234567890-891
        /// 999-1234567890
        /// 999-1234567890-1
        /// 999-1234567890-91
        /// 999-1234567890-891
        /// 联票号也可以是反向的  如 999-1234567892-890 解析成 92 91 90三个票号    不支持替换的解析  因为替换和连续无法区分 替换即为 892和890两个票号  
        /// </summary>
        /// <param name="phStr"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public static List<string> PhParse(this string phStr, out string error)
        {

            error = string.Empty;
            var phArray = phStr.Split2();
            var phs = new List<string>();
            var errorPHList = new List<string>();
            foreach (var phItem in phArray)
            {

                var ph = phItem;


                //不足10位纯数字前面补0到10位
                if (ph.Length < 10 && ph.Match(@"^\d+$"))
                {
                    ph = ph.PadLeft(10, '0');
                }
                //12位纯数字前面补0到13位
                if (ph.Match(@"^\d{12}$") && ph.Match(@"^\d+$"))
                {
                    ph = "0" + ph;
                }
                //检查票号格式         
                if (ph.Match(@"^\d{10}$") || ph.Match(@"^\d{3}-\d{10}$"))     //单票号
                {
                    phs.Add(ph);
                    continue;
                }
                else if (ph.Match(@"^\d{13}$"))
                {
                    phs.Add(ph.Insert(3, "-"));
                    continue;
                }


                //统一将三字代码后面带-的票号的-去掉
                if (ph.Match(@"^\d{3}-\d{10}"))
                {
                    ph = ph.Remove(3, 1);
                }


                if (ph.Match(@"^\d{10}-\d$"))   //连票号 10位票号
                {
                    var phNum = ph.Substring(0, 10).ToBigInt();
                    var start = ph.Substring(9, 1).ToInt();
                    var end = ph.Substring(ph.Length - 1, 1).ToInt();
                    var count = end - start;
                    for (int i = 0; i < Math.Abs(count) + 1; i++)
                    {
                        phs.Add((phNum + (count > 0 ? i : -i)).ToString().PadLeft(10, '0'));
                    }
                    continue;
                }
                else if (ph.Match(@"^\d{13}-\d$")) //13位票号
                {
                    var phNum = ph.Substring(0, 13).ToBigInt();
                    var start = ph.Substring(12, 1).ToInt();
                    var end = ph.Substring(ph.Length - 1, 1).ToInt();
                    var count = end - start;
                    for (int i = 0; i < Math.Abs(count) + 1; i++)
                    {
                        phs.Add((phNum + (count > 0 ? i : -i)).ToString().PadLeft(13, '0').Insert(3, "-"));
                    }
                    continue;
                }
                else if (ph.Match(@"^\d{10}-\d{2}$"))   //两位连票号
                {
                    var phNum = ph.Substring(0, 10).ToBigInt();
                    var start = ph.Substring(8, 2).ToInt();
                    var end = ph.Substring(ph.Length - 2, 2).ToInt();
                    var count = end - start;

                    for (int i = 0; i < Math.Abs(count) + 1; i++)
                    {
                        phs.Add((phNum + (count > 0 ? i : -i)).ToString().PadLeft(10, '0'));
                    }
                    continue;
                }
                else if (ph.Match(@"^\d{13}-\d{2}$"))   //两位连票号
                {
                    var phNum = ph.Substring(0, 13).ToBigInt();
                    var start = ph.Substring(11, 2).ToInt();
                    var end = ph.Substring(ph.Length - 2, 2).ToInt();
                    var count = end - start;
                    for (int i = 0; i < Math.Abs(count) + 1; i++)
                    {
                        phs.Add((phNum + (count > 0 ? i : -i)).ToString().PadLeft(13, '0').Insert(3, "-"));
                    }
                    continue;
                }
                else if (ph.Match(@"^\d{10}-\d{3}$"))   //两位连票号
                {
                    var phNum = ph.Substring(0, 10).ToBigInt();
                    var start = ph.Substring(7, 3).ToInt();
                    var end = ph.Substring(ph.Length - 3, 3).ToInt();
                    var count = end - start;

                    for (int i = 0; i < Math.Abs(count) + 1; i++)
                    {
                        phs.Add((phNum + (count > 0 ? i : -i)).ToString().PadLeft(10, '0'));
                    }
                    continue;
                }
                else if (ph.Match(@"^\d{13}-\d{3}$"))   //两位连票号
                {
                    var phNum = ph.Substring(0, 13).ToBigInt();
                    var start = ph.Substring(10, 3).ToInt();
                    var end = ph.Substring(ph.Length - 3, 3).ToInt();
                    var count = end - start;
                    for (int i = 0; i < Math.Abs(count) + 1; i++)
                    {
                        phs.Add((phNum + (count > 0 ? i : -i)).ToString().PadLeft(13, '0').Insert(3, "-"));
                    }
                    continue;
                }
                else
                {
                    errorPHList.Add(ph);
                }

            }
            error = string.Join(",", errorPHList);

            return phs;

        }


        #region 先知框架Sql
        /// <summary>
        /// 懒人sql
        /// </summary>
        /// <typeparam name="T">查询的表实体</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="model">参数</param>
        /// <param name="cache">是否使用缓存</param>
        /// <returns></returns>
        public static List<T> AchillesSql<T, TDbContextLocator>(this string sql, object model, bool cache) where TDbContextLocator : class, Cqwy.DatabaseAccessor.IDbContextLocator
        {
            if (cache)
            {
                Type dbContextLocatorType = typeof(TDbContextLocator);
                string sqlHash = (sql + dbContextLocatorType.Name).GetMD5();
                if (CacheHelper.GetCache(sqlHash) != null)
                {
                    return (List<T>)CacheHelper.GetCache(sqlHash);
                }
                else
                {
                    List<T> result = sql.Change<TDbContextLocator>().SqlQuery<T>(model);
                    CacheHelper.SetCache(sqlHash, result, TimeSpan.FromSeconds(10));
                    return result;
                }
            }
            else
            {
                return sql.Change<TDbContextLocator>().SqlQuery<T>(model);

            }

            return default;
        }
        /// <summary>
        /// 懒人sql
        /// </summary>
        /// <typeparam name="T">查询的表实体</typeparam>
        /// <param name="sql">sql</param>
        /// <param name="cache">是否使用缓存</param>
        /// <param name="dbContext">数据库上下文定位器IDbContextLocator</param>
        /// <returns></returns>
        public static List<T> AchillesSql<T, TDbContextLocator>(this string sql, bool cache) where TDbContextLocator : class, Cqwy.DatabaseAccessor.IDbContextLocator
        {
            if (cache)
            {
                Type dbContextLocatorType = typeof(TDbContextLocator);
                string sqlHash = (sql + dbContextLocatorType.Name).GetMD5();
                if (CacheHelper.GetCache(sqlHash) != null)
                {
                    return (List<T>)CacheHelper.GetCache(sqlHash);
                }
                else
                {
                    List<T> result = sql.Change<TDbContextLocator>().SqlQuery<T>();
                    CacheHelper.SetCache(sqlHash, result, TimeSpan.FromSeconds(10));
                    return result;
                }
            }
            else
            {
                return sql.Change<TDbContextLocator>().SqlQuery<T>();

            }

            return default;
        }
        #endregion

        public static int[] Achilles_StringConvertIntArr(this string str)
        {
            if (str.isNotNull())
            {
                string[] strArray = str.Split(','); // 将字符串拆分成字符串数组
                int[] intArray = new int[strArray.Length]; // 存储整数的数组

                for (int i = 0; i < strArray.Length; i++)
                {
                    if (int.TryParse(strArray[i], out int num))
                    {
                        intArray[i] = num; // 将字符串转换为整数并存储到整数数组中
                    }
                    else
                    {
                        // 处理无效的字符串，如抛出异常或给定默认值
                    }
                }

                return intArray; // 输出转换后的整数数组
            }
            else { return null; }

        }


        /// <summary>
        /// Sha256加密
        /// </summary>
        /// <param name="message"></param>
        /// <param name="secret"></param>
        /// <returns></returns>
        public static string SHA256Hash(string message, string secret)
        {
            secret = secret ?? "";
            var encoding = new System.Text.UTF8Encoding();
            byte[] keyByte = encoding.GetBytes(secret);
            byte[] messageBytes = encoding.GetBytes(message);
            using var hmacsha256 = new HMACSHA256(keyByte);
            byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
            return BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
        }







        /// <summary>
        /// 把坑爹的 "/Date(-62135769600000+0800)/" 转换成DateTime格式
        /// </summary>
        /// <param name="jsonDate"></param>
        /// <returns></returns>
        public static DateTime Achilles_JsonToDateTime(this string jsonDate)
        {
            string value = jsonDate.Substring(6, jsonDate.Length - 8);
            if (value == "-62135769600000+0800" || value == "-62135769600000-0800")
            {
                return Convert.ToDateTime("1970-01-01");
            }
            DateTimeKind kind = DateTimeKind.Utc;
            int index = value.IndexOf('+', 1);
            if (index == -1)
            {
                index = value.IndexOf('-', 1);
            }
            if (index != -1)
            {
                kind = DateTimeKind.Local;
                value = value.Substring(0, index);
            }

            long javaScriptTicks = long.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
            long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            DateTime utcDateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
            DateTime dateTime;
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    break;
                case DateTimeKind.Local:
                    dateTime = utcDateTime.ToLocalTime();
                    break;
                default:
                    dateTime = utcDateTime;
                    break;
            }
            return dateTime;
        }

        /// <summary>
        /// 时间转时间戳   bflag为true时获取10位时间戳,为false获取13位时间戳 默认false
        /// </summary>
        /// <param name="dateTime">时间类型</param>
        /// <param name="bflag">为true时获取10位时间戳,为false获取13位时间戳.bool bflag = false</param>
        /// <returns></returns>
        public static string Achilles_ToTimestamp(this DateTime dateTime, bool bflag = false)
        {
            TimeSpan ts = dateTime.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            string ret = string.Empty;
            if (bflag)
                ret = Convert.ToInt64(ts.TotalSeconds).ToString();
            else
                ret = Convert.ToInt64(ts.TotalMilliseconds).ToString();

            return ret;
        }

        /// <summary>
        /// 不包含
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Achilles_NoContains(this string a, string b)
        {

            if (a != null && a != "")
            {
                var result = a.Contains(b);
                if (result)
                    return false;
                else
                    return true;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 扫线比价  多个产品转枚举描述显示处理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string Achilles_ScanLinePriceProdutEnum<T>(this string enumName)
        {
            if (enumName.isNull() || enumName.IsEmpty())
            {
                return "";
            }
            string[] arrenum = Regex.Split(enumName, ",", RegexOptions.IgnoreCase);
            string result = string.Empty;
            foreach (var item in arrenum)
            {
                System.Reflection.FieldInfo field = typeof(T).GetField(item);
                if (field != null)
                {
                    object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                    if (objs == null || objs.Length == 0)
                        result += item + ",";
                    else
                    {
                        System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                        result += da.Description + ",";
                    }
                }
                else { result += item + ","; }
            }

            return result.Achilles_CutLastOneChar();
        }

        /// <summary>
        /// 扫线比价  多个产品转枚举描述显示处理(如果政策类型为全部政策，直接返回全部产品)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="enumName"></param>
        /// <returns></returns>
        public static string Achilles_ScanLinePriceProdutEnum<T>(this string enumName, string PolicyType)
        {
            string result = string.Empty;
            if (PolicyType.isNotNull() && PolicyType == "ALL")
            {
                return "全部产品";
            }
            else
            {
                if (enumName.isNull() || enumName.IsEmpty())
                {
                    return "";
                }
                string[] arrenum = Regex.Split(enumName, ",", RegexOptions.IgnoreCase);

                foreach (var item in arrenum)
                {
                    System.Reflection.FieldInfo field = typeof(T).GetField(item);
                    if (field != null)
                    {
                        object[] objs = field.GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);
                        if (objs == null || objs.Length == 0)
                            result += item + ",";
                        else
                        {
                            System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)objs[0];
                            result += da.Description + ",";
                        }
                    }
                    else { result += item + ","; }
                }
            }

            return result.Achilles_CutLastOneChar();
        }

        /// <summary>
        /// 不等于0
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Achilles_IsNot0(this int a)
        {

            if (a != 0)
            {

                return true;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 去除最后n个字符
        /// </summary>
        /// <param name="z">默认1</param>
        /// <returns></returns>
        public static string Achilles_CutLastOneChar(this string z, int n = 1)
        {
            if (n < z.Length)
            {
                z = z.Remove(z.Length - n, n);
            }
            else
            {
                z = "";
            }

            return z;
        }
        /// <summary>
        /// 获取最后n个字符
        /// </summary>
        /// <param name="z">默认1</param>
        /// <returns></returns>
        public static string Achilles_GetLastOneChar(this string z, int n = 1)
        {
            if (n < z.Length)
            {
                z = z.Substring(z.Length - n, n);
            }

            return z;
        }
        /// <summary>
        /// 携程时间格式处理
        /// </summary>
        /// <param name="m"></param>
        /// <returns></returns>
        public static DateTime Achilles_ConvertJsonDateToDateString(this string m)
        {
            string result = string.Empty;
            m = m.Achilles_ToTimetempHandle();
            if (m.Contains("-62135596800000"))
            {
                return Convert.ToDateTime("1970-01-01 00:00:00");
            }
            else
            {
                DateTime dt = new DateTime(1970, 1, 1);
                dt = dt.AddMilliseconds(long.Parse(m));
                dt = dt.ToLocalTime();
                result = dt.ToString("yyyy-MM-dd HH:mm:ss");
                return Convert.ToDateTime(result);
            }
        }

        /// <summary>
        /// 删除json时间头部和尾部时区
        /// </summary>
        /// <param name="datestr"></param>
        /// <returns></returns>
        public static string Achilles_ToTimetempHandle(this string datestr)
        {
            string str = datestr.ToString().Trim().Replace("/Date(", "").Replace("0800)/", "").Replace("0900)/", "");
            return str;
        }
        /// <summary>
        /// 携程备注jsonn格式有点点问题 巨特么离谱
        /// </summary>
        /// <param name="jsonstr"></param>
        /// <returns></returns>
        public static string Achilles_ToIssueRemark(this string jsonstr)
        {
            return jsonstr.ToString().Trim().Replace(@"""[{", @"[{").Replace(@"}]"",", @"}],").Replace(@"}]另有儿童票"",", @"}],").Replace("\n", "").Replace(" ", "").Replace("\t", "").Replace("\r", "");


        }
        /// <summary>
        /// Gzip压缩
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Achilles_CompressString(this string str)

        {
            str = HttpUtility.UrlEncode(str);
            var compressBeforeByte = Encoding.GetEncoding("UTF-8").GetBytes(str);
            var compressAfterByte = Compress(compressBeforeByte);
            string compressString = Convert.ToBase64String(compressAfterByte);
            return compressString;
        }
        /// <summary>
        /// Gzip解压
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string Achilles_DecompressString(this string str)
        {
            var compressBeforeByte = Convert.FromBase64String(str);
            var compressAfterByte = Decompress(compressBeforeByte);
            string compressString = Encoding.GetEncoding("UTF-8").GetString(compressAfterByte);
            return HttpUtility.UrlDecode(compressString);
        }



        /// <summary>
        /// Compress
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] Compress(byte[] data)
        {
            try
            {
                var ms = new MemoryStream();
                var zip = new GZipStream(ms, CompressionMode.Compress, true);
                zip.Write(data, 0, data.Length);
                zip.Close();
                var buffer = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(buffer, 0, buffer.Length);
                ms.Close();
                return buffer;

            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        /// <summary>
        /// Decompress
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private static byte[] Decompress(byte[] data)
        {
            try
            {
                var ms = new MemoryStream(data);
                var zip = new GZipStream(ms, CompressionMode.Decompress, true);
                var msreader = new MemoryStream();
                var buffer = new byte[0x1000];
                while (true)
                {
                    var reader = zip.Read(buffer, 0, buffer.Length);
                    if (reader <= 0)
                    {
                        break;
                    }
                    msreader.Write(buffer, 0, reader);
                }
                zip.Close();
                ms.Close();
                msreader.Position = 0;
                buffer = msreader.ToArray();
                msreader.Close();
                return buffer;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
