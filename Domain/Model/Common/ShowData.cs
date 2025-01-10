using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model.Common
{
    public class ShowData
    {
        public string Name { get; set; }
        public decimal Percentage { get; set; }
        public string Piao { get; set; }

        public string Color { get; set; }

        public string Image { get; set; }
    }



    public class ShowResult 
    {
        public List<BaoMing> NvShen { get; set; }
        public List<BaoMing> NanShen { get; set; }
        public List<ShowData> NanPai { get; set; }
        public List<ShowData> NvPai { get; set; }
    }

    //  { name: '虚位以待', percentage: 0, piao: '10票', color:'rgb(255,255,255)', image: '/xwyd.jpg' },
}
