using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;


namespace WebSite.Logic
{
    using Cqwy.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using Util.Ext;

    public class LocalMiddleware : IMiddleware, IScoped
    {
        protected string body { get; set; }

      


        private ILogger<LocalMiddleware> logger;
        public LocalMiddleware(ILogger<LocalMiddleware> logger)
        {
            this.logger = logger;
         
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //记录 api 执行耗时
            //
            Stopwatch sw = new Stopwatch();
            sw.Start();


            #region 读取请求 

            //启用倒带功能，就可以让 Request.Body 可以再次读取
            context.Request.EnableBuffering();
            //读取 body 信息
            var reader = new StreamReader(context.Request.Body);
            body = await reader.ReadToEndAsync();
            context.Request.Body.Position = 0;//必须存在

            #endregion


            await next.Invoke(context);
            //记录请求执行时间
            sw.Stop();
            var times = sw.ElapsedMilliseconds;
            var url = context.Request.Path.Value;
            if (!url.Contains("CheckNoticeAjax")) {
                logger.LogDebug($"{DateTime.Now.ToDateTimeString()} 请求用时：{times} {url}");
            }
        
            //if (times > 1000) {
                
            //}

        }

      

    }
}