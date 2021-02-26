using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    public class OrgAddEditDto
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public long? OrgId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Required(ErrorMessage = "部门名不能为空")]
        public string OrgName { get; set; }

        /// <summary>
        /// 上级id
        /// </summary>
        public long? ParentOrgId { get; set; }

        /// <summary>
        /// 上级名称
        /// </summary>
        public string ParentOrgName { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? SortNo { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public int Status { get; set; }
    }
}
