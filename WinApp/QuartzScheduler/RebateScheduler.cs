

using Quartz.Impl;
using Quartz.Logging;
using Quartz;
using Serilog;
using WinApp.QuartzJobs.Rebate;
using WinApp.QuartzJobs.Test;

namespace WinApp.QuartzScheduler
{

    /// <summary>
    /// 后返计算
    /// </summary>
    /// <returns></returns>
    public class RebateScheduler
    {
        public static IScheduler scheduler;

        public static async Task Init()
        {
            LogProvider.SetCurrentLogProvider(new ConsoleLogProvider());

            StdSchedulerFactory factory = new();
            scheduler = await factory.GetScheduler();


            //{
            //    IJobDetail job = JobBuilder.Create(typeof(RebateJob)).WithIdentity("后返计算", "后返").Build();
            //    //ITrigger trigger = TriggerBuilder.Create().WithIdentity("后返计算", "后返").StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
            //    ITrigger trigger = TriggerBuilder.Create().WithIdentity("后返计算", "后返").StartNow().WithCronSchedule("0 00 02 * * ? ").Build();//--------线上版本的
            //    await scheduler.ScheduleJob(job, trigger);
            //}

            //{
            //    IJobDetail job = JobBuilder.Create(typeof(OldYunbaoJob)).WithIdentity("旧云报后返同步", "旧云报").Build();
            //    //ITrigger trigger = TriggerBuilder.Create().WithIdentity("旧云报后返同步", "旧云报").StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
            //    ITrigger trigger = TriggerBuilder.Create().WithIdentity("旧云报后返同步", "旧云报").StartNow().WithCronSchedule("0 00 10 * * ? ").Build();//--------线上版本的
            //    await scheduler.ScheduleJob(job, trigger);
            //}

            {
                string jobgroup = "手工计算后返";
                IJobDetail job = JobBuilder.Create(typeof(ManualRebateJob)).WithIdentity(jobgroup, jobgroup).Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity(jobgroup, jobgroup).StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
                //ITrigger trigger = TriggerBuilder.Create().WithIdentity("手工计算后返", "手工计算后返").StartNow().WithCronSchedule("0 00 01 * * ? ").Build();
                await scheduler.ScheduleJob(job, trigger);
            }

            {
                //string jobgroup = "文件导入导出补数据";
                //IJobDetail job = JobBuilder.Create(typeof(TempJob)).WithIdentity(jobgroup, jobgroup).Build();
                //ITrigger trigger = TriggerBuilder.Create().WithIdentity(jobgroup, jobgroup).StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
                //await scheduler.ScheduleJob(job, trigger);
            }
            {
                //string jobgroup = "旧云报分组";
                //IJobDetail job = JobBuilder.Create(typeof(OldYunbaoFzJob)).WithIdentity(jobgroup, jobgroup).Build();
                //ITrigger trigger = TriggerBuilder.Create().WithIdentity(jobgroup, jobgroup).StartNow().WithCronSchedule("0/5 * * * * ? ").Build();
                //await scheduler.ScheduleJob(job, trigger);
            }

            Program.ulog?.Invoke(1, $"■■■■后返计算任务  加载成功■■■■");


            await scheduler.Start();




        }

        public static async Task Stop()
        {
            await scheduler.Shutdown(true);
            Program.ulog?.Invoke(1, $"■■■■后返计算任务 停止调度■■■■");
        }


        //public async Task Test(string groupName, string taskName)
        //{
        //    List<JobKey> jobKeys = scheduler.GetJobKeys(GroupMatcher<JobKey>.GroupEquals(groupName)).Result.ToList();
        //    JobKey jobKey = jobKeys.Where(s => scheduler.GetTriggersOfJob(s).Result.Any(x => (x as CronTriggerImpl).Name == taskName)).FirstOrDefault();
        //    var triggers = await scheduler.GetTriggersOfJob(jobKey);
        //    ITrigger trigger = triggers?.Where(x => (x as CronTriggerImpl).Name == taskName).FirstOrDefault();

        //    await scheduler.PauseTrigger(trigger.Key);//暂停

        //    //删除任务和触发器
        //    await scheduler.UnscheduleJob(trigger.Key);
        //    await scheduler.DeleteJob(trigger.JobKey);

        //}

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
