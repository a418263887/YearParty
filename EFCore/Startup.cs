using EFCore.DbContexts;
using Cqwy;
using Cqwy.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheSunFrame;
using Cqwy.DatabaseAccessor;
using Microsoft.EntityFrameworkCore.Migrations;

namespace EFCore
{
    public class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {



            services.AddDatabaseAccessor(options =>
            {
                //云报数据库注册
                options.AddDbPool<DefaultDbContext,MasterDbContextLocator>();

                //财务数据库注册
                //options.AddDbPool<CwDbContext, CwDbContextLocator>();

                ////国际电商数据库注册
                //options.AddDbPool<GjdsDbContext, GjdsDbContextLocator>();

                ////OA数据库注册
                //options.AddDbPool<OADbContext, OADbContextLocator>();

                ////国际产品库数据库注册
                //options.AddDbPool<GjProductDbContext, GjProductDbContextLocator>();

                ////旧云报数据库注册
                //options.AddDbPool<OldyunbaoDbContext, OldyunbaoDbContextLocator>();


     
            }, "Migrations");

        }
        //多数据迁移需要绑定DbContext  ------数据库自动生成指令  工具中打开unget包管理控制台  默认项目选中Migrations项目然后执行指令

        //-----Add-Migration 版本号 -Context 数据库上下文
        //-----Update-Database -Context 数据库上下文
        //--Add-Migration v1.0.3 -Context DefaultDbContext
        //--Update-Database -Context DefaultDbContext



        //数据库自动生成指令  工具中打开unget包管理控制台  默认项目选中Migrations项目然后执行指令
        //Add-Migration  版本号  
        //Update-Database  版本号  
        //Add-Migration v1.0.3
        //Update-Database v1.0.3
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //// 判断开发环境！！！必须！！！！
            //if (env.IsDevelopment())
            //{
            //    Scoped.Create((_, scope) =>
            //    {
            //        var context = scope.ServiceProvider.GetRequiredService<DefaultDbContext>();
            //        context.Database.Migrate();
            //    });
            //}

            // 其他代码
        }
    }

}
