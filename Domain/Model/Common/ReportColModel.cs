using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Common
{
    /// <summary>
    ///统计项模型       
    /// </summary>
    public class ReportColModel
    {

        /// <summary>
        /// 需要一列的显示名称
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// 是否算计算行尾合计
        /// </summary>
        public bool RowTotal { get; set; }

        /// <summary>
        /// 是不是百分比 如果是 显示的时候需要加百分号
        /// </summary>
        public bool Percent { get; set; }

        /// <summary>
        /// 列合计类型 0不计算  1合计值  2平均值 3组合列计算
        /// </summary>
        public int ColTotalType { get; set; }
        /// <summary>
        /// 通过其他列来计算的公式  公式中的列要包含在table中才生效
        /// </summary>
        public string? Formula { get; set; }

        /// <summary>
        /// sql计算公式
        /// </summary>
        public string? Sql { get; set; }



    }
}
