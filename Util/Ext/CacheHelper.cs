
using System;
using System.Collections;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;
using System.Collections.Generic;
using StackExchange.Redis;
using System.Configuration;
using System.Threading;
using Cqwy;
using Util.Ext;

namespace System
{
    public class CacheHelper
    {
        //前缀 区分不同的软件系统
        public static  string SysCustomKey = "farecore";
        //连接字符串
        private static  string conn = "127.0.0.1:6379,connectTimeout=10000,allowAdmin=true";

        private static ConnectionMultiplexer redis { get; set; }
        private static IDatabase db { get; set; }

  
   
        public static void IniRedis()
        {
            
          
            if (redis == null)
            {
                conn = App.Configuration.GetSection("redis:conn").Value;
                SysCustomKey = App.Configuration.GetSection("redis:name").Value;
                Serilog.Log.Debug($"redisStr:{conn} redisKey:{SysCustomKey}");
                if (conn == null || conn == "")
                    throw new Exception("请配置redis库连接 ConnectionStrings:redisStr");
                redis = ConnectionMultiplexer.Connect(conn);
                
                redis.ConnectionRestored += (o, e) => { Serilog.Log.Debug("ConnectionRestored: " + e.EndPoint); };
            }
            if (db == null)
            {
                db = redis.GetDatabase();
              
            }
        }

        public static string CreateOrderNo(string pre = "jp") {
            var ti = DateTime.Now;
            IniRedis();
            var t = DateTime.Now.ToString("yyyyMMddHHmmss");
            var key = $"{SysCustomKey}:orderNum:{t}";
            var ll = db.StringIncrement(key);
            //TimeSpan expiry = TimeSpan.FromMinutes(30);
            //db.KeyExpire(key, expiry);
            var tt = DateTime.Now - ti;
            if (tt.TotalMilliseconds > 200) {
                Serilog.Log.Warning("redis生成单号超时："+tt.TotalMilliseconds);        
            }
            return $"{pre}{t}{ll}";
        }

        #region HttpRuntime.Cache


     
        /// <summary>
        /// 获取数据缓存
        /// </summary>
        /// <param name="CacheKey">键</param>
        public static T GetCache<T>(string CacheKey) where T : class
        {  
            var temp= MemoryCacheHelper.Get(CacheKey) as T;   
            return temp ;
        }

        /// <summary>
        /// 设置数据缓存
        /// </summary>
        public static void SetCache(string CacheKey, object objObject)
        {

            MemoryCacheHelper.Set(CacheKey, objObject);
        }


        /// <summary>
        /// 内存缓存 带过期时间缓存 
        /// </summary>
        /// <param name="CacheKey">间</param>
        /// <param name="objObject">值</param>
        /// <param name="Timeout">过期时间（秒）</param>
        public static void SetCache(string CacheKey, object objObject, int Timeout)
        {
            MemoryCacheHelper.Set(CacheKey, objObject, Timeout);
        }






        private static readonly object lockObj = new object();
        public static object GetCache(string CacheKey) 
        {
          
            var result = MemoryCacheHelper.Get(CacheKey);
            return result == null ? null : result;

          

         

        }
      
        public static void SetCache(string CacheKey, object objObject, TimeSpan timeOut)
        {

            MemoryCacheHelper.Set(CacheKey, objObject, timeOut.TotalSeconds.ToInt());
        }



        /// <summary>
        /// 移除指定数据缓存
        /// </summary>
        public static void RemoveCache(string CacheKey)
        {
            MemoryCacheHelper.RemoveCacheByKey(CacheKey);
          
        }

        /// <summary>
        /// 移除全部缓存
        /// </summary>
        public static void RemoveAllCache()
        {
            MemoryCacheHelper.RemoveCacheAll();
           
        }

        #endregion HttpRuntime.Cache




        #region redis


    



        /// <summary>
        /// 存值并设置过期时间  redis实现缓存时建议将键设置为":"分割的成绩 例：MainCache:fares:mate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">key</param>
        /// <param name="t">实体</param>
        /// <param name="ts">过期时间间隔（秒）</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public static bool SetCacheRedis(string key, object t, int Timeout = 60)
        {
            var resule = false;

            try {
                var start = DateTime.Now;
                TimeSpan ts = new TimeSpan(0, 0, Timeout);
                string str = null;
                if (t != null)
                    str = JsonConvert.SerializeObject(t);
                IniRedis();
                resule = db.StringSetAsync(SysCustomKey + ":" + key, str, ts).Result;
                var elapsed = (DateTime.Now - start).TotalMilliseconds;
                if (elapsed > 500)
                {
                    var inter = redis.GetCounters().Interactive;
                    Serilog.Log.Warning($"redis写入超500ms {(int)elapsed}ms  {str.Length} P:{inter.PendingUnsentItems} S: {inter.SentItemsAwaitingResponse} R: {inter.ResponsesAwaitingAsyncCompletion} key:{key}");
                }
            }
            catch (Exception ex) {

                Serilog.Log.Error("redis写入异常：" + ex.Message);
            }

            return resule;
        }

        /// <summary>
        /// 
        /// 根据Key获取值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>T.</returns>
        public static T GetCacheRedis<T>(string key) where T : class
        {

            IniRedis();

            try
            {
                var start = DateTime.Now;
                var strValue = db.StringGetAsync(SysCustomKey + ":" + key).Result;
                var result= string.IsNullOrEmpty(strValue) ? null : JsonConvert.DeserializeObject<T>(strValue);
                var elapsed = (DateTime.Now - start).TotalMilliseconds;
                if (elapsed > 500) {
                    var inter = redis.GetCounters().Interactive;
                    Serilog.Log.Warning($"redis读取超500ms {(int)elapsed}ms {(strValue.IsNull?0:strValue.ToString().Length)} P:{inter.PendingUnsentItems} S: {inter.SentItemsAwaitingResponse } R: {inter.ResponsesAwaitingAsyncCompletion } key:{key}");
                }
                return result;
            }
            catch(Exception ex)
            {
                Serilog.Log.Error("redis读取异常："+ex.Message);
                return default(T);
            }

        }

        public static void RemoveCacheRedis(string pattern)
        {

          
            try
            {
                IniRedis();

                foreach (var ep in redis.GetEndPoints())
                {                   
                    var server = redis.GetServer(ep);
                    var keys = server.Keys(pattern: "*" + pattern + "*");
                    foreach (var key in keys)
                        redis.GetDatabase().KeyDelete(key);
                }

            }
            catch
            {
               
            }

        }

        #endregion redis


        #region 两级缓存


        /// <summary>
        /// 两级缓存  先读取本地内存缓存 3秒 无数据再速度redis缓存  并写入本地缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static T ReadCache<T>(string CacheKey) where T : class
        {
            var result = GetCache<T>(CacheKey);
            if (result == null)
            {
                result = GetCacheRedis<T>(CacheKey);
                if (result != null)
                {
                    SetCache(CacheKey, result, 3);
                }
            }
            return result;

        }
        /// <summary>
        /// 两级缓存 要缓存的数据先写入本地缓存 再写入redis缓存  本地缓存默认5s过期 redis过期时间参数传入
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <param name="objObject"></param>
        /// <param name="Timeout"></param>
        public static void WriteCache(string CacheKey, object objObject, int Timeout=60*5)
        {
            SetCache(CacheKey, objObject, 3);
            SetCacheRedis(CacheKey, objObject, Timeout);
        }

        public static bool CacheCheck(string key)
        {
            IniRedis();
            bool isSet = false;
         
            isSet = db.KeyExists(SysCustomKey + ":" + key);
            return isSet;

        }

        #endregion 两级缓存


        #region redis 令牌桶限流
    

        /// <summary>
        /// 检查限流
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool CheckLimit(string key = "supplyLimit:vj:search")
        {
            var ckey = $"{SysCustomKey}:{key}";
            IniRedis();
            return db.ListLeftPop(ckey) !=RedisValue.Null;
        }

        /// <summary>
        /// 开个任务 固定速率调用此方法添加令牌
        /// </summary>
        /// <param name="key"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static long AddLimitToken(string key = "supplyLimit:vj:search", int count = 3)
        {
            Serilog.Log.Verbose($"令牌桶：{key} 添加令牌 {count}");
            IniRedis();
            var ckey = $"{SysCustomKey}:{key}";
            var keys = new List<RedisValue>();
            var ccount = db.ListLength(ckey);

            for (int i = 0; i < count - ccount; i++)
            {
                keys.Add(Guid.NewGuid().ToString());
            }
            if (keys.Count > 0) {
                return db.ListRightPush(ckey, keys.ToArray());
            }

            return 0;
        }

        #endregion redis 令牌桶限流
    }
}