using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Xml;

using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using Serilog;
using System.Reflection;
using System.Configuration;
using Newtonsoft.Json.Converters;
//using System.DrawingCore;
//using System.DrawingCore.Imaging;
using Microsoft.AspNetCore.Http;
using System.Web;
using Cqwy;
using Cqwy.Logging;
using Microsoft.Extensions.Logging;
using Util.Ext;
using System.Runtime.Serialization.Formatters.Binary;

namespace System
{
    public static class WebUtil
    {

        public enum HttpMethod { GET, POST }

        /// <summary>
        /// 同步锁
        /// </summary>
        private static object objLock = new object();
        public static string BrowserRequest(string url, string data, string metohd = "get", CookieContainer cookie = null, string contextType = "text/plain")
        {
           
            Serilog.Log.Debug("BrowserRequest url: " + url);
            Serilog.Log.Debug("BrowserRequest data: " + data);

            var strResult = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Timeout = 3 * 60 * 1000;
            request.Method = metohd;
            request.ContentType = contextType;
            //request.Referer = "http://139.9.239.128:8150/asms/login";
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/77.0.3865.75 Safari/537.36";
            //request.Proxy = new WebProxy("127.0.0.1",8888);
            if (cookie != null)
            {
                request.CookieContainer = cookie;
            }
            if (data.isNotNull())
            {

                byte[] bytes = Encoding.UTF8.GetBytes(data);
                using (Stream reqstream = request.GetRequestStream())
                {
                    reqstream.Write(bytes, 0, bytes.Length);
                }
            }
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {

                Stream streamReceive = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                strResult = streamReader.ReadToEnd();
            }
            request.Abort();

            Serilog.Log.Debug("BrowserRequest res: " + strResult);

            return strResult;
        }
        public static string HttpPost2(string url, string Data, string mineType = "application/json", bool isCompress = false, int timeout = 30000, Dictionary<string, string> headers = null, string proxy = "")
        {
            string strResult = string.Empty;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeout;
                request.Method = "POST";
                request.KeepAlive = true;
                request.ContentType = mineType;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";
                if (isCompress)
                {
                    request.Headers.Add("Accept-Encoding", "gzip,deflate");
                }
                if (headers != null)
                {

                    foreach (var item in headers.Keys)
                    {
                        request.Headers.Add(item, headers[item]);
                    }
                }

                if (proxy.isNotNull())
                {
                    request.Proxy = new WebProxy(new Uri(proxy));
                }

                using (Stream reqstream = request.GetRequestStream())
                {
                    reqstream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse response;
                var statusError = "";
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    statusError = ex.Message;
                    response = (HttpWebResponse)ex.Response;
                }
                if (response != null)
                {
                    Stream streamReceive = response.GetResponseStream();
                    var compressd = response.ContentEncoding.ToLower().Contains("gzip");
                    if (compressd)
                    {
                        streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);
                    }
                    StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                    strResult = streamReader.ReadToEnd();
                    streamReceive.Dispose();
                    request.Abort();
                }

                if (statusError.isNotNull())
                {
                    strResult = $"error weberror:{statusError} {strResult} {url}";
                }
            }
            catch (TimeoutException ex)
            {
                strResult = "error httptimeout:" + ex.Message + url;

            }
            catch (Exception ex)
            {
                strResult = "error httperror:" + ex.Message + url;
            }
            return strResult;
        }
        public static string HttpPost(string url, string Data, string mineType = "application/json", bool isCompress = false, int timeout = 30000, Dictionary<string, string> headers = null, string proxy = "")
        {
            string strResult = string.Empty;
            try
            {
                byte[] bytes = Encoding.UTF8.GetBytes(Data);
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeout;
                request.Method = "POST";
                request.KeepAlive = true;
                request.ContentType = mineType;
                request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/79.0.3945.88 Safari/537.36";
                if (isCompress)
                {
                    request.Headers.Add("Accept-Encoding", "gzip,deflate");
                }
                if (headers != null)
                {

                    foreach (var item in headers.Keys)
                    {
                        request.Headers.Add(item, headers[item]);
                    }
                }

                if (proxy.isNotNull())
                {
                    request.Proxy = new WebProxy(new Uri(proxy));
                }

                using (Stream reqstream = request.GetRequestStream())
                {
                    reqstream.Write(bytes, 0, bytes.Length);
                }
                HttpWebResponse response;
                var statusError = "";
                try
                {
                    response = (HttpWebResponse)request.GetResponse();
                }
                catch (WebException ex)
                {
                    statusError = ex.Message;
                    response = (HttpWebResponse)ex.Response;
                }
                if (response != null)
                {
                    Stream streamReceive = response.GetResponseStream();
                    var compressd = response.ContentEncoding.ToLower().Contains("gzip");
                    if (compressd)
                    {
                        streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);
                    }
                    StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                    strResult = streamReader.ReadToEnd();
                    streamReceive.Dispose();
                    request.Abort();
                }

                if (statusError.isNotNull())
                {
                    strResult = $"error weberror:{statusError} {strResult} {url}";
                }
            }
            catch (TimeoutException ex)
            {
                strResult = "error httptimeout:" + ex.Message + url;

            }
            catch (Exception ex)
            {
                strResult = "error httperror:" + ex.Message + url;
            }
            return strResult;
        }

        public static string HttpGet2(string url, Dictionary<string, string> headers, string mineType = "application/json",
            bool isCompress = false, int timeout = 30000, string proxy = "")
        {

            string strResult = string.Empty;
            try
            {

                WebClient client = new WebClient();

                client.Encoding = Encoding.UTF8;
                if (proxy.isNotNull())
                {
                    client.Proxy = new WebProxy(new Uri(proxy));
                }
                if (headers != null)
                {
                    foreach (var item in headers.Keys)
                    {
                        client.Headers.Add(item, headers[item]);
                    }
                }
                if (isCompress)
                {
                    client.Headers.Add("Accept-Encoding", "gzip,deflate");
                }
                strResult = client.DownloadString(url);
            }
            catch (TimeoutException ex)
            {
                strResult = "error httptimeout:" + ex.Message;

            }
            catch (Exception ex)
            {
                strResult = "error httperror:" + ex.Message;
            }
            return strResult;



        }

        public static string HttpGet(string url, Dictionary<string, string> headers = null, string mineType = "application/json",
        bool isCompress = false, int timeout = 30000, string proxy = "")
        {

            string strResult = string.Empty;
            try
            {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Timeout = timeout;
                request.Method = "GET";
                request.KeepAlive = true;
                request.ContentType = mineType;
                if (isCompress)
                {
                    request.Headers.Add("Accept-Encoding", "gzip,deflate");
                }
                if (headers != null)
                {

                    foreach (var item in headers.Keys)
                    {
                        request.Headers.Add(item, headers[item]);
                    }
                }

                if (proxy.isNotNull())
                {
                    request.Proxy = new WebProxy(new Uri(proxy));
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream streamReceive = response.GetResponseStream();
                    var compressd = response.ContentEncoding.ToLower().Contains("gzip");
                    if (compressd)
                    {
                        streamReceive = new GZipStream(streamReceive, CompressionMode.Decompress);
                    }
                    StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                    strResult = streamReader.ReadToEnd();
                }
                request.Abort();
            }
            catch (TimeoutException ex)
            {
                strResult = "error httptimeout:" + ex.Message + url;

            }
            catch (Exception ex)
            {
                strResult = "error httperror:" + ex.Message + url;
            }
            return strResult;


        }



        public static string ToFormParameter(object o)
        {
            string urlParam = "";
            Type type = o.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (PropertyInfo item in properties)
            {
                PropertyInfo typeinfo = type.GetProperty(item.Name);
                object value = typeinfo.GetValue(o, null);
                if (value != null)
                {
                    urlParam = urlParam + item.Name + "=" + value.ToString() + "&";
                }
            }
            return urlParam.Trim('&');
        }

        public static string PostForm(string url, string data, int timeout = 60)
        {
            string strResult = string.Empty;
            GC.Collect();
            HttpWebRequest request = null;
            HttpWebResponse response = null;
            try
            {
                ServicePointManager.DefaultConnectionLimit = 500;
                Stream reqStream2 = null;
                request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.Timeout = timeout * 1000;
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("SOAPAction", "");
                byte[] dataArray = Encoding.UTF8.GetBytes(data);
                request.ContentLength = data.Length;
                reqStream2 = request.GetRequestStream();
                reqStream2.Write(dataArray, 0, data.Length);
                reqStream2.Close();
                response = (HttpWebResponse)request.GetResponse();
                Stream streamReceive = response.GetResponseStream();
                StreamReader streamReader = new StreamReader(streamReceive, Encoding.UTF8);
                StreamReader sr = new StreamReader(streamReceive, Encoding.UTF8);
                strResult = sr.ReadToEnd().Trim();
                sr.Close();
                return strResult;
            }
            catch (Exception ex)
            {
                return "error httperror: " + ex.Message;
            }
            finally
            {
                response?.Close();
                request?.Abort();
            }
        }
        public static string GetShortMD5(string input)
        {
            var result = GetMD5(input);
            return result.Substring(8, 16);
        }

        /// <summary>
        /// json序列化
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static string ToJson(object o, bool serNull = true)
        {
            if (o == null)
                return null;

            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            if (serNull)
            {

                return JsonConvert.SerializeObject(o, timeConverter);

            }
            else
            {

                JsonSerializerSettings set = new JsonSerializerSettings();
                set.NullValueHandling = NullValueHandling.Ignore;
                set.Converters.Add(timeConverter);
                return JsonConvert.SerializeObject(o, set);

            }


        }

        /// <summary>
        /// json返序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(string json) where T : class
        {
            try
            {
                if (json.isNull())
                    return default(T);
                JsonSerializer serializer = new JsonSerializer();
                StringReader sr = new StringReader(json);
                return serializer.Deserialize<T>(new JsonTextReader(sr));
            }
            catch (Exception ex)
            {
                throw new Exception("error json" + ex.Message + " type:" + typeof(T).FullName + " string:" + json);

            }
        }


        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="xml">XML字符串</param>
        /// <returns></returns>
        public static T FromXml<T>(string xml) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(xml))
                {
                    XmlSerializer xmldes = new XmlSerializer(typeof(T));
                    return xmldes.Deserialize(sr) as T;
                }
            }
            catch
            {
                throw;
            }
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static string ToXml(object obj)
        {
            MemoryStream Stream = new MemoryStream();
            XmlSerializer xml = new XmlSerializer(obj.GetType());
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");//把命名空间设置为空，这样就没有命名空间了
            try
            {
                //序列化对象
                xml.Serialize(Stream, obj, ns);
            }
            catch (InvalidOperationException ex)
            {
                string xx = ex.Message;
                throw new Exception("xml序列化错误");
            }
            Stream.Position = 0;
            StreamReader sr = new StreamReader(Stream);
            string str = sr.ReadToEnd();
            sr.Dispose();
            Stream.Dispose();
            return str.Replace("<?xml version=\"1.0\"?>", "");
        }

        public static string ToXml(object obj, bool head)
        {
            string str = string.Empty;
            XmlSerializer serializer = new XmlSerializer(obj.GetType());

            // 将对象序列化输出到文件  
            MemoryStream stream = new MemoryStream();

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "    ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = Encoding.UTF8;
            settings.OmitXmlDeclaration = true;  // 不生成声明头  
            using (XmlWriter xmlWriter = XmlWriter.Create(stream, settings))
            {
                // 强制指定命名空间，覆盖默认的命名空间  
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add(string.Empty, string.Empty);
                serializer.Serialize(xmlWriter, obj, namespaces);
                xmlWriter.Close();
            };
            StreamReader sr = new StreamReader(stream);
            str = sr.ReadToEnd();
            stream.Close();
            return str;
        }




        /// <summary>
        /// 时间戳转为C#格式时间
        /// </summary>
        /// <param name="timeStamp"></param>
        /// <returns></returns> 
        public static DateTime StampToDateTime(string timeStamp)
        {
            DateTime dateTimeStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timeStamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            return dateTimeStart.Add(toNow);
        }

        /// <summary>
        /// DateTime时间格式转换为Unix时间戳格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static int DateTimeToStamp(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }


        /// <summary>  
        /// 初始化向量  
        /// </summary>  
        public static char[] IV
        {
            get { return new char[16]; }

        }

        public static byte[] AESEncrypt(byte[] Data, string Key, string Vector)
        {
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);
            Byte[] Cryptograph = null; // 加密后的密文  
            Rijndael Aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流  
                using (MemoryStream Memory = new MemoryStream())
                {
                    // 把内存流对象包装成加密流对象  
                    using (CryptoStream Encryptor = new CryptoStream(Memory,
                     Aes.CreateEncryptor(bKey, bVector),
                     CryptoStreamMode.Write))
                    {
                        // 明文数据写入加密流  
                        Encryptor.Write(Data, 0, Data.Length);
                        Encryptor.FlushFinalBlock();

                        Cryptograph = Memory.ToArray();
                    }
                }
            }
            catch
            {
                Cryptograph = null;
            }
            return Cryptograph;
        }

        public static byte[] AESDecrypt(byte[] Data, string Key, string Vector)
        {
            Byte[] bKey = new Byte[32];
            Array.Copy(Encoding.UTF8.GetBytes(Key.PadRight(bKey.Length)), bKey, bKey.Length);
            Byte[] bVector = new Byte[16];
            Array.Copy(Encoding.UTF8.GetBytes(Vector.PadRight(bVector.Length)), bVector, bVector.Length);

            Byte[] original = null; // 解密后的明文  

            Rijndael Aes = Rijndael.Create();
            try
            {
                // 开辟一块内存流，存储密文  
                using (MemoryStream Memory = new MemoryStream(Data))
                {
                    // 把内存流对象包装成加密流对象  
                    using (CryptoStream Decryptor = new CryptoStream(Memory,
                    Aes.CreateDecryptor(bKey, bVector),
                    CryptoStreamMode.Read))
                    {
                        // 明文存储区  
                        using (MemoryStream originalMemory = new MemoryStream())
                        {
                            Byte[] Buffer = new Byte[1024];
                            Int32 readBytes = 0;
                            while ((readBytes = Decryptor.Read(Buffer, 0, Buffer.Length)) > 0)
                            {
                                originalMemory.Write(Buffer, 0, readBytes);
                            }

                            original = originalMemory.ToArray();
                        }
                    }
                }
            }
            catch
            {
                original = null;
            }
            return original;
        }

        /// <summary>  
        /// AES加密  
        /// </summary>  
        /// <param name="plainStr">明文字符串</param>  
        /// <returns>密文</returns>  
        public static string AESEncrypt(string plainStr, string key)
        {
            string encrypt = null;
            try
            {
                byte[] bKey = Encoding.UTF8.GetBytes(key);
                byte[] bIV = Encoding.UTF8.GetBytes(IV);
                byte[] byteArray = Encoding.UTF8.GetBytes(plainStr);


                Rijndael aes = Rijndael.Create();
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateEncryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        encrypt = Convert.ToBase64String(mStream.ToArray());
                    }
                }
                aes.Clear();
            }
            catch (Exception ex)
            {
                encrypt = "error encrypt" + ex.Message + " string:" + plainStr + " key:" + key;
            }
            return encrypt;
        }


        public static string AESDecrypt(string encryptStr, string key)
        {
            string decrypt = null;
            try
            {
                byte[] bKey = Encoding.UTF8.GetBytes(key);
                byte[] bIV = Encoding.UTF8.GetBytes(IV);
                byte[] byteArray = Convert.FromBase64String(encryptStr);


                Rijndael aes = Rijndael.Create();
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
                aes.Clear();
            }
            catch (Exception ex)
            {

                decrypt = "error  encrypt" + ex.Message + " string:" + encryptStr + " key" + key;
            }
            return decrypt;
        }


        public static string WXAESDecrypt(string encryptStr, string key, string iv)
        {
            string decrypt = null;
            try
            {
                byte[] bKey = Convert.FromBase64String(key);
                byte[] bIV = Convert.FromBase64String(iv);
                byte[] byteArray = Convert.FromBase64String(encryptStr);


                Rijndael aes = Rijndael.Create();
                using (MemoryStream mStream = new MemoryStream())
                {
                    using (CryptoStream cStream = new CryptoStream(mStream, aes.CreateDecryptor(bKey, bIV), CryptoStreamMode.Write))
                    {
                        cStream.Write(byteArray, 0, byteArray.Length);
                        cStream.FlushFinalBlock();
                        decrypt = Encoding.UTF8.GetString(mStream.ToArray());
                    }
                }
                aes.Clear();
            }
            catch (Exception ex)
            {

                decrypt = "error  encrypt" + ex.Message + " string:" + encryptStr + " key" + key;
            }
            return decrypt;
        }


 

        public static Color GetRandomColor(int t)
        {

            Random RandomNum_First = new Random((int)DateTime.Now.Ticks + t);
            System.Threading.Thread.Sleep(RandomNum_First.Next(2));
            Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks + t);
            int int_Red = RandomNum_First.Next(210);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            Color col = Color.FromArgb(int_Red, int_Green, int_Blue);
            return col;
        }
        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <returns></returns>
        //public static byte[] ValidateCode(out string code)
        //{
        //    Random r = new Random();
        //    string letters = "0123456789";
        //    string letter;
        //    StringBuilder s = new StringBuilder();
        //    for (int x = 0; x < 4; x++)
        //    {
        //        letter = letters[r.Next(0, letters.Length - 1)].ToString();
        //        s.Append(letter);
        //    }
        //    code = s.ToString();
        //    using (var bitmap = new Bitmap(100, 30))
        //    {
        //        using (var gph = Graphics.FromImage(bitmap))
        //        {
        //            gph.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
        //            gph.DrawRectangle(new Pen(Color.Black), 1, 1, bitmap.Width - 2, bitmap.Height - 2);
        //            using (Brush bush = new SolidBrush(Color.SteelBlue))
        //            {
        //                gph.DrawString(code, new Font("黑体", 20, FontStyle.Italic), bush, 10, 2);
        //                var random = new Random();
        //                // 画线条
        //                for (int i = 0; i < 5; i++)
        //                {
        //                    gph.DrawLine(new Pen(GetRandomColor(i)), random.Next(bitmap.Width), random.Next(bitmap.Height), random.Next(bitmap.Width), random.Next(bitmap.Height));
        //                }

        //                // 画躁点
        //                for (int i = 0; i < 100; i++)
        //                {
        //                    bitmap.SetPixel(random.Next(bitmap.Width), random.Next(bitmap.Height), GetRandomColor(i));
        //                }
        //                using (var ms = new MemoryStream())
        //                {
        //                    bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //                    var imgData = ms.GetBuffer();
        //                    return imgData;
        //                }
        //            }
        //        }
        //    }
        //}


        ///// <summary>
        ///// 图片验证码
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public static byte[] CreatImgCode(out string code)
        //{
        //    //建立Bitmap对象，绘图
        //    Bitmap basemap = new Bitmap(80, 40);
        //    Graphics graph = Graphics.FromImage(basemap);
        //    graph.Clear(Color.White);
        //    Random r = new Random();
        //    string[] fontArray = { "Arial", "Verdana", "Comic Sans MS", "Impact", "Haettenschweiler", "Lucida Sans Unicode", "Garamond", "Courier New", "Book Antiqua", "Arial Narrow" };
        //    // string letters = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789abcdefghijklmnpqrstuvwxyz";
        //    string letters = "0123456789";
        //    string letter;
        //    StringBuilder s = new StringBuilder();
        //    //添加随机字符
        //    for (int x = 0; x < 4; x++)
        //    {
        //        Font font = new Font(fontArray[r.Next(0, fontArray.Length - 1)], 30, FontStyle.Bold, GraphicsUnit.Pixel);
        //        letter = letters[r.Next(0, letters.Length - 1)].ToString();
        //        s.Append(letter);

        //        Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
        //        System.Threading.Thread.Sleep(RandomNum_First.Next(50));
        //        Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
        //        int int_Red = RandomNum_First.Next(210);
        //        int int_Green = RandomNum_Sencond.Next(180);
        //        int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
        //        int_Blue = (int_Blue > 255) ? 255 : int_Blue;
        //        Color col = Color.FromArgb(int_Red, int_Green, int_Blue);

        //        graph.DrawString(letter, font, new SolidBrush(col), x * 18, r.Next(1, 3));
        //    }
        //    //混淆背景线条
        //    for (int x = 0; x < 4; x++)
        //    {
        //        Pen linePen = new Pen(new SolidBrush(Color.FromArgb(r.Next())), 3);
        //        graph.DrawLine(linePen, new Point(r.Next(2, basemap.Width - 1), r.Next(1, basemap.Height - 1)), new Point(r.Next(2, basemap.Width - 1), r.Next(1, basemap.Height - 1)));
        //    }
        //    //混淆背景点
        //    for (int i = 0; i < 50; i++)
        //    {
        //        Point tem_point = new Point(r.Next(basemap.Width), r.Next(basemap.Height));
        //        basemap.SetPixel(tem_point.X, tem_point.Y, Color.Black);
        //    }
        //    //图片边框
        //    graph.DrawRectangle(new Pen(Color.Silver), 0, 0, basemap.Width - 1, basemap.Height - 1);
        //    MemoryStream ms = new MemoryStream();
        //    //将图片保存到输出流中      
        //    basemap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    code = s.ToString();
        //    return ms.ToArray();

        //}


        ///// <summary>
        ///// 图片验证码
        ///// </summary>
        ///// <param name="code"></param>
        ///// <returns></returns>
        //public static byte[] CreatImgCodeZH_CN(out string code)
        //{
        //    //建立Bitmap对象，绘图
        //    Bitmap basemap = new Bitmap(80, 40);
        //    Graphics graph = Graphics.FromImage(basemap);
        //    graph.Clear(Color.White);
        //    Random r = new Random();
        //    string[] fontArray = { "Arial", "Verdana", "Comic Sans MS", "Impact", "Haettenschweiler", "Lucida Sans Unicode", "Garamond", "Courier New", "Book Antiqua", "Arial Narrow" };
        //    // string letters = "ABCDEFGHIJKLMNPQRSTUVWXYZ0123456789abcdefghijklmnpqrstuvwxyz";
        //    var str = CreateRegionCode(3);

        //    StringBuilder s = new StringBuilder();
        //    //添加随机字符
        //    for (int x = 0; x < 3; x++)
        //    {
        //        Random size = new Random();
        //        string letter = str[x].ToString();
        //        Font font = new Font(fontArray[r.Next(0, fontArray.Length - 1)], size.Next(16, 20), FontStyle.Bold, GraphicsUnit.Pixel);


        //        Random RandomNum_First = new Random((int)DateTime.Now.Ticks);
        //        System.Threading.Thread.Sleep(1);
        //        Random RandomNum_Sencond = new Random((int)DateTime.Now.Ticks);
        //        int int_Red = RandomNum_First.Next(210);
        //        int int_Green = RandomNum_Sencond.Next(180);
        //        int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
        //        int_Blue = (int_Blue > 255) ? 255 : int_Blue;
        //        Color col = Color.FromArgb(int_Red, int_Green, int_Blue);

        //        graph.DrawString(letter, font, new SolidBrush(col), x * 25, r.Next(6, 16));
        //    }
        //    //混淆背景线条
        //    for (int x = 0; x < 6; x++)
        //    {
        //        Pen linePen = new Pen(new SolidBrush(Color.FromArgb(r.Next())), 3);
        //        graph.DrawLine(linePen, new Point(r.Next(2, basemap.Width - 1), r.Next(1, basemap.Height - 1)), new Point(r.Next(2, basemap.Width - 1), r.Next(1, basemap.Height - 1)));
        //    }
        //    //混淆背景点
        //    for (int i = 0; i < 100; i++)
        //    {
        //        Point tem_point = new Point(r.Next(basemap.Width), r.Next(basemap.Height));
        //        basemap.SetPixel(tem_point.X, tem_point.Y, Color.Black);
        //    }
        //    //图片边框
        //    graph.DrawRectangle(new Pen(Color.Silver), 0, 0, basemap.Width - 1, basemap.Height - 1);
        //    MemoryStream ms = new MemoryStream();
        //    //将图片保存到输出流中      
        //    basemap.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
        //    code = s.ToString();
        //    return ms.ToArray();

        //}


        public static string CreateRegionCode(int strlength)
        {
            //定义一个字符串数组储存汉字编码的组成元素 
            string[] rBase = new String[16] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "a", "b", "c", "d", "e", "f" };

            Random rnd = new Random();

            //定义一个object数组用来 
            StringBuilder sb = new StringBuilder();

            /*每循环一次产生一个含两个元素的十六进制字节数组，并将其放入bject数组中 
            每个汉字有四个区位码组成 
            区位码第1位和区位码第2位作为字节数组第一个元素 
            区位码第3位和区位码第4位作为字节数组第二个元素 
            */
            for (int i = 0; i < strlength; i++)
            {
                //区位码第1位 
                int r1 = rnd.Next(11, 14);
                string str_r1 = rBase[r1].Trim();

                //区位码第2位 
                rnd = new Random(r1 * unchecked((int)DateTime.Now.Ticks) + i);//更换随机数发生器的 

                //种子避免产生重复值 
                int r2;
                if (r1 == 13)
                {
                    r2 = rnd.Next(0, 7);
                }
                else
                {
                    r2 = rnd.Next(0, 16);
                }
                string str_r2 = rBase[r2].Trim();

                //区位码第3位 
                rnd = new Random(r2 * unchecked((int)DateTime.Now.Ticks) + i);
                int r3 = rnd.Next(10, 16);
                string str_r3 = rBase[r3].Trim();

                //区位码第4位 
                rnd = new Random(r3 * unchecked((int)DateTime.Now.Ticks) + i);
                int r4;
                if (r3 == 10)
                {
                    r4 = rnd.Next(1, 16);
                }
                else if (r3 == 15)
                {
                    r4 = rnd.Next(0, 15);
                }
                else
                {
                    r4 = rnd.Next(0, 16);
                }
                string str_r4 = rBase[r4].Trim();

                //定义两个字节变量存储产生的随机汉字区位码 
                byte byte1 = Convert.ToByte(str_r1 + str_r2, 16);
                byte byte2 = Convert.ToByte(str_r3 + str_r4, 16);
                //将两个字节变量存储在字节数组中 
                byte[] str_r = new byte[] { byte1, byte2 };

                //将产生的一个汉字的字节数组放入object数组中 
                sb.Append(Encoding.GetEncoding("GB2312").GetString(str_r));

            }
            return sb.ToString();

        }


        public static string ClearHtml(string input)
        {
            if (!string.IsNullOrEmpty(input))
            {
                string regexstr = @"<[^>]*>";
                Regex reg = new Regex(regexstr, RegexOptions.Multiline);
                input = reg.Replace(input, "");
            }
            return input;
        }



        public static string UrlEncode(string text)
        {

            return HttpUtility.UrlEncode(text);

        }
        public static string UrlDencode(string text)
        {

            return HttpUtility.UrlDecode(text);

        }

        public static string GZipCompress(string rawString)
        {
            if (string.IsNullOrEmpty(rawString) || rawString.Length == 0)
            {
                return "";
            }
            else
            {
                byte[] rawData = System.Text.Encoding.UTF8.GetBytes(rawString.ToString());
                byte[] zippedData = Compress(rawData);
                return (string)(Convert.ToBase64String(zippedData));
            }

        }
        /// <summary>
        /// GZip压缩
        /// </summary>
        /// <param name="rawData"></param>
        /// <returns></returns>
        public static byte[] Compress(byte[] rawData)
        {
            MemoryStream ms = new MemoryStream();
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Compress, true);
            compressedzipStream.Write(rawData, 0, rawData.Length);
            compressedzipStream.Close();
            return ms.ToArray();
        }
        /// <summary>
        /// 将传入的二进制字符串资料以GZip算法解压缩
        /// </summary>
        /// <param name="zippedString">经GZip压缩后的二进制字符串</param>
        /// <returns>原始未压缩字符串</returns>
        public static string GZipDecompress(string zippedString)
        {
            try
            {
                if (string.IsNullOrEmpty(zippedString) || zippedString.Length == 0)
                {
                    return "";
                }
                else
                {
                    byte[] zippedData = Convert.FromBase64String(zippedString.ToString());
                    return (string)(System.Text.Encoding.UTF8.GetString(Decompress(zippedData)));
                }
            }
            catch
            {
                return zippedString;
            }
        }
        /// <summary>
        /// ZIP解压
        /// </summary>
        /// <param name="zippedData"></param>
        /// <returns></returns>
        public static byte[] Decompress(byte[] zippedData)
        {
            MemoryStream ms = new MemoryStream(zippedData);
            GZipStream compressedzipStream = new GZipStream(ms, CompressionMode.Decompress);
            MemoryStream outBuffer = new MemoryStream();
            byte[] block = new byte[1024];
            while (true)
            {
                int bytesRead = compressedzipStream.Read(block, 0, block.Length);
                if (bytesRead <= 0)
                    break;
                else
                    outBuffer.Write(block, 0, bytesRead);
            }
            compressedzipStream.Close();
            return outBuffer.ToArray();
        }

        public static string ToBase64(string input)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            string str = Convert.ToBase64String(bytes);
            return str.Replace("/", "-").Replace("+", "_");

        }

        public static string FromBase64(string input)
        {
            input = input.Replace("-", "/").Replace("_", "+");
            byte[] outputb = Convert.FromBase64String(input);
            string orgStr = Encoding.UTF8.GetString(outputb);
            return orgStr;

        }

        public static string Random(int len)
        {

            string strsource = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            string result = string.Empty;
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++)
            {
                result += strsource[ran.Next(0, strsource.Length)];
            }
            return result;
        }
        public static string RandomChar(int len)
        {

            string strsource = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string result = string.Empty;
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++)
            {
                result += strsource[ran.Next(0, strsource.Length)];
            }
            return result;
        }
        public static string RandomNum(int len)
        {

            string strsource = "1234567890";
            string result = string.Empty;
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++)
            {
                result += strsource[ran.Next(0, strsource.Length)];
            }
            return result;
        }
        /// <summary>
        /// 随即字符串
        /// </summary>
        /// <param name="len"></param>
        /// <param name="allnumber"></param>
        /// <returns></returns>
        public static string RandomStr(int len, bool allnumber = false)
        {
            string strsource = "";
            if (allnumber)
            {
                strsource = "1234567890";
            }
            else
            {
                strsource = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890abcdefghijklmnopqrstuvwxyz";
            }

            string result = string.Empty;
            Random ran = new Random(Guid.NewGuid().GetHashCode());
            for (int i = 0; i < len; i++)
            {
                result += strsource[ran.Next(0, strsource.Length)];
            }

            return result;
        }


        /// <summary>   

        /// MD5解密   

        /// </summary>   

        /// <param name="Source">需要解密的字符串</param>   

        /// <returns>MD5解密后的字符串</returns>   

        public static string Md5Decrypt(string Source)

        {

            //将解密字符串转换成字节数组   

            byte[] bytIn = System.Convert.FromBase64String(Source);

            //给出解密的密钥和偏移量，密钥和偏移量必须与加密时的密钥和偏移量相同   

            byte[] iv = { 102, 16, 93, 156, 78, 4, 218, 32 };//定义偏移量   

            byte[] key = { 55, 103, 246, 79, 36, 99, 167, 3 };//定义密钥   

            DESCryptoServiceProvider mobjCryptoService = new DESCryptoServiceProvider();

            mobjCryptoService.Key = iv;

            mobjCryptoService.IV = key;

            //实例流进行解密   

            System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

            ICryptoTransform encrypto = mobjCryptoService.CreateDecryptor();

            CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

            StreamReader strd = new StreamReader(cs, Encoding.Default);

            return strd.ReadToEnd();

        }


        /// <summary>
        /// 获取字符串MD5值
        /// </summary>
        /// <param name="input">字符串</param>
        /// <returns></returns>
        public static string GetMD5(string input)
        {

            string md5 = "";
            MD5 creater = MD5.Create();
            Byte[] bytes = creater.ComputeHash(Encoding.UTF8.GetBytes(input));
            foreach (var item in bytes)
            {
                md5 += item.ToString("x2");
            }
            return md5;
        }

        /// <summary>
        /// 获取文件或字符串MD5值
        /// </summary>
        /// <param name="input">字符串或路径</param>
        /// <param name="isFile">文件MD5</param>
        /// <returns></returns>
        public static string GetMD5(string input, bool isFile)
        {

            string md5 = "";
            MD5 creater = MD5.Create();
            if (isFile)
            {

                using (FileStream filestream = new FileStream(input, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    Byte[] bytes = creater.ComputeHash(filestream);
                    foreach (var item in bytes)
                    {
                        md5 += item.ToString("x2");
                    }
                }
            }
            else
            {
                Byte[] bytes = creater.ComputeHash(Encoding.Default.GetBytes(input));
                foreach (var item in bytes)
                {
                    md5 += item.ToString("x2");
                }
            }

            return md5;
        }

        /// <summary>
        /// SHA1
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string SHA1(string text)
        {
            byte[] cleanBytes = Encoding.Default.GetBytes(text);
            byte[] hashedBytes = System.Security.Cryptography.SHA1.Create().ComputeHash(cleanBytes);
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }


 


        public static string ReadTex(string path, Encoding encoding)
        {

            string resut;
            using (StreamReader stream = new StreamReader(path, encoding))
            {
                resut = stream.ReadToEnd();
            }
            return resut;
        }

        public static int ParseMonth(string month)
        {
            if (month.isNull())
                return -1;
            month = month.ToUpper();
            //一月：Jan；二月：Feb；三月：Mar；四月：Apr；五月：May；六月：Jun；七月：Jul；
            // 八月：aug.；九月：Sep；十月：Oct；十一月：Nov；十二月：Dec
            int result = -1;
            switch (month)
            {
                case "JAN": result = 1; break;
                case "FEB": result = 2; break;
                case "MAR": result = 3; break;
                case "APR": result = 4; break;
                case "MAY": result = 5; break;
                case "JUN": result = 6; break;
                case "JUL": result = 7; break;
                case "AUG": result = 8; break;
                case "SEP": result = 9; break;
                case "OCT": result = 10; break;
                case "NOV": result = 11; break;
                case "DEC": result = 12; break;
                default: result = 0; break;
            }
            return result;
        }

        public static T GetClassOfInterface<T>(string className) where T : class
        {
            //var types = Assembly.GetEntryAssembly().GetTypes();
            var types = typeof(T).Assembly.GetTypes();
            var aType = typeof(T);


            foreach (Type type in types)
            {
                var xx = type.GetInterface(aType.FullName);
                //找到接口
                if (type.GetInterface(aType.FullName) != null && type.Name == className)
                {

                    var dd = type.GetConstructor(Type.EmptyTypes).Invoke(null);
                    return dd as T;
                }
            }
            return default(T);

        }


        public static string ClientInfo(HttpRequest request)
        {
            string ip = request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            string browser = request.Headers["User-Agent"].ToString();
            var info = ip + " " + browser;
            return info;

        }

        public static T Base64ToObject<T>(string str) where T : class
        {
            var buff = Convert.FromBase64String(str);
            BinaryFormatter b = new BinaryFormatter();
            var obj = b.Deserialize(new MemoryStream(buff)) as T;


            return obj;

        }
        public static string ObjectToBase64(object obj)
        {
            string result = "";
            using (System.IO.MemoryStream memory = new System.IO.MemoryStream())
            {
                //使用内存流来存这些byte[] 
                BinaryFormatter b = new BinaryFormatter();
                b.Serialize(memory, obj); //系列化datatable,MS已经对datatable实现了系列化接口,如果你自定义的类要系列化,实现IFormatter 就可以类似做法 
                var buff = memory.GetBuffer(); //这里就可你想要的byte[],可以使用它来传输 

                result = Convert.ToBase64String(buff);

            }
            return result;
        }
    }
}
