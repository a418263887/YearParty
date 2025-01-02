using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Common
{

    public class OaUserModel
    {
        public int Code { get; set; }
        public string? Msg { get; set; }
        public List<OaUser> Data { get; set; }
    }

    public class OaUser
    {
        public string username { get; set; }
        public string realname { get; set; }
        public int Duty { get; set; }
        public string DutyName { get; set; }
    }

}
