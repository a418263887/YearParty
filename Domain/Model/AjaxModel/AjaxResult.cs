using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.AjaxModel
{
    public class AjaxResult
    {


        private int _code = 1;
        /// <summary>
        /// 请求状态 1 成功  -100 未登录，-1 系统错误  0 业务错误
        /// </summary>     
        public int Code { get { return _code; } set { _code = value; } }

        private string _msg = "OK";

        public string Msg { get { return _msg; } set { _msg = value; } }
        public string Token { get; set; }
        public string Other { get; set; }

        public string Info { get; set; }

        public dynamic Data { get; set; }
        public dynamic Data2 { get; set; }
        public dynamic DataList { get; set; }
        public dynamic TotalCount { get; set; }
        public int Istatus { get; set; }
        public int Status { get; set; }


    }
}
