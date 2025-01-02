using Applications.WinAppService.AsmsService;
using Applications.WinAppService.RebateService;
using Microsoft.Extensions.DependencyInjection;
using OfficeOpenXml;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Domain.Enums.ReportEnums.SaleReportEnums;
using Util.Ext;
using Domain.OtherDataBaseModel.Oldyunbao;
using Domain.WinAppModel.Common;
using TheSunFrame;
using Cqwy.DatabaseAccessor.Extensions;
using System.Data;
using Cqwy;
using Oracle.ManagedDataAccess.Client;
using Domain.Model.Bud;
using Applications.WinAppService.Test;
using Cqwy.DatabaseAccessor;
using static Quartz.Logging.OperationName;

namespace WinApp.QuartzJobs.Test
{
    [DisallowConcurrentExecution]
    public class OldYunbaoFzJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {


                using var scope = Program.Services.CreateScope();

                List<SaleReportOldYunbao> UpdateSjHoufan = new();
                List<SaleReportOldYunbao> needSalereport = new();
                foreach (string AddPZZT in new List<string> { "正常票", "改签", "退票" })
                {
                    //var OldYunbao = await $"SELECT  * FROM [dbo].[SaleReport] where ddlx!='线下利润' and addcplx='机票' and AddGNGJ='国际' and QFSJ>='{job.StartDate.ToDateString()}' and QFSJ<'{job.EndDate.ToDateString()} 23:59:59' and AddPZZT='{AddPZZT}' {job.AirLines.ToInCondition("AddCarrier")}  order by cpsj ".SetCommandTimeout(600).Change<OldyunbaoDbContextLocator>().SqlQueryAsync<SaleReportOldYunbao>();
                    var OldYunbao = await $"SELECT  * FROM [dbo].[SaleReport] where ddlx!='线下利润' and  AddGNGJ='国际'  and AddPZZT='{AddPZZT}'  AND CYRPH='670-3009276998'  order by cpsj ".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(600).Change<OldyunbaoDbContextLocator>().SqlQueryAsync<SaleReportOldYunbao>();
                    //OldYunbao = OldYunbao.DistinctBy(x => x.CYRPH).ToList();
                    OldYunbao = OldYunbao.GroupBy(x => x.CYRPH).Select(g => g.OrderByDescending(x => x.AddHouFan).First()).ToList();
                    needSalereport.AddRange(OldYunbao);
                }
                TestService service = scope.ServiceProvider.GetRequiredService<TestService>();
                var ygList = await service.GetXSC_ASMSYG();


                var gjbudrules = await @"SELECT * FROM BudRule where ZT=1".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(60 * 10).Change<MasterDbContextLocator>().SqlQueryAsync<BudRule>();
                List<SaleReportOldYunbao> yunbaoSaleReport = await $"SELECT ID,HC,AddCarrier,KHMC,AddZCY,AddFZYWZ,AddFZSYB,AddFZQD,AddYWZTag,WCPDW FROM [dbo].[SaleReport] WHERE CYRPH in ('670-3009276999','670-3009276998')".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(60 * 10).Change<OldyunbaoDbContextLocator>().SqlQueryAsync<SaleReportOldYunbao>();


                List<SaleReportOldYunbao> Updates = new();

                foreach (var item in yunbaoSaleReport)
                {
                    if (item.AddFZSYB == "国际电商")
                    {
                        List<BudRule> gjbudrule = gjbudrules;
                        if (gjbudrule.Any())
                        {
                            var temprule = new List<BudRule>();
                            var tempwcpdw = new List<BudRule>();
                            foreach (var rule in gjbudrule)
                            {
                                if (rule.AsmsShopNameType == 1)
                                {
                                    temprule.Add(rule);
                                }
                                else
                                {
                                    var AsmsShopName = rule.AsmsShopName.Split2();
                                    if (rule.AsmsShopNameType == 2 && AsmsShopName.Contains(item.KHMC))
                                    {
                                        temprule.Add(rule);
                                    }
                                    if (rule.AsmsShopNameType == 3 && !AsmsShopName.Contains(item.KHMC))
                                    {
                                        temprule.Add(rule);
                                    }
                                }
                            }
                            gjbudrule = temprule;
                            //gjbudrule = gjbudrule.Where(x => x.Id == 8).ToList();
                            foreach (var rule in gjbudrule)
                            {
                                if (rule.WCPDWType == 1)
                                {

                                    if (rule.SegUseType == 1)
                                    {
                                        tempwcpdw.Add(rule);
                                    }
                                    else
                                    {
                                        var seg = rule.Seg.Split2();
                                        if (rule.SegUseType == 2 && seg.Contains(item.HC))
                                        {
                                            tempwcpdw.Add(rule);
                                        }
                                        if (rule.SegUseType == 3 && !seg.Contains(item.HC))
                                        {
                                            tempwcpdw.Add(rule);
                                        }
                                    }

                                }
                                else
                                {
                                    var WCPDW = rule.WCPDW.Split2();
                                    if (rule.WCPDWType == 2 && WCPDW.Contains(item.WCPDW))
                                    {
                                        if (rule.SegUseType == 1)
                                        {
                                            tempwcpdw.Add(rule);
                                        }
                                        else
                                        {
                                            var seg = rule.Seg.Split2();
                                            if (rule.SegUseType == 2 && seg.Contains(item.HC))
                                            {
                                                tempwcpdw.Add(rule);
                                            }
                                            if (rule.SegUseType == 3 && !seg.Contains(item.HC))
                                            {
                                                tempwcpdw.Add(rule);
                                            }
                                        }
                                    }
                                     if (rule.WCPDWType == 3 && !WCPDW.Contains(item.WCPDW))
                                    {
                                        if (rule.SegUseType == 1)
                                        {
                                            tempwcpdw.Add(rule);
                                        }
                                        else
                                        {
                                            var seg = rule.Seg.Split2();
                                            if (rule.SegUseType == 2 && seg.Contains(item.HC))
                                            {
                                                tempwcpdw.Add(rule);
                                            }
                                            if (rule.SegUseType == 3 && !seg.Contains(item.HC))
                                            {
                                                tempwcpdw.Add(rule);
                                            }
                                        }
                                    }
                                }
                            }
                            gjbudrule = tempwcpdw;
                            if (gjbudrule.Any())
                            {
                                string GSStr = item.AddCarrier != null ? item.AddCarrier.ToUpper() : "无";
                                var tempGS = gjbudrule.Where(a => a.GSUseType == 1 || (a.GSUseType == 2 && !string.IsNullOrEmpty(a.GS) && a.GS.Contains(GSStr)) || (a.GSUseType == 3 && !string.IsNullOrEmpty(a.GS) && !a.GS.Contains(GSStr))).ToList();


                                gjbudrule = tempGS;
                            }
                            //if (gjbudrule.Any())
                            //{
                            //    var tempsegs = new List<BudRule>();
                            //    foreach (var rule in gjbudrule)
                            //    {
                            //        if (rule.SegUseType == 1)
                            //        {
                            //            tempsegs.Add(rule);
                            //        }
                            //        else
                            //        {
                            //            var seg = rule.Seg.Split2();
                            //            if (rule.SegUseType == 2 && seg.Contains(item.HC))
                            //            {
                            //                tempsegs.Add(rule);
                            //            }
                            //            if (rule.SegUseType == 3 && !seg.Contains(item.HC))
                            //            {
                            //                tempsegs.Add(rule);
                            //            }
                            //        }
                            //    }


                            //    gjbudrule = tempsegs;
                            //}

                        }
                        if (gjbudrule.Count == 1)
                        {
                            var rule = gjbudrule.OrderByDescending(a => a.CreatedTime).FirstOrDefault();
                            var tymc = ygList.Where(x => x.BH == rule.PolicyUser).FirstOrDefault()?.TYMC;
                            if (tymc.isNotNull())
                            {
                                item.AddZCY = tymc;
                            }
                            item.AddFZYWZ = rule.RuleName;
                            item.AddYWZTag = item.AddFZSYB + item.AddFZQD + item.AddFZYWZ;
                            Updates.Add(item);
                        }
                    }
                }

                if (Updates.Count > 0)
                {
                    await service.UpdateFz(Updates);

                }
            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }


    }







}
