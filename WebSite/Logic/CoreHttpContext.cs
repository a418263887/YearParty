
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
 
namespace System
{
    public static class CoreHttpContext
    {
        private static Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostEnviroment;
        public static string WebPath => _hostEnviroment.WebRootPath;

        public static string MapPath(string path)
        {
            return Path.Combine(_hostEnviroment.WebRootPath, path.Replace("~","").Replace("/","\\").Trim('\\'));
        }

        internal static void Configure(Microsoft.AspNetCore.Hosting.IHostingEnvironment hostEnviroment)
        {
            _hostEnviroment = hostEnviroment;
        }
    }
    public static class StaticHostEnviromentExtensions
    {
        public static IApplicationBuilder UseStaticHostEnviroment(this IApplicationBuilder app)
        {
            var webHostEnvironment = app.ApplicationServices.GetRequiredService<Microsoft.AspNetCore.Hosting.IHostingEnvironment>();
            CoreHttpContext.Configure(webHostEnvironment);
            return app;
        }
    }
}
