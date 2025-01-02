using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Attributes
{
  
    /// <summary>
    /// 兼容一下efcore的comment特性设置  没实现
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class CommentAttribute : Attribute
    {
        public string name;
        public CommentAttribute(string name)
        {
            this.name = name;
        }
    }
}
