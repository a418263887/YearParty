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



Console.WriteLine("=========��ʼ����=========" + DateTime.Now);
//var options = new WebApplicationOptions
//{
//    Args = args,
//    ContentRootPath = WindowsServiceHelpers.IsWindowsService() ? AppContext.BaseDirectory : default
//};

var builder = WebApplication.CreateBuilder(args).Inject();
// Add services to the containerargs

var mvc = builder.Services.AddControllersWithViews().AddInjectBase().AddNewtonsoftJson(options =>
{
    //�޸��������Ƶ����л���ʽ������ĸСд
    options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();

    //��ֹJson������������
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    //�޸�ʱ������л���ʽ
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
    //ע��AgileConfig Configuration Provider
    var client = new ConfigClient();
    client.ConfigChanged += (x) => {
        Console.WriteLine("���ø���:" + x.ToJson());
    };
    config.AddAgileConfig(client);
});
builder.Services.AddAchillesCqwySMS(App.GetConfig<string>("gongdan:api"), App.GetConfig<string>("cpk:api8081"));

AddServices(builder);//����ע�����



#region serilog ����
//Log.Logger = new LoggerConfiguration()
//    .MinimumLevel.Debug()
//    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
//    //.MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
//    .Enrich.FromLogContext()
//    .Enrich.WithProperty("Application", "���ʵ�����վ")
//    .Enrich.FromLogContext()
//    .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(App.GetConfig<string>("log:es")))
//    {
//        TypeName = null,
//        BatchAction = ElasticOpType.Create,
//        IndexFormat = "logstash-gjds-{0:yyyy.MM.dd}",//�������ƹ���
//        EmitEventFailure = EmitEventFailureHandling.RaiseCallback,//ʧ�ܴ���ʽ FailureCallback���� FailureSink�������쳣��
//        FailureCallback = e => Console.WriteLine("Unable to submit event ---" + e.MessageTemplate),//ʧ�ܴ����ص�
//        FailureSink = new FileSink("./logs/Failure.log", new JsonFormatter(), 500 * 1024),//ʧ�ܼ�¼���ļ�
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
    .Enrich.WithProperty("Application", "���Ʊ�ϵͳ")
    .Enrich.FromLogContext()
    .CreateLogger();
#endregion serilog ����

builder.Host.UseSerilog();
//builder.Host.UseWindowsService();

Console.WriteLine("=========��������=========" + DateTime.Now);

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
//��Controller��Filter�ж�ȡ������Ҫ�������
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

Console.WriteLine("=========��������=========" + DateTime.Now);
{
    //����CSD�˼�ˢ�½ӿڲ���-����
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
/// �����ɴ���
/// </summary>

void AddServices(WebApplicationBuilder builder)
{



}