using Cqwy.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Setting:IEntity
    {

        [Key]
        public int Id { get; set; }

        public DateTime BmTimeStart { get; set; }

        public DateTime BmTimeEnd { get; set; }

        public DateTime TpTimeStart { get; set; }

        public DateTime TpTimeEnd { get; set; }
        [NotMapped]
        public string Bma { get; set; } = "";
        [NotMapped]
        public string Bmb { get; set; } = "";
        [NotMapped]
        public string Tpa { get; set; } = "";
        [NotMapped]
        public string Tpb { get; set; } = "";
    }
}
