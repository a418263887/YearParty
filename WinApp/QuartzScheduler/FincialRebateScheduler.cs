using Quartz.Impl;
using Quartz.Logging;
using Quartz;
using WinApp.QuartzJobs.Rebate;
using Serilog;
using WinApp.QuartzJobs.FincialRebate;

namespace WinApp.QuartzScheduler
{
    public class FincialRebateScheduler
    {
        public static IScheduler scheduler;

        public static async Task Init()
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            StdSchedulerFactory factory = new();
            scheduler = await factory.GetScheduler();


            //{
            //    string groupname = "财务后返计算";
            //    IJobDetail job = JobBuilder.Create(typeof(FincialRebateJobJob)).WithIdentity(groupname, groupname).Build();
            //    //ITrigger trigger = TriggerBuilder.Create().WithIdentity(groupname, groupname).StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
            //    ITrigger trigger = TriggerBuilder.Create().WithIdentity(groupname, groupname).StartNow().WithCronSchedule("0 0/1 7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23 * * ? ").Build();
            //    await scheduler.ScheduleJob(job, trigger);
            //}
            {
                //string jobgroup = "从财务后返明细查询数据导入国际后返";
                //IJobDetail job = JobBuilder.Create(typeof(TestFincialJob)).WithIdentity(jobgroup, jobgroup).Build();
                //ITrigger trigger = TriggerBuilder.Create().WithIdentity(jobgroup, jobgroup).StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
                //await scheduler.ScheduleJob(job, trigger);
            }


            Program.ulog?.Invoke(1, $"■■■■财务后返计算  加载成功■■■■");


            await scheduler.Start();

        }

        public static async Task Stop()
        {
            await scheduler.Shutdown(true);
            Program.ulog?.Invoke(1, $"■■■■财务后返计算 停止调度■■■■");
        }



        private class ConsoleLogProvider : ILogProvider
        {
            public Logger GetLogger(string name)
            {
                return (level, func, exception, parameters) =>
                {
                    if (level >= LogLevel.Info && func != null)
                    {
                        Log.Debug("[" + DateTime.Now.ToLongTimeString() + "] [" + level + "] " + func(), parameters);

                    }
                    return true;
                };
            }

            public IDisposable OpenNestedContext(string message)
            {
                throw new NotImplementedException();
            }

            public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
            {
                throw new NotImplementedException();
            }
        }
    }
}
