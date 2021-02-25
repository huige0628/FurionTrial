using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FurionTrial.Application
{
    /// <summary>
    /// 用户设置角色
    /// </summary>
    public class UserSetRoleDto
    {
        public long UserId { get; set; }

        public List<long> RoleId { get; set; }
    }
}
