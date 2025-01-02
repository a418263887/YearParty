using Cqwy.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int IsBaoming { get; set; }
    }
}
