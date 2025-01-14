using Achilles.Cqwy.SMS;
using AgileConfig.Client;
using Cqwy;
using Cqwy.FriendlyException;
using Cqwy.JsonSerialization;
using Microsoft.AspNetCore.DataProtection;
using Newtonsoft.Json.Converters;
using Serilog;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using WebSite.SignalR;



Console.WriteLine("=========初始化中=========" + DateTime.Now);
//var options = new WebApplicationOptions
//{
//    Args = args,
//    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
//};

var builder = WebApplication.CreateBuilder(args).Inject();
// Add services to the containerargs

var mvc = builder.Services.AddControllersWithViews().AddInjectBase().AddNewtonsoftJson(options =>
{
    //修改属性名称的序列化方式，首字母小写
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

    //防止Json解析无限套娃
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    //修改时间的序列化方式
    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });
}).AddDynamicApiControllers().AddInjectWithUnifyResult();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
builder.Services.AddCorsAccessor();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});
builder.Services.AddSignalR();

builder.Services.AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    options.JsonSerializerOptions.Converters.Add(new SystemTextJsonDateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
});
//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(60 * 24);
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = false;
//    options.Cookie.SecurePolicy = CookieSecurePolicy.None;
//});
builder.Services.AddSession();
builder.Services.AddDataProtection().PersistKeysToFileSystem(new DirectoryInfo(Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "DataProtection")); ;
//builder.Services.AddDatabaseLogging<DatabaseLoggingWriter>();
builder.Host.ConfigureAppConfiguration((context, config) =>
{
    //注入AgileConfig Configuration Provider
    var client = new ConfigClient();
    client.ConfigChanged += (x) => {
        Console.WriteLine("配置更新:" + x.ToJson());
    };
    config.AddAgileConfig(client);
});
builder.Services.AddAchillesCqwySMS(App.GetConfig<string>("gongdan:api"), App.GetConfig<string>("cpk:api8081"));

AddServices(builder);//服务注册入口



#region serilog 配置
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//    //.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
//    .Enrich.FromLogContext()
//    .Enrich.WithProperty("Application", "国际电商网站")
//    .Enrich.FromLogContext()
//    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(App.GetConfig<string>("log:es")))
//    {
//        TypeName = null,
//        BatchAction = ElasticOpType.Create,
//        IndexFormat = "logstash-gjds-{0:yyyy.MM.dd}",//索引名称规则
//        EmitEventFailure = EmitEventFailureHandling.RaiseCallback,//失败处理方式 FailureCallback或者 FailureSink或者抛异常等
//        FailureCallback = e => Console.WriteLine("Unable to submit event ---" + e.MessageTemplate),//失败触发回调
//        FailureSink = new FileSink("./logs/Failure.log", new JsonFormatter(), 500 * 1024),//失败记录到文件
//        AutoRegisterTemplate = true,
//        OverwriteTemplate = true,
//        CustomFormatter= new EsLogFormatter(),
//        AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
//        ModifyConnectionSettings =
//        conn =>
//        {
//            conn.ServerCertificateValidationCallback((source, certificate, chain, sslPolicyErrors) => true);
//            conn.BasicAuthentication(App.GetConfig<string>("log:esuer"), App.GetConfig<string>("log:espwd"));
//            return conn;
//        }

//    }).WriteTo.Console()
//    .CreateLogger();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.WithProperty("Application", "新云报系统")
    .Enrich.FromLogContext()
    .CreateLogger();
#endregion serilog 配置

builder.Host.UseSerilog();
//builder.Host.UseWindowsService();

Console.WriteLine("=========构建服务=========" + DateTime.Now);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseSession();
app.UseCookiePolicy();
//app.UseMiddleware<LocalMiddleware>();


app.UseCorsAccessor();
app.UseInjectBase();//.UseSpecificationDocuments();
app.UseRouting();
//在Controller或Filter中读取报文需要这个配置
app.Use(async (context, next) =>
{
    context.Request.EnableBuffering();
    await next.Invoke();

});

app.UseStatusCodePagesWithReExecute("/Home/Error", "?code={0}");
app.UseStaticHostEnviroment();
app.UseEndpoints(endpoints =>
{
  
    endpoints.MapControllerRoute(name: "areas", pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllerRoute(name: "Default", pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapControllers();
});
app.UseCors("AllowAllOrigins");
app.MapHub<RebateHub>("/rebateHub");

Console.WriteLine("=========启动服务=========" + DateTime.Now);
{
    //国际CSD运价刷新接口测试-郭泽
    //XieChengApi xieChengApi = new XieChengApi();
    //Domain.OtaModel.XieCheng.CSDRateRefreshRequest cSDRateRefreshRequest = new Domain.OtaModel.XieCheng.CSDRateRefreshRequest();
    //cSDRateRefreshRequest.agentCode = "CWYL";
    //cSDRateRefreshRequest.cid = "3505";
    //cSDRateRefreshRequest.fromCity = "SHA";
    //cSDRateRefreshRequest.fromDateRange = "20230519-20230520";
    //cSDRateRefreshRequest.toCity = "DPS";
    //cSDRateRefreshRequest.retDateRange = "";
    //cSDRateRefreshRequest.market = "TH";
    //var aa = xieChengApi.CSDRateRefreshToken();
    //var bb = xieChengApi.CSDRateRefresh(cSDRateRefreshRequest, aa.tokens[0].tokenValue);


    
}

app.Run();
/// <summary>
/// 服务由此入
/// </summary>

void AddServices(WebApplicationBuilder builder)
{



}