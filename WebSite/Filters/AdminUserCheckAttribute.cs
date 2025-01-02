
using Domain.Model.AjaxModel;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Util.Ext;
using WebSite.Logic;

namespace WebSite.Filters
{

    public class AdminUserCheckAttribute : ActionFilterAttribute
    {

        protected long uid = 0;
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            var Session = filterContext.HttpContext.Session;

            if (Session.GetString("Uid") == null)
            {

                    var req = filterContext.HttpContext.Request;
                    var isAjax = req.IsAjax();
                    if (isAjax)
                    {
                        filterContext.Result = new JsonResult(new AjaxResult() { Code = -99, Msg = "请登录" });

                    }
                    else
                    {
                        var oldUrl = req.GetDisplayUrl();
                        Session.SetString("oldUrl", oldUrl);

                        //页面可能在iframe中 直接跳转 登录页面可能会在iframe中 体验不好 返回js执行跳转
                        filterContext.Result = new ContentResult() { Content = "<script>window.top.location.href='/Home/Login';</script>", ContentType = "text/html" };

                        //var xs = $"{req.Scheme}://{req.Host}/home/authback";                  
                        //filterContext.Result = new ContentResult() { Content = $"<script>window.top.location.href='{App.GetConfig<string>("oa:api")}/user/auth?channel=1001&redirect={xs}';</script>", ContentType = "text/html" };

                    }
                


            }


        }
    }
}