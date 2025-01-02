using Domain.Model.AjaxModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebSite.Logic;

namespace WebSite.Filters
{
    public class AdminExceptionAttribute :Attribute,IExceptionFilter 
    {

        public void OnException(ExceptionContext context)
        {


            var isAjax = context.HttpContext.Request.IsAjax();

            Exception ex = context.Exception;

            //这里给系统分配标识，监控异常肯定不止一个系统。
            int sysId = 1;
            //这里获取服务器ip时，需要考虑如果是使用nginx做了负载，这里要兼容负载后的ip，
            //监控了ip方便定位到底是那台服务器出故障了
            string ip = context.HttpContext.Connection.RemoteIpAddress.ToString();

            Serilog.Log.Error($"系统编号：{sysId},主机IP:{ip},堆栈信息：{ex.StackTrace},异常描述：{ex.Message}");
            if (isAjax)
            {
                context.Result = new JsonResult(new AjaxResult() { Code = -99, Msg = "系统异常:" + ex.Message });
            }
            else
            {
                context.HttpContext.Session.SetString("errorTitle", "系统异常:" + ex.Message);
                context.HttpContext.Session.SetString("errorDetail", ex.ToString());
                context.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Home", action = "Error",code=500 }));

            }
            context.ExceptionHandled = true;
        }

    }
}