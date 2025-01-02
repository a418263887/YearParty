using Applications.WinAppService.AsmsService;
using Applications.WinAppService.BigCustomer;
using Applications.WinAppService.RebateService;
using Applications.WxRobotApi.RobotGroup;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Util.Ext;
using static Domain.Enums.ReportEnums.SaleReportEnums;

namespace WinApp.QuartzJobs.Rebate
{
    [DisallowConcurrentExecution]
    public class RebateJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                using var scope = Program.Services.CreateScope();

                Program.ulog?.Invoke(1, $"开始了");
                DateTime starttime = DateTime.Now.AddDays(-1);
                //DateTime starttime = "2024-05-31".ToDate();
                //大客户和品牌运价
                WinAppBigCustomerService ikaservice = scope.ServiceProvider.GetRequiredService<WinAppBigCustomerService>();
                //胜意导单
                WinAppAsmsImportService WinappAsmservicescope = scope.ServiceProvider.GetRequiredService<WinAppAsmsImportService>();
                //后返计算
                WinAppRebateService WinappService = scope.ServiceProvider.GetRequiredService<WinAppRebateService>();
                //返程无政策后返计算
                //DateTime whilestarttest = "2024-06-09".ToDate();
                //while (whilestarttest < "2024-06-14".ToDate())
                //{
                //    Program.ulog?.Invoke(1, $"**********开始同步 {whilestarttest.ToDateString()} 的报表**********");
                //    await Asmservice.EveryDayAsmsImportAsync(whilestarttest.ToDateString(), scope);
                //    Program.ulog?.Invoke(1, $"********** {whilestarttest.ToDateString()} 的报表 同步完成**********");
                //    //await Asmservice.SaleReportRestSegs(starttime.ToDateString(), scope);
                //    whilestarttest = whilestarttest.AddDays(1);
                //}



                await WinappAsmservicescope.SyncNeedAsmsTable(starttime.ToDateString(), Program.ulog);



                string TBstart = DateTime.Now.AddDays(-6).ToDateString();
                string end = DateTime.Now.ToDateString();
                DateTime whilestart = TBstart.ToDate();
                while (whilestart < end.ToDate())
                {
                    Program.ulog?.Invoke(1, $"-------------------开始同步 {whilestart.ToDateString()} 的报表-------------------");
                    await WinappAsmservicescope.EveryDayAsmsImportAsync(whilestart.ToDateString(), scope, Program.ulog);

                    Program.ulog?.Invoke(1, $"------------------- { whilestart.ToDateString()} 的报表 同步完成-------------------");

                    await WinappAsmservicescope.OldYunbaoChangeTicketOPHSyncNewCloudChangeTicket(whilestart.ToDateString(), Program.ulog, scope);

                    await WinappAsmservicescope.ChangeTicketSpecialhandle(whilestart.ToDateString(), Program.ulog);
                    whilestart = whilestart.AddDays(1);
                }




                await WinappService.DoWork(starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                await WinappService.DoWork(starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                await WinappService.DoWork(starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);



                await WinappService.NoCalculateRoundTicket_DoWork(BusinessTypes.正常单, Program.ulog);
                await WinappService.NoCalculateRoundTicket_DoWork(BusinessTypes.改签单, Program.ulog);




                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■任务完成分割■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                await ikaservice.DoWork(starttime.ToDateString(), BusinessTypes.正常单, Program.ulog, scope);
                await ikaservice.DoWork(starttime.ToDateString(), BusinessTypes.改签单, Program.ulog, scope);
                await ikaservice.DoWork(starttime.ToDateString(), BusinessTypes.退票单, Program.ulog, scope);
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■大客户和品牌运价任务完成分割■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");


            }
            catch (Exception ex)
            {
                Program.ulog?.Invoke(2, $"——后返计算程序出错：{ex.Message}");
                RebateRobot.Post($"——后返计算程序出错：{ex.Message}");
            }
        }




    }
}
