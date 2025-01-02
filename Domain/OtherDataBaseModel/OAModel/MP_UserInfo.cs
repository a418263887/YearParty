using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.OtherDataBaseModel.OAModel
{
   /// <summary>
   /// OA用户表
   /// </summary>
    [Table("MP_UserInfo")]
    public class MP_UserInfo
    {
        public string? username { get; set; }
        public string? realname { get; set; }
    }
}
