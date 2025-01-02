using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Common
{
    public class NavModel
    {
        public Homeinfo homeInfo { get; set; }
        public Logoinfo logoInfo { get; set; }
        public List<Menuinfo> menuInfo { get; set; }
    }

    public class Homeinfo
    {
        public string title { get; set; }
        public string href { get; set; }
    }

    public class Logoinfo
    {
        public string title { get; set; }
        public string image { get; set; }
        public string href { get; set; }
    }

    public class Menuinfo
    {
        public string title { get; set; }
        /// <summary>
        /// Font Awesome字体图表 示例 fa fa-lemon-o
        /// http://www.fontawesome.com.cn/faicons/
        /// </summary>
        public string icon { get; set; } = "fa-regular fa-circle-dot";// "fa fa-dot-circle-o";
        public string href { get; set; }
        public string target { get; set; } = "_self";
        public List<Menuinfo> child { get; set; }
    }
}
