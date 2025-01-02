
using AgileConfig.Client;

using Microsoft.AspNetCore.SignalR;
using Cqwy;
using EFCore.DbContexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System;
using Microsoft.Extensions.Configuration;
using Mapster;
using Microsoft.AspNetCore.Builder;
using Elasticsearch.Net;
using System.Text.RegularExpressions;


namespace WinApp
{
    public static class Program
    {
        public static IServiceProvider Services { get; private set; }
        public static Action<int, string> ulog;
        public  static Action<int, string> ulog2;
        public static MainForm main;

        [STAThread]
        static void Main()
        {


            // 创建一个服务容器
            //IServiceCollection? services = Inject.Create();

            var host = Serve.Run(GenericRunOptions.Default.ConfigureBuilder(builder =>
            builder.ConfigureAppConfiguration((context, config) =>
            {

                //注入AgileConfig Configuration Provider
                var client = new ConfigClient();
                client.ConfigChanged += (x) =>
                {
                    Console.WriteLine("配置更新:");
                };
                config.AddAgileConfig(client);


            })).Silence(true));   // 静默启动
            Services = host.Services;
           
            System.Windows.Forms.Application.SetHighDpiMode(HighDpiMode.SystemAware);
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);


            #region serilog 配置       
            Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(App.Configuration)
            .Enrich.WithProperty("Application", "新云报")
            .Enrich.FromLogContext()
            .CreateLogger();


            #endregion serilog 配置    
            //var aaa=Applications.AsmsEterm.RegexUtil.RegularUntil.RegularMore(" 1.WANG/FEI MR KW9WXK\n 2.  AF111  U   MO23SEP  PVGCDG HK1   2210 0630+1    SEAME 1 2E                 \n 3.  AF116  H   WE16OCT  CDGPVG HK1   2220 1655+1    SEAME 2E1                  \n 4.CKG/T CKG/T 023-63875333/CKG WANG YI AVIATION SERVICE LTD.CO/ZHU YU\n 5.T\n 6.SSR OTHS 1E OIN CNO5010 NOT FOUND\n 7.SSR OTHS 1E AF 116 16OCT APIS DEST PAX DATA REQUIRED SSR DOCS\n 8.SSR OTHS 1E AF 111 23SEP APIS DEST PAX DATA REQUIRED SSR DOCS\n 9.SSR ADTK 1E TO AF BY 21SEP 2100 CKG OTHERWISE WILL BE XLD\n10.SSR TKNE AF HK1 PVGCDG 111 U23SEP 0576317336085/1/P1\n11.SSR TKNE AF HK1 CDGPVG 116 H16OCT 0576317336085/2/P1\n12.SSR DOCS AF HK1 P/CN/PE2342946/CN/21OCT87/M/09SEP29/WANG/FEI MR/P1\n13.SSR CTCE AF HK1 15310093959//163.COM/P1\n14.SSR CTCM AF HK1 17805053494/P1\n15.OSI AF CTCT15111963059\n16.OSI AF OIN CNO5010\n17.RMK TJ CKG121\n18.RMK 1A/6DQUT8\n19.RMK FARECODE/T031898/AF111/PVGCDG/23SEP24/CNY14840.00\n20.RMK FARECODE/T031898/AF116/CDGPVG/16OCT24/CNY14840.00\n21.FN/A/IT//SCNY14840.00/C0.00/XCNY3093.00/TCNY90.00CN/TCNY72.00FR/TCNY2931.00XT\n22.TN/057-6317336085/P1\n23.FP/CASHCNY\n24.CKG121\n", @"[0-9]{1,2}[\.](?<OSIList>OSI.+)");
            //Services = ConfigureServices(services);
            //services.Build();

            ApplicationConfiguration.Initialize();

            System.Windows.Forms.Application.Run(new MainForm());

        }

    }

}