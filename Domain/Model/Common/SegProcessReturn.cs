using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Common
{
    public class SegProcessReturn
    {
        /// <summary>
        /// 航段数
        /// </summary>
        public int SegCount { get; set; } = 0;
        /// <summary>
        /// 外航航段数
        /// </summary>
        public int FSegCount { get; set; } = 0;

        /// <summary>
        /// 全部航段中文名
        /// </summary>
        public string AllSegZhCn { get; set; } = "";

        /// <summary>
        /// 第一程航段
        /// </summary>
        public string FirstSeg { get; set; } = "";
        /// <summary>
        /// 第一程航段中文名
        /// </summary>
        public string FirstSegZhC { get; set; } = "";
    }
}
