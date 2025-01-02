using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Common
{
    public class PageModel
    {
        public int code { get; set; }
        public string msg { get; set; }
        public int count { get; set; }

        public int pageCount { get; set; }
        public dynamic data { get; set; }

        public dynamic other { get; set; }
        public string extend { get; set; }


    }
}
