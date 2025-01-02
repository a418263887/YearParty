using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;


namespace WebSite.Logic
{

    public static class CoreExt
    {
       
        public static bool IsAjax(this HttpRequest req)
        {
            bool result = false;

            var xreq = req.Headers.ContainsKey("x-requested-with");
            if (xreq)
            {
                result = req.Headers["x-requested-with"] == "XMLHttpRequest";
            }

            return result;
        }

     
     

        public static  string GetClientInfo(this HttpRequest req) {

            var uaParser = UAParser.Parser.GetDefault();
            StringValues ag;
            req.Headers.TryGetValue("User-Agent", out ag);
            var uaString = ag.ToString();
            UAParser.ClientInfo c = uaParser.Parse(uaString);

            var os = c.OS.Family+ c.OS.Major;
            var br = c.UA.Family + c.UA.Major;
            return os + " " + br;

        }


        /// <summary>
        /// swagger需要给每个action设置httpmethd  这里设置一个默认post
        /// </summary>
        /// <param name="app"></param>
        /// <param name="defaultHttpMethod"></param>
        public static void AutoHttpMethodIfActionNoBind(this IApplicationBuilder app, string defaultHttpMethod = null)
        {
            //从容器中获取IApiDescriptionGroupCollectionProvider实例
            var apiDescriptionGroupCollectionProvider = app.ApplicationServices.GetRequiredService<IApiDescriptionGroupCollectionProvider>();
            var apiDescriptionGroupsItems = apiDescriptionGroupCollectionProvider.ApiDescriptionGroups.Items;
            //遍历ApiDescriptionGroups
            foreach (var apiDescriptionGroup in apiDescriptionGroupsItems)
            {
                foreach (var apiDescription in apiDescriptionGroup.Items)
                {
                    if (string.IsNullOrEmpty(apiDescription.HttpMethod))
                    {
                        //获取Action名称
                        var actionName = apiDescription.ActionDescriptor.RouteValues["action"];
                        //默认给定POST
                        string methodName = defaultHttpMethod ?? "POST";
                        ////根据Action开头单词给定HttpMethod默认值
                        //if (actionName.StartsWith("get", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    methodName = "GET";
                        //}
                        //else if (actionName.StartsWith("put", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    methodName = "PUT";
                        //}
                        //else if (actionName.StartsWith("delete", StringComparison.OrdinalIgnoreCase))
                        //{
                        //    methodName = "DELETE";
                        //}
                        apiDescription.HttpMethod = methodName;
                    }
                }
            }
        }


    }
}
