using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    /// <summary>
    /// 添加编辑用户
    /// </summary>
    public class UserAddEditDto
    {
        public long? UserId { get; set; }

        public long? CompanyId { get; set; }

        public string CompanyCode { get; set; }

        public long? OrgId { get; set; }

        [Required(ErrorMessage = "姓名不能为空")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "用户名不能为空")]
        public string UserCode { get; set; }

        [Required(ErrorMessage = "邮箱不能为空")]
        public string Email { get; set; }

        public string Password { get; set; }

        public string Phone { get; set; }

        public string Qq { get; set; }

        public int Sex { get; set; }

        public int Status { get; set; }

        public string Remark { get; set; }

        public string Position { get; set; }

        public long? SuperiorId { get; set; }
    }
}
