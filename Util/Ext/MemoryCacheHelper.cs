using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace System
{

    /// <summary>
    /// 全局缓存 静态调用 使用方便  需要注意 与注入获取的不是同一个容器  
    /// </summary>
    public class MemoryCacheHelper
    {
        public static readonly IMemoryCache memoryCache = new MemoryCache(new MemoryCacheOptions
        {

        });
        public static IMemoryCache Cache
        {
            get
            {
                return memoryCache;
            }
        }
        public static object Get(string key)
        {

            return Cache.Get(key);
        }

        public static object Set(string key, object data, int second = 10)
        {

            return Cache.Set(key, data, TimeSpan.FromSeconds(second));
        }
        public static object Set(string key, object data, TimeSpan time)
        {

            return Cache.Set(key, data, time);
        }
        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public static void RemoveCacheAll()
        {
            var keys = GetCacheKeys();
            foreach (var key in keys)
            {
                Cache.Remove(key);
            }
        }

        /// <summary>
        /// 删除所有缓存
        /// </summary>
        public static void RemoveCacheByKey(string CacheKey)
        {
            if (Get(CacheKey) != null)
            {
                Cache.Remove(CacheKey);
            }
        }


        /// <summary>
        /// 获取所有缓存键
        /// </summary>
        /// <returns></returns>
        public static List<string> GetCacheKeys()
        {
            const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
            var entries = Cache.GetType().GetField("_entries", flags).GetValue(Cache);
            var cacheItems = entries as IDictionary;
            var keys = new List<string>();
            if (cacheItems == null) return keys;
            foreach (DictionaryEntry cacheItem in cacheItems)
            {
                keys.Add(cacheItem.Key.ToString());
            }
            return keys;
        }

    }
}
