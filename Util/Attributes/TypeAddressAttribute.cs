using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Attributes
{
    /// <summary>
    /// 用于指定类型所在地址 可以通过枚举动态创建类型
    /// </summary>
    public class TypeAddressAttribute : Attribute
    {
        public TypeAddressAttribute(Type type)
        {
            this.type = type;
        }
        /// <summary>
        /// 地址
        /// </summary>
        public Type type { get; set; }
    }

}
