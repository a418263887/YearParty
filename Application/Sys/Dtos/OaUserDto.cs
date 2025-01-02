using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Sys.Dtos
{
    public class OaUserDto
    {
        /// <summary>
        /// 
        /// </summary>
        public int userid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string password { get; set; }
        /// <summary>
        /// 徐仕超
        /// </summary>
        public string realname { get; set; }
        /// <summary>
        /// 男
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string IdNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Duty { get; set; }
        /// <summary>
        /// 研发组组长
        /// </summary>
        public string DutyName { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string DutyTime { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deptid { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int DeptHead { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ip { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pic { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Isjob { get; set; }
    }

    public class OaResult<T>{
        /// <summary>
        /// 
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Msg { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public T Data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Other { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int Istatus { get; set; }
    }
}
