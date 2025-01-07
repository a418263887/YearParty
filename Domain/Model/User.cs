using Cqwy.DatabaseAccessor;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class User:IEntity
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = "";
        public string GH { get; set; } = "";
        public string Sex { get; set; } = "";
        public int PiaoCount { get; set; } 
        public int Man { get; set; }
        public int WoMan { get; set; }
        public int IsBaoming { get; set; }

        public string YiTou { get; set; } = "";


        [NotMapped]
        public int XY { get { return 1 - this.Man; } }
        [NotMapped]
        public int XX { get { return 3 - this.WoMan; } }
    }
}
