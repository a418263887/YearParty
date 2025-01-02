
using Cqwy;
using Cqwy.DatabaseAccessor;
using Cqwy.SpecificationDocument;
using Domain.Model;
using Domain.Model.AjaxModel;
using Domain.Model.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using Util.Ext;
using WebSite.Filters;

namespace WebSite.Controllers
{

    [AdminException]
    public class HomeController : Controller
    {
        AjaxResult result = new();
        private IRepository<User> repository;
        public HomeController(IRepository<User> repository)
        {
            this.repository = repository;
        }
        const string UserJson = @"[{""name"":""周红"",""code"":""5008"",""sex"":""女""},{""name"":""李恒树"",""code"":""5007"",""sex"":""女""},{""name"":""陈轶"",""code"":""7017"",""sex"":""男""},{""name"":""杨宏川"",""code"":""7064"",""sex"":""男""},{""name"":""李舒心"",""code"":""7063"",""sex"":""男""},{""name"":""朱才华"",""code"":""7049"",""sex"":""男""},{""name"":""萧亚君"",""code"":""7062"",""sex"":""女""},{""name"":""周玮"",""code"":""7067"",""sex"":""女""},{""name"":""游翼嘉"",""code"":""7069"",""sex"":""男""},{""name"":""杨阳"",""code"":""7054"",""sex"":""男""},{""name"":""徐仕超"",""code"":""7038"",""sex"":""男""},{""name"":""曹航"",""code"":""7057"",""sex"":""男""},{""name"":""郭泽"",""code"":""7058"",""sex"":""男""},{""name"":""黄鑫"",""code"":""7059"",""sex"":""男""},{""name"":""邓勇"",""code"":""7050"",""sex"":""男""},{""name"":""张亚"",""code"":""7065"",""sex"":""女""},{""name"":""童小燕"",""code"":""7068"",""sex"":""女""},{""name"":""姜丽"",""code"":""8031"",""sex"":""女""},{""name"":""刘宇浩"",""code"":""8078"",""sex"":""女""},{""name"":""殷霞"",""code"":""8071"",""sex"":""女""},{""name"":""王忠玉"",""code"":""8001"",""sex"":""女""},{""name"":""游亚玲"",""code"":""8030"",""sex"":""女""},{""name"":""李丹"",""code"":""8079"",""sex"":""女""},{""name"":""黄秋霞"",""code"":""8080"",""sex"":""女""},{""name"":""冯婷婷"",""code"":""8081"",""sex"":""女""},{""name"":""张静"",""code"":""3306"",""sex"":""女""},{""name"":""杨蕊伊"",""code"":""8019"",""sex"":""女""},{""name"":""张洁"",""code"":""5011"",""sex"":""女""},{""name"":""李永智"",""code"":""1155"",""sex"":""女""},{""name"":""马微"",""code"":""9350"",""sex"":""女""},{""name"":""杨爽"",""code"":""6001"",""sex"":""女""},{""name"":""陈露"",""code"":""6028"",""sex"":""女""},{""name"":""谭雪芹"",""code"":""6037"",""sex"":""女""},{""name"":""李星"",""code"":""2041"",""sex"":""女""},{""name"":""余成勇"",""code"":""3316"",""sex"":""女""},{""name"":""刘艺情"",""code"":""2017"",""sex"":""女""},{""name"":""罗晓娟"",""code"":""2097"",""sex"":""女""},{""name"":""俞磊"",""code"":""9429"",""sex"":""男""},{""name"":""刘淋淋"",""code"":""9425"",""sex"":""女""},{""name"":""赵小利"",""code"":""2009"",""sex"":""女""},{""name"":""赵学樑"",""code"":""9516"",""sex"":""男""},{""name"":""郑均"",""code"":""9517"",""sex"":""女""},{""name"":""王胜"",""code"":""3005"",""sex"":""男""},{""name"":""王裕玲"",""code"":""9336"",""sex"":""男""},{""name"":""曾光"",""code"":""9345"",""sex"":""男""},{""name"":""张会梅"",""code"":""9518"",""sex"":""女""},{""name"":""周小龙"",""code"":""2168"",""sex"":""男""},{""name"":""李玉"",""code"":""2151"",""sex"":""女""},{""name"":""张晓亚"",""code"":""1120"",""sex"":""女""},{""name"":""陈思言"",""code"":""1098"",""sex"":""女""},{""name"":""姜婷"",""code"":""1097"",""sex"":""女""},{""name"":""付冰心"",""code"":""1135"",""sex"":""女""},{""name"":""陈红"",""code"":""1129"",""sex"":""女""},{""name"":""蒋敏"",""code"":""3108"",""sex"":""女""},{""name"":""刘小妹"",""code"":""3080"",""sex"":""女""},{""name"":""陈晓娇"",""code"":""9270"",""sex"":""女""},{""name"":""孙露"",""code"":""1124"",""sex"":""女""},{""name"":""冯小清"",""code"":""9295"",""sex"":""女""},{""name"":""郑露"",""code"":""1117"",""sex"":""女""},{""name"":""刘丹"",""code"":""9404"",""sex"":""女""},{""name"":""刘兰"",""code"":""1136"",""sex"":""女""},{""name"":""谢双霞"",""code"":""1050"",""sex"":""女""},{""name"":""罗圆圆"",""code"":""9273"",""sex"":""女""},{""name"":""张玉"",""code"":""1122"",""sex"":""女""},{""name"":""周丽娜"",""code"":""3010"",""sex"":""女""},{""name"":""马丹"",""code"":""2187"",""sex"":""女""},{""name"":""陈宇"",""code"":""2171"",""sex"":""女""},{""name"":""邹舟"",""code"":""2173"",""sex"":""女""},{""name"":""呙燕华"",""code"":""2112"",""sex"":""女""},{""name"":""邱青松"",""code"":""2196"",""sex"":""男""},{""name"":""何林"",""code"":""2202"",""sex"":""男""},{""name"":""谢朝刚"",""code"":""2206"",""sex"":""男""},{""name"":""龙欢"",""code"":""1126"",""sex"":""女""},{""name"":""田坤"",""code"":""2207"",""sex"":""男""},{""name"":""李忠玲"",""code"":""3047"",""sex"":""女""},{""name"":""包玲芳"",""code"":""9307"",""sex"":""女""},{""name"":""许甜"",""code"":""2175"",""sex"":""女""},{""name"":""陈强"",""code"":""9306"",""sex"":""男""},{""name"":""韩亦"",""code"":""9406"",""sex"":""女""},{""name"":""姜欣禹"",""code"":""2192"",""sex"":""女""},{""name"":""李莹银"",""code"":""2209"",""sex"":""女""},{""name"":""张佳静"",""code"":""9437"",""sex"":""女""},{""name"":""陈运"",""code"":""9308"",""sex"":""女""},{""name"":""钱发聪"",""code"":""9300"",""sex"":""男""},{""name"":""旷运丹"",""code"":""9305"",""sex"":""女""},{""name"":""苏秋"",""code"":""2001"",""sex"":""女""},{""name"":""胡诗岚"",""code"":""9424"",""sex"":""女""},{""name"":""杨蓉"",""code"":""9435"",""sex"":""女""},{""name"":""杨佳"",""code"":""9438"",""sex"":""女""},{""name"":""张蜜"",""code"":""9430"",""sex"":""女""},{""name"":""周悦"",""code"":""9421"",""sex"":""女""},{""name"":""邓蓉"",""code"":""2188"",""sex"":""女""},{""name"":""梁婷"",""code"":""3121"",""sex"":""女""},{""name"":""周相君"",""code"":""1315"",""sex"":""女""},{""name"":""余港平"",""code"":""9303"",""sex"":""男""},{""name"":""赵岳"",""code"":""9423"",""sex"":""男""},{""name"":""刘德燕"",""code"":""9426"",""sex"":""女""},{""name"":""甘星宇"",""code"":""9407"",""sex"":""男""},{""name"":""雷弋靖"",""code"":""9433"",""sex"":""女""},{""name"":""郭权国"",""code"":""9301"",""sex"":""男""},{""name"":""朱婷婷"",""code"":""9405"",""sex"":""女""},{""name"":""杨双玲"",""code"":""9436"",""sex"":""女""},{""name"":""殷慧慧"",""code"":""2194"",""sex"":""女""},{""name"":""罗家琳"",""code"":""9418"",""sex"":""女""},{""name"":""杨小丽"",""code"":""9311"",""sex"":""女""}]";



        protected ActionResult JsonData(object obj)
        {
            return Content(obj.ToJson(), "application/json");
        }
        //供应商登录
        public ActionResult SIndex()

        {

            return new RedirectResult("/SupplierSystem/home/index");
            //return View();
        }
        //供应商登录
        public ActionResult SDefult()
        {

            return new RedirectResult("/SupplierSystem/home/Defult");
            //return View();
        }
        //登录
        public ActionResult Login()
        {
            //ViewBag.oaapi = App.GetConfig<string>("oa:api");
            //ViewBag.oaapi = "http://localhost:56110";
            return View();
        }

        public ActionResult AjaxLogin()
        {

            string userid = Request.Form["uid"];



            if (!string.IsNullOrEmpty(userid))
            {
                userid = userid.Trim();

                var userimodel = UserJson.JsonToObject<List<UserModel>>();
                if (userimodel.Any(x => x.code == userid))
                {

                    var tempuser = userimodel.Where(x => x.code == userid).First();

                    var userdata = repository.DetachedEntities.FirstOrDefault(x => x.GH == userid);
                    var key = "token-Year";
                    if (userdata != null)
                    {
                        HttpContext.Session.SetString("Uid", userdata.GH.ToString());
                        // + Request.HttpContext.Connection.LocalPort;
                        var token = JWTHelper.GetJWT(userdata);
                        HttpContext.Session.SetString(key, token);
                        Response.Cookies.Append(key, token, new CookieOptions() { Expires = DateTime.Now.AddDays(7), HttpOnly = true });

                        return new RedirectResult("/home/index");
                        //记住我  通过jwt cookie实现  默认有效期30天

                    }
                    else
                    {
                        User user = new() { GH = userid, Sex = tempuser.sex, IsBaoming = 0, Man = 0, PiaoCount = 4, Name = tempuser.name };
                        HttpContext.Session.SetString("Uid", user.GH.ToString());
                        var re = repository.InsertNow(user).Entity;
                        var token = JWTHelper.GetJWT(re);
                        HttpContext.Session.SetString(key, token);
                        Response.Cookies.Append(key, token, new CookieOptions() { Expires = DateTime.Now.AddDays(7), HttpOnly = true });
                        return new RedirectResult("/home/index");

                    }
                }
                else
                {
                    result.Code = 0;
                    result.Msg = "只有指定员工可以参加投票";
                }
            }
            else
            {
                result.Code = 0;
                result.Msg = "工号不正确";
            }
            return JsonData(result);
        }
        public ActionResult Loginout()
        {
            HttpContext.Session.Clear();
            var key = "token-Year";
            Response.Cookies.Delete(key);
            AjaxResult result = new AjaxResult();
            result.Code = 1;
            return JsonData(result);

        }
        //异常


        [Route("/home/error")]
        public ActionResult Error(int code = 0)
        {
            ViewBag.code = code;
            ViewBag.errorTitle = HttpContext.Session.GetString("errorTitle");
            var message = HttpContext.Session.GetString("errorDetail");

            switch (code)
            {
                case 404:
                    message = "对不起，请求的资源不存在。";
                    break;
                case 401:
                    message = "对不起，您无权限访问此页面。";
                    break;
                case 500:
                    break;
                default:
                    message = "服务异常，请稍后重试！";
                    break;
            }

            ViewBag.Message = message;
            return View();
        }



        //public ActionResult AuthBack()
        //{

        //    var token = Request.Query["token"].FirstOrDefault();
        //    OaAccountService oaAccountService = new OaAccountService();
        //    var res = oaAccountService.Verify(token);
        //    if (res.Code == 1)
        //    {

        //        DapperHelper sqlHelper = new DapperHelper();
        //        if (userdata != null)
        //        {
        //            HttpContext.Session.SetString("XybLoginId", userdata.UserName.ToString());
        //            HttpContext.Session.SetString("XybNickName", userdata.NickName.ToString());
        //            HttpContext.Session.SetString("XybRid", userdata.RoleId.ToString());
        //            HttpContext.Session.SetString("XybUid", userdata.Id.ToString());

        //            {
        //                HttpContext.Session.SetString("Xybtokoen", token);
        //                var key = "token-newyb";// + Request.HttpContext.Connection.LocalPort;           
        //                Response.Cookies.Append(key, token, new CookieOptions() { Expires = DateTime.Now.AddDays(7), HttpOnly = true });

        //            }
        //            //内联框架 直接返回首页好处理些
        //            //var oldUrl = HttpContext.Session.GetString("oldUrl");
        //            //if (oldUrl != null && oldUrl != "")
        //            //{
        //            //    return new RedirectResult(oldUrl);
        //            //}
        //            return new RedirectResult("/home/index");
        //        }
        //        else
        //        {
        //            return new RedirectResult("/home/Empty");

        //        }
        //    }
        //    else
        //    {
        //        return Content("账号验证失败：" + res.Msg);
        //    }

        //}

        [AdminUserCheck]
        public ActionResult Index()
        {

            //ViewBag.oaapi = App.GetConfig<string>("oa:api");
            //ViewBag.oaapi = "http://localhost:56110";
            return View();
        }

        public ActionResult Empty()
        {

            ViewBag.oaapi = App.GetConfig<string>("oa:api");
            return View();
        }

        [AdminUserCheck]
        public ActionResult Default()
        {

            return View();
        }
        private DateTime getMonday()
        {
            DateTime now = DateTime.Now;
            DateTime temp = new DateTime(now.Year, now.Month, now.Day);
            int count = now.DayOfWeek - DayOfWeek.Monday;
            if (count == -1) count = 6;

            return temp.AddDays(-count);
        }

        public ActionResult ETest()
        {

            DateTime.Parse("123");
            return View();
        }


        [AdminUserCheck]

        //注册
        public ActionResult Register(string uid = "", string pwd = "")
        {
            return Json(new { Code = 1 });

        }

        [AdminUserCheck]
        public ActionResult ClearCacheAjax()
        {

            return Json(new { Code = 1 });
        }


        public ActionResult ClearSession(string pwd = "")
        {
            if (pwd != null && pwd.ToLower() == "rebuqi")
            {
                HttpContext.Session.Clear();
                return Content("操作成功");
            }
            else
            {
                return Content("密码错误");
            }

        }


        //public ActionResult ImageCode()
        //{

        //    string code;
        //    var img = WebUtil.CreatImgCode(out code);
        //    HttpContext.Session.SetString("imgcode", code);
        //    return File(img, "image/jpeg");
        //}


        //public ActionResult ImageCodeH()
        //{

        //    string code;
        //    var img = WebUtil.CreatImgCodeZH_CN(out code);
        //    HttpContext.Session.SetString("imgcode", code);
        //    return File(img, "image/jpeg");
        //}



    }

    public class ChartModel
    {
        public string Channel { get; set; }
        public string DataStr { get; set; }
        public decimal Counts { get; set; }

    }
}