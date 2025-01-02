using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.AjaxModel
{
    public class AjaxModel
    {

        /// <summary>
        /// 请求状态 1 成功  -100 未登录，-1 系统错误  0 业务错误
        /// </summary>     
        public int Code { get; set; } = -1;

        public string Msg { get; set; } = "";
        public string Token { get; set; }
        public string Other { get; set; }
        public dynamic Data { get; set; }
        public dynamic DataList { get; set; }

        public dynamic Extend { get; set; }
        public int PageCount { get; set; }
        public int DataCount { get; set; }
        public int Status { get; set; }

        public string SessionId { get; set; }
    }


    public class ResponseModel<T>
    {

        /// <summary>
        /// 状态码
        /// </summary>
        public int? StatusCode { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 数据列
        /// </summary>
        public List<T> DataList { get; set; }
        /// <summary>
        /// 执行成功
        /// </summary>
        public bool Succeeded { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public object Errors { get; set; }

        /// <summary>
        /// 附加数据
        /// </summary>
        public object Extras { get; set; }

        /// <summary>
        /// 时间戳
        /// </summary>
        public long Timestamp { get; set; }
    }


    /// <summary>
    /// 接口统一返回模型
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// 请求状态 1 成功  -100 未登录，-1 系统错误  0 业务错误
        /// </summary>     
        public int Code { get; set; } = -1;
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "";
        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; set; }
        /// <summary>
        /// 数据列
        /// </summary>
        public dynamic DataList { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// 扩展
        /// </summary>
        public dynamic Extend { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int DataCount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态 true：成功 false失败
        /// </summary>
        public bool succeeded => Code == 1;

        public string Token { get; set; }
        public string SessionId { get; set; }

        public string RecordId { get; set; }
    }

    /// <summary>
    /// 接口统一返回模型
    /// </summary>
    public class ResultModel<T>
    {
        /// <summary>
        /// 请求状态 1 成功  -100 未登录，-1 系统错误  0 业务错误
        /// </summary>     
        public int Code { get; set; } = -1;
        /// <summary>
        /// 消息
        /// </summary>
        public string Msg { get; set; } = "";
        /// <summary>
        /// 数据
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 数据列
        /// </summary>
        public List<T> DataList { get; set; }
        /// <summary>
        /// 其他
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// 扩展
        /// </summary>
        public dynamic Extend { get; set; }
        /// <summary>
        /// 页数
        /// </summary>
        public int PageCount { get; set; }
        /// <summary>
        /// 总数
        /// </summary>
        public int DataCount { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 状态 true：成功 false失败
        /// </summary>
        public bool succeeded => Code == 1;

        public string Token { get; set; }
        public string SessionId { get; set; }

        public string RecordId { get; set; }
    }
}
