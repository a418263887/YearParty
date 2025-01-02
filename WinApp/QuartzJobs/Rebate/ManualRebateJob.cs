using Applications.WinAppService.AsmsService;
using Applications.WinAppService.BigCustomer;
using Applications.WinAppService.RebateService;
using Applications.WinAppService.SaleJob;
using Applications.WinAppService.SyncYunbao;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Util.Ext;
using static Domain.Enums.ReportEnums.SaleReportEnums;

namespace WinApp.QuartzJobs.Rebate
{
    [DisallowConcurrentExecution]
    public class ManualRebateJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {


                using var scope = Program.Services.CreateScope();
                ////后返计算
                //RebateService service = scope.ServiceProvider.GetRequiredService<RebateService>();
                //返程无政策后返计算
                //NoCalculateRoundTicketService noCalculateRoundTicketService = scope.ServiceProvider.GetRequiredService<NoCalculateRoundTicketService>();
                //var aaa = await service.DoWork(DateTime.Now.ToDateString(), BusinessTypes.正常单);
                //var bbb = await service.ManualDoWork(DateTime.Now.ToDateString(), BusinessTypes.改签单);
                //var ccc = await service.ManualDoWork(DateTime.Now.ToDateString(), BusinessTypes.退票单);

                //var ddd = await noCalculateRoundTicketService.NoCalculateRoundTicket_DoWork(BusinessTypes.正常单);
                //var eee = await noCalculateRoundTicketService.NoCalculateRoundTicket_DoWork(BusinessTypes.改签单);
                Program.ulog?.Invoke(1, $"开始了");
                //大客户和品牌运价
                WinAppBigCustomerService ikaservice = scope.ServiceProvider.GetRequiredService<WinAppBigCustomerService>();
                //Program.ulog?.Invoke(1, $"开始了");
                ////胜意导单
                WinAppAsmsImportService Asmservice = scope.ServiceProvider.GetRequiredService<WinAppAsmsImportService>();
                //后返计算
                WinAppRebateService service = scope.ServiceProvider.GetRequiredService<WinAppRebateService>();
                //后返销售任务
                WinAppSaleJobService salejobservice = scope.ServiceProvider.GetRequiredService<WinAppSaleJobService>();
                WinAppYunbaoRebateSyncService ybservice = scope.ServiceProvider.GetRequiredService<WinAppYunbaoRebateSyncService>();
                //await salejobservice.DoWork(scope, Program.ulog);

                //string starttime = "2024-10-01";
                var starttime = "2024-12-19".ToDate();
                //while (starttime < "2024-12-24".ToDate())
                //{

                //    await ybservice.DoWork(scope, starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                //    await ybservice.DoWork(scope, starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                //    await ybservice.DoWork(scope, starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);
                //    Program.ulog?.Invoke(1, $"------------------------旧云报后返更新------------------{starttime.ToDateString()}完成了");

                //    //await ikaservice.DoWork(starttime.ToDateString(), BusinessTypes.正常单, Program.ulog, scope);
                //    //await ikaservice.DoWork(starttime.ToDateString(), BusinessTypes.改签单, Program.ulog, scope);
                //    //await ikaservice.DoWork(starttime.ToDateString(), BusinessTypes.退票单, Program.ulog, scope);
                //    //await ikaservice.BFRSyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                //    //await ikaservice.BFRSyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                //    //await ikaservice.BFRSyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);

                //    starttime = starttime.AddDays(1);
                //}

                // await ikaservice.DoWork(starttime, BusinessTypes.正常单, Program.ulog, scope);
                //await ikaservice.DoWork(starttime, BusinessTypes.改签单, Program.ulog, scope);
                //await ikaservice.DoWork(starttime, BusinessTypes.退票单, Program.ulog, scope);


                //await ikaservice.BFRSyncOldyunbao(scope,starttime, BusinessTypes.正常单, Program.ulog);
                //await ikaservice.BFRSyncOldyunbao(scope, starttime, BusinessTypes.改签单, Program.ulog);
                //await ikaservice.BFRSyncOldyunbao(scope, starttime, BusinessTypes.退票单, Program.ulog);
                //await ikaservice.IKASyncOldyunbao(scope, starttime, BusinessTypes.正常单, Program.ulog);
                //await ikaservice.IKASyncOldyunbao(scope, starttime, BusinessTypes.改签单, Program.ulog);
                //await ikaservice.IKASyncOldyunbao(scope, starttime, BusinessTypes.退票单, Program.ulog);

                //await Asmservice.SyncNeedAsmsTable(starttime, Program.ulog);
                //Program.ulog?.Invoke(1, $"-------------------开始同步 {starttime} 的报表-------------------");
                //await Asmservice.EveryDayAsmsImportAsync(starttime, scope, Program.ulog);

                //Program.ulog?.Invoke(1, $"------------------- {starttime} 的报表 同步完成-------------------");
                //await Asmservice.ChangeTicketSpecialhandle(starttime, Program.ulog);

                //await service.DoWork(starttime, BusinessTypes.正常单, Program.ulog);
                //await service.DoWork(starttime, BusinessTypes.改签单, Program.ulog);
                //await service.DoWork(starttime, BusinessTypes.退票单, Program.ulog);

                #region 手动操作

                //DateTime starttime = DateTime.Now.AddDays(-1);
                //string TBstart = DateTime.Now.AddDays(-1).ToDateString();
                //string end = DateTime.Now.ToDateString();
                //DateTime whilestart = TBstart.ToDate();
                //while (whilestart < end.ToDate())
                //{
                //    Program.ulog?.Invoke(1, $"-------------------开始同步 {whilestart.ToDateString()} 的报表-------------------");
                //    await Asmservice.EveryDayAsmsImportAsync(whilestart.ToDateString(), scope, Program.ulog);

                //    Program.ulog?.Invoke(1, $"------------------- {whilestart.ToDateString()} 的报表 同步完成-------------------");
                //    await Asmservice.ChangeTicketSpecialhandle(whilestart.ToDateString(), Program.ulog);
                //    whilestart = whilestart.AddDays(1);
                //}




                //await service.DoWork(starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                //await service.DoWork(starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                //await service.DoWork(starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);



                //await service.NoCalculateRoundTicket_DoWork(BusinessTypes.正常单, Program.ulog);
                //await service.NoCalculateRoundTicket_DoWork(BusinessTypes.改签单, Program.ulog);
                #endregion
                //string end = DateTime.Now.ToDateString();
                //DateTime whilestart = "2024-07-02".ToDate();
                //while (whilestart < "2024-11-29".ToDate())
                //{
                //    // await Asmservice.SaleReportRestSegs(whilestart.ToDateString(), scope, Program.ulog);
                //    //await Asmservice.EveryDayAsmsImportAsync(whilestart.ToDateString(), scope, Program.ulog);



                //    //await Asmservice.OldYunbaoChangeTicketOPHSyncNewCloudChangeTicket(whilestart.ToDateString(), Program.ulog, scope);
                //    await Asmservice.ChangeTicketSpecialhandle(whilestart.ToDateString(), Program.ulog);


                //    whilestart = whilestart.AddDays(1);
                //}


                //await Asmservice.ChangeTicketSpecialhandle("2024-07-18", Program.ulog, "876-6315087214");
                //返程无政策后返计算
                //NoCalculateRoundTicketService noCalculateRoundTicketService = scope.ServiceProvider.GetRequiredService<NoCalculateRoundTicketService>();
                //旧云报更新

                //await service.Zenggaungguotai();
                //手动计算后返
                //await service.Test("876-6315631845", BusinessTypes.正常单, Program.ulog, false);
                List<string> list = new List<string>() { "479-6315915427","479-6315568624","479-6315137147","479-6315137148","018-6314106743","479-6315316251","479-6315316252","479-6315316262","479-6315316265","479-6315316266","479-6315316314","479-6314106814","018-6401597506","479-6401597531","018-6401597542","018-6401597543","018-6401597544","479-6401597554","479-6401597555","018-6401597562","018-6401597564","018-6401597563" };
                foreach (string s in list) 
                {
                    await service.Test(s, BusinessTypes.正常单, Program.ulog, false);
                }

                //var phs = new List<string>() { "479-4906850898", "018-4906851098", "018-4906851099", "479-4906853912", "781-4906727538", "781-4906727539" };
                WinAppYunbaoRebateSyncService Oldyunbaoservice = scope.ServiceProvider.GetRequiredService<WinAppYunbaoRebateSyncService>();
                //foreach (var item in phs)
                //{
                //    await service.Test(item, BusinessTypes.正常单, Program.ulog, false, 11);
                //}

                //await service.DoChangeFU("999-4906837774", "999-4901096361");
                //await Asmservice.EveryDayAsmsImportAsync("2024-07-13", scope, Program.ulog);

                // var aaa = await service.DoWorkTest("479-4906487244", BusinessTypes.正常单, Program.ulog);
                //var starttime = "2024-11-13".ToDate();
                //while (starttime < "2024-11-14".ToDate())
                //{
                //    //await Asmservice.EveryDayAsmsImportAsync(starttime.ToDateString(), scope, Program.ulog);
                //    //var aaa = await service.DoWork(starttime.ToDateString(), BusinessTypes.正常单);
                //    //var bbb = await service.DoWork(starttime.ToDateString(), BusinessTypes.改签单,true);
                //    //var ccc = await service.DoWork(starttime.ToDateString(), BusinessTypes.退票单);

                //    //var ddd = await noCalculateRoundTicketService.NoCalculateRoundTicket_DoWork(BusinessTypes.正常单);
                //    //var eee = await noCalculateRoundTicketService.NoCalculateRoundTicket_DoWork(BusinessTypes.改签单);




                //    await Oldyunbaoservice.DoWork(scope, starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                //    await Oldyunbaoservice.DoWork(scope, starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                //    await Oldyunbaoservice.DoWork(scope, starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);
                //    starttime = starttime.AddDays(1);
                //}

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }
    }
}
