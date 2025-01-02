using System.Data;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebSite.Filters;
using Cqwy.DatabaseAccessor;
using EFCore.DbContexts;
using Util.Ext;
using Domain.Model.AjaxModel;
using Domain.Model;

namespace WebSite.Controllers
{
    [AdminException]
    [AdminUserCheck]
    //页面不允许写入session   允许写入的话 session会有锁  同一用户 一个页面在加载中就打不开其他页面了

    public class AdminBaseController : Controller
    {
        private  readonly IRepository<User> repository;
        public AdminBaseController() 
        {
            this.repository = Db.GetRepository<User>(); 
        }



        protected DateTime initTime = new DateTime(1970, 1, 1);
        protected string uid { get { return HttpContext.Session.GetString("Uid"); } }
        protected const int pageSize = 10;
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
                catch (Exception ex){

                    return  new User() { GH="unknow",Name="未知用户"};

                }

            }
        }
        protected AjaxResult ajaxResult = new AjaxResult();


        //public void InitData()
        //{
        //    SetUserInfo();
        //}

        //private void SetUserInfo()
        //{
        //    long uid = HttpContext.Session.GetString("XybUid").ToBigInt();
        //    // UserInfo = data.AdminUserInfo.AsNoTracking().FirstOrDefault(p => p.uid == uid);
        //}

        //private Sys_User GetUserInfo()
        //{
        //    if (UserInfo == null)
        //    {
        //        long uid = HttpContext.Session.GetString("XybUid").ToBigInt();
        //        //UserInfo = data.AdminUserInfo.AsNoTracking().FirstOrDefault(p => p.uid == uid);
        //    }
        //    return UserInfo;


        //}

        /// <summary>
        /// 根据主键获取实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetByKey<T>(long id) where T : class
        {
            var data = new object();
            using (DefaultDbContext context = (DefaultDbContext)Db.GetDbContext())
            {
                data = context.Set<T>().Find(id);
            }
            return data as T;

        }


        /// <summary>
        /// 添加实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        public int AddData<T>(T entity) where T : class
        {
            using (DefaultDbContext context = (DefaultDbContext)Db.GetDbContext())
            {
                context.Set<T>().Add(entity);
                return context.SaveChanges();
            }
        }



        /// <summary>
        /// 更新实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="entity"></param>
        public int UpdateData<T>(long id, T entity) where T : class
        {

            using (DefaultDbContext context = (DefaultDbContext)Db.GetDbContext())
            {
                var data = context.Set<T>().Find(id);
                Type type = typeof(T);
                foreach (var item in type.GetProperties())
                {
                    var typeinfo = type.GetProperty(item.Name);
                    typeinfo.SetValue(data, typeinfo.GetValue(entity, null), null);
                }

                return context.SaveChanges();
            }
        }
        //public PathInfo SaveFile(IFormFile file, string rootPath = "~/upload")
        //{
        //    Stopwatch sw = new Stopwatch(); sw.Start();

        //    string savename = Guid.NewGuid().ToString().Replace("-", "");
        //    string extension = Path.GetExtension(file.FileName);
        //    string cfmc = savename + extension;
        //    string filepath = string.Empty;
        //    if (!Directory.Exists(CoreHttpContext.MapPath(rootPath)))
        //        Directory.CreateDirectory(CoreHttpContext.MapPath(rootPath));
        //    filepath = CoreHttpContext.MapPath(rootPath) + @"\" + cfmc;


        //    using (var stream = new FileStream(filepath, FileMode.Create))
        //    {
        //        file.CopyTo(stream);
        //        stream.Flush();
        //    }



        //    PathInfo pathInfo = new PathInfo();
        //    pathInfo.AbsolutPath = filepath;
        //    pathInfo.RelativePath = rootPath + "/" + cfmc;
        //    pathInfo.FileName = cfmc;
        //    pathInfo.UploadFileName = file.FileName;
        //    return pathInfo;
        //}

        //public List<TicketFileinfo> SaveFile(IFormFileCollection files, string rootPath = "~/upload")
        //{
        //    var res = new List<TicketFileinfo>();
        //    Stopwatch sw = new Stopwatch(); sw.Start();


        //    foreach (var file in files)
        //    {
        //        string savename = Guid.NewGuid().ToString().Replace("-", "");
        //        string extension = Path.GetExtension(file.FileName);
        //        string cfmc = savename + extension;
        //        string filepath = string.Empty;
        //        if (!Directory.Exists(CoreHttpContext.MapPath(rootPath)))
        //            Directory.CreateDirectory(CoreHttpContext.MapPath(rootPath));
        //        filepath = CoreHttpContext.MapPath(rootPath) + @"\" + cfmc;


        //        using (var stream = new FileStream(filepath, FileMode.Create))
        //        {
        //            file.CopyTo(stream);
        //            stream.Flush();
        //        }

        //        var t = new TicketFileinfo()
        //        {
        //            FileName = file.FileName,
        //            FileUrl = "/" + cfmc,
        //            UpTime = DateTime.Now,
        //            UpUser = UserInfo.NickName
        //        };
        //        res.Add(t);


        //    }
        //    return res;
        //}


        //public ActionResult DownLoadExcel(DataTable table)
        //{

        //    var virPath = "~/OutFile/Excel/" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
        //    var path = CoreHttpContext.MapPath(virPath);
        //    NPOIExcel nPOIExcel = new NPOIExcel(path);
        //    nPOIExcel.SaveExcel(table);
        //    return new RedirectResult(Url.Content(virPath));

        //}

        //public ActionResult DownLoadExcelAjax(DataTable table)
        //{
        //    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
        //    var virPath = "~/OutFile/Excel/" + fileName;
        //    var path = CoreHttpContext.MapPath(virPath);
        //    NPOIExcel nPOIExcel = new NPOIExcel(path);
        //    nPOIExcel.SaveExcel(table);
        //    ajaxResult.Code = 1;
        //    ajaxResult.Data = Url.Content(virPath);
        //    ajaxResult.Other = fileName;
        //    ajaxResult.Msg = "生成成功";
        //    return Json(ajaxResult);

        //}
        //public ActionResult DownLoadExcelAjax<T>(List<T> table)
        //{
        //    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
        //    var virPath = "~/OutFile/Excel/" + fileName;
        //    var path = CoreHttpContext.MapPath(virPath);
        //    NPOIExcel nPOIExcel = new NPOIExcel(path);
        //    nPOIExcel.SaveExcel(table);
        //    ajaxResult.Code = 1;
        //    ajaxResult.Data = Url.Content(virPath);
        //    ajaxResult.Other = fileName;
        //    ajaxResult.Msg = "生成成功";
        //    return Json(ajaxResult);

        //}



        //public ActionResult DownLoadCsvAjax(DataTable table)
        //{
        //    AjaxResult ajaxResult = new AjaxResult();
        //    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
        //    var virPath = "~/OutFile/Excel/" + fileName;
        //    var path = CoreHttpContext.MapPath(virPath);

        //    Stopwatch sw = new Stopwatch(); sw.Start();
        //    int col = table.Columns.Count;
        //    List<string[]> ls = new List<string[]>();

        //    for (int i = 0; i < table.Rows.Count; i++)
        //    {
        //        ls.Add(table.Rows[i].ItemArray.Select(x => x.ToString()).ToArray());

        //    }
        //    var cols = new List<string>();
        //    foreach (DataColumn item in table.Columns)
        //    {
        //        cols.Add(item.Caption);
        //    }
        //    var csv = CsvWriter.WriteToText(cols.ToArray(), ls, ',');
        //    System.IO.File.WriteAllText(path, csv, System.Text.Encoding.UTF8);

        //    ajaxResult.Msg += $"生成报表用时：{sw.ElapsedMilliseconds}";

        //    ajaxResult.Code = 1;
        //    ajaxResult.Data = Url.Content(virPath);
        //    ajaxResult.Other = fileName;

        //    return Json(ajaxResult);

        //}
        //public DataTable ExcelToTable(string path)
        //{

        //    NPOIExcel nPOI = new NPOIExcel(path);
        //    var table = nPOI.ExcelToDataTable();
        //    return table;
        //}

        //public DataTable ExcelToTable(string path)
        //{

     
        //    var table = EPPlusHelper<object>.WorksheetToTable(path);
        //    return table;
        //}


        //public ActionResult DownLoadCsvAjax(List<string> headlers, IEnumerable<string[]> datas, AjaxResult ajaxResult)
        //{
        //    string fileName = DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".csv";
        //    var virPath = "~/OutFile/Excel/" + fileName;
        //    var path = CoreHttpContext.MapPath(virPath);

        //    Stopwatch sw = new Stopwatch(); sw.Start();
        //    var csv = CsvWriter.WriteToText(headlers.ToArray(), datas, ',');
        //    System.IO.File.WriteAllText(path, csv, System.Text.Encoding.UTF8);
        //    ajaxResult.Msg += $"生成报表用时：{sw.ElapsedMilliseconds}";

        //    ajaxResult.Code = 1;
        //    ajaxResult.Data = Url.Content(virPath);
        //    ajaxResult.Other = fileName;

        //    return Json(ajaxResult);

        //}



        public ActionResult JsonData(object obj)
        {
            return Content(obj.ToJson(), "application/json");
        }



        /// <summary>
        /// 压缩duo个文件
        /// </summary>
        /// <param name="list">要进行压缩的文件名</param>
        /// <param name="zipurl">压缩后生成的压缩文件名</param>
        //public static void ZipFile(List<string> list, string zipurl)
        //{

        //    foreach (string path in list)
        //    {
        //        //如果文件没有找到，则报错
        //        if (!System.IO.File.Exists(path))
        //        {
        //            throw new System.IO.FileNotFoundException("指定要压缩的文件: " + path + " 不存在!");
        //        }
        //    }

        //    using (FileStream ZipFile = System.IO.File.Create(zipurl))
        //    {
        //        using (ICSharpCode.SharpZipLib.Zip.ZipOutputStream ZipStream = new ICSharpCode.SharpZipLib.Zip.ZipOutputStream(ZipFile))
        //        {
        //            foreach (string url in list)
        //            {
        //                string fileName = url.Substring(url.LastIndexOf("\\"));
        //                using (FileStream fs = System.IO.File.OpenRead(url))
        //                {
        //                    byte[] buffer = new byte[fs.Length];
        //                    fs.Read(buffer, 0, buffer.Length);
        //                    fs.Close();

        //                    ICSharpCode.SharpZipLib.Zip.ZipEntry zipEntry = new ICSharpCode.SharpZipLib.Zip.ZipEntry(fileName);
        //                    ZipStream.PutNextEntry(zipEntry);
        //                    ZipStream.SetLevel(0);

        //                    ZipStream.Write(buffer, 0, buffer.Length);
        //                }
        //            }
        //            ZipStream.Finish();
        //            ZipStream.Close();
        //        }
        //    }
        //}

    }
}