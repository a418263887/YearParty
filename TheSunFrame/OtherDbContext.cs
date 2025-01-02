using Cqwy.DatabaseAccessor;
using Cqwy.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TheSunFrame
{

    /// <summary>
    /// 财务数据库上下文定位器
    /// </summary>
    [SuppressSniffer]
    public sealed class CwDbContextLocator : IDbContextLocator
    {
    }
    /// <summary>
    /// 国际电商数据库上下文定位器
    /// </summary>
    [SuppressSniffer]
    public sealed class GjdsDbContextLocator : IDbContextLocator
    {
    }
    /// <summary>
    /// OA数据库上下文定位器
    /// </summary>
    [SuppressSniffer]
    public sealed class OADbContextLocator : IDbContextLocator
    {
    }
    /// <summary>
    /// 国际产品库数据库上下文定位器
    /// </summary>
    [SuppressSniffer]
    public sealed class GjProductDbContextLocator : IDbContextLocator
    {
    }

    /// <summary>
    /// 旧云报数据库上下文定位器
    /// </summary>
    [SuppressSniffer]
    public sealed class OldyunbaoDbContextLocator : IDbContextLocator
    {
    }


}
