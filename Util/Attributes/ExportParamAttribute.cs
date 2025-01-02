using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util.Attributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ExportParamAttribute : Attribute
    {
        public ExportParamAttribute(string exportname)
        {
            this.ExportName = exportname;
        }
        /// <summary>
        /// 字段导出映射名称
        /// </summary>
        public string ExportName { get; set; }
    }
}
