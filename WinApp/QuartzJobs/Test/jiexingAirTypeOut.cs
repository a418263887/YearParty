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

namespace WinApp.QuartzJobs.Test
{
    [DisallowConcurrentExecution]
    public class jiexingAirTypeOut : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {


                using var scope = Program.Services.CreateScope();
                string outputFilePath = @"E:\yunbaoCjrdata\jiexingairtype.xlsx";

                ExcelService excelService = new();


                var SupplyRoutes = await $"SELECT SupplyUid,FromCity,ToCity,Carrier FROM [dbo].[SupplyRoute] where SupplyUid='jiexing'".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(60 * 10).Change<GjdsDbContextLocator>().SqlQueryAsync<SupplyRoute_jiexing>();
                foreach (var SupplyRout in SupplyRoutes)
                {
                    var F_ordermain = await $"SELECT top 1 Kuid FROM [dbo].[F_OrderMain] where SupplyUid='{SupplyRout.SupplyUid}' and Route='{SupplyRout.FromCity}{SupplyRout.ToCity}' and IStatus=14 and Carrier='{SupplyRout.Carrier}'".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(60 * 10).Change<GjdsDbContextLocator>().SqlQueryAsync<F_OrderMain_jiexing>();
                    if (F_ordermain.Count > 0)
                    {
                        var F_OrderSegment = await $"SELECT AircraftCode FROM [dbo].[F_OrderSegment] where OrderKuid='{F_ordermain[0].Kuid}'".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(60 * 10).Change<GjdsDbContextLocator>().SqlQueryAsync<F_OrderSegment_jiexing>();

                        SupplyRout.AircraftCode = F_OrderSegment[0].AircraftCode;
                    }
                }


     

                excelService.WriteListToExcel(SupplyRoutes, outputFilePath);

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }


    }






    public class ExcelService
    {


        public void WriteListToExcel(List<SupplyRoute_jiexing> dataList, string outputPath)
        {
            FileInfo newFile = new FileInfo(outputPath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 设置许可证上下文
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("NewSheet1");

                // Header row
                worksheet.Cells[1, 1].Value = "销售渠道";
                worksheet.Cells[1, 2].Value = "出发地";
                worksheet.Cells[1, 3].Value = "到达地";
                worksheet.Cells[1, 4].Value = "航司";
                worksheet.Cells[1, 5].Value = "机型";
                for (int i = 0; i < dataList.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = dataList[i].SupplyUid;
                    worksheet.Cells[i + 2, 2].Value = dataList[i].FromCity;
                    worksheet.Cells[i + 2, 3].Value = dataList[i].ToCity;
                    worksheet.Cells[i + 2, 4].Value = dataList[i].Carrier;
                    worksheet.Cells[i + 2, 5].Value = dataList[i].AircraftCode;
                }

                package.Save();
            }
        }
    }




    public class SupplyRoute_jiexing 
    {
        public string SupplyUid { get; set; } = "";
        public string FromCity { get; set; } = "";
        public string ToCity { get; set; } = "";
        public string Carrier { get; set; } = "";
        public string AircraftCode { get; set; } = "";
    }

    public class F_OrderMain_jiexing 
    {
        public string Kuid { get; set; } = "";
    } 
    public class F_OrderSegment_jiexing 
    {
        public string AircraftCode { get; set; } = "";
    }

}
