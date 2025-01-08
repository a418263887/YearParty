using Cqwy.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util.Ext;

namespace Domain.Model
{
    public class BaoMing : IEntity
    {

        [Key]
        public int Id { get; set; }
        /// <summary>
        /// 工号
        /// </summary>
        public string Gh { get; set; } = "";
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; } = "";
        /// <summary>
        /// 男女
        /// </summary>
        public string Sex { get; set; } = "";
        /// <summary>
        /// 宣言
        /// </summary>
        public string XuanYan { get; set; } = "";
        /// <summary>
        /// 照片地址
        /// </summary>
        public string FilePath { get; set; } = "";
        /// <summary>
        /// 票数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public string BuMen { get; set; } = "";

        [NotMapped]
        public List<string> FilePathList { get { return new List<string> { this.FilePath ?? "" }; } }


        [NotMapped]
        public string TXFilePathList
        {
            get
            {
                var newpath = this.FilePath.Replace("IntlStudent", "TX");
                if (newpath.isNotNull())
                {
                    var pathlist = newpath.Split('.');
                    return pathlist[0] + ".png";
                }
                return "";
            }
        }
        [NotMapped]
        public string DTXFilePathList
        {
            get
            {
                var newpath = this.FilePath.Replace("IntlStudent", "DTX");
                if (newpath.isNotNull())
                {
                    var pathlist = newpath.Split('.');
                    return pathlist[0] + ".png";
                }
                return "";
            }
        }
        [NotMapped]
        public string SLTFilePathList
        {
            get
            {
                var newpath = this.FilePath.Replace("IntlStudent", "SLT");
                if (newpath.isNotNull())
                {
                    var pathlist = newpath.Split('.');
                    return pathlist[0] + ".jpg";
                }
                return "";
            }
        }
    }
}
