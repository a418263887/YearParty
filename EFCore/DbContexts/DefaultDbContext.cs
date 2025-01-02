using Cqwy;
using Cqwy.DatabaseAccessor;
using Cqwy.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using TheSunFrame;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace EFCore.DbContexts
{
    #region 运行用配置服务器链接

    //public class DefaultDbContext : AppDbContext<DefaultDbContext>
    //{
    //    public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
    //    {
    //        //InsertOrUpdateIgnoreNullValues = true;

    //    }

    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(App.GetConfig<string>("yb:connectionmetadata"),b => b.MigrationsAssembly("Migrations"));
    //        base.OnConfiguring(optionsBuilder);
    //    }
    //}
    //public class CwDbContext : AppDbContext<CwDbContext, CwDbContextLocator>
    //{
    //    public CwDbContext(DbContextOptions<CwDbContext> options) : base(options)
    //    {
    //    }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(App.GetConfig<string>("cw:connectionmetadata"), b => b.MigrationsAssembly("Migrations"));
    //        base.OnConfiguring(optionsBuilder);
    //    }

    //}
    //public class GjdsDbContext : AppDbContext<GjdsDbContext, GjdsDbContextLocator>
    //{
    //    public GjdsDbContext(DbContextOptions<GjdsDbContext> options) : base(options)
    //    {
    //    }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(App.GetConfig<string>("DB:GjdsSystem"), b => b.MigrationsAssembly("Migrations"));
    //        base.OnConfiguring(optionsBuilder);
    //    }

    //}
    //public class OADbContext : AppDbContext<OADbContext, OADbContextLocator>
    //{
    //    public OADbContext(DbContextOptions<OADbContext> options) : base(options)
    //    {
    //    }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(App.GetConfig<string>("DB:OA"), b => b.MigrationsAssembly("Migrations"));
    //        base.OnConfiguring(optionsBuilder);
    //    }

    //}



    //public class GjProductDbContext : AppDbContext<GjProductDbContext, GjProductDbContextLocator>
    //{
    //    public GjProductDbContext(DbContextOptions<GjProductDbContext> options) : base(options)
    //    {
    //    }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(App.GetConfig<string>("DB:GjdsProduct"), b => b.MigrationsAssembly("Migrations"));
    //        base.OnConfiguring(optionsBuilder);
    //    }

    //}

    //public class OldyunbaoDbContext : AppDbContext<OldyunbaoDbContext, OldyunbaoDbContextLocator>
    //{
    //    public OldyunbaoDbContext(DbContextOptions<OldyunbaoDbContext> options) : base(options)
    //    {
    //    }
    //    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    {
    //        optionsBuilder.UseSqlServer(App.GetConfig<string>("DB:CloudReport"), b => b.MigrationsAssembly("Migrations"));
    //        base.OnConfiguring(optionsBuilder);
    //    }

    //}




    #endregion
    #region 迁移用本地数据库链接
    [AppDbContext("SqlServerConnectionString", DbProvider.SqlServer)]
    public class DefaultDbContext : AppDbContext<DefaultDbContext>
    {
        public DefaultDbContext(DbContextOptions<DefaultDbContext> options) : base(options)
        {
            // InsertOrUpdateIgnoreNullValues = true;

        }
    }
    #endregion



}
