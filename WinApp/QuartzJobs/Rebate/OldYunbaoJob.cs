using Applications.WinAppService.BigCustomer;
using Applications.WinAppService.SaleJob;
using Applications.WinAppService.SyncYunbao;
using Applications.WxRobotApi.RobotGroup;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Ext;
using static Domain.Enums.ReportEnums.SaleReportEnums;

namespace WinApp.QuartzJobs.Rebate
{
    [DisallowConcurrentExecution]
    public class OldYunbaoJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {


                using var scope = Program.Services.CreateScope();
                //大客户和品牌运价
                WinAppBigCustomerService ikaservice = scope.ServiceProvider.GetRequiredService<WinAppBigCustomerService>();
                WinAppYunbaoRebateSyncService service = scope.ServiceProvider.GetRequiredService<WinAppYunbaoRebateSyncService>();
                //后返销售任务
                WinAppSaleJobService salejobservice = scope.ServiceProvider.GetRequiredService<WinAppSaleJobService>();
                Program.ulog?.Invoke(1, $"开始了");
                string start = DateTime.Now.AddDays(-6).ToDateString();

                string end = DateTime.Now.ToDateString();
                DateTime starttime = start.ToDate();
                while (starttime < end.ToDate())
                {

                    await service.DoWork(scope, starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                    await service.DoWork(scope, starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                    await service.DoWork(scope, starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);
                    Program.ulog?.Invoke(1, $"------------------------旧云报后返更新------------------{starttime.ToDateString()}完成了");


                    await ikaservice.BFRSyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                    await ikaservice.BFRSyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                    await ikaservice.BFRSyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);
                    await ikaservice.IKASyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.正常单, Program.ulog);
                    await ikaservice.IKASyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.改签单, Program.ulog);
                    await ikaservice.IKASyncOldyunbao(scope, starttime.ToDateString(), BusinessTypes.退票单, Program.ulog);




                    starttime = starttime.AddDays(1);
                }
                //旧云报更新
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■任务完成分割■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                RebateRobot.SyncOldYunbaoSuccess($"{start}—{starttime.AddDays(-1).ToDateString()}");
                Program.ulog?.Invoke(1, $"开始-----------------------销售任务更新-------------------------开始");
                await salejobservice.DoWork(scope, Program.ulog);
                Program.ulog?.Invoke(1, $"完成-----------------------销售任务更新-------------------------完成");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■任务完成分割■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                Program.ulog?.Invoke(1, $"■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■■");
                RebateRobot.SaleJobOldYunbaoSuccess($"{DateTime.Now.AddDays(-1).ToDateString()}");

            }
            catch (Exception ex)
            {
                Program.ulog?.Invoke(2, $"——老云报后返同步程序出错：{ex.Message}");
                RebateRobot.Post($"——老云报后返同步程序出错：{ex.Message}");
            }
        }
    }
}
