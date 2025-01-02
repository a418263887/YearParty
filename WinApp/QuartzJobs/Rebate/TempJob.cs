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

namespace WinApp.QuartzJobs.Rebate
{
    [DisallowConcurrentExecution]
    public class TempJob : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            try
            {


                using var scope = Program.Services.CreateScope();
                string inputFilePath = @"E:\yunbaoCjrdata\file.xlsx";
                string outputFilePath = @"E:\yunbaoCjrdata\newfile.xlsx";

                ExcelService excelService = new ExcelService();

                List<MyDataModel> dataList = excelService.ReadExcelToList(inputFilePath);
                List<string> newph = new();
                var ph= dataList.Select(x=>x.PH).ToList();
                foreach (string item in ph)
                {
                    newph.Add("826-" + item);
                }
                var salereport =await GetSaleReportOldYunbao(newph, scope);



                foreach (var item in dataList)
                {
                    var singel = salereport.FirstOrDefault(x => x.CYR == item.CYR && x.PH == item.PH);
                    if (singel.isNotNull()) 
                    {
                        item.CJR = singel.CJR;
                        item.NDLF = singel.DLF;
                        item.SEGTYPE = singel.HCLX;
                        item.CPTIME = singel.CPSJ;
                        item.KHMC = singel.KHMC;
                        item.WCPDW = singel.WCPDW;
                        item.TP = singel.TFZT;
                        item.STAUS = singel.TicketSStatus;
                    }
                    var ddd =await GetGQInfo(item.PH, item.CYR, scope);
                    item.TP = ddd.Tp;
                    item.GQ = ddd.Gq.ToString()+"次改签";
                }

                excelService.WriteListToExcel(dataList, outputFilePath);

            }
            catch (Exception ex)
            {

                string aa = ex.Message;
            }
        }

        public async Task<Tempclass> GetGQInfo(string ph, string szdm, IServiceScope scope)
        {
            Tempclass tempclass = new()
            {
                Gq = 0,
                Tp = "没退票"
            };

            string newph = "";
            var qgrecord = new DataTable();
            var sql = $"select ID, GQDH,N_TKNO, TKNO, SZDH, HKGS, HC, O_HBH,O_QFSJ,O_CW, N_QFSJ,N_HBH,N_CW, CJR, n_TKNO, CJRID from veasms.t_gqsqb_mx where szdh = '{szdm}' and tkno ='{ph}'  order by id desc";
            qgrecord = GetOracleData(sql);
            if (qgrecord == null || qgrecord.Rows.Count == 0)
            {
                return tempclass;
            }
            tempclass.Gq++;
            //多次改签找出最后一条
            for (int i = 0; i < 10; i++)
            {
                var ph2 = qgrecord.Rows[0]["N_TKNO"] as string;
                if (ph2.IsNull())
                {
                    break;
                }
                newph=ph2;
                var table2 = GetOracleData($"select ID, GQDH,N_TKNO, TKNO, SZDH, HKGS, HC, O_HBH,O_QFSJ,O_CW, N_QFSJ,N_HBH,N_CW, CJR, n_TKNO, CJRID from veasms.t_gqsqb_mx where szdh = '{szdm}' and tkno ='{ph2}'  order by id desc");

                if (table2 != null && table2.Rows.Count > 0)
                {
                    tempclass.Gq++;
                    qgrecord = table2;

                }
                else
                {

                    break;
                }



            }
            var yunbaoSaleReport = await $"SELECT CYR,PH,CPSJ,CYRPH,DLF,CJR,KHMC,WCPDW,TFZT,SaleReport.HCLX,TicketSStatus FROM [dbo].[SaleReport] WHERE AddGNGJ='国内' and AddPZZT='退票'   and AddCPLX='机票' and CYRPH IN ('{szdm}-{ph}'{(newph.isNotNull() ? ",'" + szdm + "-" + newph + "'" : "")})".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(60 * 10).Change<OldyunbaoDbContextLocator>().SqlQueryAsync<SaleReportOldYunbao>();
            if (yunbaoSaleReport.Count > 0)
            {
                var tpph = yunbaoSaleReport.Select(x => x.CYRPH).ToList();
                tempclass.Tp = $"有退票-票号：{string.Join(",", tpph)}";
            }



            return tempclass;
        }

        public class Tempclass 
        {
            public string Tp { get; set; }
            public int Gq { get; set; }
        }

        public static DataTable GetOracleData(string sql)
        {
            // sql = "select  BH,MC,LXR,DH,DZ,XYED,JSFS,SFXF,KHLX,KMYE,QKED,JSLX,FKKM,SJ from veasms.ve_dept where  gs='FX' and bh='SPC259'";
            string connString = App.GetConfig<string>("ImportAsmsDB:Asms").Replace2("Oracle-");
            using OracleConnection conn = new(connString);
            conn.Open();

            OracleCommand comm = new(sql, conn) { CommandTimeout = 600 };
            DataTable dt = new();
            OracleDataAdapter add = new(comm);
            add.Fill(dt);
            return dt;

        }

        private static async Task<List<SaleReportOldYunbao>> GetSaleReportOldYunbao(List<string> ph, IServiceScope scope)
        {
            List<SaleReportOldYunbao> SaleReport = new();
            List<FenGeClass> FG = new();
            if (ph.Count > 1000)
            {
                for (int i = 0; i < ph.Count; i += 999)
                {
                    FenGeClass singel = new();
                    List<string> batch = ph.Skip(i).Take(999).ToList();
                    singel.Strings = batch;
                    FG.Add(singel);
                }

            }
            else
            {
                FenGeClass singel = new()
                {
                    Strings = ph
                };
                FG.Add(singel);
            }

            foreach (var item in FG)
            {
                List<SaleReportOldYunbao> yunbaoSaleReport = new();

                for (int i = 0; i < 100; i++)
                {
                    if (ph.isNull() || ph.Count == 0)
                    {
                        break;
                    }
                    yunbaoSaleReport = await $"SELECT CYR,PH,CPSJ,CYRPH,DLF,CJR,KHMC,WCPDW,TFZT,SaleReport.HCLX,TicketSStatus FROM [dbo].[SaleReport] WHERE AddGNGJ='国内'   and AddCPLX='机票' and CYRPH IN ('{string.Join("','", item.Strings)}')".SetContextScoped(scope.ServiceProvider).SetCommandTimeout(60 * 10).Change<OldyunbaoDbContextLocator>().SqlQueryAsync<SaleReportOldYunbao>();
                    //var aaa = await repository.DetachedEntities.Where(x =>new List<string> { "876-6644162590", "876-6644162527", "876-6644126550", "876-6644170637", "876-6644162509" }.Contains(x.CYRPH)).ToListAsync();
                    if (yunbaoSaleReport.isNotNull() && yunbaoSaleReport.Count > 0)
                    {
                        SaleReport.AddRange(yunbaoSaleReport);
                        break;
                    }
                    await Task.Delay(500);
                }
            }


            return SaleReport;
        }

    }






    public class ExcelService
    {
        public List<MyDataModel> ReadExcelToList(string filePath)
        {
            List<MyDataModel> dataList = new List<MyDataModel>();

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 设置许可证上下文




            FileInfo fileInfo = new FileInfo(filePath);

            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0]; // Assuming the first worksheet is your target

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row is header
                {
                    dataList.Add(new MyDataModel
                    {
                        CYR = worksheet.Cells[row, 1].Value?.ToString(),
                        PH = worksheet.Cells[row, 2].Value?.ToString(),
                        Office = worksheet.Cells[row, 3].Value?.ToString(),
                        DYL = worksheet.Cells[row, 4].Value?.ToString(),
                        CJR = worksheet.Cells[row, 5].Value?.ToString(),
                        NDLF = worksheet.Cells[row, 6].Value?.ToString(),
                        KHMC = worksheet.Cells[row, 7].Value?.ToString(),
                        WCPDW = worksheet.Cells[row, 8].Value?.ToString(),
                        CPTIME = worksheet.Cells[row, 9].Value?.ToString(),
                        TP = worksheet.Cells[row, 10].Value?.ToString(),
                        GQ = worksheet.Cells[row, 11].Value?.ToString(),
                        SEGTYPE = worksheet.Cells[row, 12].Value?.ToString(),
                        STAUS = worksheet.Cells[row, 13].Value?.ToString(),
                    });
                }
            }

            return dataList;
        }

        public void WriteListToExcel(List<MyDataModel> dataList, string outputPath)
        {
            FileInfo newFile = new FileInfo(outputPath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // 设置许可证上下文
            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("NewSheet1");

                // Header row
                worksheet.Cells[1, 1].Value = "结算码";
                worksheet.Cells[1, 2].Value = "票号";
                worksheet.Cells[1, 3].Value = "Office号";
                worksheet.Cells[1, 4].Value = "代理费";
                worksheet.Cells[1, 5].Value = "乘机人姓名";
                worksheet.Cells[1, 6].Value = "代理费";
                worksheet.Cells[1, 7].Value = "客户名称";
                worksheet.Cells[1, 8].Value = "外出票单位";
                worksheet.Cells[1, 9].Value = "出票日期";
                worksheet.Cells[1, 10].Value = "胜意是否有退票";
                worksheet.Cells[1, 11].Value = "胜意是否有改签";
                worksheet.Cells[1, 12].Value = "航程类型";
                worksheet.Cells[1, 13].Value = "客票状态";
                for (int i = 0; i < dataList.Count; i++)
                {
                    worksheet.Cells[i + 2, 1].Value = dataList[i].CYR;
                    worksheet.Cells[i + 2, 2].Value = dataList[i].PH;
                    worksheet.Cells[i + 2, 3].Value = dataList[i].Office;
                    worksheet.Cells[i + 2, 4].Value = dataList[i].DYL;
                    worksheet.Cells[i + 2, 5].Value = dataList[i].CJR;
                    worksheet.Cells[i + 2, 6].Value = dataList[i].NDLF;
                    worksheet.Cells[i + 2, 7].Value = dataList[i].KHMC;
                    worksheet.Cells[i + 2, 8].Value = dataList[i].WCPDW;
                    worksheet.Cells[i + 2, 9].Value = dataList[i].CPTIME;
                    worksheet.Cells[i + 2, 10].Value = dataList[i].TP;
                    worksheet.Cells[i + 2, 11].Value = dataList[i].GQ;
                    worksheet.Cells[i + 2, 12].Value = dataList[i].SEGTYPE;
                    worksheet.Cells[i + 2, 13].Value = dataList[i].STAUS;
                }

                package.Save();
            }
        }
    }

    public class MyDataModel
    {
        public string CYR  { get; set; }
        public string PH { get; set; }
        public string Office { get; set; }
        public string DYL { get; set; }
        public string CJR { get; set; }
        public string NDLF { get; set; }
        public string KHMC { get; set; }
        public string WCPDW { get; set; }
        public string CPTIME { get; set; }
        public string TP { get; set; }
        public string GQ { get; set; }
        public string SEGTYPE { get; set; }
        public string STAUS { get; set; }
    }
}
