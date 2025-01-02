using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Common
{
    public class ElementTableModel
    {
        public int Code { get; set; }
        public string? Msg { get; set; }
        public List<ElementColModel>? Cols { get; set; }
        public dynamic? Datas { get; set; }
        public List<object>? TotalRow { get; set; }
        public  int TotalCount { get; set; }
    }
    public class ElementColModel
    {
        /// <summary>
        /// 列对应宽度
        /// </summary>
        public int width { get; set; } = 100;
        /// <summary>
        /// 列对应json字段
        /// </summary>
        public string? field { get; set; }
        /// <summary>
        /// 列显示名称
        /// </summary>
        public string? title { get; set; }

        /// <summary>
        /// 是否可排序
        /// </summary>
        public bool sort { get; set; }
        /// <summary>
        /// 是否计算合计
        /// </summary>

        public bool totalRow { get; set; }
        //     totalF=colSet.RowTotal + "," + colSet.ColTotalType + "," + colSet.Percent + "," + colSet.Formula;

        public bool percent { get; set; }

        public int totalType { get; set; }

        public string? formula { get; set; }

        public List<ElementColModel>? children { get; set; }


    }
}
