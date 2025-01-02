using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Applications.WinAppService.FincialRebate;
using Applications.WxRobotApi.RobotGroup;

namespace WinApp.QuartzJobs.FincialRebate
{

    [DisallowConcurrentExecution]
    public class FincialRebateJobJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {

                using var scope = Program.Services.CreateScope();


                WinAppFincialRebateService service = scope.ServiceProvider.GetRequiredService<WinAppFincialRebateService>();
                await service.StartWork(scope, Program.ulog2);



            }
            catch (Exception ex)
            {
                Program.ulog2?.Invoke(2, $"财务后返程序出错：{ex.Message}");
                FincialRebateRobot.Post($"财务后返程序出错：{(ex.Message== "Value cannot be null. (Parameter 'connectionString')"?"数据库连接失败(小问题 不用在意)":ex.Message)}");
            }
        }
    }
}
