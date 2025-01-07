
using AngleSharp.Io;
using AngleSharp.Io.Dom;
using Cqwy;
using Cqwy.DatabaseAccessor;
using Cqwy.SpecificationDocument;
using Domain.Model;
using Domain.Model.AjaxModel;
using Domain.Model.Common;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Drawing.Imaging;
using System.Drawing;
using System.Net.WebSockets;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Util.Ext;
using WebSite.Filters;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;
using System.Drawing.Drawing2D;

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
        protected AjaxResult ajaxResult = new AjaxResult();
        protected string uid { get { return HttpContext.Session.GetString("Uid"); } }
        protected User UserInfo
        {
            get
            {
                var key = $"YearUserInfo:{uid}";
                try
                {
                    var u = CacheHelper.GetCache(key) as User;
                    if (u == null)
                    {
                        u = repository.DetachedEntities.FirstOrDefault(p => p.GH == uid);
                        CacheHelper.SetCache(key, u, 30);
                    }
                    return u;
                }
                catch (Exception ex)
                {

                    return new User() { GH = "unknow", Name = "未知用户" };

                }

            }
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
        public ActionResult AjaxLogin(string uid)
        {

            string userid = uid;



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
                        //return new ContentResult() { Content = "<script>window.top.location.href='/Home/Index';</script>", ContentType = "text/html" };
                        result.Code = 1;
                        result.Info = "/home/index";
                        //return new RedirectResult("/home/index");
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
                        result.Code = 1;
                        result.Info = "/home/index";
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
        public ActionResult Paihang()
        {
            var guize = repository.Change<Setting>().DetachedEntities.First();
            if (guize.isNotNull())
            {
                guize.Bma = guize.BmTimeStart.ToString("MM-dd HH:mm");
                guize.Bmb = guize.BmTimeEnd.ToString("MM-dd HH:mm");
                guize.Tpa = guize.TpTimeStart.ToString("MM-dd HH:mm");
                guize.Tpb = guize.TpTimeEnd.ToString("MM-dd HH:mm");
            }
            ViewBag.Guize = guize.ToJson();
            return View();
        }
        [AdminUserCheck]
        public ActionResult Index()
        {
            var uu = repository.DetachedEntities.FirstOrDefault(x => x.Id == UserInfo.Id);

            var guize = repository.Change<Setting>().DetachedEntities.First();
            if (guize.isNotNull())
            {
                guize.Bma = guize.BmTimeStart.ToString("MM-dd HH:mm");
                guize.Bmb = guize.BmTimeEnd.ToString("MM-dd HH:mm");
                guize.Tpa = guize.TpTimeStart.ToString("MM-dd HH:mm");
                guize.Tpb = guize.TpTimeEnd.ToString("MM-dd HH:mm");
            }
            ViewBag.User = uu.ToJson();
            ViewBag.Guize = guize.ToJson();
            return View();
        }
        [AdminUserCheck]
        public ActionResult NamIndex()
        {
            var uu = repository.DetachedEntities.FirstOrDefault(x => x.Id == UserInfo.Id);
            var guize = repository.Change<Setting>().DetachedEntities.First();
            if (guize.isNotNull())
            {
                guize.Bma = guize.BmTimeStart.ToString("MM-dd HH:mm");
                guize.Bmb = guize.BmTimeEnd.ToString("MM-dd HH:mm");
                guize.Tpa = guize.TpTimeStart.ToString("MM-dd HH:mm");
                guize.Tpb = guize.TpTimeEnd.ToString("MM-dd HH:mm");
            }
            ViewBag.User = uu.ToJson();
            ViewBag.Guize = guize.ToJson();
            return View();
        }
        [AdminUserCheck]
        public IActionResult TouPiao(string Gh, string Sex)
        {
            AjaxResult ajaxResult = new() { Code = 0 };

            try
            {
                var uu = repository.DetachedEntities.FirstOrDefault(x => x.Id == UserInfo.Id);
                if (uu.PiaoCount <= 0)
                {
                    ajaxResult.Msg = "你没有票了哦";
                    return Json(ajaxResult);
                }
                var yitou = uu.YiTou.Split2();
                if (yitou.Contains(Gh))
                {
                    string xb = Sex == "男" ? "他" : "她";
                    ajaxResult.Msg = $"已经给{xb}投过{xb}票了";
                    return Json(ajaxResult);
                }


                var guize = repository.Change<Setting>().DetachedEntities.First();

                var pp = repository.Change<BaoMing>().DetachedEntities.FirstOrDefault(x => x.Gh == Gh);
                if (!(DateTime.Now >= guize.TpTimeStart && DateTime.Now <= guize.TpTimeEnd))
                {
                    ajaxResult.Msg = $"{guize.TpTimeStart:MM-dd HH:mm}至{guize.TpTimeEnd:MM-dd HH:mm}期间才可以投票哦";
                    return Json(ajaxResult);
                }
                if (Gh == uu.GH)
                {
                    ajaxResult.Msg = "不可以投票给自己";
                    return Json(ajaxResult);
                }

                if (Sex == "男" && uu.Man == 1)
                {
                    ajaxResult.Msg = "你已经投了一票男神哦";
                    return Json(ajaxResult);
                }
                if (Sex == "男" && uu.WoMan >= 3)
                {
                    ajaxResult.Msg = "你已经投了三票女神了";
                    return Json(ajaxResult);
                }
                using (var transaction = repository.Database.BeginTransaction())
                {
                    try
                    {
                        pp.Count += 1;
                        repository.Change<BaoMing>().Update(pp);
                        repository.Change<BaoMing>().SaveNow();
                        uu.PiaoCount -= 1;
                        if (Sex == "男")
                        {
                            uu.Man += 1;
                        }
                        else
                        {
                            uu.WoMan += 1;
                        }
                        yitou.Add(Gh);
                        uu.YiTou = string.Join(",", yitou);
                        repository.Update(uu);
                        repository.SaveNow();

                        // 提交事务
                        transaction.Commit();
                        ajaxResult.Code = 1;
                        return Json(ajaxResult);

                    }
                    catch (Exception ex)
                    {
                        // 回滚事务
                        // transaction.RollBack(); // 新版本自动回滚了
                        ajaxResult.Msg = $"系统出错:{ex.Message}";
                        return Json(ajaxResult);
                    }
                }

            }
            catch (Exception ex)
            {
                ajaxResult.Msg = $"系统出错:{ex.Message}";
                return Json(ajaxResult);
            }
        }
        [AdminUserCheck]
        public IActionResult SearchPage(string Sousuo, string Sex, int page, int limit)
        {
            AjaxResult ajaxResult = new() { Code = 0 };
            try
            {




                var where = PredicateBuilder.New<BaoMing>(true);
                if (Sousuo.isNotNull())
                    where.And(x => x.Name.Contains(Sousuo.Replace(" ", "")) || x.Gh.Contains(Sousuo.Replace(" ", "")));


                where.And(x => x.Sex == Sex);


                var data = repository.Change<BaoMing>().DetachedEntities.Where(where).OrderByDescending(i => i.Id).ToPagedList(page, limit);




                ajaxResult.Data = data.Items;
                ajaxResult.Code = 1;
                ajaxResult.TotalCount = data.TotalCount;


            }
            catch (Exception ex)
            {
                ajaxResult.Msg = ex.Message;
            }
            return Json(ajaxResult);
        }



        [AdminUserCheck]
        public IActionResult PaiHangSearchPage()
        {
            AjaxResult ajaxResult = new() { Code = 0 };
            try
            {



                var data = repository.Change<BaoMing>().DetachedEntities.OrderByDescending(i => i.Count).ToList();




                ajaxResult.Data = data;
                ajaxResult.Code = 1;
                ajaxResult.TotalCount = data.Count;


            }
            catch (Exception ex)
            {
                ajaxResult.Msg = ex.Message;
            }
            return Json(ajaxResult);
        }


        [AdminUserCheck]
        public IActionResult BaomingClick()
        {
            AjaxResult ajaxResult = new() { Code = 0 };

            try
            {
                var uu = repository.DetachedEntities.FirstOrDefault(x => x.Id == UserInfo.Id);
                if (uu.IsBaoming == 1)
                {
                    ajaxResult.Msg = "你已经报名过了";
                    return Json(ajaxResult);
                }


                var guize = repository.Change<Setting>().DetachedEntities.First();
                if (!(DateTime.Now >= guize.BmTimeStart && DateTime.Now <= guize.BmTimeEnd))
                {
                    ajaxResult.Msg = $"{guize.BmTimeStart:MM-dd HH:mm}至{guize.BmTimeEnd:MM-dd HH:mm}期间才可以报名哦";
                    return Json(ajaxResult);
                }
                ajaxResult.Code = 1;
                return Json(ajaxResult);

            }
            catch (Exception ex)
            {
                ajaxResult.Msg = $"系统出错:{ex.Message}";
                return Json(ajaxResult);
            }
        }


        [AdminUserCheck]
        public IActionResult BaomingUpdateClick()
        {
            AjaxResult ajaxResult = new() { Code = 0 };

            try
            {



                var guize = repository.Change<Setting>().DetachedEntities.First();
                if (!(DateTime.Now >= guize.BmTimeStart && DateTime.Now <= guize.BmTimeEnd))
                {
                    ajaxResult.Msg = $"{guize.BmTimeStart:MM-dd HH:mm}至{guize.BmTimeEnd:MM-dd HH:mm}期间才可以修改哦";
                    return Json(ajaxResult);
                }
                ajaxResult.Code = 1;
                return Json(ajaxResult);

            }
            catch (Exception ex)
            {
                ajaxResult.Msg = $"系统出错:{ex.Message}";
                return Json(ajaxResult);
            }
        }
        [AdminUserCheck]
        public ActionResult Upload()
        {
            var baoMing=repository.Change<BaoMing>().DetachedEntities.FirstOrDefault(x=>x.Gh==UserInfo.GH);
            baoMing=baoMing ?? new() { Gh = UserInfo.GH, Name = UserInfo.Name, Sex = UserInfo.Sex };
            ViewBag.User = baoMing.ToJson();
            return View();
        }






        [AdminUserCheck]
        public async Task<IActionResult> FileUpload(string json)
        {
            try
            {

                #region 文件上传
                var files = Request.Form.Files;

                var dircstr = $"Uploads/IntlStudent";
                var txcstr = $"Uploads/TX";
                var dtxcstr = $"Uploads/DTX";
                var input = json.ToObject<BaoMing>();
                List<FileList> newFiles = new();

                foreach (var file in files)
                {
                    var Oldfilename = Path.GetFileName(file.FileName);
                    if (Oldfilename.isNull()) continue;

                    string singelfinename = $"File{ToolHelper.SnowflakeNumber()}";
                    #region 多保存一份头像文件
                    using (var stream = new MemoryStream())
                    {
                        await file.CopyToAsync(stream);
                        using (var originalImage = Image.FromStream(stream))
                        {



                            var txsavePath = Path.Combine(App.WebHostEnvironment.WebRootPath, txcstr);
                            if (!Directory.Exists(txsavePath)) Directory.CreateDirectory(txsavePath);

                            int originalWidth = originalImage.Width;
                            int originalHeight = originalImage.Height;

                            // 计算裁剪的中心点
                            int centerX = originalWidth / 2;
                            int centerY = originalHeight / 2;

                            // 创建一个500px的圆形裁剪区域
                            int diameter = originalWidth> originalHeight? originalHeight: originalWidth;
                            Rectangle cropRect = new Rectangle(centerX - diameter / 2, centerY - diameter / 2, diameter, diameter);

                            // 创建用于裁剪的图像
                            using (var croppedImage = new Bitmap(diameter, diameter))
                            {
                                // 创建图形对象
                                using (var g = Graphics.FromImage(croppedImage))
                                {
                                    // 创建圆形区域
                                    using (var path = new GraphicsPath())
                                    {
                                        path.AddEllipse(0, 0, diameter, diameter);
                                        g.SetClip(path);

                                        // 在裁剪区域内绘制原始图像
                                        g.DrawImage(originalImage, new Rectangle(-cropRect.X, -cropRect.Y, originalWidth, originalHeight));
                                    }
                                }

                                // 将裁剪后的图像缩放到50px
                                using (var resizedImage = new Bitmap(croppedImage, new Size(50, 50)))
                                {
                                    // 设置保存文件的路径
                                    string txfileName = $"{singelfinename}.png"; // 使用 GUID 生成唯一文件名
                                    string txfilePath = Path.Combine(txsavePath, txfileName);
                                    // 保存最终图像
                                    resizedImage.Save(txfilePath, ImageFormat.Png);
                                }
                            }
                        }
                        using (var originalImage = Image.FromStream(stream))
                        {



                            var DtxsavePath = Path.Combine(App.WebHostEnvironment.WebRootPath, dtxcstr);
                            if (!Directory.Exists(DtxsavePath)) Directory.CreateDirectory(DtxsavePath);

                            int originalWidth = originalImage.Width;
                            int originalHeight = originalImage.Height;

                            // 计算裁剪的中心点
                            int centerX = originalWidth / 2;
                            int centerY = originalHeight / 2;

                            // 创建一个500px的圆形裁剪区域
                            int diameter = originalWidth > originalHeight ? originalHeight : originalWidth;
                            Rectangle cropRect = new Rectangle(centerX - diameter / 2, centerY - diameter / 2, diameter, diameter);

                            // 创建用于裁剪的图像
                            using (var croppedImage = new Bitmap(diameter, diameter))
                            {
                                // 创建图形对象
                                using (var g = Graphics.FromImage(croppedImage))
                                {
                                    // 创建圆形区域
                                    using (var path = new GraphicsPath())
                                    {
                                        path.AddEllipse(0, 0, diameter, diameter);
                                        g.SetClip(path);

                                        // 在裁剪区域内绘制原始图像
                                        g.DrawImage(originalImage, new Rectangle(-cropRect.X, -cropRect.Y, originalWidth, originalHeight));
                                    }
                                }

                                // 将裁剪后的图像缩放到50px
                                using (var resizedImage = new Bitmap(croppedImage, new Size(300, 300)))
                                {
                                    // 设置保存文件的路径
                                    string txfileName = $"{singelfinename}.png"; // 使用 GUID 生成唯一文件名
                                    string DtxfilePath = Path.Combine(DtxsavePath, txfileName);
                                    // 保存最终图像
                                    resizedImage.Save(DtxfilePath, ImageFormat.Png);
                                }
                            }
                        }

                      
                    }
                    #endregion


                    //if (input.Id > 0)
                    //{
                    //    if (input.FileList.Any(x => x.name == Oldfilename))
                    //    {
                    //        newFiles.Add(input.FileList.First(x => x.name == Oldfilename));
                    //        continue;
                    //    }
                    //}

                    var fileext = Path.GetExtension(Oldfilename).ToLower();
                    // 如：保存到网站根目录下的 uploads 目录
                    //var savePath = Path.Combine(App.HostEnvironment.ContentRootPath, dircstr);
                    var savePath = Path.Combine(App.WebHostEnvironment.WebRootPath, dircstr);
                    if (!Directory.Exists(savePath)) Directory.CreateDirectory(savePath);

                    //// 这里还可以获取文件的信息
                    // var size = file.Length / 1024.0;  // 文件大小 KB
                    // var clientFileName = file.FileName; // 客户端上传的文件名
                    // var contentType = file.ContentType; // 获取文件 ContentType 或解析 MIME 类型

                    // 避免文件名重复，采用 雪花ID 生成
                    var fileName = $"{singelfinename}{Path.GetExtension(file.FileName)}";
                    var filePath = Path.Combine(savePath, fileName);
                    var DownPath = $"/{dircstr}/{fileName}";
                    // 保存到指定路径
                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await file.CopyToAsync(stream);
                    }






                    FileList fileList = new()
                    {
                        name = Oldfilename,
                        url = DownPath
                    };
                    newFiles.Add(fileList);




                }
                input.FilePath = newFiles[0].url;
                var baom = repository.Change<BaoMing>().DetachedEntities.FirstOrDefault(x => x.Gh == input.Gh);
                if (baom.isNotNull())
                {
                    input.Id = baom.Id;
                    await repository.Change<BaoMing>().UpdateNowAsync(input);
                }
                else
                {
                    await repository.Change<BaoMing>().InsertNowAsync(input);
                }
                var uu = repository.DetachedEntities.FirstOrDefault(x => x.Id == UserInfo.Id);
                uu.IsBaoming = 1;
                await repository.UpdateNowAsync(uu);
                ajaxResult.Code = 1;
                #endregion

            }
            catch (Exception ex)
            {

                ajaxResult.Code = -1;
                ajaxResult.Msg = ex.Message;
            }
            return Json(ajaxResult);
        }


        

        [AdminUserCheck]
        public ActionResult Jieshao()
        {
            var uu = repository.DetachedEntities.FirstOrDefault(x => x.Id == UserInfo.Id);

            var guize = repository.Change<Setting>().DetachedEntities.First();
            if (guize.isNotNull())
            {
                guize.Bma = guize.BmTimeStart.ToString("MM-dd HH:mm");
                guize.Bmb = guize.BmTimeEnd.ToString("MM-dd HH:mm");
                guize.Tpa = guize.TpTimeStart.ToString("MM-dd HH:mm");
                guize.Tpb = guize.TpTimeEnd.ToString("MM-dd HH:mm");
            }
            ViewBag.User = uu.ToJson();
            ViewBag.Guize = guize.ToJson();
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