using Applications.WinAppService.FincialRebate;
using Applications.WxRobotApi.RobotGroup;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WinApp.QuartzJobs.FincialRebate
{
    [DisallowConcurrentExecution]
    public class TestFincialJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                using var scope = Program.Services.CreateScope();


                WinAppFincialRebateService service = scope.ServiceProvider.GetRequiredService<WinAppFincialRebateService>();
                //await service.TestGuoJiHouFan(scope,11);

                await service.TestOldYunbaoXinxiaHouFanCha(scope, 11);

            }
            catch (Exception ex)
            {
                Program.ulog2?.Invoke(2, $"财务后返程序出错：{ex.Message}");
                FincialRebateRobot.Post($"财务后返程序出错：{(ex.Message == "Value cannot be null. (Parameter 'connectionString')" ? "数据库连接失败(小问题 不用在意)" : ex.Message)}");
            }
        }
    }
}
